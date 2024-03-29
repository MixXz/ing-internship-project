﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.RegularExpressions;
using VacaYAY.Data.DataTransferObjects.Employees;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Helpers;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Data.Repository;

internal class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
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

    public async Task<Employee?> GetDetailedById(string id)
    {
        return await _context.Employees
                        .Include(e => e.Position)
                        .Include(e => e.Contracts)
                        .Include(e => e.LeaveRequests)
                            .ThenInclude(l => l.LeaveType)
                        .Include(e => e.LeaveRequests)
                            .ThenInclude(e => e.Response)
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

    public async Task<IEnumerable<Employee>> GetByFilters(EmployeeView filters)
    {
        var employees = _context.Employees
                        .Include(e => e.Position)
                        .AsQueryable();

        var query = employees.Where(e => false);

        if (!string.IsNullOrEmpty(filters.SearchInput))
        {
            var tokens = filters.SearchInput.Split(' ');

            foreach (var token in tokens)
            {
                query = query
                        .Union(employees.Where(e => e.FirstName.Contains(token)
                                                 || e.LastName.Contains(token)));
            }
        }

        if (filters.StartDateFilter is not null)
        {
            employees = employees.Where(e => e.EmployeeStartDate >= filters.StartDateFilter);
        }

        if (filters.EndDateFilter is not null)
        {
            employees = employees.Where(e => e.EmployeeEndDate <= filters.EndDateFilter);
        }

        if (filters.SelectedPositionIds.Any())
        {
            employees = employees.Where(e => filters.SelectedPositionIds.Contains(e.Position.ID));
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

    public async Task<IEnumerable<Employee>> GetWithRemainingDaysOff()
    {
        return await _context.Employees
                    .Where(e => e.DaysOffNumber > 0)
                    .ToListAsync();
    }

    public async Task<ServiceResult<Employee>> Insert(EmployeeCreate data, Position position)
    {
        ServiceResult<Employee> result = new();

        result.Errors = Validate(data.FirstName, data.LastName, data.EmployeeStartDate, data.EmployeeEndDate);

        if (result.Errors.Any())
        {
            return result;
        }

        Employee employee = new()
        {
            FirstName = data.FirstName,
            LastName = data.LastName,
            Address = data.Address,
            IDNumber = data.IDNumber,
            DaysOffNumber = data.DaysOffNumber,
            EmployeeStartDate = data.EmployeeStartDate,
            EmployeeEndDate = data.EmployeeEndDate,
            InsertDate = DateTime.Now,
            Email = data.Email,
            Position = position
        };

        await _userStore.SetUserNameAsync(employee, employee.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(employee, employee.Email, CancellationToken.None);

        var res = await _userManager.CreateAsync(employee, data.Password);

        foreach (var error in res.Errors)
        {
            result.Errors.Add(new()
            {
                Property = string.Empty,
                Text = error.Description
            });

            return result;
        }

        if (data.MakeAdmin)
        {
            await SetAdminPrivileges(employee, true);
        }

        result.Entity = employee;

        return result;
    }

    public async Task<ServiceResult<Employee>> Update(EmployeeEdit employeeData)
    {
        ServiceResult<Employee> result = new();

        result.Errors = Validate(employeeData.FirstName, employeeData.LastName, employeeData.EmployeeStartDate, employeeData.EmployeeEndDate);

        var employeeEntity = await GetById(employeeData.Id);

        if (employeeEntity is null)
        {
            return result;
        }

        if (employeeEntity.Email != employeeData.Email
            && _context.Users.Any(u => u.Email == employeeData.Email))
        {
            result.Errors.Add(new()
            {
                Property = nameof(Employee.Email),
                Text = "Entered email is already registered."
            });
        }

        if (result.Errors.Any())
        {
            return result;
        }

        employeeEntity.FirstName = employeeData.FirstName;
        employeeEntity.LastName = employeeData.LastName;
        employeeEntity.Address = employeeData.Address;
        employeeEntity.IDNumber = employeeData.IDNumber;
        employeeEntity.DaysOffNumber = employeeData.DaysOffNumber;
        employeeEntity.EmployeeStartDate = employeeData.EmployeeStartDate;
        employeeEntity.EmployeeEndDate = employeeData.EmployeeEndDate;
        employeeEntity.Position = employeeData.SelectedPosition;

        await SetAdminPrivileges(employeeEntity, employeeData.MakeAdmin);
        await _userManager.SetEmailAsync(employeeEntity, employeeData.Email);
        await _userStore.SetUserNameAsync(employeeEntity, employeeEntity.Email, CancellationToken.None);

        var res = await _userManager.UpdateAsync(employeeEntity);

        foreach (var error in res.Errors)
        {
            result.Errors.Add(new()
            {
                Property = string.Empty,
                Text = error.Description
            });

            return result;
        }

        result.Entity = employeeEntity;

        return result;
    }

    public void RemoveOldDaysOff()
    {
        _context.Employees
                .Where(e => e.DaysOffNumber > 0)
                .ExecuteUpdate(setters => setters
                    .SetProperty(p => p.OldDaysOffNumber, 0));
    }

    public void AddNewDaysOff(int numOfDays)
    {
        _context.Employees
                .Where(e => e.DaysOffNumber > 0)
                .ExecuteUpdate(setters => setters
                    .SetProperty(p => p.OldDaysOffNumber, p => p.DaysOffNumber)
                    .SetProperty(p => p.DaysOffNumber, numOfDays));
    }

    private List<CustomValidationResult> Validate(
        string firstName,
        string lastName,
        DateTime startDate,
        DateTime? endDate)
    {
        List<CustomValidationResult> errors = new();

        if (!Regex.IsMatch(firstName, @"^[a-zA-Z]+$"))
        {
            errors.Add(new()
            {
                Property = nameof(Employee.FirstName),
                Text = "First name can only consist of letters."
            });
        }

        if (!Regex.IsMatch(lastName, @"^[a-zA-Z]+$"))
        {
            errors.Add(new()
            {
                Property = nameof(Employee.LastName),
                Text = "Last name can only consist of letters."
            });
        }

        if (endDate is not null
            && startDate > endDate)
        {
            errors.Add(new()
            {
                Property = nameof(Employee.EmployeeStartDate),
                Text = "The end date cannot be earlier than the start date."
            });
        }
        return errors;
    }

    public async Task<ServiceResult<Employee>> Delete(string id)
    {
        ServiceResult<Employee> result = new();

        var employee = await GetById(id);

        if (employee is null)
        {
            result.Errors.Add(new()
            {
                Text = "Employee not found in database!"
            });

            return result;
        }

        result.Entity = employee;
        employee.DeleteDate = DateTime.Now;

        await _userManager.SetLockoutEnabledAsync(employee, true);
        var lockoutResult = await _userManager.SetLockoutEndDateAsync(employee, DateTime.Today.AddYears(10));

        if (!lockoutResult.Succeeded)
        {
            result.Errors.Add(new()
            {
                Text = "Failed to lock employee account."
            });
        }

        _context.Update(employee);
        await _context.SaveChangesAsync();

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

    public async Task<bool> IsAuthorized(ClaimsPrincipal userClaims)
    {
        var user = await GetCurrent(userClaims);

        if (user is null)
        {
            return false;
        }

        return await IsAdmin(user);
    }

    public async Task<bool> IsAuthorizedToSee(ClaimsPrincipal userClaims, string authorId)
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

    public List<EmployeeOld>? ExtractEmployeeData(string jsonResponse)
    {
        return JsonConvert.DeserializeObject<List<EmployeeOld>>(jsonResponse);
    }
}
