using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using VacaYAY.Business.Contracts.RepositoryContracts;
using VacaYAY.Data;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Repository;

public class LeaveTypeRepository : RepositoryBase<LeaveType>, ILeaveTypeRepository
{
    private readonly Context _context;
    public LeaveTypeRepository(Context context)
        : base(context)
    {
        _context = context;
    }

    public async Task<LeaveType?> GetByCaption(string caption)
    {
        return await _context.LeaveTypes
                        .Where(t => t.Caption == caption)
                        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<LeaveType>> GetRestrictedAll()
    {
        return await _context.LeaveTypes
                        .Where(l => l.Caption != VacationType.CollectiveVacation)
                        .ToListAsync();
    }

    public List<CustomValidationResult> Validate(LeaveType leaveType)
    {
        List<CustomValidationResult> errors = new();

        if (char.IsDigit(leaveType.Caption[0]) || !Regex.IsMatch(leaveType.Caption, @"^(?! )[A-Za-z0-9 ]+$"))
        {
            errors.Add(new()
            {
                Property = nameof(LeaveType.Caption),
                Text = "The leave type name cannot start with a number and must not contain special characters."
            });
        }

        return errors;
    }
}
