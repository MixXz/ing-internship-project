using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Web.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class LeaveTypesController : BaseController
{
    private readonly ILeaveTypeService _leaveTypeService;

    public LeaveTypesController(
        ILeaveTypeService leaveTypeService,
        INotyfService toaster)
        : base(toaster)
    {
        _leaveTypeService = leaveTypeService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _leaveTypeService.GetAll());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var leaveType = await _leaveTypeService.GetById(id);

        if (leaveType is null)
        {
            return NotFound();
        }

        return View(leaveType);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveType leaveType)
    {
        var result = await _leaveTypeService.Create(leaveType);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(leaveType);
        }

        Notification($"Leave type {leaveType.Caption} successfully created.");

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var leaveType = await _leaveTypeService.GetById(id);

        if (leaveType is null)
        {
            return NotFound();
        }

        return View(leaveType);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(LeaveType leaveType)
    {
        var result = await _leaveTypeService.Update(leaveType);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(leaveType);
        }

        Notification($"Leave type {leaveType.Caption} successfully edited.");

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _leaveTypeService.Delete(id);

        if (result.Entity is null)
        {
            HandleErrors(result.Errors);
            return RedirectToAction(nameof(Index));
        }

        Notification($"Leave type {result.Entity.Caption} successfully deleted.");

        return RedirectToAction(nameof(Index));
    }
}
