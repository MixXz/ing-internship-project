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

        ViewBag.DaysOffNumber = user.DaysOfNumber;

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
        var user = await _unitOfWork.Employee.GetCurrent(User);
        if (user is null)
        {
            return Unauthorized();
        }

        ViewBag.LeaveTypes = await _unitOfWork.LeaveType.GetAll();
        ViewBag.DaysOffNumber = user.DaysOfNumber;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRequest(RequestCreate requestData)
    {
        var leaveTypes = await _unitOfWork.LeaveType.GetAll();
        ViewBag.LeaveTypes = leaveTypes;

        var author = await _unitOfWork.Employee.GetCurrent(User);
        if (author is null)
        {
            return Unauthorized();
        }

        ViewBag.DaysOffNumber = author.DaysOfNumber;

        var leaveType = leaveTypes.FirstOrDefault(l => l.ID == requestData.LeaveTypeID);
        if (leaveType is null)
        {
            return View(requestData);
        }

        var errors = await _unitOfWork.Request.ValidateOnCreate(requestData, author);
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

        _unitOfWork.Request.Insert(requestEntity);
        await _unitOfWork.SaveChangesAsync();

        var isAdmin = _unitOfWork.Employee.IsAdmin(User);

        return isAdmin ?
            RedirectToAction(nameof(AdminPanel))
            :
            Redirect(nameof(MyRequests));
    }

    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> CreateResponse(int? id)
    {
        ViewBag.LeaveTypes = await _unitOfWork.LeaveType.GetAll();

        if (id is null)
        {
            return NotFound();
        }

        var request = await _unitOfWork.Request.GetById((int)id);

        if (request is null)
        {
            return NotFound();
        }

        if (!await _unitOfWork.Employee.isAuthorized(User))
        {
            return Unauthorized();
        }

        return View(new ResponseCreate { LeaveType = request.LeaveType });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> CreateResponse(int id, ResponseCreate responseData)
    {
        var reviewer = await _unitOfWork.Employee.GetCurrent(User);

        if (reviewer is null)
        {
            return Unauthorized();
        }

        var request = await _unitOfWork.Request.GetById(id);
        var leaveTypes = await _unitOfWork.LeaveType.GetAll();
        var leaveType = leaveTypes.FirstOrDefault(l => l.ID == responseData.LeaveType.ID);

        ViewBag.LeaveTypes = leaveTypes;

        if (request is null
            || leaveType is null
            || request.Response is not null)
        {
            return NotFound();
        }

        Response response = new()
        {
            IsApproved = responseData.IsApproved,
            Comment = responseData.Comment,
            RequstID = request.ID,
            Request = request,
            ReviewedBy = reviewer
        };

        _unitOfWork.Response.Insert(response);

        var seeker = request.CreatedBy;
        if (response.IsApproved)
        {
            seeker.DaysOfNumber -= request.NumOfDaysRequested;
            _unitOfWork.Employee.Update(seeker);
        }

        request.Response = response;
        request.LeaveType = leaveType;
        _unitOfWork.Request.Update(request);

        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(AdminPanel));
    }

    public async Task<IActionResult> EditRequest(int? id)
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
    public async Task<IActionResult> EditRequest(int id, RequestEdit requestData)
    {
        ViewBag.LeaveTypes = await _unitOfWork.LeaveType.GetAll();

        var requestEntity = await _unitOfWork.Request.GetById(requestData.ID);
        var leaveType = await _unitOfWork.LeaveType.GetById(requestData.LeaveType.ID);

        if (requestEntity is null || leaveType is null)
        {
            return NotFound();
        }

        var errors = await _unitOfWork.Request.ValidateOnEdit(requestData, requestEntity.CreatedBy);
        foreach (var error in errors)
        {
            ModelState.AddModelError(error.Property, error.Text);
        }

        if (!ModelState.IsValid)
        {
            return View(requestData);
        }

        if (!await _unitOfWork.Employee.isAuthorizedToSee(User, requestEntity.CreatedBy.Id))
        {
            return Unauthorized();
        }

        var response = requestEntity.Response;

        if (response is not null)
        {
            requestEntity.Response = null;
            _unitOfWork.Response.Delete(response);
        }

        _mapper.Map(requestData, requestEntity);
        requestEntity.LeaveType = leaveType;

        _unitOfWork.Request.Update(requestEntity);
        await _unitOfWork.SaveChangesAsync();

        return _unitOfWork.Employee.IsAdmin(User) ?
            RedirectToAction(nameof(AdminPanel))
            :
            RedirectToAction(nameof(MyRequests));
    }

    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> EditResponse(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        ViewBag.LeaveTypes = await _unitOfWork.LeaveType.GetAll();

        var response = await _unitOfWork.Response.GetById((int)id);

        if (response is null)
        {
            return NotFound();
        }

        var request = await _unitOfWork.Request.GetById(response.RequstID);

        if (!await _unitOfWork.Employee.isAuthorized(User))
        {
            return Unauthorized();
        }

        var responseData = _mapper.Map<ResponseEdit>(response);
        responseData.LeaveType = request!.LeaveType;

        return View(responseData);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> EditResponse(int id, ResponseEdit responseData)
    {
        var reviewer = await _unitOfWork.Employee.GetCurrent(User);

        if (reviewer is null
            || !await _unitOfWork.Employee.IsAdmin(reviewer))
        {
            return Forbid();
        }

        var request = await _unitOfWork.Request.GetById(responseData.RequstID);

        if (request is null)
        {
            return NotFound();
        }

        var seeker = request.CreatedBy;
        var response = request.Response;
        var leaveType = await _unitOfWork.LeaveType.GetById(responseData.LeaveType.ID);

        if (response is null
            || leaveType is null
            || seeker is null
            || id != response.ID)
        {
            return NotFound();
        }

        var oldStatus = response.Status;
        var newStatus = responseData.Status;

        if (oldStatus != newStatus)
        {
            if (oldStatus is RequestStatus.Rejected && newStatus is RequestStatus.Approved)
            {
                seeker.DaysOfNumber -= request.NumOfDaysRequested;
            }

            if (oldStatus is RequestStatus.Approved && newStatus is RequestStatus.Rejected)
            {
                seeker.DaysOfNumber += request.NumOfDaysRequested;
            }

            _unitOfWork.Employee.Update(seeker);
        }

        request.LeaveType = leaveType;
        _unitOfWork.Request.Update(request);

        response.IsApproved = responseData.IsApproved;
        response.Comment = responseData.Comment;
        response.ReviewedBy = reviewer;
        _unitOfWork.Response.Update(response);

        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(AdminPanel));
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
