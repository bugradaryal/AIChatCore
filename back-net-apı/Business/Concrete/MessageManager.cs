using Business.Abstract;
using DataAccess.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DTO;
using System.Collections.Immutable;
using Utilitys.Mapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Entities.Enums;
using OpenAI.Chat;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrete
{
    public class MessageManager : IMessageServices
    {
        private readonly IGenericRepository<UserMessage> _genericUser;
        private readonly IGenericRepository<AiMessage> _genericAi;
        private readonly IGenericRepository<Session> _genericSession;
        private readonly IMessageRepository _messageRepository;
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly ChatClient _client;
        public MessageManager(IMessageRepository messageRepository, IMapper mapper, IGenericRepository<UserMessage> genericUser, ChatClient client, IGenericRepository<AiMessage> genericAi, ILogRepository logRepository, IGenericRepository<Session> genericSession)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _genericUser = genericUser;
            _client = client;
            _genericAi = genericAi;
            _logRepository = logRepository;
            _genericSession = genericSession;
        }

        public async Task<DTOChatHistoryResult> GetChatHistoryAsync(int sessionId)
        {
            var result = await _messageRepository.ChatHistory(sessionId);

            var dtoList = result.SelectMany(x => new[]
            {
                new DTOChatHistory
                {
                    message = x.user_message,
                    role = Roles.user,
                    date = x.user_message_date
                },
                new DTOChatHistory
                {
                    message = x.aiMessage?.ai_message,
                    role = Roles.assistant,
                    date = x.aiMessage?.ai_message_date ?? DateTime.MinValue
                }
            }).ToList();

            var session = await _genericSession.GetValue(sessionId);

            return new DTOChatHistoryResult
            {
                History = dtoList,
                Title = session?.title
            };
        }

        public async Task<DTOAiResponse> SendMessageAsync(DTOSendMessage sendMessage)
        {
            int sessionId;
            string? title = null;
            ICollection<ChatMessage> messages = new List<ChatMessage>();

            if (sendMessage.sessionId is null)
            {
                // Yeni session — title üret, DB'ye kaydet
                var prompt = $"Bu mesaj için en fazla 5 kelimelik kısa bir başlık üret: \"{sendMessage.message.Substring(0, Math.Min(300, sendMessage.message.Length))}\"";
                ChatCompletion titleCompletion = await _client.CompleteChatAsync(prompt);
                title = titleCompletion.Content[0].Text.Trim();

                var session = new Session { title = title, created_date = DateTime.UtcNow };
                await _genericSession.Add(session);

                sessionId = session.id;
            }
            else
            {
                // Var olan session — geçmişi çek
                sessionId = sendMessage.sessionId.Value;
                if(await _genericSession.Any(sessionId))
                {

                }
                var historyResult = await GetChatHistoryAsync(sessionId);

                messages = historyResult.History
                    .Select(x =>
                    {
                        if (x.role == Roles.user)
                            return (ChatMessage)new UserChatMessage(x.message!);
                        else
                            return (ChatMessage)new AssistantChatMessage(x.message!);
                    })
                    .ToList();
            }

            messages.Add(ChatMessage.CreateUserMessage(sendMessage.message));

            ChatCompletion completion = await _client.CompleteChatAsync(messages);
            string aiResponse = completion.Content[0].Text;

            var userMessage = new UserMessage
            {
                user_message = sendMessage.message,
                SessionId = sessionId
            };
            await _genericUser.Add(userMessage);

            var aiMessage = new AiMessage { ai_message = aiResponse, UserMessageId = userMessage.id };
            await _genericAi.Add(aiMessage);

            return new DTOAiResponse
            {
                ai_message = aiMessage.ai_message,
                ai_message_date = aiMessage.ai_message_date,
                sessionId = sessionId,
                title = title
            };
        }

        public async Task DeleteSessionAsync(int sessionId)
        {
            await _genericSession.Delete(sessionId);
        }

        public async Task<ICollection<Session>> GetSectionListAsync()
        {
            return await _genericSession.GetAll();
        }
    }
}
