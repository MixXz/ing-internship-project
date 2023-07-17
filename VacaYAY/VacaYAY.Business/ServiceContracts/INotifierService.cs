namespace VacaYAY.Business.ServiceContracts;

public interface INotifierService
{
    Task<bool> NotifyEmployee((string? email, string subject, string content) message, bool withPdf = false);
    Task NotifyHRTeam((string? email, string subject, string content) message);
}
