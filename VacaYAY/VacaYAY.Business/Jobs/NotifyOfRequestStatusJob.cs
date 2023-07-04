using Quartz;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Helpers;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Business.Jobs;

public class NotifyOfRequestStatusJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotifierSerivice _notifierService;

    public NotifyOfRequestStatusJob(
        IUnitOfWork unitOfWork,
        INotifierSerivice notifierSerivice)
    {
        _unitOfWork = unitOfWork;
        _notifierService = notifierSerivice;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var requests = await _unitOfWork.Request.GetRequestsWhereAuthorIsntNotified();

        foreach (var request in requests)
        {
            RequestEmailTemplates templates = new(request.CreatedBy, request);
            bool isNotified = false;

            switch (request.NotificationStatus)
            {
                case NotificationStatus.NotNotifiedOfCreation:
                    isNotified = await _notifierService.NotifyEmployee(templates.Created);
                    break;
                case NotificationStatus.NotNotifiedOfChange:
                    isNotified = await _notifierService.NotifyEmployee(templates.Edited);
                    break;
                case NotificationStatus.NotNotifiedOfDeletion:
                    isNotified = await _notifierService.NotifyEmployee(templates.Deleted);
                    break;
                case NotificationStatus.NotNotifiedOfReponse:
                    isNotified = await _notifierService
                        .NotifyEmployee(request.Status is RequestStatus.Approved ?
                        templates.Approved
                        :
                        templates.Rejected);
                    break;
                default:
                    break;
            }

            if (isNotified)
            {
                request.NotificationStatus = NotificationStatus.Notified;

                _unitOfWork.Request.Update(request);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
