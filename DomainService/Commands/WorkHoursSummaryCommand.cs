using Core.Entites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Commands
{
    public record AddWorkHoursSummaryCommand(WorkHoursSummaryEntity entity) : IRequest<WorkHoursSummaryEntity>;
    public record DeleteWorkHoursSummaryCommand(Guid id) : IRequest;
    public record UpdateWorkHoursSummaryCommand(WorkHoursSummaryEntity entity) : IRequest<WorkHoursSummaryEntity>;
}
