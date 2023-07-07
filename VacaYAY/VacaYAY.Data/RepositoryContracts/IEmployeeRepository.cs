using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VacaYAY.Data.DataTransferObjects.Employees;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Data.RepositoryContracts;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
    Task<Employee?> GetById(string id);
    Task<Employee?> GetDetailedById(string id);
    Task<IEnumerable<Employee>> GetByFilters(EmployeeView filters);
    Task<Employee?> GetCurrent(ClaimsPrincipal userClaims);
    Task<IEnumerable<Employee>> GetAdmins();
    Task<IEnumerable<Employee>> GetWithRemainingDaysOff();
    Task<ServiceResult<Employee>> Insert(EmployeeCreate data, Position position);
    Task<ServiceResult<Employee>> Update(EmployeeEdit employeeData);
    void RemoveOldDaysOff();
    void AddNewDaysOff(int numOfDays);
    Task<ServiceResult<Employee>> Delete(string id);
    Task<bool> IsAdmin(Employee employee);
    bool IsAdmin(ClaimsPrincipal userClaims);
    Task<bool> IsAuthorized(ClaimsPrincipal userClaims);
    Task<bool> IsAuthorizedToSee(ClaimsPrincipal userClaims, string authorId);
    Task<IdentityResult> SetAdminPrivileges(Employee employee, bool makeAdmin);
    List<EmployeeOld>? ExtractEmployeeData(string jsonResponse);
}
