using Core.Entites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Commands
{
    public record AddDepartmentCommand(DepartmentEntity entity) : IRequest<DepartmentEntity>;
    public record DeleteDepartmentCommand(Guid id) : IRequest;
    public record UpdateDepartmentCommand(DepartmentEntity entity) : IRequest<DepartmentEntity>;
}
