using VacaYAY.Data.Entities;

namespace VacaYAY.Business.ServiceContracts;

public interface ILeaveTypeService
{
    Task<LeaveType?> GetById(int id);
    Task<IEnumerable<LeaveType>> GetAll(bool restricted = true);
}
