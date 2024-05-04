using Core.Entites;
using DomainService.Services.EmployeeService;

namespace WebAppDemo.Tests
{
    public class UnitTestEmployee
    {
        private readonly IEmployeeService _employeeService;

        public UnitTestEmployee(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Fact]
        public async Task Get_All_Employee_Returns_List()
        {
            // Act
            var result = await _employeeService.GetAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<EmployeeEntity>>(result);
        }

        [Fact]
        public async Task Get_By_Id_Success_Returns_EmployeeEntity()
        {
            // Arrange
            var id = Guid.Parse("5f458681-fa38-42de-8529-42b6b85e7b93");

            // Act
            var result = await _employeeService.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<EmployeeEntity>(result);
        }
        [Fact]
        public async Task Get_By_Id_Fail()
        {
            // Arrange: Set up any necessary dependencies and context
            Guid invalidId = Guid.NewGuid(); // Assuming this ID does not exist in the database

            // Act: Perform the operation to be tested
            var result = await _employeeService.GetByIdAsync(invalidId, CancellationToken.None);

            // Assert: Verify that the specified exception is thrown when the action is executed
            Assert.Null(result);
        }

        [Fact]
        public async Task Create_Employee_Returns_New_Id()
        {
            // Arrange
            var newEmployee = new EmployeeEntity { FirstName = "Demo", LastName = "Unit Test", UserId = new Guid().ToString(), DateOfBirth = DateTime.Now.ToUniversalTime() };

            // Act
            var newEmployeeId = await _employeeService.CreateAsync(newEmployee, CancellationToken.None);

            // Assert
            Assert.NotEqual(string.Empty, newEmployeeId.Id);
        }
        [Fact]
        public async Task Update_Employee_Succeeds()
        {
            // Arrange
            var idToUpdate = Guid.Parse("5f458681-fa38-42de-8529-42b6b85e7b93");
            var updatedEmployee = await _employeeService.GetByIdAsync(idToUpdate, CancellationToken.None);
            updatedEmployee.FirstName = "Updated Employee";
            // Act
            await _employeeService.UpdateAsync(updatedEmployee, CancellationToken.None);

            // Assert
            var employee = await _employeeService.GetByIdAsync(idToUpdate, CancellationToken.None);
            Assert.NotNull(employee);
            Assert.Equal(updatedEmployee.FirstName, employee.FirstName);
        }

        [Fact]
        public async Task Delete_Employee_Succeeds()
        {
            // Arrange
            var idToDelete = Guid.Parse("5f458681-fa38-42de-8529-42b6b85e7b93");

            // Act
            await _employeeService.DeleteAsync(idToDelete, CancellationToken.None);

            // Assert
            var employee = await _employeeService.GetByIdAsync(idToDelete, CancellationToken.None);
            Assert.Null(employee);
        }
    }
}