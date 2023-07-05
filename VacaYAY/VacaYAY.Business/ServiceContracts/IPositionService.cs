using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.ServiceContracts;

public interface IPositionService
{
    Task<Position?> GetById(int id);
    Task<IEnumerable<Position>> GetAll();
    Task<ServiceResult<Position>> Create(Position position);
    Task<ServiceResult<Position>> Update(Position position);
    Task<ServiceResult<Position>> Delete(int id);
}
