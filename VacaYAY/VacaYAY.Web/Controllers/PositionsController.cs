using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Web.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class PositionsController : BaseController
{
    private readonly IPositionService _positionService;

    public PositionsController(
        IPositionService positionService,
        INotyfService toaster)
        : base(toaster)
    {
        _positionService = positionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _positionService.GetAll());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var position = await _positionService.GetById(id);

        if (position is null)
        {
            return NotFound();
        }

        return View(position);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Position position)
    {
        var result = await _positionService.Create(position);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(position);
        }

        Notification($"Position {result.Entity.Caption} successfully created.");

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var position = await _positionService.GetById(id);

        if (position is null)
        {
            return NotFound();
        }

        return View(position);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Position position)
    {
        var result = await _positionService.Update(position);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(position);
        }

        Notification($"Position {result.Entity.Caption} successfully edited.");

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _positionService.Delete(id);

        if (result.Entity is null)
        {
            HandleErrors(result.Errors);
            return RedirectToAction(nameof(Index));
        }

        Notification($"Position {result.Entity.Caption} successfully deleted.");

        return RedirectToAction(nameof(Index));
    }
}
