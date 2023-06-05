using Microsoft.AspNetCore.Identity;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Contracts;

public interface IEmployeeRepository
{
    Task<Employee?> GetById(string id);
    Task<IEnumerable<Employee>> GetAll();
    Task<IEnumerable<Employee>> GetByFilters(string? searchInput, DateTime? startDate, DateTime? endDate);
    Task<IdentityResult> Insert(Employee employee, string password);
    Task<IdentityResult> Update(string id, EmployeeEdit employeeData);
    Task<IdentityResult> Delete(string id);
    Task<bool> isAdmin(Employee employee);
    Task<IdentityResult> SetAdminPrivileges(Employee employee, bool makeAdmin);
}
