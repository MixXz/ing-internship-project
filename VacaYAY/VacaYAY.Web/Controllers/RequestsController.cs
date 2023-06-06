using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    // GET: Requests
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> AdminPanel()
    {
        return View(await _unitOfWork.Request.GetAll());
    }

    // GET: Requests
    public async Task<IActionResult> MyRequests()
    {
        var user = await _unitOfWork.Employee.GetCurrent(User);

        if (user is null)
        {
            return Unauthorized();
        }

        return View(await _unitOfWork.Request.GetByUser(user.Id));
    }

    // GET: Requests/Review/5
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

        var user = await _unitOfWork.Employee.GetCurrent(User);

        if (user is null)
        {
            return Unauthorized();
        }

        var isAdmin = await _unitOfWork.Employee.isAdmin(user);

        if (!isAdmin && request.CreatedBy.Id != user.Id)
        {
            return Forbid();
        }

        return View(request);
    }

    // GET: Requests/Create
    public async Task<IActionResult> CreateRequest()
    {
        ViewBag.LeaveTypes = await _unitOfWork.LeaveType.GetAll();

        return View();
    }

    // POST: Requests/Create
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

        var isAdmin = await _unitOfWork.Employee.isAdmin(author);

        return isAdmin ?
            RedirectToAction(nameof(AdminPanel))
            :
            Redirect("~/");
    }

    // GET: Requests/Create
    public async Task<IActionResult> CreateResponse(int? id)
    {
        if (!await _unitOfWork.Employee.isAuthorized(User))
        {
            return Unauthorized();
        }

        if (id is null)
        {
            return NotFound();
        }

        return View();
    }

    // POST: Requests/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateResponse(ResponseCreate responseData)
    {
        var user = await _unitOfWork.Employee.GetCurrent(User);

        if (user is null || !await _unitOfWork.Employee.isAdmin(user))
        {
            return Unauthorized();
        }

        var request = await _unitOfWork.Request.GetById(responseData.ID);

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

    // GET: Requests/Edit/5
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

        var requestData = _mapper.Map<RequestEdit>(request);

        return View(requestData);
    }

    // POST: Requests/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RequestEdit requestData)
    {
        if (id != requestData.ID)
        {
            return NotFound();
        }

        var user = await _unitOfWork.Employee.GetCurrent(User);

        if (user is null)
        {
            return Unauthorized();
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

        var isAdmin = await _unitOfWork.Employee.isAdmin(user);

        if (!isAdmin && requestEnitity.CreatedBy.Id != user.Id)
        {
            return Forbid();
        }

        _mapper.Map(requestData, requestEnitity);
        requestEnitity.LeaveType = leaveType;

        _unitOfWork.Request.Update(requestEnitity);
        await _unitOfWork.SaveChangesAsync();

        return isAdmin ?
            RedirectToAction(nameof(AdminPanel))
            :
            RedirectToAction(nameof(MyRequests));
    }

    // GET: Requests/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        var user = await _unitOfWork.Employee.GetCurrent(User);

        if (user is null)
        {
            return Unauthorized();
        }

        if (id is null)
        {
            return NotFound();
        }

        var request = await _unitOfWork.Request.GetById((int)id);
        var isAdmin = await _unitOfWork.Employee.isAdmin(user);

        if (request is null)
        {
            return NotFound();
        }

        if (!isAdmin && request.CreatedBy.Id != user.Id)
        {
            return Forbid();
        }

        return View(request);
    }

    // POST: Requests/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _unitOfWork.Employee.GetCurrent(User);

        if (user is null)
        {
            return Unauthorized();
        }

        var isAdmin = await _unitOfWork.Employee.isAdmin(user);
        var request = await _unitOfWork.Request.GetById(id);

        var redirect = RedirectToAction(isAdmin ?
                                        nameof(AdminPanel)
                                        :
                                        nameof(MyRequests));
        if (request is null)
        {
            return redirect;
        }

        if (!isAdmin && request.CreatedBy.Id != user.Id)
        {
            return Forbid();
        }

        _unitOfWork.Request.Delete(request);
        await _unitOfWork.SaveChangesAsync();

        return redirect;
    }
}
