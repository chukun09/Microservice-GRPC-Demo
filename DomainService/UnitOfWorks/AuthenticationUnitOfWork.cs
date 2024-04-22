using Core.Entites;
using Core.IRepositories;
using Data;
using Data.Repositories;
using DomainService.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.UnitOfWorks
{
    public class AuthenticationUnitOfWork : UnitOfWork<AuthenticationDbContext>, IAuthenticationUnitOfWork
    {
        public AuthenticationUnitOfWork(AuthenticationDbContext context) : base(context)
        {
        }

        private IRepository<RefreshTokenEntity, AuthenticationDbContext>? _refreshToken;
        public IRepository<RefreshTokenEntity, AuthenticationDbContext> RefreshTokenRepository => _refreshToken ??= new Repository<RefreshTokenEntity, AuthenticationDbContext>(context);
    }
}
