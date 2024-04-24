using Core.Entites;

namespace WebAppBlazor.Services.Department
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentEntity>> GetAllAsync();
        Task<DepartmentEntity> AddAsync(DepartmentEntity entity);
        Task<DepartmentEntity> UpdateAsync(DepartmentEntity entity);
        Task<bool> DeleteAsync(string id);
    }
}
