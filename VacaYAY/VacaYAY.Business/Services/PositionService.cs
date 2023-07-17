using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Business.Services;

public class PositionService : IPositionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PositionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Position>> GetAll()
    {
        return await _unitOfWork.Position.GetAll();
    }

    public async Task<Position?> GetById(int id)
    {
        return await _unitOfWork.Position.GetById(id);
    }

    public async Task<ServiceResult<Position>> Create(Position position)
    {
        ServiceResult<Position> result = new();

        var validationErrors = _unitOfWork.Position.Validate(position);

        if (validationErrors.Any())
        {
            result.Errors = validationErrors;
            return result;
        }

        _unitOfWork.Position.Insert(position);
        await _unitOfWork.SaveChangesAsync();

        result.Entity = position;
        return result;
    }

    public async Task<ServiceResult<Position>> Update(Position position)
    {
        ServiceResult<Position> result = new();

        var validationErrors = _unitOfWork.Position.Validate(position);

        if (validationErrors.Any())
        {
            result.Errors = validationErrors;
            return result;
        }

        _unitOfWork.Position.Update(position);
        await _unitOfWork.SaveChangesAsync();

        result.Entity = position;
        return result;
    }

    public async Task<ServiceResult<Position>> Delete(int id)
    {
        ServiceResult<Position> result = new();

        var position = await GetById(id);

        if (position is null)
        {
            result.Errors.Add(new()
            {
                Text = "Position not found in database."
            });
            return result;
        }

        _unitOfWork.Position.Delete(position);
        await _unitOfWork.SaveChangesAsync();

        result.Entity = position;

        return result;
    }
}
