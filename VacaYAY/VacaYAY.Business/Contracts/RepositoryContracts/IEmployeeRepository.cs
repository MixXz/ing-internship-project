using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Contracts.RepositoryContracts;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
    Task<Employee?> GetById(string id);
    Task<IEnumerable<Employee>> GetByFilters(EmployeeView filters);
    Task<Employee?> GetCurrent(ClaimsPrincipal userClaims);
    Task<IEnumerable<Employee>> GetAdmins();
    Task<IEnumerable<Employee>> GetWithRemainingDaysOff();
    Task<ServiceResult<Employee>> Insert(EmployeeCreate data, Position position);
    Task<ServiceResult<Employee>> Update(string id, EmployeeEdit employeeData);
    void RemoveOldDaysOff();
    Task<IdentityResult> Delete(string id);
    Task<bool> IsAdmin(Employee employee);
    bool IsAdmin(ClaimsPrincipal userClaims);
    Task<bool> isAuthorized(ClaimsPrincipal userClaims);
    Task<bool> isAuthorizedToSee(ClaimsPrincipal userClaims, string authorId);
    Task<IdentityResult> SetAdminPrivileges(Employee employee, bool makeAdmin);
    List<EmployeeOld>? ExtractEmployeeData(string jsonResponse);
}
