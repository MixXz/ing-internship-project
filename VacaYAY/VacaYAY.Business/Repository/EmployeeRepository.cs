using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Business.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly Context _context;
    private readonly UserManager<Employee> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserStore<Employee> _userStore;
    private readonly IUserEmailStore<Employee> _emailStore;

    public EmployeeRepository(
        Context context,
        UserManager<Employee> userManager,
        RoleManager<IdentityRole> roleManager,
        IUserStore<Employee> userStore)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = (IUserEmailStore<Employee>)_userStore;
    }

    public async Task<Employee?> GetById(string id)
    {
        return await _context.Employees
                        .Include(e => e.Position)
                        .Where(e => e.Id == id)
                        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Employee>> GetAll()
    {
        return await _context.Employees
                        .Include(e => e.Position)
                        .OrderByDescending(e => e.InsertDate)
                        .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByFilters(string? searchInput, DateTime? startDate, DateTime? endDate)
    {
        if ((string.IsNullOrEmpty(searchInput) || string.IsNullOrWhiteSpace(searchInput))
            && startDate is null
            && endDate is null)
        {
            return await GetAll();
        }

        var employees = _context.Employees
                        .Include(e => e.Position)
                        .AsQueryable();

        if (!string.IsNullOrEmpty(searchInput))
        {
            var tokens = searchInput.Split(' ');

            employees = employees
                        .Where(e =>
                            e.FirstName.Contains(tokens[0])
                            || (tokens.Count() > 1 && e.FirstName.Contains(tokens[1]))
                            || e.LastName.Contains(tokens[0])
                            || (tokens.Count() > 1 && e.LastName.Contains(tokens[1])));
        }

        if (startDate is not null)
        {
            employees = employees.Where(e => e.EmployeeStartDate >= startDate);
        }

        if (endDate is not null)
        {
            employees = employees.Where(e => e.EmployeeEndDate <= endDate);
        }

        return await employees
                    .OrderByDescending(e => e.InsertDate)
                    .ToListAsync();
    }

    public async Task<IdentityResult> Insert(Employee employee, string password)
    {
        if (employee is null || string.IsNullOrEmpty(password))
        {
            return IdentityResult.Failed();
        }

        var employeeInDb = await _userManager.FindByEmailAsync(employee.Email!);

        if (employeeInDb is not null)
        {
            return IdentityResult.Failed();
        }

        await _userStore.SetUserNameAsync(employee, employee.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(employee, employee.Email, CancellationToken.None);

        return await _userManager.CreateAsync(employee, password);
    }

    public async Task<IdentityResult> Update(string id, EmployeeEdit employeeData)
    {
        var employeeEntity = await GetById(id);

        if (employeeEntity is null)
        {
            return IdentityResult.Failed();
        }

        if (employeeEntity.Email != employeeData.Email)
        {
            await _userManager.SetEmailAsync(employeeEntity, employeeData.Email);
        }

        employeeEntity.FirstName = employeeData.FirstName;
        employeeEntity.LastName = employeeData.LastName;
        employeeEntity.Address = employeeData.Address;
        employeeEntity.IDNumber = employeeData.IDNumber;
        employeeEntity.DaysOfNumber = employeeData.DaysOfNumber;
        employeeEntity.EmployeeStartDate = employeeData.EmployeeStartDate;
        employeeEntity.EmployeeEndDate = employeeData.EmployeeEndDate;
        employeeEntity.Position = employeeData.Position;

        await SetAdminPrivileges(employeeEntity, employeeData.MakeAdmin);

        return await _userManager.UpdateAsync(employeeEntity);
    }

    public async Task<IdentityResult> Delete(string id)
    {
        var employee = await GetById(id);

        if (employee is null)
        {
            return IdentityResult.Failed();
        }

        employee.DeleteDate = DateTime.Now;

        var result = await _userManager.UpdateAsync(employee);

        if (!result.Succeeded)
        {
            return IdentityResult.Failed();
        }

        await _userManager.SetLockoutEnabledAsync(employee, true);
        result = await _userManager.SetLockoutEndDateAsync(employee, DateTime.Today.AddYears(10));

        return result;
    }

    public async Task<IdentityResult> SetAdminPrivileges(Employee employee, bool makeAdmin)
    {
        string role = nameof(Roles.Admin);

        if (makeAdmin)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            return await _userManager.AddToRoleAsync(employee, role);
        }

        if (await isAdmin(employee))
        {
            return await _userManager.RemoveFromRoleAsync(employee, nameof(Roles.Admin));
        }

        return IdentityResult.Failed();
    }

    public Task<bool> isAdmin(Employee employee)
    {
        return _userManager.IsInRoleAsync(employee, nameof(Roles.Admin));
    }
}
