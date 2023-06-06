using VacaYAY.Data.Entities;

namespace VacaYAY.API.Contracts;

public interface IDataGenService
{
    Position GeneratePosition();
    List<Position> GeneratePositions(int count);
    List<Employee> GenerateEmployees(int count);
}
