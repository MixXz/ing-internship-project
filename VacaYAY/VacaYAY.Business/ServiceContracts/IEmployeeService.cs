using System.Security.Claims;
using VacaYAY.Data.DataTransferObjects.Employees;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.ServiceContracts;

public interface IEmployeeService
{
    Task<Employee?> GetCurrent(ClaimsPrincipal claims);
    Task<Employee?> GetById(string id);
    Task<Employee?> GetByIdDetailed(string id);
    Task<IEnumerable<Employee>> GetAll();
    Task<IEnumerable<Employee>> GetByFilters(EmployeeView filters);
    Task<IEnumerable<Employee>> GetWithRemainingDaysOff();
    Task<ServiceResult<Employee>> Register(EmployeeCreate employeeData);
    Task<ServiceResult<Employee>> Edit(EmployeeEdit employeeData);
    Task<ServiceResult<Employee>> Delete(string id);
    Task<int> InsertOldEmployees(string jsonResponse);
    Task<bool> IsAdmin(Employee employee);
    Task<bool> IsAuthorized(ClaimsPrincipal userClaims);
    Task<bool> IsAuthorizedToSee(ClaimsPrincipal userClaims, string authorId);
    EmployeeEdit GetEditDto(Employee employee);
    void RemoveOldDaysOff();
    void AddNewDaysOff(int days);
}
