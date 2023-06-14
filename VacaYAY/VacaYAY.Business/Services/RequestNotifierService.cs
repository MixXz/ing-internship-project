using Microsoft.Extensions.Configuration;
using VacaYAY.Business.Contracts;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Services;

public class RequestNotifierService : IRequestNotifierSerivice
{
    private readonly IEmailSenderService _emailSender;
    private readonly IUnitOfWork _unitOfWork;
    public RequestNotifierService(
        IEmailSenderService emailSender,
        IUnitOfWork unitOfWork)
    {
        _emailSender = emailSender;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> NotifyEmployee((string? email, string subject, string content) message)
    {
        (string? email, string subject, string content) = message;

        if (email is null)
        {
            return false;
        }

        var response = await _emailSender.SendEmail(email, subject, content);

        if (response is null)
        {
            return false;
        }

        return response.IsSuccessStatusCode;
    }

    public async Task NotifyHRTeam((string subject, string content) message)
    {
        var hrTeam = await _unitOfWork.Employee.GetAdmins();

        (string subject, string content) = message;

        foreach (var hr in hrTeam)
        {
            if (hr.Email is null)
            {
                continue;
            }

            await _emailSender.SendEmail(hr.Email, subject, content);
        }
    }
}
