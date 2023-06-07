using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Contracts;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
    Task<Employee?> GetById(string id);
    Task<IEnumerable<Employee>> GetByFilters(string? searchInput, DateTime? startDate, DateTime? endDate);
    Task<Employee?> GetCurrent(ClaimsPrincipal userClaims);
    Task<IdentityResult> Insert(Employee employee, string password);
    Task<IdentityResult> Update(string id, EmployeeEdit employeeData);
    Task<IdentityResult> Delete(string id);
    Task<bool> IsAdmin(Employee employee);
    bool IsAdmin(ClaimsPrincipal userClaims);
    Task<bool> isAuthorized(ClaimsPrincipal userClaims);
    Task<bool> isAuthorizedToSee(ClaimsPrincipal userClaims, string authorId);
    Task<IdentityResult> SetAdminPrivileges(Employee employee, bool makeAdmin);
}
