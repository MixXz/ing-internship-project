using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.Contracts;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Web.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class LeaveTypesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotyfService _toaster;

    public LeaveTypesController(
        IUnitOfWork unitOfWork,
        INotyfService toaster)
    {
        _unitOfWork = unitOfWork;
        _toaster = toaster;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _unitOfWork.LeaveType.GetAll());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var leaveType = await _unitOfWork.LeaveType.GetById((int)id);

        if (leaveType is null)
        {
            return NotFound();
        }

        return View(leaveType);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LeaveType leaveType)
    {
        var errors = _unitOfWork.LeaveType.Validate(leaveType);

        foreach (var error in errors)
        {
            ModelState.AddModelError(error.Property, error.Text);
        }

        if (!ModelState.IsValid)
        {
            return View(leaveType);
        }

        _unitOfWork.LeaveType.Insert(leaveType);
        await _unitOfWork.SaveChangesAsync();

        _toaster.Success($"Leave type {leaveType.Caption} successfully created.");

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var leaveType = await _unitOfWork.LeaveType.GetById((int)id);

        if (leaveType is null)
        {
            return NotFound();
        }

        return View(leaveType);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LeaveType leaveType)
    {
        if (id != leaveType.ID)
        {
            return NotFound();
        }

        var errors = _unitOfWork.LeaveType.Validate(leaveType);

        foreach (var error in errors)
        {
            ModelState.AddModelError(error.Property, error.Text);
        }

        if (!ModelState.IsValid)
        {
            return View(leaveType);
        }

        _unitOfWork.LeaveType.Update(leaveType);
        await _unitOfWork.SaveChangesAsync();

        _toaster.Success($"Leave type {leaveType.Caption} successfully edited.");

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var leaveType = await _unitOfWork.LeaveType.GetById(id);

        if (leaveType is null)
        {
            _toaster.Error("Leave type deletion failed.");
            return RedirectToAction(nameof(Index));
        }

        _unitOfWork.LeaveType.Delete(leaveType);
        await _unitOfWork.SaveChangesAsync();

        _toaster.Success($"Leave type {leaveType.Caption} successfully deleted.");

        return RedirectToAction(nameof(Index));
    }
}
