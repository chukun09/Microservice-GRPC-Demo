using DomainService.Services.EmployeeService;
using EventBus.Message.Attendances;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Consumer
{
    public class UserAttendancedConsumer : IConsumer<UserAttendancedEvent>
    {
        private readonly ILogger<UserAttendancedConsumer> _logger;
        private readonly IEmployeeService _employeeService;

        public UserAttendancedConsumer(ILogger<UserAttendancedConsumer> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        public async Task Consume(ConsumeContext<UserAttendancedEvent> context)
        {
            _logger.LogInformation($"Consumed Attendance Created Message. Details: Attendance {context.Message.Id}");
            var employee = await _employeeService.GetByIdAsync(new Guid(context.Message.EmployeeId ?? ""), context.CancellationToken);
            if (employee != null)
            {
                //if()
            }
            await Task.CompletedTask;

        }
    }
}
