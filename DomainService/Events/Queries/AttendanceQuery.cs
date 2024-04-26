using Core.Entites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Events.Queries
{
    public record GetAllAttendanceQuery : IRequest<List<AttendanceEntity>>;
    public record GetAttendanceByIdQuery(Guid id) : IRequest<AttendanceEntity>;
    public record GetAttendanceByEmployeeIdQuery(string employeeId) : IRequest<AttendanceEntity>;
    public record GetAllAttendanceByEmployeeIdQuery(string employeeId) : IRequest<List<AttendanceEntity>>;
}
