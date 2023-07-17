using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.ServiceContracts;

public interface ILeaveTypeService
{
    Task<LeaveType?> GetById(int id);
    Task<IEnumerable<LeaveType>> GetAll(bool restricted = true);
    Task<ServiceResult<LeaveType>> Create(LeaveType leaveType);
    Task<ServiceResult<LeaveType>> Update(LeaveType leaveType);
    Task<ServiceResult<LeaveType>> Delete(int id);
}
