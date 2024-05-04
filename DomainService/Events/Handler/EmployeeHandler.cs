using Core.Entites;
using DomainService.Commands;
using DomainService.Events.Notifications;
using DomainService.Events.Queries;
using DomainService.Services.EmployeeService;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MassTransit.ValidationResultExtensions;

namespace DomainService.Events.Handler
{
    /// <summary>
    /// Get All Employee
    /// </summary>
    public class GelAllEmployeeHandler : IRequestHandler<GetAllEmployeeQuery, List<EmployeeEntity>>
    {
        private readonly IEmployeeService _service;

        public GelAllEmployeeHandler(IEmployeeService service)
        {
            _service = service;
        }

        public async Task<List<EmployeeEntity>> Handle(GetAllEmployeeQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAsync(cancellationToken);
        }
    }
    /// <summary>
    /// Get By Id
    /// </summary>
    public class GelByIdEmployeeHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeEntity>
    {
        private readonly IEmployeeService _service;

        public GelByIdEmployeeHandler(IEmployeeService service)
        {
            _service = service;
        }

        public async Task<EmployeeEntity> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(request.id, cancellationToken);
        }
    }
    /// <summary>
    /// Get By UserId
    /// </summary>
    public class GelByUserIdEmployeeHandler : IRequestHandler<GetEmployeeByUserIdQuery, EmployeeEntity>
    {
        private readonly IEmployeeService _service;

        public GelByUserIdEmployeeHandler(IEmployeeService service)
        {
            _service = service;
        }

        public async Task<EmployeeEntity> Handle(GetEmployeeByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.FirstOrDefaultAsync(x => x.UserId == request.userId, cancellationToken);
        }
    }
    /// <summary>
    /// Add User Employee Handler Command
    /// </summary>
    public class AddUserEmployeeHandler : BaseHandler<AddEmployeeCommand, EmployeeEntity, IEmployeeService>
    {
        public AddUserEmployeeHandler(IMediator mediator, IEmployeeService service) : base(mediator, service)
        {
        }

        public override async Task<EmployeeEntity> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(request.entity, cancellationToken);
            //await _mediator.Publish(new UserEmployeedNotification(result), cancellationToken);
            return result;
        }
    }
    /// <summary>
    /// Update User Employee Handler Command
    /// </summary>
    public class UpdateUserEmployeeHandler : BaseHandler<UpdateEmployeeCommand, EmployeeEntity, IEmployeeService>
    {
        public UpdateUserEmployeeHandler(IMediator mediator, IEmployeeService service) : base(mediator, service)
        {
        }

        public override async Task<EmployeeEntity> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(request.entity, cancellationToken);
            return result;
        }
    }
    /// <summary>
    /// Delete User Employee Handler Command
    /// </summary>
    public class DeleteUserEmployeeHandler : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IEmployeeService _employeeService;
        protected readonly IMediator _mediator;

        public DeleteUserEmployeeHandler(IEmployeeService employeeService, IMediator mediator)
        {
            _employeeService = employeeService;
            _mediator = mediator;
        }

        public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            await _employeeService.DeleteAsync(request.id, cancellationToken);
            await _mediator.Publish(new DeleteEmployeeNotification(request.id.ToString()), cancellationToken);

        }
    }
}
