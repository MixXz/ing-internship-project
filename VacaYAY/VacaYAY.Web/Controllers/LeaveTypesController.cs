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

    public LeaveTypesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        if (!ModelState.IsValid)
        {
            return View(leaveType);
        }

        _unitOfWork.LeaveType.Insert(leaveType);
        await _unitOfWork.SaveChangesAsync();

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

        if (!ModelState.IsValid)
        {
            return View(leaveType);
        }

        _unitOfWork.LeaveType.Update(leaveType);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
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

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var leaveType = await _unitOfWork.LeaveType.GetById(id);

        if (leaveType is not null)
        {
            _unitOfWork.LeaveType.Delete(leaveType);
            await _unitOfWork.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
