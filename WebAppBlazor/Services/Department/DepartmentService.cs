using Core.Entites;
using DepartmentMicroservice;
using Grpc.Core;

namespace WebAppBlazor.Services.Department
{
    public class DepartmentService : IDepartmentService
    {
        private readonly Departmenter.DepartmenterClient _departmenterClient;

        public DepartmentService(Departmenter.DepartmenterClient departmenterClient)
        {
            _departmenterClient = departmenterClient;
        }

        public async Task<DepartmentEntity> AddAsync(DepartmentEntity entity)
        {
            var departmentRequest = new AddDepartmentRequest()
            {
                Description = entity.Description,
                Name = entity.Name
            };
            _ = await _departmenterClient.AddDepartmentAsync(departmentRequest);
            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteRequest = new DeleteDepartmentRequest()
            {
                Id = id
            };
            _ = await _departmenterClient.DeleteDepartmentAsync(deleteRequest);
            return true;
        }

        public async Task<IEnumerable<DepartmentEntity>> GetAllAsync()
        {
            var departments = new List<DepartmentEntity>();
            using var call = _departmenterClient.GetAllDepartment();
            var readTask = Task.Run(async () =>
            {
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    departments = new List<DepartmentEntity>();
                    var departmentsMapping = response.Departments;
                    foreach (var department in departmentsMapping)
                    {
                        var mappingDepartment = new DepartmentEntity()
                        {
                            Name = department.Name,
                            Description = department.Description,
                            Id = department.Id,
                        };
                        departments.Add(mappingDepartment);

                    }
                }
            });
            // while (true)
            // {
            await call.RequestStream.WriteAsync(new Empty());
            // };
            await call.RequestStream.CompleteAsync();
            await readTask;
            return departments;
        }

        public async Task<DepartmentEntity> UpdateAsync(DepartmentEntity entity)
        {
            var departmentUpdateEntity = new DepartmentMessage
            {
                Name = entity.Name,
                Description = entity.Description,
                Id = entity.Id,
            };
            var departmentInput = new DepartmentRequest()
            {
                Department = departmentUpdateEntity
            };
            await _departmenterClient.UpdateDepartmentAsync(departmentInput);
            return entity;
        }
    }
}
