using VacaYAY.Data.DataTransferObjects.Requests;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Data.RepositoryContracts;

public interface IRequestRepository : IRepositoryBase<Request>
{
    Task<IEnumerable<Request>> GetByUser(string userId);
    Task<IEnumerable<Request>> GetByFilters(RequestView filters);
    Task<IEnumerable<Request>> GetRequestsWhereAuthorIsntNotified();
    Task<List<CustomValidationResult>> ValidateOnCreate(RequestCreate reqData, Employee user);
    Task<List<CustomValidationResult>> ValidateOnEdit(RequestEdit reqData, Employee user);
    (int removeFromOldDays, int removeFromNewDays) GetDaysOffDistribution(int empOldDays, int empNewDays, Request request);
}

