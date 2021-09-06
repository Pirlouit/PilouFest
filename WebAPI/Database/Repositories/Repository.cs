using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebAPI.Database.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected Context Context { get; }
        protected DbSet<T> Table => Context.Set<T>();

        protected Repository(Context context)
        {
            Context = context;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public virtual async Task<List<T>> GetAllAsync<TProperty>(Expression<Func<T, TProperty>> includePath)
        {
            return await Context.Set<T>().Include(includePath).ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public virtual async Task<List<T>> SearchForAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().Where(predicate).ToListAsync();
        }

        public T Add(T entity)
        {
            return Context.Set<T>().Add(entity).Entity;
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }
    }
}
