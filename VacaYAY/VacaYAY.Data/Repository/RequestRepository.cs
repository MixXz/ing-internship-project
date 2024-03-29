﻿using Microsoft.EntityFrameworkCore;
using VacaYAY.Data.DataTransferObjects.Requests;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Helpers;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Data.Repository;

internal class RequestRepository : RepositoryBase<Request>, IRequestRepository
{
    private readonly Context _context;
    public RequestRepository(Context context)
    : base(context)
    {
        _context = context;
    }

    public override async Task<Request?> GetById(int id)
    {
        return await _context.Requests
                        .Include(r => r.LeaveType)
                        .Include(r => r.Response)
                        .Include(r => r.CreatedBy)
                        .FirstOrDefaultAsync(r => r.ID == id);
    }

    public override async Task<IEnumerable<Request>> GetAll()
    {
        return await _context.Requests
                        .Include(r => r.LeaveType)
                        .Include(r => r.Response)
                        .Include(r => r.CreatedBy)
                        .OrderByDescending(r => r.Response == null)
                        .ToListAsync();
    }

    public async Task<IEnumerable<Request>> GetByFilters(RequestView filters)
    {
        var requests = _context.Requests
                        .Include(r => r.LeaveType)
                        .Include(r => r.Response)
                        .Include(r => r.CreatedBy)
                        .AsQueryable();

        var query = requests.Where(q => false);

        if (!string.IsNullOrEmpty(filters.SearchInput))
        {
            var tokens = filters.SearchInput.Trim().Split(' ');

            foreach (var token in tokens)
            {
                query = query
                        .Union(requests.Where(r => r.CreatedBy.FirstName.Contains(token)
                                                || r.CreatedBy.LastName.Contains(token)));
            }
        }

        if (filters.SelectedLeaveTypeID is not null)
        {
            requests = requests
                        .Where(r => r.LeaveType.ID == filters.SelectedLeaveTypeID);
        }

        switch (filters.Status)
        {
            case RequestStatus.Pending:
                requests = requests
                        .Where(r => r.Response == null);
                break;
            case RequestStatus.Approved:
                requests = requests
                        .Where(r => (r.Response != null && r.Response.IsApproved == true));
                break;
            case RequestStatus.Rejected:
                requests = requests
                        .Where(r => (r.Response != null && r.Response.IsApproved == false));
                break;
            default:
                break;
        }

        if (filters.StartDateFilter is not null)
        {
            requests = requests
                        .Where(r => r.StartDate >= filters.StartDateFilter);
        }

        if (filters.EndDateFilter is not null)
        {
            requests = requests
                        .Where(r => r.EndDate <= filters.EndDateFilter);
        }

        if (query.Any())
        {
            requests = requests.Intersect(query);
        }

        return await requests
                    .OrderByDescending(r => r.Response == null)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Request>> GetByUser(string userId)
    {
        return await _context.Requests
                        .Include(r => r.LeaveType)
                        .Include(r => r.Response)
                        .Where(r => r.CreatedBy.Id == userId
                                 || r.LeaveType.Caption == VacationType.CollectiveVacation)
                        .OrderByDescending(r => r.Response == null)
                        .ToListAsync();
    }

    public async Task<IEnumerable<Request>> GetRequestsWhereAuthorIsntNotified()
    {
        return await _context.Requests
                        .Include(r => r.CreatedBy)
                        .Where(r => r.NotificationStatus != NotificationStatus.Notified)
                        .ToListAsync();
    }

    public (int removeFromOldDays, int removeFromNewDays) GetDaysOffDistribution(int empOldDays, int empNewDays, Request request)
    {
        int requestedDays = request.NumOfDaysRequested;
        int removeFromOldDays = 0;
        int removeFromNewDays = 0;

        var jan1st = new DateTime(request.StartDate.Year, 1, 1);
        var june30th = new DateTime(request.StartDate.Year, 6, 30);

        var isInOldDaysSpan = request.StartDate >= jan1st && request.EndDate <= june30th;

        if (isInOldDaysSpan && requestedDays <= empOldDays)
        {
            removeFromOldDays = requestedDays;
            return (removeFromOldDays, removeFromNewDays);
        }

        if (request.StartDate > june30th)
        {
            removeFromNewDays = requestedDays;
            return (removeFromOldDays, removeFromNewDays);
        }

        if (request.StartDate >= jan1st && request.StartDate <= june30th)
        {
            var daysInOldDaysSpan = (june30th - request.StartDate).Days + 1;
            removeFromOldDays = Math.Min(empOldDays, daysInOldDaysSpan);
            removeFromNewDays = requestedDays - removeFromOldDays;
            return (removeFromOldDays, removeFromNewDays);
        }

        return (removeFromOldDays, removeFromNewDays);
    }

    public async Task<List<CustomValidationResult>> ValidateOnCreate(RequestCreate reqData, Employee user)
    {
        var totalNumberOfDaysRequested = await GetNumOfRequestedDays(user.Id);
        var availableDays = user.OldDaysOffNumber + user.DaysOffNumber - totalNumberOfDaysRequested;

        var errors = ValidateDates(
            reqData.StartDate,
            reqData.EndDate,
            user.OldDaysOffNumber,
            user.DaysOffNumber,
            availableDays,
            reqData.NumOfDaysRequested,
            totalNumberOfDaysRequested);

        if (CheckForOverlappingRequest(reqData.StartDate, reqData.EndDate, user.Id))
        {
            errors.Add(new()
            {
                Property = string.Empty,
                Text = "You already have a request that overlaps with the defined scope."
            });
        }

        return errors;
    }

    public async Task<List<CustomValidationResult>> ValidateOnEdit(RequestEdit reqData, Employee user)
    {
        var totalNumberOfDaysRequested = await GetNumOfRequestedDays(user.Id, reqData.ID);
        var availableDays = user.OldDaysOffNumber + user.DaysOffNumber + reqData.NumOfDaysRequested - totalNumberOfDaysRequested;

        var oldDays = user.OldDaysOffNumber;
        var newDays = user.DaysOffNumber;

        if (reqData.Response is not null)
        {
            oldDays += reqData.Response.NumOfDaysRemovedFromOldDaysOff;
            newDays += reqData.Response.NumOfDaysRemovedFromNewDaysOff;
        }

        var errors = ValidateDates(
            reqData.StartDate,
            reqData.EndDate,
            oldDays,
            newDays,
            availableDays,
            reqData.NumOfDaysRequested,
            totalNumberOfDaysRequested);

        if (CheckForOverlappingRequest(reqData.StartDate, reqData.EndDate, user.Id, reqData.ID))
        {
            errors.Add(new()
            {
                Property = string.Empty,
                Text = "You already have a request that overlaps with the defined scope."
            });
        }

        return errors;
    }

    private async Task<int> GetNumOfRequestedDays(string employeeId, int? requestIdToExclude = null)
    {
        var days = await _context.Requests
                        .Where(r => (r.ID != requestIdToExclude)
                                    && (r.CreatedBy.Id == employeeId)
                                    && (r.Response == null))
                        .SumAsync(r => EF.Functions.DateDiffDay(r.StartDate, r.EndDate));
        return days;
    }

    private bool CheckForOverlappingRequest(DateTime start, DateTime end, string authorId, int? requestIdToExclude = null)
    {
        var result = _context.Requests
                       .Any(r => (r.ID != requestIdToExclude)
                                && (r.StartDate <= end)
                                && (r.EndDate >= start)
                                && (r.CreatedBy.Id == authorId)
                                && (r.LeaveType.Caption != VacationType.CollectiveVacation));
        return result;
    }

    private bool CheckForOverlappingWithCollectiveVacation(DateTime start, DateTime end)
    {
        var result = _context.Requests
                       .Any(r => (r.StartDate <= end)
                                && (r.EndDate >= start)
                                && (r.LeaveType.Caption == VacationType.CollectiveVacation));
        return result;
    }

    private List<CustomValidationResult> ValidateDates(
        DateTime start,
        DateTime end,
        int oldDays,
        int newDays,
        int availableDays,
        int requestedDays,
        int totalRequestedDays)
    {
        List<CustomValidationResult> errors = new();

        if (availableDays is 0)
        {
            errors.Add(new()
            {
                Property = string.Empty,
                Text = totalRequestedDays > 0 ?
                    $"You have no more free days left, you have already requested {totalRequestedDays} days."
                    :
                    "You have no more free days left."
            });

            return errors;
        }

        if (requestedDays > availableDays)
        {
            errors.Add(new()
            {
                Property = string.Empty,
                Text = "The selected number of days exceeds the available number of free days."
            });

            return errors;
        }

        if (start <= DateTime.Now)
        {
            errors.Add(new()
            {
                Property = nameof(Request.StartDate),
                Text = "The starting date cannot be in the past."
            });
        }

        if (start >= end)
        {
            errors.Add(new()
            {
                Property = nameof(Request.EndDate),
                Text = "The end date cannot be earlier than the start date."
            });
        }

        var june30th = new DateTime(end.Year, 6, 30);

        if (start >= june30th)
        {
            if (newDays < requestedDays)
            {
                errors.Add(new()
                {
                    Property = string.Empty,
                    Text = "The selected number of days exceeds the available number of free days, your old days off expire after June 30th."
                });
            }
        }

        if (CheckForOverlappingWithCollectiveVacation(start, end))
        {
            errors.Add(new()
            {
                Property = string.Empty,
                Text = "You already have a collective vacation that overlaps with the defined scope."
            });
        }

        return errors;
    }
}