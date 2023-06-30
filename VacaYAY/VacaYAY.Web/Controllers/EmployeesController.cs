using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.Contracts;
using VacaYAY.Data.DataTransferObjects;
using AutoMapper;
using VacaYAY.Data.Enums;
using VacaYAY.Business.Contracts.ServiceContracts;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace VacaYAY.Web.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class EmployeesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly IHttpClientService _httpClientService;
    private readonly INotyfService _toaster;

    public EmployeesController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IConfiguration configuration,
        IHttpClientService httpClientService,
        INotyfService toaster)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _config = configuration;
        _httpClientService = httpClientService;
        _toaster = toaster;
    }

    public async Task<IActionResult> Index(EmployeeView filters)
    {
        EmployeeView model = new()
        {
            Positions = await _unitOfWork.Position.GetAll(),
            SelectedPositionIds = filters.SelectedPositionIds,
            Employees = await _unitOfWork.Employee.GetByFilters(filters)
        };

        return View(model);
    }

    public async Task<IActionResult> Register()
    {
        return View(new EmployeeCreate { Positions = await _unitOfWork.Position.GetAll() });
    }

    [HttpPost]
    public async Task<IActionResult> Register(EmployeeCreate employeeData)
    {
        var positions = await _unitOfWork.Position.GetAll();
        employeeData.Positions = positions;

        var position = positions.FirstOrDefault(p => p.ID == employeeData.SelectedPositionID);

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

        _toaster.Success($"Employee {result.Entity.Name} successfully registered.");

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

        _toaster.Success($"Successfully loaded {oldEmployees.Count()} from old database.");

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var employee = await _unitOfWork.Employee.GetDetailedById(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    public async Task<IActionResult> Edit(string? id)
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

        var model = _mapper.Map<EmployeeEdit>(employee);

        model.MakeAdmin = await _unitOfWork.Employee.IsAdmin(employee);
        model.Positions = await _unitOfWork.Position.GetAll();
        model.SelectedPosition = employee.Position;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, EmployeeEdit employee)
    {
        var positions = await _unitOfWork.Position.GetAll();
        var position = await _unitOfWork.Position.GetById(employee.SelectedPosition.ID);

        if (position is null)
        {
            return View(employee);
        }

        employee.SelectedPosition = position;
        var result = await _unitOfWork.Employee.Update(id, employee);

        if (result.Entity is null)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Property, error.Text);
            }
            return View(employee);
        }

        _toaster.Success($"Employee {result.Entity.Name} has been successfully edited.");

        return RedirectToAction(nameof(Details), new { employee.Id });
    }

    public async Task<IActionResult> Delete(string id)
    {
        var result = await _unitOfWork.Employee.Delete(id);

        if (result.Entity is null)
        {
            foreach (var error in result.Errors)
            {
                _toaster.Error(error.Text);
            }
            return RedirectToAction(nameof(Index));
        }

        _toaster.Success($"Employee {result.Entity.Name} has been successfully deleted.");

        return RedirectToAction(nameof(Index));
    }
}