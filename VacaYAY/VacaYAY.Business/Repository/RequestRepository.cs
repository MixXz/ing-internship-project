using Microsoft.EntityFrameworkCore;
using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
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
            && !filters.ShowPendingOnly)
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

        if (filters.ShowPendingOnly)
        {
            requests = requests
                        .Where(r => r.Response == null);
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

    public List<CustomValidationResult> ValidateOnCreate(RequestCreate regData, Employee user)
    {
        List<CustomValidationResult> errors = new();
        var requestedLeaveDays = (regData.EndDate - regData.StartDate).TotalDays;

        if (user.DaysOfNumber is 0)
        {
            errors.Add(new()
            {
                Property = string.Empty,
                Text = "You have no more free days left."
            });
        }

        if (regData.StartDate <= DateTime.Now)
        {
            errors.Add(new()
            {
                Property = nameof(regData.StartDate),
                Text = "The starting date cannot be in the past"
            });
        }

        if (requestedLeaveDays > user.DaysOfNumber)
        {
            errors.Add(new()
            {
                Property = nameof(regData.EndDate),
                Text = "The selected number of days exceeds the available number of free days."
            });
        }

        var hasAnOverlappingRequest = _context.Requests
                       .Include(r => r.CreatedBy)
                       .Any(r => (r.StartDate <= regData.EndDate) 
                                && (r.EndDate >= regData.StartDate)
                                && (r.CreatedBy.Id == user.Id));

        if(hasAnOverlappingRequest)
        {
            errors.Add(new()
            {
                Property = string.Empty,
                Text = "You already have a request that overlaps with the defined scope."
            });
        }

        return errors;
    }
}