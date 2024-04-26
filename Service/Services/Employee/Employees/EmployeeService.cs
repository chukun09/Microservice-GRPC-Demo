using Core.Entites;
using Core.Helpers;
using DomainService.Services.EmployeeService;
using DomainService.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Employee.Employees
{
    public class EmployeeService : IEmployeeService
    {
        #region Constructor
        private IEmployeeUnitOfWork _unitOfWork;

        public EmployeeService(IEmployeeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion Constructor

        public async Task<EmployeeEntity> CreateAsync(EmployeeEntity entity, CancellationToken ct)
        {
            if (entity.DepartmentId != null)
            {
                var department = await _unitOfWork.DepartmentRepository.FirstOrDefaultAsync(x => x.Id == entity.DepartmentId);
                if (department == null)
                {
                    throw new Exception("Department doesn't exist");
                }
            }
            await _unitOfWork.EmployeeRepository.InsertAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return entity;
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var employee = await _unitOfWork.EmployeeRepository.FirstOrDefaultAsync(x => x.Id == id.ToString());
            if (employee != null)
            {
                await _unitOfWork.EmployeeRepository.Delete(employee.Id);
                await _unitOfWork.SaveChangesAsync(ct);
                return;
            }
            throw new CustomException("Id not match with any information, please check again");
        }

        public async Task<EmployeeEntity> FirstOrDefaultAsync(Expression<Func<EmployeeEntity, bool>> expression, CancellationToken ct)
        {
            return await _unitOfWork.EmployeeRepository.FirstOrDefaultAsync(expression);
        }

        public async Task<List<EmployeeEntity>> GetAsync(CancellationToken ct)
        {
            var results = await _unitOfWork.EmployeeRepository.GetAllAsync(includes: new List<string>() { "Department" });

            // Check that we actually got some employees from the database
            if (results == null || results.Count() == 0)
            {
                // No employees were available in the database
                throw new KeyNotFoundException("There are no records in the database. Please add one or more employees and try again.");
            }

            // We got 1 or more employees to return
            return new List<EmployeeEntity>(results);
        }

        public async Task<EmployeeEntity> GetByConditionAsync(Expression<Func<EmployeeEntity, bool>> condition, CancellationToken ct)
        {
            return await _unitOfWork.EmployeeRepository.FirstOrDefaultAsync(condition);
        }

        public async Task<EmployeeEntity> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _unitOfWork.EmployeeRepository.FirstOrDefaultAsync(x => x.Id == id.ToString());
        }

        public async Task<EmployeeEntity> UpdateAsync(EmployeeEntity entity, CancellationToken ct)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(x => x.Id == entity.Id);
            if (employee != null)
            {
                await _unitOfWork.EmployeeRepository.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync(ct);
                return entity;
            }
            throw new CustomException("Id not match with any information, please check again");
        }
    }
}
