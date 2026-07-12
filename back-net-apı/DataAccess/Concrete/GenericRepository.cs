using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataDbContext _dbContext;
        public GenericRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<T>> GetAll()
        {
                return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetValue(int id)
        {
                return await _dbContext.Set<T>().FirstOrDefaultAsync(x => EF.Property<int>(x, "id") == id);
        }
        public async Task Add(T generic)
        {
                _dbContext.Set<T>().Add(generic);
                await _dbContext.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
                var value = await _dbContext.Set<T>().FirstOrDefaultAsync(x => EF.Property<int>(x, "id") == id);
                _dbContext.Set<T>().Remove(value);
                await _dbContext.SaveChangesAsync();
        }
        public async Task Update(T generic)
        {
                _dbContext.Set<T>().Update(generic);
                await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> Any(int id)
        {
                bool value = await _dbContext.Set<T>().AnyAsync(x => EF.Property<int>(x, "id") == id);
                return value;
        }
    }
}
