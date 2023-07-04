using VacaYAY.Data.Entities;

namespace VacaYAY.Business.ServiceContracts;

public interface IPositionService
{
    Task<IEnumerable<Position>> GetAll();
}
