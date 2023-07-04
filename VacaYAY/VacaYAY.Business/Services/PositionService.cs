using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.Entities;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Business.Services;

public class PositionService : IPositionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PositionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<IEnumerable<Position>> GetAll()
    {
        return _unitOfWork.Position.GetAll();
    }
}
