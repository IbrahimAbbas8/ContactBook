using ContactBook.Core.Entities;
using ContactBook.Core.Interfaces;
using ContactBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ContactBookDbContext context;

        public GenericRepository(ContactBookDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(T Entity)
        {
            await context.Set<T>().AddAsync(Entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll()
        {
            // لما أقلو AsNoTracking فهيك بسرع عملية البحث
            return context.Set<T>().AsNoTracking().ToList();
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = context.Set<T>();
            foreach (var item in includes)
            {
                queryable = queryable.Include(item);
            }
            return await queryable.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(int id, T Entity)
        {
            var entity = await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (entity is not null)
            {
                context.Set<T>().Update(Entity);
                context.SaveChanges();
            }
        }

        public async Task<int> CountAsync()
        {
            return await context.Set<T>().CountAsync();
        }

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}
