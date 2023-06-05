using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Contracts;

public interface IPositionRepository : IRepositoryBase<Position>
{
    Task<Position?> GetByCaption(string caption);
}

