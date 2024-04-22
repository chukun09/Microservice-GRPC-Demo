using Core.Entites;
using DepartmentMicroservice;
using DomainService.Commands;
using DomainService.Events.Queries;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using static Grpc.Core.Metadata;

namespace DepartmentMicroservice.Services
{
    [Authorize]
    public class DepartmentService : Departmenter.DepartmenterBase
    {
        private readonly ILogger<DepartmentService> _logger;
        private readonly IMediator _mediator;

        public DepartmentService(ILogger<DepartmentService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        /// <summary>
        /// Create new Department
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<DepartmentReply> AddDepartment(AddDepartmentRequest request, ServerCallContext context)
        {

            //Map request message to object
            var department = new DepartmentEntity()
            {
                Name = request.Name,
                Description = request.Description,
            };
            _logger.LogInformation("Send command add department");
            department = await _mediator.Send(new AddDepartmentCommand(department), context.CancellationToken);
            return await Task.FromResult(new DepartmentReply
            {
                Message = "Thêm mới thành công"
            });
        }
        /// <summary>
        /// Delete Department
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<DepartmentReply> DeleteDepartment(DeleteDepartmentRequest request, ServerCallContext context)
        {
            await _mediator.Send(new DeleteDepartmentCommand(new Guid(request.Id)), context.CancellationToken);
            return await Task.FromResult(new DepartmentReply
            {
                Message = "Xóa thành công"
            });
        }
        /// <summary>
        /// Get All Department
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task GetAllDepartment(IAsyncStreamReader<Empty> requestStream, IServerStreamWriter<GetAllDepartmentReply> responseStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                var departments = await _mediator.Send(new GetAllDepartmentQuery(), context.CancellationToken);
                var result = new GetAllDepartmentReply();
                foreach (var entity in departments)
                {
                    //Map request message to object
                    var reply = new Department()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                    };
                    result.Departments.Add(reply);
                }
                await responseStream.WriteAsync(result);
            }
        }
        /// <summary>
        /// Update Information for Department
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<DepartmentReply> UpdateDepartment(DepartmentRequest request, ServerCallContext context)
        {
            //Map request message to object
            var department = new DepartmentEntity()
            {
                Id = request.Department.Id,
                Name = request.Department.Name,
                Description = request.Department.Description,
            };
            _logger.LogInformation("Send command add department");
            department = await _mediator.Send(new UpdateDepartmentCommand(department), context.CancellationToken);
            return await Task.FromResult(new DepartmentReply
            {
                Message = "Cập nhật thông tin thành công"
            });
        }
    }
}