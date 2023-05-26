using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Contracts;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
    Task<IEnumerable<Employee>> GetAll();
}

