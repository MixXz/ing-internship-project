using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Web.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class PositionsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotyfService _toaster;

    public PositionsController(
        IUnitOfWork unitOfWork,
        INotyfService toaster)
    {
        _unitOfWork = unitOfWork;
        _toaster = toaster;
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
        var errors = _unitOfWork.Position.Validate(position);

        foreach (var error in errors)
        {
            ModelState.AddModelError(error.Property, error.Text);
        }

        if (!ModelState.IsValid)
        {
            return View(position);
        }

        _unitOfWork.Position.Insert(position);
        await _unitOfWork.SaveChangesAsync();

        _toaster.Success($"Position {position.Caption} successfully created.");

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

        var errors = _unitOfWork.Position.Validate(position);

        foreach (var error in errors)
        {
            ModelState.AddModelError(error.Property, error.Text);
        }

        if (!ModelState.IsValid)
        {
            return View(position);
        }

        _unitOfWork.Position.Update(position);
        await _unitOfWork.SaveChangesAsync();

        _toaster.Success($"Position {position.Caption} successfully edited.");

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var position = await _unitOfWork.Position.GetById(id);

        if (position is null)
        {
            _toaster.Error("Position deletion failed.");
            return RedirectToAction(nameof(Index));
        }

        _unitOfWork.Position.Delete(position);
        await _unitOfWork.SaveChangesAsync();

        _toaster.Success($"Position {position.Caption} successfully deleted.");

        return RedirectToAction(nameof(Index));
    }
}
