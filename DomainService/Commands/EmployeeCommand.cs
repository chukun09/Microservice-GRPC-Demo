using Core.Entites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Commands
{
        public record AddEmployeeCommand(EmployeeEntity entity) : IRequest<EmployeeEntity>;
        public record DeleteEmployeeCommand(Guid id) : IRequest;
        public record UpdateEmployeeCommand(EmployeeEntity entity) : IRequest<EmployeeEntity>;
}
