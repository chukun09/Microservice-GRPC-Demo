using Core.Entites;
using DomainService.Services.EmployeeService;
using EventBus.Message.Users;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
namespace DomainService.Consumer
{
    public class UserLoggedInConsumer : IConsumer<UserLoggedInEvent>
    {
        private readonly ILogger<UserLoggedInConsumer> _logger;
        private readonly IEmployeeService _employeeService;

        public UserLoggedInConsumer(ILogger<UserLoggedInConsumer> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        public async Task Consume(ConsumeContext<UserLoggedInEvent> context)
        {
            _logger.LogInformation($"Consumed User Logged In Message. Details: User {context.Message.UserId}");
            var message = context.Message;
            var employee = await _employeeService.GetByConditionAsync(x => x.UserId == message.UserId, context.CancellationToken);
            if (employee == null)
            {
                var newEmployee = new EmployeeEntity()
                {
                    FirstName = message?.FirstName ?? "",
                    LastName = message?.LastName ?? "",
                    UserId = message.UserId,
                };
                await _employeeService.CreateAsync(newEmployee, context.CancellationToken);
            }
        }
    }
}
