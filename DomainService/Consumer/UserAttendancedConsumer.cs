using Core.Entites;
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
        private readonly IWorkHoursSummaryService _workHoursSummaryService;

        public UserAttendancedConsumer(ILogger<UserAttendancedConsumer> logger, IEmployeeService employeeService, IWorkHoursSummaryService workHoursSummaryService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _workHoursSummaryService = workHoursSummaryService;
        }

        public async Task Consume(ConsumeContext<UserAttendancedEvent> context)
        {
            _logger.LogInformation($"Consumed Attendance Created Message. Details: Attendance {context.Message.Id}");
            if (context.Message.CheckoutTime != null)
            {
                var employee = await _employeeService.GetByIdAsync(new Guid(context.Message.EmployeeId ?? ""), context.CancellationToken);
                if (employee != null)
                {
                    // Calculate work duration
                    TimeSpan workDuration = (DateTime)context.Message.CheckoutTime - context.Message.CheckinTime;

                    // Calculate total hours worked and round the result
                    var totalHoursWorked = (short)Math.Round(workDuration.TotalHours);
                    var newWorkSummary = new WorkHoursSummaryEntity()
                    {
                        EmployeeId = employee.Id,
                        SummaryDate = context.Message.Date,
                        TotalWorkedHours = totalHoursWorked
                    };
                    await _workHoursSummaryService.CreateAsync(newWorkSummary, context.CancellationToken);
                }
            }
            await Task.CompletedTask;
        }
    }
}
