using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISessionServices
    {
        Task DeleteSessionAsync(int sessionId);
        Task<ICollection<Session>> GetSectionListAsync();
        Task AddSessionAsync(Session session);
        Task<bool> SessionAnyAsync(int sessionId);
        Task<Session> GetValueSessionAsync(int sessionId);
    }
}
