using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.Contracts;
using VacaYAY.Data.DataTransferObjects;
using AutoMapper;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Entities;
using VacaYAY.Business.Contracts.ServiceContracts;

namespace VacaYAY.Web.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class EmployeesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly IHttpClientService _httpClientService;

    public EmployeesController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IConfiguration configuration,
        IHttpClientService httpClientService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _config = configuration;
        _httpClientService = httpClientService;
    }

    public async Task<IActionResult> Index(string? searchInput, DateTime? startDateFilter, DateTime? endDateFilter)
    {
        var employees = await _unitOfWork.Employee.GetByFilters(searchInput, startDateFilter, endDateFilter);

        return View(new EmployeeView { Employees = employees });
    }

    public async Task<IActionResult> Register()
    {
        ViewBag.Positions = await _unitOfWork.Position.GetAll();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(EmployeeCreate employeeData)
    {
        ViewBag.Positions = await _unitOfWork.Position.GetAll();

        var position = await _unitOfWork.Position.GetById(employeeData.SelectedPositionID);

        if (position is null)
        {
            return NotFound();
        }

        var result = await _unitOfWork.Employee.Insert(employeeData, position);

        if (result.Entity is null)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Property, error.Text);
            }

            return View(employeeData);
        }

        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction("Create", "Contracts", new { result.Entity.Id });
    }

    public async Task<IActionResult> LoadExistingEmployees()
    {
        var response = await _httpClientService.GetAsync("RandomData");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var oldEmployees = _unitOfWork.Employee.ExtractEmployeeData(jsonResponse);

        if (oldEmployees is null)
        {
            return NotFound();
        }

        foreach (var oldEmployee in oldEmployees)
        {
            var position = await _unitOfWork.Position.GetByCaption(oldEmployee.Position.Caption);

            if (position is null)
            {
                position = new()
                {
                    Caption = oldEmployee.Position.Caption,
                    Description = oldEmployee.Position.Description
                };

                _unitOfWork.Position.Insert(position);
                await _unitOfWork.SaveChangesAsync();
            }

            var employeeData = _mapper.Map<EmployeeCreate>(oldEmployee);
            employeeData.Password = $"{oldEmployee.FirstName}123!";

            await _unitOfWork.Employee.Insert(employeeData, position);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var employee = await _unitOfWork.Employee.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    public async Task<IActionResult> Edit(string? id)
    {
        ViewBag.Positions = await _unitOfWork.Position.GetAll();

        if (id is null)
        {
            return NotFound();
        }

        var employee = await _unitOfWork.Employee.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        var employeeEdit = _mapper.Map<EmployeeEdit>(employee);

        employeeEdit.MakeAdmin = await _unitOfWork.Employee.IsAdmin(employee);

        return View(employeeEdit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, EmployeeEdit employee)
    {
        ViewBag.Positions = await _unitOfWork.Position.GetAll();

        var position = await _unitOfWork.Position.GetById(employee.Position.ID);

        if (position is null)
        {
            return View(employee);
        }

        employee.Position = position;
        var result = await _unitOfWork.Employee.Update(id, employee);

        if (result.Entity is null)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Property, error.Text);
            }
            return View(employee);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var employee = await _unitOfWork.Employee.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        await _unitOfWork.Employee.Delete(id);

        return RedirectToAction(nameof(Index));
    }
}