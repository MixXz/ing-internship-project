using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Contracts;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
    Task<Employee?> GetById(string id);
    Task<IEnumerable<Employee>> GetByFilters(string? searchInput, DateTime? startDate, DateTime? endDate);
    Task<Employee?> GetCurrent(ClaimsPrincipal claims);
    Task<IdentityResult> Insert(Employee employee, string password);
    Task<IdentityResult> Update(string id, EmployeeEdit employeeData);
    Task<IdentityResult> Delete(string id);
    Task<bool> isAdmin(Employee employee);
    Task<bool> isAuthorized(ClaimsPrincipal claims);
    Task<IdentityResult> SetAdminPrivileges(Employee employee, bool makeAdmin);
}
