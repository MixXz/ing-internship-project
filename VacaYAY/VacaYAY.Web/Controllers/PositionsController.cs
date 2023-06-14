using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.Contracts;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Web.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class PositionsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public PositionsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _unitOfWork.Position.GetAll());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var position = await _unitOfWork.Position.GetById((int)id);

        if (position is null)
        {
            return NotFound();
        }

        return View(position);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Position position)
    {
        if (!ModelState.IsValid)
        {
            return View(position);
        }

        _unitOfWork.Position.Insert(position);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var position = await _unitOfWork.Position.GetById((int)id);

        if (position is null)
        {
            return NotFound();
        }

        return View(position);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Position position)
    {
        if (id != position.ID)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(position);
        }

        _unitOfWork.Position.Update(position);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var position = await _unitOfWork.Position.GetById((int)id);

        if (position is null)
        {
            return NotFound();
        }

        return View(position);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var position = await _unitOfWork.Position.GetById(id);

        if (position is not null)
        {
            _unitOfWork.Position.Delete(position);
            await _unitOfWork.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
