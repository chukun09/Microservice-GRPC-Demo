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

namespace Service.Services.Department.Departments
{
    public class DepartmentService : IDepartmentService
    {
        #region Constructor
        private IEmployeeUnitOfWork _unitOfWork;

        public DepartmentService(IEmployeeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion Constructor

        public async Task<DepartmentEntity> CreateAsync(DepartmentEntity entity, CancellationToken ct)
        {
           var result = await _unitOfWork.DepartmentRepository.InsertAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return result;
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var employee = await _unitOfWork.DepartmentRepository.FirstOrDefaultAsync(x => x.Id == id.ToString());
            if (employee != null)
            {
                await _unitOfWork.DepartmentRepository.Delete(employee.Id);
                await _unitOfWork.SaveChangesAsync(ct);
                return;
            }
            throw new CustomException("Id not match with any information, please check again");
        }

        public async Task<DepartmentEntity> FirstOrDefaultAsync(Expression<Func<DepartmentEntity, bool>> expression, CancellationToken ct)
        {
            return await _unitOfWork.DepartmentRepository.FirstOrDefaultAsync(expression);
        }

        public async Task<List<DepartmentEntity>> GetAsync(CancellationToken ct)
        {
            var results = await _unitOfWork.DepartmentRepository.GetAllAsync();

            // Check that we actually got some employees from the database
            //if (results == null || results.Count() == 0)
            //{
            //    // No employees were available in the database
            //    throw new KeyNotFoundException("There are no records in the database. Please add one or more employees and try again.");
            //}

            // We got 1 or more employees to return
            return new List<DepartmentEntity>(results);
        }

        public async Task<DepartmentEntity> GetByConditionAsync(Expression<Func<DepartmentEntity, bool>> condition, CancellationToken ct)
        {
            return await _unitOfWork.DepartmentRepository.FirstOrDefaultAsync(condition);
        }

        public async Task<DepartmentEntity> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _unitOfWork.DepartmentRepository.FirstOrDefaultAsync(x => x.Id == id.ToString());
        }

        public async Task<DepartmentEntity> UpdateAsync(DepartmentEntity entity, CancellationToken ct)
        {
            var employee = await _unitOfWork.DepartmentRepository.GetAsync(x => x.Id == entity.Id);
            if (employee != null)
            {
                await _unitOfWork.DepartmentRepository.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync(ct);
                return entity;
            }
            throw new CustomException("Id not match with any information, please check again");
        }
    }
}
