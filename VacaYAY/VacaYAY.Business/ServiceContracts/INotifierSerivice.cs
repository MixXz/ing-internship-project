namespace VacaYAY.Business.ServiceContracts;

public interface INotifierSerivice
{
    Task<bool> NotifyEmployee((string? email, string subject, string content) message, bool withPdf = false);
    Task NotifyHRTeam((string subject, string content) message);
}
