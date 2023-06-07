using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using VacaYAY.Business.Contracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Web.Controllers;

[Authorize]
public class RequestsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RequestsController(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> AdminPanel(RequestView filters)
    {
        ViewBag.LeaveTypes = await _unitOfWork.LeaveType.GetAll();

        var requests = await _unitOfWork.Request.GetByFilters(filters);

        return View(new RequestView { Requests = requests });
    }

    public async Task<IActionResult> MyRequests()
    {
        var user = await _unitOfWork.Employee.GetCurrent(User);

        if (user is null)
        {
            return Unauthorized();
        }

        return View(await _unitOfWork.Request.GetByUser(user.Id));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var request = await _unitOfWork.Request.GetById((int)id);

        if (request is null)
        {
            return NotFound();
        }

        if (!await _unitOfWork.Employee.isAuthorizedToSee(User, request.CreatedBy.Id))
        {
            return Unauthorized();
        }

        return View(request);
    }

    public async Task<IActionResult> CreateRequest()
    {
        ViewBag.LeaveTypes = await _unitOfWork.LeaveType.GetAll();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRequest(RequestCreate requestData)
    {
        ViewBag.LeaveTypes = await _unitOfWork.LeaveType.GetAll();

        var author = await _unitOfWork.Employee.GetCurrent(User);
        var leaveType = await _unitOfWork.LeaveType.GetById(requestData.LeaveTypeID);

        if (leaveType is null || author is null)
        {
            return View(requestData);
        }

        var errors = _unitOfWork.Request.ValidateOnCreate(requestData, author);
        foreach (var error in errors)
        {
            ModelState.AddModelError(error.Property, error.Text);
        }

        if (!ModelState.IsValid)
        {
            return View(requestData);
        }

        Request requestEntity = new()
        {
            LeaveType = leaveType,
            StartDate = requestData.StartDate,
            EndDate = requestData.EndDate,
            CreatedBy = author,
            Comment = requestData.Comment,
        };

        author.DaysOfNumber -= (int)(requestData.EndDate - requestData.StartDate).TotalDays;

        _unitOfWork.Request.Insert(requestEntity);
        _unitOfWork.Employee.Update(author);

        await _unitOfWork.SaveChangesAsync();

        var isAdmin = _unitOfWork.Employee.IsAdmin(User);

        return isAdmin ?
            RedirectToAction(nameof(AdminPanel))
            :
            Redirect("~/");
    }

    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> CreateResponse(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        if (!await _unitOfWork.Employee.isAuthorized(User))
        {
            return Unauthorized();
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> CreateResponse(int id, ResponseCreate responseData)
    {
        var user = await _unitOfWork.Employee.GetCurrent(User);

        if (user is null)
        {
            return Unauthorized();
        }

        var request = await _unitOfWork.Request.GetById(id);

        if (request is null || request.Response is not null)
        {
            return NotFound();
        }

        Response response = new()
        {
            IsApproved = responseData.IsApproved,
            Comment = responseData.Comment,
            RequstID = request.ID,
            Request = request,
            ReviewedBy = user
        };

        request.Response = response;

        _unitOfWork.Response.Insert(response);
        _unitOfWork.Request.Update(request);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(AdminPanel));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        ViewBag.LeaveTypes = await _unitOfWork.LeaveType.GetAll();

        var request = await _unitOfWork.Request.GetById((int)id);

        if (request is null)
        {
            return NotFound();
        }

        if (!await _unitOfWork.Employee.isAuthorizedToSee(User, request.CreatedBy.Id))
        {
            return Unauthorized();
        }

        var requestData = _mapper.Map<RequestEdit>(request);

        return View(requestData);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RequestEdit requestData)
    {
        if (id != requestData.ID)
        {
            return NotFound();
        }

        ViewBag.LeaveTypes = await _unitOfWork.LeaveType.GetAll();

        var requestEnitity = await _unitOfWork.Request.GetById(requestData.ID);
        var leaveType = await _unitOfWork.LeaveType.GetById(requestData.LeaveType.ID);

        if (requestEnitity is null || leaveType is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(requestData);
        }

        if (!await _unitOfWork.Employee.isAuthorizedToSee(User, requestEnitity.CreatedBy.Id))
        {
            return Unauthorized();
        }

        _mapper.Map(requestData, requestEnitity);
        requestEnitity.LeaveType = leaveType;

        _unitOfWork.Request.Update(requestEnitity);
        await _unitOfWork.SaveChangesAsync();

        return _unitOfWork.Employee.IsAdmin(User) ?
            RedirectToAction(nameof(AdminPanel))
            :
            RedirectToAction(nameof(MyRequests));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var request = await _unitOfWork.Request.GetById((int)id);

        if (request is null)
        {
            return NotFound();
        }

        if (!await _unitOfWork.Employee.isAuthorizedToSee(User, request.CreatedBy.Id))
        {
            return Forbid();
        }

        return View(request);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var isAdmin = _unitOfWork.Employee.IsAdmin(User);

        var redirect = RedirectToAction(isAdmin ?
                                        nameof(AdminPanel)
                                        :
                                        nameof(MyRequests));

        var request = await _unitOfWork.Request.GetById(id);

        if (request is null)
        {
            return redirect;
        }

        if (!await _unitOfWork.Employee.isAuthorizedToSee(User, request.CreatedBy.Id))
        {
            return Forbid();
        }

        _unitOfWork.Request.Delete(request);
        await _unitOfWork.SaveChangesAsync();

        return redirect;
    }
}
