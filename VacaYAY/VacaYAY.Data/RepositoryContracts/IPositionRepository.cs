using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Data.RepositoryContracts;

public interface IPositionRepository : IRepositoryBase<Position>
{
    Task<Position?> GetByCaption(string caption);
    List<CustomValidationResult> Validate(Position position);
}

