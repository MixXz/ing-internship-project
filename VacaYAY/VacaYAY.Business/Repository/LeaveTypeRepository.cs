using System.Text.RegularExpressions;
using VacaYAY.Business.Contracts.RepositoryContracts;
using VacaYAY.Data;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Repository;

public class LeaveTypeRepository : RepositoryBase<LeaveType>, ILeaveTypeRepository
{
    public LeaveTypeRepository(Context context)
        : base(context)
    {
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
