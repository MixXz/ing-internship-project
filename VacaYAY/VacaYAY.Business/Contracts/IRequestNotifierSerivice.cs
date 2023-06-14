using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Contracts;

public interface IRequestNotifierSerivice
{
    Task<bool> NotifyEmployee((string? email, string subject, string content) message);
    Task NotifyHRTeam((string subject, string content) message);
}
