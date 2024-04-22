using Core.Entites;
using MediatR;

namespace DomainService.Events.Notifications
{
    public record UserAttendancedNotification(AttendanceEntity attendance) : INotification;
}
