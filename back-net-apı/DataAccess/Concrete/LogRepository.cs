using DataAccess.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class LogRepository : ILogRepository
    {
        public readonly DataDbContext _dbContext;
        public LogRepository(DataDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task AddLogAsync(Log log)
        {
            await _dbContext.AddAsync(log);
            await _dbContext.SaveChangesAsync();
        }
    }
}
