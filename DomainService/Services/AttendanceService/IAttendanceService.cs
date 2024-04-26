using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Services.AttendanceService
{
    public interface IAttendanceService : IBaseService<AttendanceEntity>
    {
        Task<List<AttendanceEntity>> GetAllAttandanceByEmployeeIdAsync(string employeeId, CancellationToken ct);
    }
}
