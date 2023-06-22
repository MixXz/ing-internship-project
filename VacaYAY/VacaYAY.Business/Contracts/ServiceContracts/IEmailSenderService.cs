﻿using SendGrid;

namespace VacaYAY.Business.Contracts.ServiceContracts;

public interface IEmailSenderService
{
    Task<Response?> SendEmail(string emailTo, string subject, string content);
    Task<Response?> SendEmailWithPdf(string emailTo, string subject, string content);

}
