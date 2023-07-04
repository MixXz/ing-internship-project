using AutoMapper;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Helpers;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Business.Services;

public class RequestService : IRequestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotifierSerivice _notifierService;
    private readonly IMapper _mapper;

    public RequestService(
        IUnitOfWork unitOfWork,
        INotifierSerivice notifierService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _notifierService = notifierService;
        _mapper = mapper;
    }

    public async Task<Request?> GetById(int id)
    {
        return await _unitOfWork.Request.GetById(id);
    }

    public async Task<IEnumerable<Request>> GetAll()
    {
        return await _unitOfWork.Request.GetAll();
    }

    public async Task<IEnumerable<Request>> GetByFilters(RequestView filters)
    {
        return await _unitOfWork.Request.GetByFilters(filters);
    }

    public async Task<IEnumerable<Request>> GetByEmployeeId(string id)
    {
        return await _unitOfWork.Request.GetByUser(id);
    }

    public Task<Response?> GetResponseById(int id)
    {
        return _unitOfWork.Response.GetById(id);
    }

    public async Task<ServiceResult<Request>> Create(RequestCreate requestData, Employee author)
    {
        ServiceResult<Request> result = new();

        var leaveType = await _unitOfWork.LeaveType.GetById(requestData.LeaveTypeID);
        if (leaveType is null)
        {
            result.Errors.Add(new()
            {
                Property = nameof(requestData.LeaveTypeID),
                Text = "Leave type invalid."
            });
            return result;
        }

        var errors = await _unitOfWork.Request.ValidateOnCreate(requestData, author);

        if (errors.Any())
        {
            result.Errors.AddRange(errors);
            return result;
        }

        Request requestEntity = new()
        {
            LeaveType = leaveType,
            StartDate = requestData.StartDate,
            EndDate = requestData.EndDate,
            CreatedBy = author,
            Comment = requestData.Comment,
        };

        RequestEmailTemplates templates = new(author, requestEntity);

        bool isNotified = await _notifierService.NotifyEmployee(templates.Created);
        await _notifierService.NotifyHRTeam(templates.HRCreated);

        requestEntity.NotificationStatus = isNotified ?
            NotificationStatus.Notified
            :
            NotificationStatus.NotNotifiedOfCreation;

        _unitOfWork.Request.Insert(requestEntity);
        await _unitOfWork.SaveChangesAsync();

        result.Entity = requestEntity;

        return result;
    }

    public async Task<ServiceResult<Request>> Edit(Request requestEntity, RequestEdit requestData)
    {
        ServiceResult<Request> result = new();

        var leaveType = await _unitOfWork.LeaveType.GetById(requestData.SelectedLeaveTypeID);

        if (leaveType is null)
        {
            result.Errors.Add(new()
            {
                Property = nameof(requestData.SelectedLeaveTypeID),
                Text = "Invalid leave type."
            });
            return result;
        }

        var validationErrors = await _unitOfWork.Request.ValidateOnEdit(requestData, requestEntity.CreatedBy);

        if (validationErrors.Any())
        {
            result.Errors.AddRange(validationErrors);
            return result;
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

        result.Entity = requestEntity;

        return result;
    }

    public async Task<ServiceResult<Request>> Delete(Request request)
    {
        _unitOfWork.Request.Delete(request);
        await _unitOfWork.SaveChangesAsync();

        RequestEmailTemplates templates = new(request.CreatedBy, request);
        await _notifierService.NotifyEmployee(templates.Deleted);
        await _notifierService.NotifyHRTeam(templates.HRDeleted);

        return new ServiceResult<Request> { Entity = request };
    }

    public async Task<ServiceResult<Request>> CreateResponse(Employee reviewer, Request request, ResponseCreate responseData)
    {
        ServiceResult<Request> result = new();

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

        var leaveType = await _unitOfWork.LeaveType.GetById(responseData.SelectedLeaveTypeID);

        if (leaveType is null)
        {
            result.Errors.Add(new()
            {
                Property = nameof(responseData.SelectedLeaveTypeID),
                Text = "Leave type invalid."
            });

            return result;
        }

        request.Response = response;
        request.LeaveType = leaveType;

        RequestEmailTemplates templates = new(seeker, request);
        var isNotified = await _notifierService.NotifyEmployee(response.IsApproved ?
            templates.Approved
            :
            templates.Rejected, true);

        request.NotificationStatus = isNotified ?
            NotificationStatus.Notified
            :
            NotificationStatus.NotNotifiedOfReponse;

        _unitOfWork.Request.Update(request);
        await _unitOfWork.SaveChangesAsync();

        result.Entity = request;

        return result;
    }

    public async Task<ServiceResult<Request>> EditResponse(Request request, Response response, ResponseEdit responseData, Employee reviewer)
    {
        ServiceResult<Request> result = new();

        var leaveType = await _unitOfWork.LeaveType.GetById(responseData.SelectedLeaveTypeID);

        if (leaveType is null)
        {
            result.Errors.Add(new()
            {
                Property = nameof(responseData.SelectedLeaveTypeID),
                Text = "Invalid leave type."
            });
            return result;
        }

        var seeker = request.CreatedBy;

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

        result.Entity = request;

        return result;
    }

    public RequestEdit ConvertToEditDto(Request request)
    {
        return _mapper.Map<RequestEdit>(request);
    }

    public ResponseEdit ConvertToEditDto(Response response)
    {
        return _mapper.Map<ResponseEdit>(response);
    }

    public async Task<ServiceResult<Request>> CreateCollectiveVacation(Employee author, CollectiveVacationCreate data)
    {
        ServiceResult<Request> result = new();

        if (data.StartDate > data.EndDate)
        {
            result.Errors.Add(new()
            {
                Text = "Vacation not created, end date cannot be earlier than the start date."
            });
            return result;
        }

        var leaveType = await _unitOfWork.LeaveType.GetByCaption(VacationType.CollectiveVacation);

        if (leaveType is null)
        {
            result.Errors.Add(new()
            {
                Text = "Invalid leave type."
            });
            return result;
        }

        Request req = new()
        {
            LeaveType = leaveType,
            StartDate = data.StartDate,
            EndDate = data.EndDate,
            Comment = data.Comment,
            CreatedBy = author
        };

        Response resp = new()
        {
            IsApproved = true,
            Request = req,
            ReviewedBy = author
        };

        req.Response = resp;

        _unitOfWork.Request.Insert(req);
        _unitOfWork.Response.Insert(resp);

        var employees = await _unitOfWork.Employee.GetAll();

        foreach (var emp in employees)
        {
            var distrib = _unitOfWork.Request.GetDaysOffDistribution(emp.OldDaysOffNumber, emp.DaysOffNumber, req);

            emp.DaysOffNumber = Math.Max(emp.DaysOffNumber - distrib.removeFromNewDays, 0);
            emp.OldDaysOffNumber = Math.Max(emp.OldDaysOffNumber - distrib.removeFromOldDays, 0);

            RequestEmailTemplates templates = new(emp, req);
            await _notifierService.NotifyEmployee(templates.CollectiveVacation);

            _unitOfWork.Employee.Update(emp);
        }

        await _unitOfWork.SaveChangesAsync();

        result.Entity = req;

        return result;
    }
}
