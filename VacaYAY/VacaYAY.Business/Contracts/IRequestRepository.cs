using System.ComponentModel.DataAnnotations;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Contracts;

public interface IRequestRepository : IRepositoryBase<Request>
{
    Task<IEnumerable<Request>> GetByUser(string userId);
    Task<IEnumerable<Request>> GetByFilters(RequestView filters);
    Task<IEnumerable<Request>> GetRequestsWhereAuthorIsntNotified();
    Task<List<CustomValidationResult>> ValidateOnCreate(RequestCreate reqData, Employee user);
    Task<List<CustomValidationResult>> ValidateOnEdit(RequestEdit reqData, Employee user);
}

