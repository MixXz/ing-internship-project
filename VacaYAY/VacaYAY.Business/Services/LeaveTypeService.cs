using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;
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

    public async Task<ServiceResult<LeaveType>> Create(LeaveType leaveType)
    {
        ServiceResult<LeaveType> result = new();

        var validationErrors = _unitOfWork.LeaveType.Validate(leaveType);

        if (validationErrors.Any())
        {
            result.Errors.AddRange(validationErrors);
            return result;
        }

        _unitOfWork.LeaveType.Insert(leaveType);
        await _unitOfWork.SaveChangesAsync();

        result.Entity = leaveType;
        return result;
    }

    public async Task<ServiceResult<LeaveType>> Update(LeaveType leaveType)
    {
        ServiceResult<LeaveType> result = new();

        var validationErrors = _unitOfWork.LeaveType.Validate(leaveType);

        if (validationErrors.Any())
        {
            result.Errors.AddRange(validationErrors);
            return result;
        }

        _unitOfWork.LeaveType.Update(leaveType);
        await _unitOfWork.SaveChangesAsync();

        result.Entity = leaveType;
        return result;
    }

    public async Task<ServiceResult<LeaveType>> Delete(int id)
    {
        ServiceResult<LeaveType> result = new();

        var leaveType = await GetById(id);

        if (leaveType is null)
        {
            result.Errors.Add(new()
            {
                Text = "Leave type not found in database."
            });
            return result;
        }

        _unitOfWork.LeaveType.Delete(leaveType);
        await _unitOfWork.SaveChangesAsync();

        result.Entity = leaveType;

        return result;
    }
}
