using Business.Abstract;
using DataAccess.Abstract;
using Entities;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Mapper;

namespace Business.Concrete
{
    public class SessionManager : ISessionServices
    {
        private readonly IGenericRepository<Session> _genericSession;
        private readonly IMapper _mapper;
        public SessionManager(IMapper mapper, IGenericRepository<Session> genericSession) 
        {
        
        }
        public async Task DeleteSessionAsync(int sessionId)
        {
            await _genericSession.Delete(sessionId);
        }

        public async Task<ICollection<Session>> GetSectionListAsync()
        {
            return await _genericSession.GetAll();
        }
        public async Task AddSessionAsync(Session session)
        {
            await _genericSession.Add(session);
        }
        public async Task<bool> SessionAnyAsync(int sessionId)
        {
            return await _genericSession.Any(sessionId);
        }
        public async Task<Session> GetValueSessionAsync(int sessionId)
        {
            return await _genericSession.GetValue(sessionId);
        }
    }
}
