using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Contracts.RepositoryContracts;

public interface IPositionRepository : IRepositoryBase<Position>
{
    Task<Position?> GetByCaption(string caption);
}

