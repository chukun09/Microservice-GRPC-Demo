using Core.Entites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Events.Queries
{
    public record GetAllWorkHoursSummaryQuery : IRequest<List<WorkHoursSummaryEntity>>;
    public record GetWorkHoursSummaryByIdQuery(Guid id) : IRequest<WorkHoursSummaryEntity>;
}
