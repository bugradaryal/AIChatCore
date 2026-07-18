using Business.Abstract;
using DataAccess.Abstract;
using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Logging.ErrorHandling.ExceptionHandler;
using Utilities.Mapper;

namespace Business.Concrete
{
    public class SessionManager : ISessionServices
    {
        private readonly IGenericRepository<Session> _genericSession;
        private readonly IMapper _mapper;
        public SessionManager(IMapper mapper, IGenericRepository<Session> genericSession) 
        {
            _mapper = mapper;
            _genericSession = genericSession;
        }
        public async Task DeleteSessionAsync(int sessionId)
        {
            if (!await SessionAnyAsync(sessionId))
                throw new CustomException("Silinecek Kayıt Bulunamadı!", exceptionLevel: (int)ELog.Warn, errorCode: StatusCodes.Status404NotFound);
            await _genericSession.Delete(sessionId);
        }

        public async Task<ICollection<Session>> GetSectionListAsync()
        {
            var sessions = await _genericSession.GetAll();
            return sessions
                .OrderByDescending(x => x.created_date)
                .ToList();
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
            if (!await SessionAnyAsync(sessionId))
                throw new CustomException("Getirilecek Kayıt Bulunamadı!", exceptionLevel: (int)ELog.Warn, errorCode: StatusCodes.Status404NotFound);
            return await _genericSession.GetValue(sessionId);
        }
    }
}
