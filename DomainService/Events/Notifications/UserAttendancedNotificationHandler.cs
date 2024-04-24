using AutoMapper;
using EventBus.Message.Attendances;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DomainService.Events.Notifications
{
    public class UserAttendancedNotificationHandler : INotificationHandler<UserAttendancedNotification>
    {
        private readonly IPublishEndpoint _publisher;
        private readonly ILogger<UserAttendancedNotificationHandler> _logger;

        public UserAttendancedNotificationHandler(IPublishEndpoint publisher, ILogger<UserAttendancedNotificationHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task Handle(UserAttendancedNotification notification, CancellationToken cancellationToken)
        {
            var attendance = notification.attendance;
            var attendanceCreatedEvent = new UserAttendancedEvent()
            {
                CheckinTime = attendance.CheckinTime,
                CheckoutTime = attendance.CheckoutTime,
                Date = attendance.Date,
                EmployeeId = attendance.EmployeeId
            };
            {
                _logger.LogInformation($"Publishing attendance created event to message bus service for employee: {attendanceCreatedEvent.EmployeeId}");
                await _publisher.Publish(attendanceCreatedEvent, cancellationToken);
                //throw new NotImplementedException();
            }
        }
    }
}
