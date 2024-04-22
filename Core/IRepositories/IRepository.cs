using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IRepository<T, key, dbContext> where T : class where dbContext : DbContext
    {
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            List<string> includes = null);

        Task<T?> GetAsync(Expression<Func<T, bool>> expression, List<string> includes = null);
        Task<T> InsertAsync(T enity);
        Task InsertRangeAsync(IEnumerable<T> entites);
        Task<bool> Delete(key id);
        void DeleteRange(IEnumerable<T> entites);
        void Update(T enity);
        Task UpdateAsync(T entity);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression = null);
        void SaveChange();
    }
    public interface IRepository<T, dbContext> : IRepository<T, string, dbContext>  where T : class where dbContext : DbContext
    {

    }
}
