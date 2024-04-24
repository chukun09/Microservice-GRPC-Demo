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

namespace Service.Services.Employee.WorkHoursSummary
{
    public class WorkHoursSummaryService : IWorkHoursSummaryService
    {
        #region Constructor
        private readonly IEmployeeUnitOfWork _unitOfWork;

        public WorkHoursSummaryService(IEmployeeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion Constructor

        public async Task<WorkHoursSummaryEntity> CreateAsync(WorkHoursSummaryEntity entity, CancellationToken ct)
        {
            await _unitOfWork.WorkHoursSummaryRepository.InsertAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return entity;
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var employee = await _unitOfWork.WorkHoursSummaryRepository.FirstOrDefaultAsync(x => x.Id == id.ToString());
            if (employee != null)
            {
                await _unitOfWork.WorkHoursSummaryRepository.Delete(employee.Id);
                await _unitOfWork.SaveChangesAsync(ct);
                return;
            }
            throw new CustomException("Id not match with any information, please check again");
        }

        public async Task<List<WorkHoursSummaryEntity>> GetAsync(CancellationToken ct)
        {
            var results = await _unitOfWork.WorkHoursSummaryRepository.GetAllAsync();

            // Check that we actually got some employees from the database
            if (results == null || results.Count == 0)
            {
                // No employees were available in the database
                throw new KeyNotFoundException("There are no records in the database. Please add one or more employees and try again.");
            }

            // We got 1 or more employees to return
            return new List<WorkHoursSummaryEntity>(results);
        }

        public async Task<WorkHoursSummaryEntity> GetByConditionAsync(Expression<Func<WorkHoursSummaryEntity, bool>> condition, CancellationToken ct)
        {
            return await _unitOfWork.WorkHoursSummaryRepository.FirstOrDefaultAsync(condition);
        }

        public async Task<WorkHoursSummaryEntity> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _unitOfWork.WorkHoursSummaryRepository.FirstOrDefaultAsync(x => x.Id == id.ToString());
        }

        public async Task<WorkHoursSummaryEntity> UpdateAsync(WorkHoursSummaryEntity entity, CancellationToken ct)
        {
            var employee = await _unitOfWork.WorkHoursSummaryRepository.GetAsync(x => x.Id == entity.Id);
            if (employee != null)
            {
                await _unitOfWork.WorkHoursSummaryRepository.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync(ct);
                return entity;
            }
            throw new CustomException("Id not match with any information, please check again");
        }
    }
}
