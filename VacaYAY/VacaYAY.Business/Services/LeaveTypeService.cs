using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.Entities;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Business.Services;

public class LeaveTypeService : ILeaveTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    public LeaveTypeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LeaveType?> GetById(int id)
    {
        return await _unitOfWork.LeaveType.GetById(id);
    }

    public async Task<IEnumerable<LeaveType>> GetAll(bool restricted = true)
    {
        return restricted ?
            await _unitOfWork.LeaveType.GetRestrictedAll()
            :
            await _unitOfWork.LeaveType.GetAll();
    }
}
