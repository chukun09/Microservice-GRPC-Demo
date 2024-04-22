using Core.Entites;
using Core.IRepositories;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.UnitOfWorks.Interfaces
{
    public interface IAuthenticationUnitOfWork : IUnitOfWork<AuthenticationDbContext>
    {
        IRepository<RefreshTokenEntity, AuthenticationDbContext> RefreshTokenRepository { get; }
        //IRepository<UserEntity, AuthenticationDbContext> UserRepository { get; }
    }
}
