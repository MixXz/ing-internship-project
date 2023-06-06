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

    // GET: LeaveTypes
    public async Task<IActionResult> Index()
    {
        return View(await _unitOfWork.LeaveType.GetAll());
    }

    // GET: LeaveTypes/Details/5
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

    // GET: LeaveTypes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: LeaveTypes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ID,Caption,Description")] LeaveType leaveType)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.LeaveType.Insert(leaveType);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(leaveType);
    }

    // GET: LeaveTypes/Edit/5
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

    // POST: LeaveTypes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ID,Caption,Description")] LeaveType leaveType)
    {
        if (id != leaveType.ID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _unitOfWork.LeaveType.Update(leaveType);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        return View(leaveType);
    }

    // GET: LeaveTypes/Delete/5
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

    // POST: LeaveTypes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var leaveType = await _unitOfWork.LeaveType.GetById(id);

        if (leaveType is not null)
        {
            _unitOfWork.LeaveType.Delete(leaveType);
        }

        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
