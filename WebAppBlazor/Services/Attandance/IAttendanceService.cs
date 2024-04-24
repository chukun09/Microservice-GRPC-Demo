using Core.Entites;

namespace WebAppBlazor.Services.Attandance
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceEntity>> GetAllAsync();
        Task<AttendanceEntity> AddAsync(AttendanceEntity entity);
        Task<AttendanceEntity> UpdateAsync(AttendanceEntity entity);
        Task<bool> DeleteAsync(string id);
    }
}
