using Core.Entites;
using System.Linq.Expressions;

namespace WebAppBlazor.Services.Attendance
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceEntity>> GetAllAsync();
        Task<AttendanceEntity> AddAsync(AttendanceEntity entity);
        Task<AttendanceEntity> UpdateAsync(AttendanceEntity entity);
        Task<bool> DeleteAsync(string id);
        Task<AttendanceEntity> GetAttendanceByEmployeeIdAsync(string employeeId);
        Task<List<AttendanceEntity>> GetAllAttendanceByEmployeeIdAsync(string employeeId);
    }
}
