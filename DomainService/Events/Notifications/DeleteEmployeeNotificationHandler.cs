using EventBus.Message.Attendances;
using EventBus.Message.Employee;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Events.Notifications
{
    public class DeleteEmployeeNotificationHandler : INotificationHandler<DeleteEmployeeNotification>
    {
        private readonly IPublishEndpoint _publisher;
        private readonly ILogger<DeleteEmployeeNotificationHandler> _logger;

        public DeleteEmployeeNotificationHandler(IPublishEndpoint publisher, ILogger<DeleteEmployeeNotificationHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task Handle(DeleteEmployeeNotification notification, CancellationToken cancellationToken)
        {
            var employeeId = notification.employeeId;
            var deleteEmployeeEvent = new DeleteEmployeeEvent()
            {
                EmployeeId = employeeId
            };
            {
                _logger.LogInformation($"Publishing delete Employeee event to message bus service for employee: {deleteEmployeeEvent.EmployeeId}");
                await _publisher.Publish(deleteEmployeeEvent, cancellationToken);
            }
        }
    }
}