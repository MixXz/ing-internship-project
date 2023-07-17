using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.DataTransferObjects.Contracts;
using VacaYAY.Data.Enums;

namespace VacaYAY.Web.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class ContractsController : BaseController
{
    private readonly IContractService _contractService;
    private readonly IEmployeeService _employeeService;

    public ContractsController(
        IContractService contractService,
        IEmployeeService employeeService,
        INotyfService toaster)
        : base(toaster)
    {
        _contractService = contractService;
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string id)
    {
        var employee = await _employeeService.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        var contracts = await _contractService.GetByEmployeeId(id);

        ContractView model = new()
        {
            Employee = employee,
            Contracts = contracts
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> PreviewDocument(int id)
    {
        var contract = await _contractService.GetById(id);

        if (contract is null)
        {
            return NotFound();
        }

        var stream = await _contractService.GetDocumentStream(contract.DocumentURL);

        return File(stream, "application/pdf");
    }

    [HttpGet]
    public async Task<IActionResult> DownloadDocument(int id)
    {
        var contract = await _contractService.GetById(id);

        if (contract is null)
        {
            return NotFound();
        }

        (Stream data, string contentType) = await _contractService.DownloadDocument(contract.DocumentURL);

        Response.ContentType = contentType;
        Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{Path.GetFileName(contract.DocumentURL)}\"");

        return File(data, contentType);
    }

    [HttpGet]
    public async Task<IActionResult> Create(string id)
    {
        var employee = await _employeeService.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(new ContractCreate() { EmployeeId = employee.Id });
    }

    [HttpPost]
    public async Task<IActionResult> Create(ContractCreate contractData)
    {
        var employee = await _employeeService.GetById(contractData.EmployeeId);

        if (employee is null)
        {
            return NotFound();
        }

        var result = await _contractService.Create(contractData, employee);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(contractData);
        }

        Notification($"Contract successfully added to employee {employee.Name}.");

        return RedirectToAction(nameof(Index), new { employee.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var contractEdit = await _contractService.GetEditDto(id);

        if (contractEdit is null)
        {
            return NotFound();
        }

        return View(contractEdit);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ContractEdit contractData)
    {
        var user = await _employeeService.GetCurrent(User);
        if (user is null)
        {
            return Unauthorized();
        }

        var result = await _contractService.Update(contractData);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(contractData);
        }

        Notification($"Contract successfully edited.");

        return RedirectToAction(nameof(Index), new { result.Entity.Employee.Id });
    }
}
