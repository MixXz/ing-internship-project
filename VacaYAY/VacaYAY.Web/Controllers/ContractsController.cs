using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Data.DataServiceContracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Enums;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Web.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class ContractsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBlobService _blobService;
    private readonly INotyfService _toaster;
    public ContractsController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IBlobService blobService,
        INotyfService toaster)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _blobService = blobService;
        _toaster = toaster;
    }

    public async Task<IActionResult> Index(string id)
    {
        var employee = await _unitOfWork.Employee.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        var contracts = await _unitOfWork.Contract.GetByEmployeeId(id);

        ContractView model = new()
        {
            Employee = employee,
            Contracts = contracts
        };

        return View(model);
    }

    public async Task<IActionResult> PreviewDocument(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var blobUrl = await _unitOfWork.Contract.GetDocumentUrlByContractId((int)id);

        if (blobUrl is null)
        {
            return NotFound();
        }

        var stream = await _blobService.DownloadToPdfStream(blobUrl);

        return File(stream, "application/pdf");
    }

    public async Task<IActionResult> DownloadDocument(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var blobUrl = await _unitOfWork.Contract.GetDocumentUrlByContractId((int)id);

        if (blobUrl is null)
        {
            return NotFound();
        }

        (Stream data, string contentType) = await _blobService.DownloadDocument(blobUrl);

        Response.ContentType = contentType;
        Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{Path.GetFileName(blobUrl)}\"");

        return File(data, contentType);
    }

    public async Task<IActionResult> Create(string id)
    {
        var employee = await _unitOfWork.Employee.GetById(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(new ContractCreate() { EmployeeId = employee.Id });
    }

    [HttpPost]
    public async Task<IActionResult> Create(ContractCreate contractData)
    {
        var employee = await _unitOfWork.Employee.GetById(contractData.EmployeeId);

        if (employee is null)
        {
            return NotFound();
        }

        var contractResult = await _unitOfWork.Contract.Create(contractData, employee);

        if (contractResult.Entity is null)
        {
            foreach (var error in contractResult.Errors)
            {
                ModelState.AddModelError(error.Property, error.Text);
            }
        }

        if (!ModelState.IsValid)
        {
            return View(contractData);
        }

        employee.Contracts.Add(contractResult.Entity!);

        _unitOfWork.Employee.Update(employee);
        await _unitOfWork.SaveChangesAsync();

        _toaster.Success($"Contract successfully added to employee {employee.Name}.");

        return RedirectToAction(nameof(Index), new { employee.Id });
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var contract = await _unitOfWork.Contract.GetById((int)id);
        var contractEdit = _mapper.Map<ContractEdit>(contract);

        return View(contractEdit);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, ContractEdit contractData)
    {
        if (id != contractData.ID)
        {
            return NotFound();
        }

        var user = await _unitOfWork.Employee.GetCurrent(User);
        if (user is null || !await _unitOfWork.Employee.IsAdmin(user))
        {
            return Forbid();
        }

        var result = await _unitOfWork.Contract.Update(contractData);

        if (result.Entity is null)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Property, error.Text);
            }

            return View(contractData);
        }

        await _unitOfWork.SaveChangesAsync();

        _toaster.Success($"Contract successfully edited.");

        return RedirectToAction(nameof(Index), new { result.Entity.Employee.Id });
    }

}
