using Core.Entites;
using Core.IRepositories;
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.UnitOfWorks.Interfaces
{
    public interface IEmployeeUnitOfWork : IUnitOfWork<EmployeeDbContext>
    {
        IRepository<EmployeeEntity, EmployeeDbContext> EmployeeRepository { get; }
        IRepository<WorkHoursSummaryEntity, EmployeeDbContext> WorkHoursSummaryRepository { get; }
        IRepository<DepartmentEntity, EmployeeDbContext> DepartmentRepository { get; }
    }


}
