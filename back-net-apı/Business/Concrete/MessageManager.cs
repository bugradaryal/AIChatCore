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
using Utilities.Mapper;
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
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly ChatClient _client;
        private readonly ISessionServices _sessionServices;
        public MessageManager(IMessageRepository messageRepository, IMapper mapper, IGenericRepository<UserMessage> genericUser, ChatClient client, IGenericRepository<AiMessage> genericAi, ISessionServices sessionServices)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _genericUser = genericUser;
            _client = client;
            _genericAi = genericAi;
            _sessionServices = sessionServices;
        }
        public async Task<ICollection<DTOChatHistory>> GetAllChatHistoryAsync()
        {
            var result = await _messageRepository.AllChatHistory();

            var dtoList = result.SelectMany(x => new[]
            {
                new DTOChatHistory
                {
                    id = x.id,
                    message = x.user_message,
                    role = ERoles.user,
                    date = x.user_message_date
                },
                new DTOChatHistory
                {
                    id = x.aiMessage?.id,
                    message = x.aiMessage?.ai_message,
                    role = ERoles.assistant,
                    date = x.aiMessage?.ai_message_date ?? DateTime.MinValue
                }
            }).ToList();
            return dtoList;
        }
        public async Task<DTOChatHistoryResult> GetChatHistoryAsync(int sessionId)
        {
            var result = await _messageRepository.ChatHistory(sessionId);

            var dtoList = result.SelectMany(x => new[]
            {
                new DTOChatHistory
                {
                    id = x.id,
                    message = x.user_message,
                    role = ERoles.user,
                    date = x.user_message_date
                },
                new DTOChatHistory
                {
                    id = x.aiMessage?.id,
                    message = x.aiMessage?.ai_message,
                    role = ERoles.assistant,
                    date = x.aiMessage?.ai_message_date ?? DateTime.MinValue
                }
            }).ToList();

            var session = await _sessionServices.GetValueSessionAsync(sessionId);

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

            if (!sendMessage.sessionId.HasValue || !await _sessionServices.SessionAnyAsync(sendMessage.sessionId.Value))
            {
                // Yeni session — title üret, DB'ye kaydet
                var prompt = $"Bu mesaj için en fazla 5 kelimelik kısa bir başlık üret: \"{sendMessage.message.Substring(0, Math.Min(300, sendMessage.message.Length))}\"";
                ChatCompletion titleCompletion = await _client.CompleteChatAsync(prompt);
                title = titleCompletion.Content[0].Text.Trim();

                var session = new Session { title = title, created_date = DateTime.UtcNow };
                await _sessionServices.AddSessionAsync(session);

                sessionId = session.id;
            }
            else
            {
                sessionId = sendMessage.sessionId.Value;
                var historyResult = await GetChatHistoryAsync(sessionId);

                messages = historyResult.History
                    .Select(x =>
                    {
                        if (x.role == ERoles.user)
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
                user_id = userMessage.id,
                ai_id = aiMessage.id,
                session_id = sessionId,
                ai_message = aiMessage.ai_message,
                ai_message_date = aiMessage.ai_message_date,
                title = title
            };
        }

    }
}
