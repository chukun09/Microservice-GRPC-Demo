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
    public class AttendanceUnitOfWork : UnitOfWork<AttendanceDbContext>, IAttendanceUnitOfWork
    {
        public AttendanceUnitOfWork(AttendanceDbContext context) : base(context)
        {
        }
        private IRepository<AttendanceEntity, AttendanceDbContext>? _attendance;
        public IRepository<AttendanceEntity, AttendanceDbContext> AttendanceRepository => _attendance ??= new Repository<AttendanceEntity, AttendanceDbContext>(context);
        //public IRepository<AttendanceEntity, AttendanceDbContext> AttendanceRepository => throw new NotImplementedException();
    }
}
