﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.Contracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using AutoMapper;
using VacaYAY.Data.Enums;

namespace VacaYAY.Web.Controllers;

[Authorize(Roles = "Admin")]
public class EmployeesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Employee> _userManager;
    private readonly IMapper _mapper;
    public EmployeesController(
        IUnitOfWork unitOfWork,
        UserManager<Employee> userManager,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index()
    {
        return View(await _unitOfWork.Employee.GetAll());
    }

    // GET: Employee/Details/5
    public async Task<IActionResult> Details(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var employee = await _unitOfWork.Employee.GetEmployeeById(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    // GET: Employee/Edit/5
    public async Task<IActionResult> Edit(string? id)
    {
        ViewBag.Positions = await _unitOfWork.Position.GetAll();

        if (id is null)
        {
            return NotFound();
        }

        var employee = await _unitOfWork.Employee.GetEmployeeById(id);

        if (employee is null)
        {
            return NotFound();
        }

        var employeeEdit = _mapper.Map<EmployeeEdit>(employee);

        employeeEdit.MakeAdmin = await _userManager.IsInRoleAsync(employee, nameof(Roles.Admin));

        return View(employeeEdit);
    }

    // POST: Employee/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, EmployeeEdit employee)
    {
        ViewBag.Positions = await _unitOfWork.Position.GetAll();

        var position = await _unitOfWork.Position.GetById(employee.Position.ID);

        if (position is null)
        {
            ModelState.AddModelError("Position", "Invalid position");
            return View(employee);
        }

        var employeeEntity = await _unitOfWork.Employee.GetEmployeeById(id);

        if (!ModelState.IsValid || employeeEntity is null)
        {
            return View(employee);
        }

        var isAdmin = await _userManager.IsInRoleAsync(employeeEntity, nameof(Roles.Admin));

        if (employee.MakeAdmin && !isAdmin)
        {
            await _userManager.AddToRoleAsync(employeeEntity, nameof(Roles.Admin));
        }

        if(!employee.MakeAdmin && isAdmin)
        {
            await _userManager.RemoveFromRoleAsync(employeeEntity, nameof(Roles.Admin));
        }

        employeeEntity.FirstName = employee.FirstName;
        employeeEntity.LastName = employee.LastName;
        employeeEntity.Address = employee.Address;
        employeeEntity.IDNumber = employee.IDNumber;
        employeeEntity.DaysOfNumber = employee.DaysOfNumber;
        employeeEntity.EmployeeStartDate = employee.EmployeeStartDate;
        employeeEntity.EmployeeEndDate = employee.EmployeeEndDate;
        employeeEntity.Position = position;

        if (employeeEntity.Email != employee.Email)
        {
            await _userManager.SetEmailAsync(employeeEntity, employee.Email);
        }

        var result = await _userManager.UpdateAsync(employeeEntity);

        if (!result.Succeeded)
        {
            return View(employee);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Employee/Delete/5
    public async Task<IActionResult> Delete(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var employee = await _unitOfWork.Employee.GetEmployeeById(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    // POST: Employee/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var employee = await _unitOfWork.Employee.GetEmployeeById(id);

        if (employee is null)
        {
            return RedirectToAction(nameof(Index));
        }

        employee.DeleteDate = DateTime.Now;

        var result = await _userManager.UpdateAsync(employee);

        if (result.Succeeded)
        {
            await _userManager.SetLockoutEnabledAsync(employee, true);
            await _userManager.SetLockoutEndDateAsync(employee, DateTime.Today.AddYears(10));
        }

        return RedirectToAction(nameof(Index));
    }
}