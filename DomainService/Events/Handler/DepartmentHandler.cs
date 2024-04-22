using DomainService.Commands;
using DomainService.Events.Notifications;
using Core.Entites;
using MediatR;
using Autofac.Core;
using DomainService.Events.Queries;
using DomainService.Services.EmployeeService;

namespace DomainService.Events.Handler
{
    /// <summary>
    /// Get All Department
    /// </summary>
    public class GelAllDepartmentHandler : IRequestHandler<GetAllDepartmentQuery, List<DepartmentEntity>>
    {
        private readonly IDepartmentService _service;

        public GelAllDepartmentHandler(IDepartmentService service)
        {
            _service = service;
        }

        public async Task<List<DepartmentEntity>> Handle(GetAllDepartmentQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAsync(cancellationToken);
        }
    }
    /// <summary>
    /// Get By Id
    /// </summary>
    public class GelByIdDepartmentHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentEntity>
    {
        private readonly IDepartmentService _service;

        public GelByIdDepartmentHandler(IDepartmentService service)
        {
            _service = service;
        }

        public async Task<DepartmentEntity> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(request.id, cancellationToken);
        }
    }
    /// <summary>
    /// Add User Department Handler Command
    /// </summary>
    public class AddUserDepartmentHandler : BaseHandler<AddDepartmentCommand, DepartmentEntity, IDepartmentService>
    {
        public AddUserDepartmentHandler(IMediator mediator, IDepartmentService service) : base(mediator, service)
        {
        }

        public override async Task<DepartmentEntity> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(request.entity, cancellationToken);
            //await _mediator.Publish(new UserDepartmentdNotification(result), cancellationToken);
            return result;
        }
    }
    /// <summary>
    /// Update User Department Handler Command
    /// </summary>
    public class UpdateUserDepartmentHandler : BaseHandler<UpdateDepartmentCommand, DepartmentEntity, IDepartmentService>
    {
        public UpdateUserDepartmentHandler(IMediator mediator, IDepartmentService service) : base(mediator, service)
        {
        }

        public override async Task<DepartmentEntity> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(request.entity, cancellationToken);
            return result;
        }
    }
    /// <summary>
    /// Delete User Department Handler Command
    /// </summary>
    public class DeleteUserDepartmentHandler : IRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IDepartmentService _departmentService;

        public DeleteUserDepartmentHandler(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            await _departmentService.DeleteAsync(request.id, cancellationToken);
        }
    }
}
