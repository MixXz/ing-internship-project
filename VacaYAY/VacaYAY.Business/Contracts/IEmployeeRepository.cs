using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Contracts;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
    Task<Employee?> GetEmployeeById(string id);
    Task<IEnumerable<Employee>> GetByFilters(string? searchInput, DateTime? startDate, DateTime? endDate);
}

