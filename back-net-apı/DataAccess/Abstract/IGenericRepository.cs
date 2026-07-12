using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IGenericRepository<T>
    {
        Task Delete(int id);
        Task Update(T generic);
        Task Add(T generic);
        Task<T> GetValue(int id);
        Task<List<T>> GetAll();
        Task<bool> Any(int id);
    }
}
