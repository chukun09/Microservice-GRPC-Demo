using Core.Entites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Events.Queries
{
    public record GetAllDepartmentQuery : IRequest<List<DepartmentEntity>>;
    public record GetDepartmentByIdQuery(Guid id) : IRequest<DepartmentEntity>;
}
