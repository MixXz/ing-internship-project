using Microsoft.EntityFrameworkCore;
using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Repository;

public class RequestRepository : RepositoryBase<Request>, IRequestRepository
{
    private readonly Context _context;
    public RequestRepository(Context context)
    : base(context)
    {
        _context = context;
    }

    public override async Task<Request?> GetById(int id)
    {
        return await _context.Requests
                        .Include(r => r.LeaveType)
                        .Include(r => r.Response)
                        .Include(r => r.CreatedBy)
                        .FirstOrDefaultAsync(r => r.ID == id);
    }

    public override async Task<IEnumerable<Request>> GetAll()
    {
        return await _context.Requests
                        .Include(r => r.LeaveType)
                        .Include(r => r.Response)
                        .Include(r => r.CreatedBy)
                        .ToListAsync();
    }

    public async Task<IEnumerable<Request>> GetByUser(string userId)
    {
        return await _context.Requests
                        .Include(r => r.LeaveType)
                        .Include(r => r.Response)
                        .Where(r => r.CreatedBy.Id == userId)
                        .ToListAsync();
    }
}