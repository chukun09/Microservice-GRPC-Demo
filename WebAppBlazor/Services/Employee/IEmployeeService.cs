using Core.Entites;

namespace WebAppBlazor.Services.Employee
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeEntity>> GetAllAsync();
        Task<EmployeeEntity> AddAsync(EmployeeEntity entity);
        Task<EmployeeEntity> UpdateAsync(EmployeeEntity entity);
        Task<bool> DeleteAsync(string id);
    }
}
