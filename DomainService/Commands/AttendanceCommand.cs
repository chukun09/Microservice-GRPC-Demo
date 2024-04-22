using Core.Entites;
using MediatR;

namespace DomainService.Commands
{
    public record AddAttendanceCommand(AttendanceEntity entity) : IRequest<AttendanceEntity>;
    public record DeleteAttendanceCommand(Guid id) : IRequest;
    public record UpdateAttendanceCommand(AttendanceEntity entity) : IRequest<AttendanceEntity>;
    //public record AddAttendanceCommand(AttendanceEntity entity) : IRequest<AttendanceEntity>;
}
