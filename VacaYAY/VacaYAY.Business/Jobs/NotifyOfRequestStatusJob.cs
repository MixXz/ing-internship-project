using Quartz;
using VacaYAY.Business.ServiceContracts;

namespace VacaYAY.Business.Jobs;

public class NotifyOfRequestStatusJob : IJob
{
    private readonly IRequestService _requestService;

    public NotifyOfRequestStatusJob(
        IRequestService requestService)
    {
        _requestService = requestService;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        await _requestService.NotifyUninformed();
    }
}
