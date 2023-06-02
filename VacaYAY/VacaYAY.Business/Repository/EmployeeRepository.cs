using Microsoft.EntityFrameworkCore;
using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    private readonly Context _context;
    public EmployeeRepository(Context context)
        : base(context)
    {
        _context = context;
    }
    public override async Task<IEnumerable<Employee>> GetAll()
    {
        return await _context.Employees
                        .Include(e => e.Position)
                        .OrderByDescending(e => e.InsertDate)
                        .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByFilters(string? searchInput, DateTime? startDate, DateTime? endDate)
    {
        if ((string.IsNullOrEmpty(searchInput) || string.IsNullOrWhiteSpace(searchInput))
            && startDate is null
            && endDate is null)
        {
            return await GetAll();
        }

        var employees = _context.Employees
                        .Include(e => e.Position)
                        .AsQueryable();

        if (!string.IsNullOrEmpty(searchInput))
        {
            var tokens = searchInput.Split(',');

            employees = employees
                        .Where(e =>
                            e.FirstName.Contains(tokens[0])
                            || (tokens.Count() > 1 && e.FirstName.Contains(tokens[1]))
                            || e.LastName.Contains(tokens[0])
                            || (tokens.Count() > 1 && e.LastName.Contains(tokens[1])));
        }

        if (startDate is not null)
        {
            employees = employees.Where(e => e.EmployeeStartDate >= startDate);
        }

        if (endDate is not null)
        {
            employees = employees.Where(e => e.EmployeeEndDate <= endDate);
        }

        return await employees
                    .OrderByDescending(e => e.InsertDate)
                    .ToListAsync();
    }

    public async Task<Employee?> GetEmployeeById(string id)
    {
        return await _context.Employees
                        .Include(e => e.Position)
                        .Where(e => e.Id == id)
                        .FirstOrDefaultAsync();
    }
}

