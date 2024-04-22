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
    public interface IAttendanceUnitOfWork : IUnitOfWork<AttendanceDbContext>
    {
        IRepository<AttendanceEntity, AttendanceDbContext> AttendanceRepository { get; }
    }
}
