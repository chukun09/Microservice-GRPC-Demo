using Core.Entites;
using DomainService.Services.EmployeeService;


namespace WebAppDemo.Tests
{
    public class UnitTestDepartment
    {
        private readonly IDepartmentService _departmentService;

        public UnitTestDepartment(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [Fact]
        public async Task Get_All_Department_Returns_List()
        {
            // Act
            var result = await _departmentService.GetAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<DepartmentEntity>>(result);
        }

        [Fact]
        public async Task Get_By_Id_Success_Returns_DepartmentEntity()
        {
            // Arrange
            var id = Guid.Parse("50924dce-5553-4505-a666-b2ed5b946c89");

            // Act
            var result = await _departmentService.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DepartmentEntity>(result);
        }
        [Fact]
        public async Task Get_By_Id_Fail()
        {
            // Arrange: Set up any necessary dependencies and context
            Guid invalidId = Guid.NewGuid(); // Assuming this ID does not exist in the database

            // Act: Perform the operation to be tested
            var result = await _departmentService.GetByIdAsync(invalidId, CancellationToken.None);

            // Assert: Verify that the specified exception is thrown when the action is executed
            Assert.Null(result);
        }

        [Fact]
        public async Task Create_Department_Returns_New_Id()
        {
            // Arrange
            var newDepartment = new DepartmentEntity { Name = "Test Department", Description = "Unit Test" };

            // Act
            var newDepartmentId = await _departmentService.CreateAsync(newDepartment, CancellationToken.None);

            // Assert
            Assert.NotEqual(string.Empty, newDepartmentId.Id);
        }
        [Fact]
        public async Task Update_Department_Succeeds()
        {
            // Arrange
            var idToUpdate = Guid.Parse("50924dce-5553-4505-a666-b2ed5b946c89");
            var updatedDepartment = await _departmentService.GetByIdAsync(idToUpdate, CancellationToken.None);
            updatedDepartment.Name = "Updated Department Unit Test";
            // Act
            await _departmentService.UpdateAsync(updatedDepartment, CancellationToken.None);

            // Assert
            var department = await _departmentService.GetByIdAsync(idToUpdate, CancellationToken.None);
            Assert.NotNull(department);
            Assert.Equal(updatedDepartment.Name, department.Name);
        }

        [Fact]
        public async Task Delete_Department_Succeeds()
        {
            // Arrange
            var idToDelete = Guid.Parse("50924dce-5553-4505-a666-b2ed5b946c89");

            // Act
            await _departmentService.DeleteAsync(idToDelete, CancellationToken.None);

            // Assert
            var department = await _departmentService.GetByIdAsync(idToDelete, CancellationToken.None);
            Assert.Null(department);
        }
    }
}