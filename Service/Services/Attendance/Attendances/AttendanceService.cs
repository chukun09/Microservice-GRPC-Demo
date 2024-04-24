using Core.Entites;
using Core.Helpers;
using DomainService.Services.AttendanceService;
using DomainService.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Attendance.Attendances
{
    public class AttendanceService : IAttendanceService
    {
        #region Constructor
        private IAttendanceUnitOfWork _unitOfWork;

        public AttendanceService(IAttendanceUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion Constructor

        public async Task<AttendanceEntity> CreateAsync(AttendanceEntity entity, CancellationToken ct)
        {
            await _unitOfWork.AttendanceRepository.InsertAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return entity;
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var employee = await _unitOfWork.AttendanceRepository.FirstOrDefaultAsync(x => x.Id == id.ToString());
            if (employee != null)
            {
                await _unitOfWork.AttendanceRepository.Delete(employee.Id);
                await _unitOfWork.SaveChangesAsync(ct);
                return;
            }
            throw new CustomException("Id not match with any information, please check again");
        }

        public async Task<List<AttendanceEntity>> GetAsync(CancellationToken ct)
        {
            var results = await _unitOfWork.AttendanceRepository.GetAllAsync();

            // Check that we actually got some employees from the database
            if (results == null || results.Count() == 0)
            {
                // No employees were available in the database
                throw new KeyNotFoundException("There are no records in the database. Please add one or more employees and try again.");
            }

            // We got 1 or more employees to return
            return new List<AttendanceEntity>(results);
        }

        /// <summary>
        /// Get by condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<AttendanceEntity> GetByConditionAsync(Expression<Func<AttendanceEntity, bool>> condition, CancellationToken ct)
        {
            return await _unitOfWork.AttendanceRepository.FirstOrDefaultAsync(condition);
        }

        public async Task<AttendanceEntity> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _unitOfWork.AttendanceRepository.FirstOrDefaultAsync(x => x.Id == id.ToString());
        }

        public async Task<AttendanceEntity> UpdateAsync(AttendanceEntity entity, CancellationToken ct)
        {
            var employee = await _unitOfWork.AttendanceRepository.GetAsync(x => x.Id == entity.Id);
            if (employee != null)
            {
                await _unitOfWork.AttendanceRepository.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync(ct);
                return entity;
            }
            throw new CustomException("Id not match with any information, please check again");
        }
    }
}
