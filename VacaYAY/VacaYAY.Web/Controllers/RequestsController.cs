using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.Contracts;
using VacaYAY.Business.Contracts.ServiceContracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Web.Controllers;

[Authorize]
public class RequestsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotifierSerivice _notifierService;
    private readonly IMapper _mapper;

    public RequestsController(
        IUnitOfWork unitOfWork,
        INotifierSerivice notifierService,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _notifierService = notifierService;
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

        ViewBag.DaysOffNumber = user.DaysOffNumber;
        ViewBag.OldDaysOffNumber = user.OldDaysOffNumber;

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

        RequestCreate model = new()
        {
            LeaveTypes = await _unitOfWork.LeaveType.GetAll(),
            NewDaysOff = user.DaysOffNumber,
            OldDaysOff = user.OldDaysOffNumber,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(2)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRequest(RequestCreate requestData)
    {
        var author = await _unitOfWork.Employee.GetCurrent(User);
        if (author is null)
        {
            return Unauthorized();
        }

        requestData.LeaveTypes = await _unitOfWork.LeaveType.GetAll();
        requestData.NewDaysOff = author.DaysOffNumber;
        requestData.OldDaysOff = author.OldDaysOffNumber;

        var leaveType = await _unitOfWork.LeaveType.GetById(requestData.LeaveTypeID);
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

        RequestEmailTemplates templates = new(author, requestEntity);

        bool isNotified = await _notifierService.NotifyEmployee(templates.Created);
        await _notifierService.NotifyHRTeam(templates.HRCreated);

        requestEntity.NotificationStatus = isNotified ?
            NotificationStatus.Notified
            :
            NotificationStatus.NotNotifiedOfCreation;

        return Redirect(nameof(MyRequests));
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

        ViewBag.Request = request;

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
            RequestID = request.ID,
            Request = request,
            ReviewedBy = reviewer
        };

        _unitOfWork.Response.Insert(response);

        var seeker = request.CreatedBy;
        if (response.IsApproved)
        {
            var distrib = _unitOfWork.Request.GetDaysOffDistribution(
                seeker.OldDaysOffNumber,
                seeker.DaysOffNumber,
                request);

            seeker.OldDaysOffNumber -= distrib.removeFromOldDays;
            seeker.DaysOffNumber -= distrib.removeFromNewDays;

            response.NumOfDaysRemovedFromNewDaysOff = distrib.removeFromNewDays;
            response.NumOfDaysRemovedFromOldDaysOff = distrib.removeFromOldDays;

            _unitOfWork.Employee.Update(seeker);
        }

        request.Response = response;
        request.LeaveType = leaveType;

        RequestEmailTemplates templates = new(seeker, request);
        var isNotified = await _notifierService
            .NotifyEmployee(response.IsApproved ? templates.Approved : templates.Rejected, true);

        request.NotificationStatus = isNotified ?
            NotificationStatus.Notified
            :
            NotificationStatus.NotNotifiedOfReponse;

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

        requestData.Response = requestEntity.Response;
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
        var seeker = requestEntity.CreatedBy;

        if (response is not null)
        {
            if (requestEntity.Status is RequestStatus.Approved)
            {
                seeker.DaysOffNumber += response.NumOfDaysRemovedFromNewDaysOff;
                seeker.OldDaysOffNumber += response.NumOfDaysRemovedFromOldDaysOff;
            }

            _unitOfWork.Response.Delete(response);
            _unitOfWork.Employee.Update(seeker);
        }

        _mapper.Map(requestData, requestEntity);
        requestEntity.LeaveType = leaveType;

        RequestEmailTemplates templates = new(seeker, requestEntity);

        var isNotified = await _notifierService.NotifyEmployee(templates.Edited);
        await _notifierService.NotifyHRTeam(templates.HREdited);

        requestEntity.NotificationStatus = isNotified ?
            NotificationStatus.Notified
            :
            NotificationStatus.NotNotifiedOfChange;

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

        var request = await _unitOfWork.Request.GetById(response.RequestID);
        ViewBag.Request = request;

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

        var request = await _unitOfWork.Request.GetById(responseData.RequestID);

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

        var oldStatus = request.Status;
        var newStatus = responseData.IsApproved ?
                        RequestStatus.Approved
                        :
                        RequestStatus.Rejected;

        if (oldStatus != newStatus)
        {
            if (oldStatus is RequestStatus.Rejected
                && newStatus is RequestStatus.Approved)
            {
                var distrib = _unitOfWork.Request.GetDaysOffDistribution(
                    seeker.OldDaysOffNumber,
                    seeker.DaysOffNumber,
                    request);

                seeker.OldDaysOffNumber -= distrib.removeFromOldDays;
                seeker.DaysOffNumber -= distrib.removeFromNewDays;

                response.NumOfDaysRemovedFromNewDaysOff = distrib.removeFromNewDays;
                response.NumOfDaysRemovedFromOldDaysOff = distrib.removeFromOldDays;
            }

            if (oldStatus is RequestStatus.Approved
                && newStatus is RequestStatus.Rejected)
            {
                seeker.DaysOffNumber += response.NumOfDaysRemovedFromNewDaysOff;
                seeker.OldDaysOffNumber += response.NumOfDaysRemovedFromOldDaysOff;
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

    public async Task<IActionResult> Delete(int id)
    {
        var author = await _unitOfWork.Employee.GetCurrent(User);
        if (author is null)
        {
            return Unauthorized();
        }

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

        RequestEmailTemplates templates = new(author, request);
        await _notifierService.NotifyEmployee(templates.Deleted);
        await _notifierService.NotifyHRTeam(templates.HRDeleted);

        return redirect;
    }
}
