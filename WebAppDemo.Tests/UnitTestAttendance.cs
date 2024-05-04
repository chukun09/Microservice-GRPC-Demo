using Core.Entites;
using DomainService.Services.AttendanceService;

namespace WebAppDemo.Tests
{
    public class UnitTestAttendance
    {
        private readonly IAttendanceService _attendanceService;

        public UnitTestAttendance(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [Fact]
        public async Task Get_All_Attendance_Returns_List()
        {
            // Act
            var result = await _attendanceService.GetAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<AttendanceEntity>>(result);
        }

        [Fact]
        public async Task Get_By_Id_Success_Returns_AttendanceEntity()
        {
            // Arrange
            var id = Guid.Parse("c8081bd8-2438-457b-81ac-f93b824d5458");

            // Act
            var result = await _attendanceService.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AttendanceEntity>(result);
        }
        [Fact]
        public async Task Get_By_Id_Fail()
        {
            // Arrange: Set up any necessary dependencies and context
            Guid invalidId = Guid.NewGuid(); // Assuming this ID does not exist in the database

            // Act: Perform the operation to be tested
            var result = await _attendanceService.GetByIdAsync(invalidId, CancellationToken.None);

            // Assert: Verify that the specified exception is thrown when the action is executed
            Assert.Null(result);
        }

        [Fact]
        public async Task Create_Attendance_Returns_New_Id()
        {
            // Arrange
            var newAttendance = new AttendanceEntity { CheckinTime = DateTime.Now.AddHours(-7).ToUniversalTime(), CheckoutTime = DateTime.Now.ToUniversalTime(), Date = DateOnly.FromDateTime(DateTime.Now.AddHours(7).ToUniversalTime()), EmployeeId = "5f458681-fa38-42de-8529-42b6b85e7b93" };

            // Act
            var newAttendanceId = await _attendanceService.CreateAsync(newAttendance, CancellationToken.None);

            // Assert
            Assert.NotEqual(string.Empty, newAttendanceId.Id);
        }
        [Fact]
        public async Task Update_Attendance_Succeeds()
        {
            // Arrange
            var idToUpdate = Guid.Parse("c8081bd8-2438-457b-81ac-f93b824d5458");
            var updatedAttendance = await _attendanceService.GetByIdAsync(idToUpdate, CancellationToken.None);
            updatedAttendance.CheckinTime = DateTime.MinValue.ToUniversalTime();
            // Act
            await _attendanceService.UpdateAsync(updatedAttendance, CancellationToken.None);

            // Assert
            var attendance = await _attendanceService.GetByIdAsync(idToUpdate, CancellationToken.None);
            Assert.NotNull(attendance);
            Assert.Equal(updatedAttendance.CheckinTime, attendance.CheckinTime);
        }

        [Fact]
        public async Task Delete_Attendance_Succeeds()
        {
            // Arrange
            var idToDelete = Guid.Parse("c8081bd8-2438-457b-81ac-f93b824d5458");

            // Act
            await _attendanceService.DeleteAsync(idToDelete, CancellationToken.None);

            // Assert
            var attendance = await _attendanceService.GetByIdAsync(idToDelete, CancellationToken.None);
            Assert.Null(attendance);
        }
    }
}