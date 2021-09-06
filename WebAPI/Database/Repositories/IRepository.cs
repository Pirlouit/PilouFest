using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebAPI.Database.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> SearchForAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(long id);
        T Add(T entity);
        void Delete(T entity);
    }
}
