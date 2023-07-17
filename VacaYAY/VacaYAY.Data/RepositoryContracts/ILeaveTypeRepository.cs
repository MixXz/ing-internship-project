using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Data.RepositoryContracts;
public interface ILeaveTypeRepository : IRepositoryBase<LeaveType>
{
    List<CustomValidationResult> Validate(LeaveType leaveType);
    Task<LeaveType?> GetByCaption(string caption);
    Task<IEnumerable<LeaveType>> GetRestrictedAll();
}

