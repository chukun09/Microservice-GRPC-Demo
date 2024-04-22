using Core.Entites;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UnitOfWork<dbContext> : IUnitOfWork<dbContext> where dbContext : DbContext
    { 
        protected readonly dbContext context;
        //private IRepository<PersonEntity> _people;
        //public IRepository<PersonEntity> People => _people ??= new Repository<PersonEntity>(context);

        public UnitOfWork(dbContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
           context.Dispose();
            GC.SuppressFinalize(this);
        }

        public void SaveChanges()
        {
           context.SaveChanges();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
