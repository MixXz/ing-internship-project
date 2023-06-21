using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VacaYAY.Business.Contracts.RepositoryContracts;
using VacaYAY.Data;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Business.Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
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
        : base(context)
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

    public async Task<Employee?> GetCurrent(ClaimsPrincipal userClaims)
    {
        return await _userManager.GetUserAsync(userClaims);
    }

    public override async Task<IEnumerable<Employee>> GetAll()
    {
        return await _context.Employees
                        .Include(e => e.Position)
                        .OrderByDescending(e => e.InsertDate)
                        .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByFilters(string? searchInput, DateTime? startDate, DateTime? endDate)
    {
        var employees = _context.Employees
                        .Include(e => e.Position)
                        .AsQueryable();

        var query = employees.Where(e => false);

        if (!string.IsNullOrEmpty(searchInput))
        {
            var tokens = searchInput.Split(' ');

            foreach (var token in tokens)
            {
                query = query
                        .Union(employees.Where(e => e.FirstName.Contains(token)
                                                 || e.LastName.Contains(token)));
            }
        }

        if (startDate is not null)
        {
            employees = employees.Where(e => e.EmployeeStartDate >= startDate);
        }

        if (endDate is not null)
        {
            employees = employees.Where(e => e.EmployeeEndDate <= endDate);
        }

        if (query.Any())
        {
            employees = employees.Intersect(query);
        }

        return await employees
                    .OrderByDescending(e => e.InsertDate)
                    .ToListAsync();
    }
    public async Task<IEnumerable<Employee>> GetAdmins()
    {
        return await _userManager.GetUsersInRoleAsync(nameof(Roles.Admin));
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
        employeeEntity.DaysOffNumber = employeeData.DaysOffNumber;
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

        if (await IsAdmin(employee))
        {
            return await _userManager.RemoveFromRoleAsync(employee, nameof(Roles.Admin));
        }

        return IdentityResult.Failed();
    }

    public Task<bool> IsAdmin(Employee employee)
    {
        return _userManager.IsInRoleAsync(employee, nameof(Roles.Admin));
    }
    public bool IsAdmin(ClaimsPrincipal userClaims)
    {
        return userClaims.IsInRole(nameof(Roles.Admin));
    }

    public async Task<bool> isAuthorized(ClaimsPrincipal userClaims)
    {
        var user = await GetCurrent(userClaims);

        if (user is null)
        {
            return false;
        }

        return await IsAdmin(user);
    }

    public async Task<bool> isAuthorizedToSee(ClaimsPrincipal userClaims, string authorId)
    {
        var user = await GetCurrent(userClaims);

        if (user is null)
        {
            return false;
        }

        if (!IsAdmin(userClaims) && user.Id != authorId)
        {
            return false;
        }

        return true;
    }
}
