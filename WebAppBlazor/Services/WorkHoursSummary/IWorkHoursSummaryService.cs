using Core.Entites;

namespace WebAppBlazor.Services.WorkHoursSummary
{
    public interface IWorkHoursSummaryService
    {
        Task<IEnumerable<WorkHoursSummaryEntity>> GetAllWorkHoursSummary();
        Task<IEnumerable<WorkHoursSummaryEntity>> GetAllWorkHoursSummaryByEmployee(string employeeId);
    }
}
