using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Enums;

namespace VacaYAY.Web.Controllers;

[Authorize]
public class RequestsController : BaseController
{
    private readonly IRequestService _requestService;
    private readonly IEmployeeService _employeeService;
    private readonly ILeaveTypeService _leaveTypeService;

    public RequestsController(
        IRequestService requestService,
        IEmployeeService employeeService,
        ILeaveTypeService leaveTypeService,
        INotyfService toaster
        ) : base(toaster)
    {
        _requestService = requestService;
        _employeeService = employeeService;
        _leaveTypeService = leaveTypeService;
    }

    [HttpGet]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> AdminPanel(RequestView filters)
    {
        RequestView view = new()
        {
            LeaveTypes = await _leaveTypeService.GetAll(restricted: false),
            Requests = await _requestService.GetByFilters(filters),
        };

        return View(view);
    }

    [HttpGet]
    public async Task<IActionResult> MyRequests()
    {
        var user = await _employeeService.GetCurrent(User);

        if (user is null)
        {
            return Unauthorized();
        }

        return View(await _requestService.GetByEmployeeId(user.Id));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var request = await _requestService.GetById(id);

        if (request is null)
        {
            return NotFound();
        }

        if (!await _employeeService.IsAuthorizedToSee(User, request.CreatedBy.Id))
        {
            return Unauthorized();
        }

        return View(request);
    }

    [HttpGet]
    public async Task<IActionResult> CreateRequest()
    {
        var user = await _employeeService.GetCurrent(User);

        if (user is null)
        {
            return Unauthorized();
        }

        RequestCreate model = new()
        {
            LeaveTypes = await _leaveTypeService.GetAll(),
            NewDaysOff = user.DaysOffNumber,
            OldDaysOff = user.OldDaysOffNumber,
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRequest(RequestCreate requestData)
    {
        var author = await _employeeService.GetCurrent(User);
        if (author is null)
        {
            return Unauthorized();
        }

        requestData.LeaveTypes = await _leaveTypeService.GetAll();
        requestData.NewDaysOff = author.DaysOffNumber;
        requestData.OldDaysOff = author.OldDaysOffNumber;

        var result = await _requestService.Create(requestData, author);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(requestData);
        }

        Notification("Vacation request successfully submitted.");

        return RedirectToAction(nameof(Details), new { result.Entity.ID });
    }

    [HttpGet]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> CreateResponse(int id)
    {
        var request = await _requestService.GetById(id);

        if (request is null)
        {
            return NotFound();
        }

        if (!await _employeeService.IsAuthorized(User))
        {
            return Forbid();
        }

        ResponseCreate model = new()
        {
            LeaveTypes = await _leaveTypeService.GetAll(),
            Request = request,
            SelectedLeaveTypeID = request.LeaveType.ID
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> CreateResponse(int id, ResponseCreate responseData)
    {
        var reviewer = await _employeeService.GetCurrent(User);

        if (reviewer is null)
        {
            return Unauthorized();
        }

        var request = await _requestService.GetById(id);

        if (request is null
            || request.Response is not null)
        {
            return NotFound();
        }

        responseData.LeaveTypes = await _leaveTypeService.GetAll();
        responseData.Request = request;

        var result = await _requestService.CreateResponse(reviewer, request, responseData);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(responseData);
        }

        Notification($"Vacation request successfully {request.Status.ToString().ToLower()}.");

        return RedirectToAction(nameof(AdminPanel));
    }

    [HttpGet]
    public async Task<IActionResult> EditRequest(int id)
    {
        var request = await _requestService.GetById(id);

        if (request is null)
        {
            return NotFound();
        }

        if (!await _employeeService.IsAuthorizedToSee(User, request.CreatedBy.Id))
        {
            return Forbid();
        }

        var requestData = _requestService.ConvertToEditDto(request);
        requestData.LeaveTypes = await _leaveTypeService.GetAll();
        requestData.SelectedLeaveTypeID = request.LeaveType.ID;

        return View(requestData);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRequest(RequestEdit requestData)
    {
        var requestEntity = await _requestService.GetById(requestData.ID);

        if (requestEntity is null)
        {
            return NotFound();
        }

        if (!await _employeeService.IsAuthorizedToSee(User, requestEntity.CreatedBy.Id))
        {
            return Forbid();
        }

        requestData.LeaveTypes = await _leaveTypeService.GetAll();
        requestData.Response = requestEntity.Response;

        var result = await _requestService.Edit(requestEntity, requestData);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(requestData);
        }

        Notification("Request successfully edited.");

        return RedirectToAction(nameof(Details), new { result.Entity.ID });
    }

    [HttpGet]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> EditResponse(int id)
    {
        var response = await _requestService.GetResponseById(id);

        if (response is null)
        {
            return NotFound();
        }

        var request = await _requestService.GetById(response.RequestID);

        if (request is null)
        {
            return NotFound();
        }

        if (!await _employeeService.IsAuthorized(User))
        {
            return Unauthorized();
        }

        var responseEdit = _requestService.ConvertToEditDto(response);
        responseEdit.Request = request;
        responseEdit.LeaveTypes = await _leaveTypeService.GetAll();
        responseEdit.SelectedLeaveTypeID = request.LeaveType.ID;

        return View(responseEdit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> EditResponse(int id, ResponseEdit responseData)
    {
        var reviewer = await _employeeService.GetCurrent(User);

        if (reviewer is null
            || !await _employeeService.IsAuthorized(User))
        {
            return Forbid();
        }

        var request = await _requestService.GetById(responseData.Request.ID);

        if (request is null
            || request.Response is null)
        {
            return NotFound();
        }

        var result = await _requestService.EditResponse(request, request.Response, responseData, reviewer);

        if (result.Entity is null)
        {
            HandleModelErrors(result.Errors);
            return View(responseData);
        }

        Notification("Response successfully edited.");

        return RedirectToAction(nameof(AdminPanel));
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _employeeService.GetCurrent(User);

        if (user is null)
        {
            return Unauthorized();
        }

        var request = await _requestService.GetById(id);

        if (request is null)
        {
            return NotFound();
        }

        if (!await _employeeService.IsAuthorizedToSee(User, request.CreatedBy.Id))
        {
            return Forbid();
        }

        await _requestService.Delete(request);

        Notification("Request successfully deleted.");

        return RedirectToAction(user.Id == request.CreatedBy.Id ?
            nameof(MyRequests)
            :
            nameof(AdminPanel));
    }

    [HttpPost]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> CreateCollectiveVacation(CollectiveVacationCreate data)
    {
        var user = await _employeeService.GetCurrent(User);
        if (user is null)
        {
            return Unauthorized();
        }

        if (!await _employeeService.IsAuthorized(User))
        {
            return Forbid();
        }

        var result = await _requestService.CreateCollectiveVacation(user, data);

        if (result.Entity is null)
        {
            HandleErrors(result.Errors);
            return RedirectToAction(nameof(AdminPanel));
        }

        Notification("Collective vacation successfully created.");

        return RedirectToAction(nameof(AdminPanel));
    }
}
