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

namespace DomainService.Events.Handler
{
    /// <summary>
    /// Get All WorkHoursSummary
    /// </summary>
    public class GelAllWorkHoursSummaryHandler : IRequestHandler<GetAllWorkHoursSummaryQuery, List<WorkHoursSummaryEntity>>
    {
        private readonly IWorkHoursSummaryService _service;

        public GelAllWorkHoursSummaryHandler(IWorkHoursSummaryService service)
        {
            _service = service;
        }

        public async Task<List<WorkHoursSummaryEntity>> Handle(GetAllWorkHoursSummaryQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAsync(cancellationToken);
        }
    }    /// <summary>
         /// Get All WorkHoursSummary By EmployeeId
         /// </summary>
    public class GelAllWorkHoursSummaryByEmployeeIdHandler : IRequestHandler<GetAllWorkHoursSummaryByEmployeeIdQuery, List<WorkHoursSummaryEntity>>
    {
        private readonly IWorkHoursSummaryService _service;

        public GelAllWorkHoursSummaryByEmployeeIdHandler(IWorkHoursSummaryService service)
        {
            _service = service;
        }

        public async Task<List<WorkHoursSummaryEntity>> Handle(GetAllWorkHoursSummaryByEmployeeIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAllWorkHoursSummaryEntityIdAsync(request.employeeId, cancellationToken);
        }
    }
    /// <summary>
    /// Get By Id
    /// </summary>
    public class GelByIdWorkHoursSummaryHandler : IRequestHandler<GetWorkHoursSummaryByIdQuery, WorkHoursSummaryEntity>
    {
        private readonly IWorkHoursSummaryService _service;

        public GelByIdWorkHoursSummaryHandler(IWorkHoursSummaryService service)
        {
            _service = service;
        }

        public async Task<WorkHoursSummaryEntity> Handle(GetWorkHoursSummaryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(request.id, cancellationToken);
        }
    }
    /// <summary>
    /// Add User WorkHoursSummary Handler Command
    /// </summary>
    public class AddWorkHoursSummaryHandler : BaseHandler<AddWorkHoursSummaryCommand, WorkHoursSummaryEntity, IWorkHoursSummaryService>
    {
        public AddWorkHoursSummaryHandler(IMediator mediator, IWorkHoursSummaryService service) : base(mediator, service)
        {
        }

        public override async Task<WorkHoursSummaryEntity> Handle(AddWorkHoursSummaryCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.CreateAsync(request.entity, cancellationToken);
            //await _mediator.Publish(new WorkHoursSummarydNotification(result), cancellationToken);
            return result;
        }
    }
    /// <summary>
    /// Update User WorkHoursSummary Handler Command
    /// </summary>
    public class UpdateWorkHoursSummaryHandler : BaseHandler<UpdateWorkHoursSummaryCommand, WorkHoursSummaryEntity, IWorkHoursSummaryService>
    {
        public UpdateWorkHoursSummaryHandler(IMediator mediator, IWorkHoursSummaryService service) : base(mediator, service)
        {
        }

        public override async Task<WorkHoursSummaryEntity> Handle(UpdateWorkHoursSummaryCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.UpdateAsync(request.entity, cancellationToken);
            return result;
        }
    }
    /// <summary>
    /// Delete User WorkHoursSummary Handler Command
    /// </summary>
    public class DeleteWorkHoursSummaryHandler : IRequestHandler<DeleteWorkHoursSummaryCommand>
    {
        private readonly IWorkHoursSummaryService _workHoursSummaryService;

        public DeleteWorkHoursSummaryHandler(IWorkHoursSummaryService workHoursSummaryService)
        {
            _workHoursSummaryService = workHoursSummaryService;
        }

        public async Task Handle(DeleteWorkHoursSummaryCommand request, CancellationToken cancellationToken)
        {
            await _workHoursSummaryService.DeleteAsync(request.id, cancellationToken);
        }
    }
}
