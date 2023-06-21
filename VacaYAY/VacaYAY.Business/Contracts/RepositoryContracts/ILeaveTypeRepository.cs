using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Contracts.RepositoryContracts;
public interface ILeaveTypeRepository : IRepositoryBase<LeaveType>
{
    List<CustomValidationResult> Validate(LeaveType leaveType);
}

