using Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Services
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<T> GetByConditionAsync(Expression<Func<T, bool>> condition, CancellationToken ct);
        Task<T> GetByIdAsync(Guid id, CancellationToken ct);
        Task<List<T>> GetAsync(CancellationToken ct);
        Task<T> CreateAsync(T entity, CancellationToken ct);
        Task<T> UpdateAsync(T entity, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, CancellationToken ct);
    }
}
