using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Services.AuthenticationService
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenEntity> GetByIdAsync(Guid id, CancellationToken ct);
        Task<List<RefreshTokenEntity>> GetAsync(CancellationToken ct);
        Task<RefreshTokenEntity> CreateAsync(RefreshTokenEntity entity, CancellationToken ct);
        Task<RefreshTokenEntity> UpdateAsync(RefreshTokenEntity entity, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
