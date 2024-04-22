using DomainService.Services;
using DomainService.Services.AttendanceService;
using Entities.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Events.Handler
{
    public class BaseHandler<TCommand, TEntity, TService> : IRequestHandler<TCommand, TEntity> where TEntity : BaseEntity where TService : IBaseService<TEntity> where TCommand : IRequest<TEntity>
    {
        protected readonly IMediator _mediator;
        protected readonly TService _service;

        public BaseHandler(IMediator mediator, TService service)
        {
            _mediator = mediator;
            _service = service;
        }

        public virtual Task<TEntity> Handle(TCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
