using Core.Entites;
using Grpc.Core;
using WorkHoursSummaryMicroservice;

namespace WebAppBlazor.Services.WorkHoursSummary
{
    public class WorkHoursSummaryService : IWorkHoursSummaryService
    {
        private readonly WorkHoursSummaryServicer.WorkHoursSummaryServicerClient _workHoursSummaryServicerClient;

        public WorkHoursSummaryService(WorkHoursSummaryServicer.WorkHoursSummaryServicerClient workHoursSummaryServicerClient)
        {
            _workHoursSummaryServicerClient = workHoursSummaryServicerClient;
        }

        public async Task<IEnumerable<WorkHoursSummaryEntity>> GetAllWorkHoursSummary()
        {
            var summaries = new List<WorkHoursSummaryEntity>();
            using var call = _workHoursSummaryServicerClient.GetAllWorkHoursSummary();
            var readTask = Task.Run(async () =>
            {
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    summaries = new List<WorkHoursSummaryEntity>();
                    var summariesMapping = response.WorkHoursSummarys;
                    foreach (var summary in summariesMapping)
                    {
                        var mappingWorkHoursSummary = new WorkHoursSummaryEntity()
                        {
                            Id = summary.Id,
                            EmployeeId = summary.EmployeeId,
                            SummaryDate = DateOnly.FromDateTime(summary.SummaryDate.ToDateTime()),
                            TotalWorkedHours = (short?)summary.TotalWorkedHours,
                        };
                        summaries.Add(mappingWorkHoursSummary);

                    }
                }
            });
            await call.RequestStream.WriteAsync(new Empty());
            await call.RequestStream.CompleteAsync();
            await readTask;
            return summaries;
        }

        public async Task<IEnumerable<WorkHoursSummaryEntity>> GetAllWorkHoursSummaryByEmployee(string employeeId)
        {
            var input = new GetAllWorkHoursSummaryByEmployeeRequest()
            {
                EmployeeId = employeeId
            };
            var summaries = await _workHoursSummaryServicerClient.GetAllWorkHoursSummaryByEmployeeAsync(input);
            var result = new List<WorkHoursSummaryEntity>();
            foreach (var summary in summaries.WorkHoursSummarys)
            {
                var entity = new WorkHoursSummaryEntity()
                {
                    EmployeeId = summary.EmployeeId,
                    Id = summary.Id,
                    SummaryDate = DateOnly.FromDateTime(summary.SummaryDate.ToDateTime()),
                    TotalWorkedHours = (short?)summary.TotalWorkedHours,
                };
                result.Add(entity);
            }
            return result;
        }
    }
}
