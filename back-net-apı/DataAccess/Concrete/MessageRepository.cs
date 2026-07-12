using DataAccess.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataDbContext _dbContext;
        public MessageRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<UserMessage>> ChatHistory(int sessionId)
        {
            return await _dbContext.userMessages
                            .Where(x => x.SessionId == sessionId)
                            .Include(u => u.aiMessage)
                            .OrderByDescending(u => u.user_message_date)
                            .Take(20)
                            .OrderBy(u => u.user_message_date)
                            .ToListAsync();
        }
        public async Task<ICollection<Session>> GetRecentSessionsAsync()
        {
            var sessions = await _dbContext.sessions
                .OrderByDescending(s => s.created_date)
                .ToListAsync();

            return sessions;
        }
    }
}
