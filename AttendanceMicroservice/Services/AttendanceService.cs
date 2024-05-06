using Core.Entites;
using DomainService.Commands;
using DomainService.Events.Queries;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using static Grpc.Core.Metadata;

namespace AttendanceMicroservice.Services
{
    [Authorize]
    public class AttendanceService : Attendancer.AttendancerBase
    {
        private readonly ILogger<AttendanceService> _logger;
        private readonly IMediator _mediator;

        public AttendanceService(ILogger<AttendanceService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Create new Attendance
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<AttendanceReply> AddAttendance(AddAttendanceRequest request, ServerCallContext context)
        {
            //Map request message to object
            var attendance = new AttendanceEntity()
            {
                CheckinTime = request.CheckinTime.ToDateTime(),
                CheckoutTime = request.CheckoutTime.ToDateTime(),
                Date = DateOnly.FromDateTime(request.Date.ToDateTime()),
                EmployeeId = request.EmployeeId,
            };
            _logger.LogInformation("Send command add attendance");
            attendance = await _mediator.Send(new AddAttendanceCommand(attendance), context.CancellationToken);
            return await Task.FromResult(new AttendanceReply
            {
                Message = "Thêm mới thành công"
            });
        }
        /// <summary>
        /// Get All Attendance
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GetAllAttendance(IAsyncStreamReader<Empty> requestStream, IServerStreamWriter<GetAllAttendanceReply> responseStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                var attendances = await _mediator.Send(new GetAllAttendanceQuery(), context.CancellationToken);
                var result = new GetAllAttendanceReply();
                foreach (var entity in attendances)
                {
                    //Map request message to object
                    var reply = new AttendanceMessage()
                    {
                        Id = entity.Id,
                        CheckinTime = Timestamp.FromDateTime(entity.CheckinTime.ToUniversalTime()),
                        CheckoutTime = Timestamp.FromDateTime(entity.CheckoutTime.GetValueOrDefault().ToUniversalTime()),
                        Date = entity.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime().ToTimestamp(),
                        EmployeeId = entity.EmployeeId,
                    };
                    result.Attendances.Add(reply);
                }
                await responseStream.WriteAsync(result);
            }
        }
        /// <summary>
        /// Get All Attendance By Employee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<GetAllAttendanceReply> GetAllAttendanceByEmployeeId(GetAttendanceByEmployeeIdRequest request, ServerCallContext context)
        {
            var attendances = await _mediator.Send(new GetAllAttendanceByEmployeeIdQuery(request.EmployeeId), context.CancellationToken);
            var result = new GetAllAttendanceReply();
            foreach (var entity in attendances)
            {
                //Map request message to object
                var reply = new AttendanceMessage()
                {
                    Id = entity.Id,
                    CheckinTime = Timestamp.FromDateTime(entity.CheckinTime),
                    CheckoutTime = Timestamp.FromDateTime(entity.CheckoutTime.GetValueOrDefault().ToUniversalTime()),
                    Date = entity.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime().ToTimestamp(),
                    EmployeeId = entity.EmployeeId,
                };
                result.Attendances.Add(reply);
            }
            return result;
        }
        /// <summary>
        /// Update Attendance
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<AttendanceReply> UpdateAttendance(AttendanceRequest request, ServerCallContext context)
        {
            //Map request message to object
            var attendance = new AttendanceEntity()
            {
                Id = request.Attendance.Id,
                CheckinTime = request.Attendance.CheckinTime.ToDateTime(),
                CheckoutTime = request.Attendance.CheckoutTime.ToDateTime(),
                Date = DateOnly.FromDateTime(request.Attendance.Date.ToDateTime()),
                EmployeeId = request.Attendance.EmployeeId,
            };
            _logger.LogInformation("Send command add attendance");
            attendance = await _mediator.Send(new UpdateAttendanceCommand(attendance), context.CancellationToken);
            return await Task.FromResult(new AttendanceReply
            {
                Message = "Cập nhật thông tin thành công"
            });
        }
        /// <summary>
        /// Delete Attendance
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<AttendanceReply> DeleteAttendance(DeleteAttendanceRequest request, ServerCallContext context)
        {
            await _mediator.Send(new DeleteAttendanceCommand(new Guid(request.EmployeeId)), context.CancellationToken);
            return await Task.FromResult(new AttendanceReply
            {
                Message = "Xóa thành công"
            });
        }
        /// <summary>
        /// Get Attendance By Employee Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<GetAttendanceByEmployeeIdResponse> GetAttendanceByEmployeeId(GetAttendanceByEmployeeIdRequest request, ServerCallContext context)
        {
            var Attendance = await _mediator.Send(new GetAttendanceByEmployeeIdQuery(request.EmployeeId), context.CancellationToken);
            if (Attendance == null) return await Task.FromResult(new GetAttendanceByEmployeeIdResponse()
            {
                Empty = new Empty()
            });
            var dateTime = Attendance.Date.ToDateTime(TimeOnly.MaxValue);
            var checkInTime = Timestamp.FromDateTime(Attendance.CheckinTime.ToUniversalTime());
            var checkOutTime = Timestamp.FromDateTime(Attendance.CheckoutTime.GetValueOrDefault().ToUniversalTime());
            var result = new GetAttendanceByEmployeeIdResponse()
            {
                Result = new AttendanceMessage()
                {
                    Id = Attendance.Id,
                    CheckinTime = checkInTime,
                    CheckoutTime = checkOutTime,
                    Date = dateTime.ToUniversalTime().ToTimestamp(),
                    EmployeeId = Attendance.EmployeeId,
                }
            };
            return result;

        }

    }
}
