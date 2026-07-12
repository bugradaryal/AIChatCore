using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class LogRepository : ILogRepository
    {
        private readonly DataDbContext _dbContext;
        public LogRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<string?> GetLatestTitleAsync()
        {
            var log = await _dbContext.logs
                .OrderByDescending(l => l.date)
                .FirstOrDefaultAsync();

            return log?.prop;
        }
    }
}
