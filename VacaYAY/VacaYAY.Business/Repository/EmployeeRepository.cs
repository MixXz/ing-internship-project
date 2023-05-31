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
                        .Where(e => e.DeleteDate == null)
                        .ToListAsync();
    }

    public async Task<Employee?> GetEmployeeById(string id)
    {
        return await _context.Employees
                        .Include(e => e.Position)
                        .Where(e => e.DeleteDate == null && e.Id == id)
                        .FirstOrDefaultAsync();
    }
}

