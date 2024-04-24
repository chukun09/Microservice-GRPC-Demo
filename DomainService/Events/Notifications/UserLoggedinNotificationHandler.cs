using EventBus.Message.Attendances;
using EventBus.Message.Users;
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
    public class UserLoggedinNotificationHandler : INotificationHandler<UserLoggedinNotification>
    {
        private readonly IPublishEndpoint _publisher;
        private readonly ILogger<UserLoggedinNotificationHandler> _logger;

        public UserLoggedinNotificationHandler(IPublishEndpoint publisher, ILogger<UserLoggedinNotificationHandler> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task Handle(UserLoggedinNotification notification, CancellationToken cancellationToken)
        {
            var signInResult = notification.signInResult;
            var loggedInEvent = new UserLoggedInEvent()
            {
                UserId = signInResult.UserId,
                AccessToken = signInResult.AccessToken,
                RefreshToken = signInResult.RefreshToken,
                UserName = signInResult.UserName,
                FirstName = signInResult.FirstName,
                LastName = signInResult.LastName,
            };
            _logger.LogInformation($"User Signed In to System: {loggedInEvent.UserId} {loggedInEvent.UserName}");
            await _publisher.Publish(loggedInEvent, cancellationToken);
        }
    }
}
