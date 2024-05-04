using Core.Entites;
using EmployeeMicroservice;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using WebAppBlazor.Pages.Application;
using Empty = EmployeeMicroservice.Empty;

namespace WebAppBlazor.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly Employeer.EmployeerClient _employeerClient;

        public EmployeeService(Employeer.EmployeerClient employeerClient)
        {
            _employeerClient = employeerClient;
        }

        public async Task<EmployeeEntity> AddAsync(EmployeeEntity entity)
        {
            var newEmployee = new AddEmployeeRequest()
            {
                Address = entity.Address,
                DateOfBirth = entity.DateOfBirth.GetValueOrDefault().ToTimestamp(),
                DepartmentId = entity.DepartmentId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Position = entity.Position,
                UserId = entity.UserId
            };
            await _employeerClient.AddEmployeeAsync(newEmployee);
            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteRequest = new DeleteEmployeeRequest()
            {
                Id = id
            };
            _ = await _employeerClient.DeleteEmployeeAsync(deleteRequest);
            return true;
        }

        public async Task<IEnumerable<EmployeeEntity>> GetAllAsync()
        {
            var employees = new List<EmployeeEntity>();
            using var call = _employeerClient.GetAllEmployee();
            var readTask = Task.Run(async () =>
            {
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    employees = new List<EmployeeEntity>();
                    var employeesMapping = response.Employees;
                    foreach (var employee in employeesMapping)
                    {
                        var mappingEmployee = new EmployeeEntity()
                        {
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Position = employee.Position,
                            UserId = employee.UserId,
                            Address = employee.Address,
                            DateOfBirth = employee.DateOfBirth?.ToDateTime(),
                            Id = employee.Id,
                            DepartmentId = employee.DepartmentId,
                            Department = new DepartmentEntity()
                            {
                                Name = employee.Department
                            }
                        };
                        employees.Add(mappingEmployee);

                    }
                }
            });
            // while (true)
            // {
            await call.RequestStream.WriteAsync(new Empty());
            // };
            await call.RequestStream.CompleteAsync();
            await readTask;
            return employees;

        }

        public async Task<EmployeeEntity> GetEmployeeByUserId(string userId)
        {
            var employee = await _employeerClient.GetEmployeeByUserIdAsync(new GetEmployeeByUserIdRequest()
            {
                UserId = userId
            });
            var result = new EmployeeEntity()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Position = employee.Position,
                UserId = employee.UserId,
                Address = employee.Address,
                DateOfBirth = employee.DateOfBirth?.ToDateTime(),
                Id = employee.Id,
                DepartmentId = employee.DepartmentId,
                Department = new DepartmentEntity()
                {
                    Name = employee.Department
                }
            };
            return result;
        }

        public async Task<EmployeeEntity> UpdateAsync(EmployeeEntity entity)
        {
            var employeeUpdateEntity = new EmployeeMessage
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Position = entity.Position,
                UserId = entity.UserId,
                Address = entity.Address,
                DateOfBirth = entity.DateOfBirth.GetValueOrDefault().AddHours(7).ToUniversalTime().ToTimestamp(),
                Id = entity.Id,
                DepartmentId = entity.DepartmentId,
            };
            var employeeInput = new EmployeeRequest()
            {
                Employee = employeeUpdateEntity
            };
            await _employeerClient.UpdateEmployeeAsync(employeeInput);
            return entity;
        }
    }
}
