using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using VacaYAY.Business.Contracts.ServiceContracts;

namespace VacaYAY.Business.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly IConfiguration _config;
    private readonly ISendGridClient _sendGridClient;

    public EmailSenderService(
        IConfiguration config,
        ISendGridClient sendGridClient)
    {
        _config = config;
        _sendGridClient = sendGridClient;
    }

    public async Task<Response?> SendEmail(string emailTo, string subject, string content)
    {
        string fromEmail = _config["EmailSenderSettings:FromEmail"]!;
        string fromName = _config["EmailSenderSettings:FromName"]!;

        var message = new SendGridMessage()
        {
            From = new EmailAddress(fromEmail, fromName),
            Subject = subject,
            HtmlContent = content
        };
        message.AddTo(emailTo);

        return await _sendGridClient.SendEmailAsync(message);
    }
}
