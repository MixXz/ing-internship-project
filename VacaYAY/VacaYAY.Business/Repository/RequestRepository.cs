using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Repository;

public class RequestRepository : RepositoryBase<Request>, IRequestRepository
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
        if ((string.IsNullOrEmpty(filters.SearchInput) || string.IsNullOrWhiteSpace(filters.SearchInput))
            && filters.SelectedLeaveTypeID is null
            && filters.StartDateFilter is null
            && filters.EndDateFilter is null
            && filters.Status is RequestStatus.All)
        {
            return await GetAll();
        }

        var requests = _context.Requests
                        .Include(r => r.LeaveType)
                        .Include(r => r.Response)
                        .Include(r => r.CreatedBy)
                        .AsQueryable();

        if (!string.IsNullOrEmpty(filters.SearchInput))
        {
            var tokens = filters.SearchInput.Split(' ');

            requests = requests
                        .Where(r =>
                            r.CreatedBy.FirstName.Contains(tokens[0])
                            || (tokens.Count() > 1 && r.CreatedBy.FirstName.Contains(tokens[1]))
                            || r.CreatedBy.LastName.Contains(tokens[0])
                            || (tokens.Count() > 1 && r.CreatedBy.LastName.Contains(tokens[1]))
                            || r.Comment != null && r.Comment.Contains(tokens[0])
                            || (r.Comment != null && tokens.Count() > 1 && r.Comment.Contains(tokens[1])));
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

        return await requests
                    .OrderByDescending(r => r.Response == null)
                    .ToListAsync();
    }

    public async Task<IEnumerable<Request>> GetByUser(string userId)
    {
        return await _context.Requests
                        .Include(r => r.LeaveType)
                        .Include(r => r.Response)
                        .Where(r => r.CreatedBy.Id == userId)
                        .OrderByDescending(r => r.Response == null)
                        .ToListAsync();
    }

    public async Task<List<CustomValidationResult>> ValidateOnCreate(RequestCreate reqData, Employee user)
    {
        var totalNumberOfDaysRequested = await GetNumOfRequestedDays(user.Id);
        var availableDays = user.DaysOffNumber - totalNumberOfDaysRequested;

        var errors = ValidateDates(reqData.StartDate, reqData.EndDate, reqData.NumOfDaysRequested, availableDays, totalNumberOfDaysRequested);

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
        var availableDays = user.DaysOffNumber - totalNumberOfDaysRequested;

        var errors = ValidateDates(reqData.StartDate, reqData.EndDate, reqData.NumOfDaysRequested, availableDays, totalNumberOfDaysRequested);

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
                                && (r.CreatedBy.Id == authorId));
        return result;
    }

    private List<CustomValidationResult> ValidateDates(DateTime start, DateTime end, int requestedDays, int availableDays, int totalRequestedDays)
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

        return errors;
    }
}