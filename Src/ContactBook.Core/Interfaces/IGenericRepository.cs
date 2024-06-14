using ContactBook.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);

        Task<T> GetByIdAsync(int id);
        Task AddAsync(T Entity);
        Task UpdateAsync(int id, T Entity);
        Task DeleteAsync(int id);
        Task<int> CountAsync();
        Task SaveChanges();
    }
}
