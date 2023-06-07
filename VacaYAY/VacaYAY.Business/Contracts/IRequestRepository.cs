using System.ComponentModel.DataAnnotations;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Contracts;

public interface IRequestRepository : IRepositoryBase<Request>
{
    Task<IEnumerable<Request>> GetByUser(string userId);
    Task<IEnumerable<Request>> GetByFilters(RequestView filters);
    List<CustomValidationResult> ValidateOnCreate(RequestCreate regData, Employee user);
}

