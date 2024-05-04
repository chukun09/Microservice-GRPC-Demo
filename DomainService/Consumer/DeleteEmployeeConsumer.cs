using DomainService.Services.AttendanceService;
using EventBus.Message.Attendances;
using EventBus.Message.Employee;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Consumer
{
    public class DeleteEmployeeConsumer : IConsumer<DeleteEmployeeEvent>
    {
        private IAttendanceService _attendanceService;

        public DeleteEmployeeConsumer(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public async Task Consume(ConsumeContext<DeleteEmployeeEvent> context)
        {
            var listAttendance = await _attendanceService.GetAllAttandanceByEmployeeIdAsync(context.Message.EmployeeId, context.CancellationToken);
            foreach (var attendance in listAttendance)
            {
                await _attendanceService.DeleteAsync(new Guid(attendance.Id), context.CancellationToken);
            }
            await Task.CompletedTask;
        }
    }
}
