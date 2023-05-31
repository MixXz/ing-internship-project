using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Contracts;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
    Task<Employee?> GetEmployeeById(string id);
}

