using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using VacaYAY.Business.ServiceContracts;

namespace VacaYAY.Business.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly IConfiguration _config;
    private readonly ISendGridClient _sendGridClient;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailSenderService(
        IConfiguration config,
        ISendGridClient sendGridClient)
    {
        _config = config;
        _sendGridClient = sendGridClient;
        _fromEmail = _config["EmailSenderSettings:FromEmail"]!;
        _fromName = _config["EmailSenderSettings:FromName"]!;
    }

    public async Task<Response?> SendEmail(string emailTo, string subject, string content)
    {
        var message = new SendGridMessage()
        {
            From = new EmailAddress(_fromEmail, _fromName),
            Subject = subject,
            HtmlContent = content,
        };
        message.AddTo(emailTo);

        return await _sendGridClient.SendEmailAsync(message);
    }

    public async Task<Response?> SendEmailWithPdf(string emailTo, string subject, string content)
    {
        var message = new SendGridMessage()
        {
            From = new EmailAddress(_fromEmail, _fromName),
            Subject = subject,
            PlainTextContent = subject,
        };
        message.AddTo(emailTo);

        var pdf = new HtmlToPdf();
        var pdfBytes = pdf.RenderHtmlAsPdf(content).BinaryData;

        message.AddAttachment("Response.pdf", Convert.ToBase64String(pdfBytes), "application/pdf");

        return await _sendGridClient.SendEmailAsync(message);
    }
}
