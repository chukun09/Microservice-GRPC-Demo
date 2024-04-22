using Core.Entites;
using Core.IRepositories;
using Data;
using Data.Repositories;
using DomainService.UnitOfWorks.Interfaces;

namespace DomainService.UnitOfWorks
{
    public class EmployeeUnitOfWork : UnitOfWork<EmployeeDbContext>, IEmployeeUnitOfWork
    {
        public EmployeeUnitOfWork(EmployeeDbContext context) : base(context)
        {
        }
        private IRepository<EmployeeEntity, EmployeeDbContext>? _employee;
        public IRepository<EmployeeEntity, EmployeeDbContext> EmployeeRepository => _employee ??= new Repository<EmployeeEntity, EmployeeDbContext>(context);

        private IRepository<WorkHoursSummaryEntity, EmployeeDbContext>? _workHoursSummary;
        public IRepository<WorkHoursSummaryEntity, EmployeeDbContext> WorkHoursSummaryRepository => _workHoursSummary ??= new Repository<WorkHoursSummaryEntity, EmployeeDbContext>(context);

        private IRepository<DepartmentEntity, EmployeeDbContext>? _department;
        public IRepository<DepartmentEntity, EmployeeDbContext> DepartmentRepository => _department ??= new Repository<DepartmentEntity, EmployeeDbContext>(context);

    }
}
 
