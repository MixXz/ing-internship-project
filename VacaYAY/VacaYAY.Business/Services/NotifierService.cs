using VacaYAY.Business.Contracts;
using VacaYAY.Business.Contracts.ServiceContracts;

namespace VacaYAY.Business.Services;

public class NotifierService : INotifierSerivice
{
    private readonly IEmailSenderService _emailSender;
    private readonly IUnitOfWork _unitOfWork;
    public NotifierService(
        IEmailSenderService emailSender,
        IUnitOfWork unitOfWork)
    {
        _emailSender = emailSender;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> NotifyEmployee((string? email, string subject, string content) message, bool withPdf = false)
    {
        (string? email, string subject, string content) = message;

        if (email is null)
        {
            return false;
        }

        //var response = withPdf ?
        //    await _emailSender.SendEmailWithPdf(email, subject, content)
        //    :
        //    await _emailSender.SendEmail(email, subject, content);

        //if (response is null)
        //{
        //    return false;
        //}

        return true;
    }

    public async Task NotifyHRTeam((string subject, string content) message)
    {
        var hrTeam = await _unitOfWork.Employee.GetAdmins();

        (string subject, string content) = message;

        //foreach (var hr in hrTeam)
        //{
        //    if (hr.Email is null)
        //    {
        //        continue;
        //    }

        //    await _emailSender.SendEmail(hr.Email, subject, content);
        //}
    }
}
