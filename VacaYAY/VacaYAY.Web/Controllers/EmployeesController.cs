using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Enums;
using AspNetCoreHero.ToastNotification.Abstractions;
using VacaYAY.Business.ServiceContracts;

namespace VacaYAY.Web.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class EmployeesController : BaseController
{
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;
    private readonly IHttpClientService _httpClientService;

    public EmployeesController(
        IEmployeeService employeeService,
        IPositionService positionService,
        IHttpClientService httpClientService,
        INotyfService toaster)
        : base(toaster)
    {
        _employeeService = employeeService;
        _positionService = positionService;
        _httpClientService = httpClientService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(EmployeeView filters)
    {
        EmployeeView view = new()
        {
            Positions = await _positionService.GetAll(),
            SelectedPositionIds = filters.SelectedPositionIds,
            Employees = await _employeeService.GetByFilters(filters)
        };

        return View(view);
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View(new EmployeeCreate { Positions = await _positionService.GetAll() });
    }

    [HttpPost]
    public async Task<IActionResult> Register(EmployeeCreate employeeData)
    {
        employeeData.Positions = await _positionService.GetAll();

        var result = await _employeeService.Register(employeeData);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(employeeData);
        }

        Notification($"Employee {result.Entity.Name} successfully registered.");

        return RedirectToAction("Create", "Contracts", new { result.Entity.Id });
    }

    [HttpPost]
    public async Task<IActionResult> LoadExistingEmployees()
    {
        var response = await _httpClientService.GetAsync("RandomData");

        if (!response.IsSuccessStatusCode)
        {
            Notification("Error loading employees from old database.", NotificationType.Error);
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var insertedEmpCount = await _employeeService.InsertOldEmployees(jsonResponse);

        if (insertedEmpCount > 0)
        {
            Notification($"Successfully loaded {insertedEmpCount} from old database.");
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        var employee = await _employeeService.GetByIdDetailed(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpGet]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> Edit(string id)
    {
        var employee = await _employeeService.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        var employeeData = _employeeService.GetEditDto(employee);

        employeeData.MakeAdmin = await _employeeService.IsAdmin(employee);
        employeeData.Positions = await _positionService.GetAll();
        employeeData.SelectedPosition = employee.Position;

        return View(employeeData);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> Edit(EmployeeEdit employee)
    {
        employee.Positions = await _positionService.GetAll();

        var result = await _employeeService.Edit(employee);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(employee);
        }

        Notification($"Employee {result.Entity.Name} has been successfully edited.");

        return RedirectToAction(nameof(Details), new { employee.Id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _employeeService.Delete(id);

        if (result.Entity is null)
        {
            HandleErrors(result.Errors);
            return RedirectToAction(nameof(Index));
        }

        Notification($"Employee {result.Entity.Name} has been successfully deleted.");

        return RedirectToAction(nameof(Index));
    }
}