using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using VacaYAY.Data.Entities;

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
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _userManager = userManager;
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

    public async Task<Employee?> GetEmployeeById(string id)
    {
        return await _context.Employees
                        .Include(e => e.Position)
                        .Where(e => e.Id == id)
                        .FirstOrDefaultAsync();
    }

    public async Task<IdentityResult> InsertViaManager(Employee employee)
    {
        var employeeInDb = await _userManager.FindByEmailAsync(employee.Email!);

        if (employeeInDb is not null)
        {
            return IdentityResult.Failed();
        }

        await _userStore.SetUserNameAsync(employee, employee.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(employee, employee.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(employee, "Nzm123za!");

        return result;
    }

    private IUserEmailStore<Employee> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<Employee>)_userStore;
    }
}

