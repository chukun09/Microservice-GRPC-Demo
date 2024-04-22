using Core.IRepositories;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class Repository<T, key, dbContext> : IRepository<T, key, dbContext> where T : class where dbContext : DbContext
    {
        private readonly dbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(dbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<bool> Delete(key id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            return true;
        }

        public void DeleteRange(IEnumerable<T> entites)
        {
            _dbSet.RemoveRange(entites);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null) return await _dbSet.FirstOrDefaultAsync();
            var result = await _dbSet.FirstOrDefaultAsync(expression);
            return result;
        }

        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {

            IQueryable<T> query = _dbSet;
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);

        }

        public async Task<T> InsertAsync(T enity)
        {
            var result = await _dbSet.AddAsync(enity);
            return result.Entity;
        }

        public async Task InsertRangeAsync(IEnumerable<T> entites)
        {
            await _dbSet.AddRangeAsync(entites);
        }

        public void SaveChange()
        {
            _context.SaveChanges();
        }

        public void Update(T enity)
        {
            _dbSet.Attach(enity);
            _context.Entry(enity).State = EntityState.Modified;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
    public class Repository<T, dbContext> : Repository<T, string, dbContext>, IRepository<T, dbContext> where T : class where dbContext : DbContext
    {
        public Repository(dbContext context) : base(context)
        {
        }
    }
}
