using DomainService.Events.Queries;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using WorkHoursSummaryMicroservice;

namespace EmployeeMicroservice.Services
{
    public class WorkHoursSummaryService : WorkHoursSummaryServicer.WorkHoursSummaryServicerBase
    {
        private readonly ILogger<WorkHoursSummaryService> _logger;
        private readonly IMediator _mediator;

        public WorkHoursSummaryService(ILogger<WorkHoursSummaryService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        /// <summary>
        /// Get All Work Hours Summary 
        ///  
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task GetAllWorkHoursSummary(IAsyncStreamReader<WorkHoursSummaryMicroservice.Empty> requestStream, IServerStreamWriter<GetAllWorkHoursSummaryReply> responseStream, ServerCallContext context)
        {

            await foreach (var request in requestStream.ReadAllAsync())
            {
                var summaries = await _mediator.Send(new GetAllWorkHoursSummaryQuery(), context.CancellationToken);
                var result = new GetAllWorkHoursSummaryReply();
                foreach (var entity in summaries)
                {
                    var dateSummary = entity.SummaryDate.ToDateTime(TimeOnly.MaxValue).ToUniversalTime().ToTimestamp();
                    //Map request message to object
                    var reply = new WorkHoursSummary()
                    {
                        EmployeeId = entity.EmployeeId,
                        Id = entity.Id,
                        SummaryDate = dateSummary,
                        TotalWorkedHours = (int)(entity.TotalWorkedHours ?? 0),

                    };
                    result.WorkHoursSummarys.Add(reply);
                }
                await responseStream.WriteAsync(result);
            }
        }
        /// <summary>
        /// Get All WorkHours By Employee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GetAllWorkHoursSummaryReply> GetAllWorkHoursSummaryByEmployee(GetAllWorkHoursSummaryByEmployeeRequest request, ServerCallContext context)
        {
            var summaries = await _mediator.Send(new GetAllWorkHoursSummaryByEmployeeIdQuery(request.EmployeeId), context.CancellationToken);
            var result = new GetAllWorkHoursSummaryReply();
            foreach (var entity in summaries)
            {
                var dateSummary = entity.SummaryDate.ToDateTime(TimeOnly.MaxValue).ToUniversalTime().ToTimestamp();
                //Map request message to object
                var reply = new WorkHoursSummary()
                {
                    EmployeeId = entity.EmployeeId,
                    Id = entity.Id,
                    SummaryDate = dateSummary,
                    TotalWorkedHours = (int)(entity.TotalWorkedHours ?? 0),
                };
                result.WorkHoursSummarys.Add(reply);
            }
            return result;
        }
        /// <summary>
        /// Don't need
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<WorkHoursSummaryReply> AddWorkHoursSummary(AddWorkHoursSummaryRequest request, ServerCallContext context)
        {
            return base.AddWorkHoursSummary(request, context);
        }
    }
}
