using Quartz;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Helpers;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Business.Jobs;

public class NotifyOfRequestStatusJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotifierService _notifierService;

    public NotifyOfRequestStatusJob(
        IUnitOfWork unitOfWork,
        INotifierService notifierService)
    {
        _unitOfWork = unitOfWork;
        _notifierService = notifierService;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var requests = await _unitOfWork.Request.GetRequestsWhereAuthorIsntNotified();

        foreach (var request in requests)
        {
            EmailTemplateType type = request.NotificationStatus switch
            {
                NotificationStatus.NotNotifiedOfCreation => EmailTemplateType.Created,
                NotificationStatus.NotNotifiedOfChange => EmailTemplateType.Edited,
                NotificationStatus.NotNotifiedOfDeletion => EmailTemplateType.Deleted,
                NotificationStatus.NotNotifiedOfReponse when request.Status is RequestStatus.Approved => EmailTemplateType.Approved,
                NotificationStatus.NotNotifiedOfReponse => EmailTemplateType.Rejected,
                _ => throw new InvalidOperationException("Invalid notification status.")
            };

            var isNotified = await _notifierService.NotifyEmployee(RequestEmailTemplates.GetEmail(
                type,
                request.CreatedBy,
                request));

            if (isNotified)
            {
                request.NotificationStatus = NotificationStatus.Notified;

                _unitOfWork.Request.Update(request);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
