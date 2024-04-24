using Core.Entites;
using DomainService.Commands;
using DomainService.Events.Queries;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Service.Services.Employee.Employees;

namespace EmployeeMicroservice.Services
{
    [Authorize]
    public class EmployeeService : Employeer.EmployeerBase
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMediator _mediator;

        public EmployeeService(ILogger<EmployeeService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        /// <summary>
        /// Create new Employee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<EmployeeReply> AddEmployee(AddEmployeeRequest request, ServerCallContext context)
        {

            //Map request message to object
            var employee = new EmployeeEntity()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserId = request.UserId,
                Address = request.Address,
                DateOfBirth = request.DateOfBirth.ToDateTime(),
                DepartmentId = request.DepartmentId,
                Position = request.Position,
            };
            _logger.LogInformation("Send command add employee");
            employee = await _mediator.Send(new AddEmployeeCommand(employee), context.CancellationToken);
            return await Task.FromResult(new EmployeeReply
            {
                Message = "Thêm mới thành công"
            });
        }
        /// <summary>
        /// Delete Employee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<EmployeeReply> DeleteEmployee(DeleteEmployeeRequest request, ServerCallContext context)
        {
            await _mediator.Send(new DeleteEmployeeCommand(new Guid(request.Id)), context.CancellationToken);
            return await Task.FromResult(new EmployeeReply
            {
                Message = "Xóa thành công"
            });
        }
        /// <summary>
        /// Get All Employee
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task GetAllEmployee(IAsyncStreamReader<Empty> requestStream, IServerStreamWriter<GetAllEmployeeReply> responseStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                var employees = await _mediator.Send(new GetAllEmployeeQuery(), context.CancellationToken);
                var result = new GetAllEmployeeReply();
                foreach (var entity in employees)
                {
                    //Map request message to object
                    var reply = new EmployeeMessage()
                    {
                        Id = entity.Id,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        UserId = entity.UserId,
                        Address = entity.Address ?? "",
                        DateOfBirth = entity.DateOfBirth == null ? null : Timestamp.FromDateTime((DateTime)entity.DateOfBirth),
                        DepartmentId = entity.DepartmentId ?? "",
                        Position = entity.Position ?? "",
                    };
                    result.Employees.Add(reply);
                }
                await responseStream.WriteAsync(result);
            }
        }
        /// <summary>
        /// Update Information for Employee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<EmployeeReply> UpdateEmployee(EmployeeRequest request, ServerCallContext context)
        {
            //Map request message to object
            var employee = new EmployeeEntity()
            {
                Id = request.Employee.Id,
                FirstName = request.Employee.FirstName,
                LastName = request.Employee.LastName,
                UserId = request.Employee.UserId,
                Address = request.Employee.Address,
                DateOfBirth = request.Employee.DateOfBirth.ToDateTime(),
                DepartmentId = request.Employee.DepartmentId,
                Position = request.Employee.Position,
            };
            _logger.LogInformation("Send command add employee");
            employee = await _mediator.Send(new UpdateEmployeeCommand(employee), context.CancellationToken);
            return await Task.FromResult(new EmployeeReply
            {
                Message = "Cập nhật thông tin thành công"
            });
        }
    }
}
