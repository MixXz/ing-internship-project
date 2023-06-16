using SendGrid;

namespace VacaYAY.Business.Contracts;

public interface IEmailSenderService
{
    Task<Response?> SendEmail(string emailTo, string subject, string content);
}
