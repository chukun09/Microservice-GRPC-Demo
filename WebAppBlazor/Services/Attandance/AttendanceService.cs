using AttendanceMicroservice;
using Core.Entites;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Grpc.Core;

namespace WebAppBlazor.Services.Attandance
{
    public class AttendanceService : IAttendanceService
    {
        private readonly Attendancer.AttendancerClient _attendanceClient;

        public AttendanceService(Attendancer.AttendancerClient attendanceClient)
        {
            _attendanceClient = attendanceClient;
        }

        public async Task<AttendanceEntity> AddAsync(AttendanceEntity entity)
        {

            var attendanceRequest = new AddAttendanceRequest()
            {
                CheckinTime = entity.CheckinTime.ToUniversalTime().ToTimestamp(),
                CheckoutTime = entity.CheckoutTime.GetValueOrDefault().ToUniversalTime().ToTimestamp(),
                Date = entity.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime().ToTimestamp(),
                EmployeeId = entity.EmployeeId,
            };
            _ = await _attendanceClient.AddAttendanceAsync(attendanceRequest);
            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteRequest = new DeleteAttendanceRequest()
            {
                EmployeeId = id
            };
            _ = await _attendanceClient.DeleteAttendanceAsync(deleteRequest);
            return true;
        }

        public async Task<IEnumerable<AttendanceEntity>> GetAllAsync()
        {
            var attendances = new List<AttendanceEntity>();
            using var call = _attendanceClient.GetAllAttendance();
            var readTask = Task.Run(async () =>
            {
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    attendances = new List<AttendanceEntity>();
                    var attendancesMapping = response.Attendances;
                    foreach (var attendance in attendancesMapping)
                    {
                        var mappingAttendance = new AttendanceEntity()
                        {
                            Id = attendance.Id,
                            EmployeeId = attendance.EmployeeId,
                            Date = DateOnly.FromDateTime(attendance.Date.ToDateTime()),
                            CheckinTime = attendance.CheckinTime.ToDateTime(),
                            CheckoutTime = attendance.CheckoutTime.ToDateTime(),
                        };
                        attendances.Add(mappingAttendance);

                    }
                }
            });
            // while (true)
            // {
            await call.RequestStream.WriteAsync(new AttendanceMicroservice.Empty());
            // };
            await call.RequestStream.CompleteAsync();
            await readTask;
            return attendances;
        }

        public async Task<AttendanceEntity> UpdateAsync(AttendanceEntity entity)
        {
            var attendanceUpdateEntity = new AttendanceMessage
            {
                Id = entity.Id,
                CheckinTime = Timestamp.FromDateTime(entity.CheckinTime),
                CheckoutTime = entity.CheckoutTime == null ? null : Timestamp.FromDateTime((System.DateTime)entity.CheckoutTime),
                Date = entity.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime().ToTimestamp(),
                EmployeeId = entity.EmployeeId,

            };
            var attendanceInput = new AttendanceRequest()
            {
                Attendance = attendanceUpdateEntity
            };
            await _attendanceClient.UpdateAttendanceAsync(attendanceInput);
            return entity;
        }
    }
}
