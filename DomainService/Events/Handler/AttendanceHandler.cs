using DomainService.Commands;
using DomainService.Events.Notifications;
using Core.Entites;
using MediatR;
using DomainService.Services.AttendanceService;
using Autofac.Core;
using DomainService.Events.Queries;

namespace DomainService.Events.Handler
{
    /// <summary>
    /// Get All Attendance
    /// </summary>
    public class GelAllAttendanceHandler : IRequestHandler<GetAllAttendanceQuery, List<AttendanceEntity>>
    {
        private readonly IAttendanceService _service;

        public GelAllAttendanceHandler(IAttendanceService service)
        {
            _service = service;
        }

        public async Task<List<AttendanceEntity>> Handle(GetAllAttendanceQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAsync(cancellationToken);
        }
    }
    /// <summary>
    /// Get By Id
    /// </summary>
    public class GelByIdAttendanceHandler : IRequestHandler<GetAttendanceByIdQuery, AttendanceEntity>
    {
        private readonly IAttendanceService _service;

        public GelByIdAttendanceHandler(IAttendanceService service)
        {
            _service = service;
        }

        public async Task<AttendanceEntity> Handle(GetAttendanceByIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(request.id, cancellationToken);
        }
    }
    /// <summary>
    /// Add User Attendance Handler Command
    /// </summary>
    public class AddUserAttendanceHandler : BaseHandler<AddAttendanceCommand, AttendanceEntity, IAttendanceService>
    {
        public AddUserAttendanceHandler(IMediator mediator, IAttendanceService service) : base(mediator, service)
        {
        }

        public override async Task<AttendanceEntity> Handle(AddAttendanceCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(request.entity, cancellationToken);
            await _mediator.Publish(new UserAttendancedNotification(result), cancellationToken);
            return result;
        }
    }
    /// <summary>
    /// Update User Attendance Handler Command
    /// </summary>
    public class UpdateUserAttendanceHandler : BaseHandler<UpdateAttendanceCommand, AttendanceEntity, IAttendanceService>
    {
        public UpdateUserAttendanceHandler(IMediator mediator, IAttendanceService service) : base(mediator, service)
        {
        }

        public override async Task<AttendanceEntity> Handle(UpdateAttendanceCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(request.entity, cancellationToken);
            return result;
        }
    }
    /// <summary>
    /// Delete User Attendance Handler Command
    /// </summary>
    public class DeleteUserAttendanceHandler : IRequestHandler<DeleteAttendanceCommand>
    {
        private readonly IAttendanceService _attendanceService;

        public DeleteUserAttendanceHandler(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public async Task Handle(DeleteAttendanceCommand request, CancellationToken cancellationToken)
        {
            await _attendanceService.DeleteAsync(request.id, cancellationToken);
        }
    }
}
