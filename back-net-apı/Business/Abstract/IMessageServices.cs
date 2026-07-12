using DTO;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMessageServices
    {
        Task<DTOChatHistoryResult> GetChatHistoryAsync(int sessionId);
        Task<DTOAiResponse> SendMessageAsync(DTOSendMessage sendMessage);
        Task DeleteSessionAsync(int sessionId);
        Task<ICollection<Session>> GetSectionListAsync();
    }
}
