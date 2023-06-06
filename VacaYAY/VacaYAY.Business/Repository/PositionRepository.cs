using Microsoft.EntityFrameworkCore;
using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Repository;

public class PositionRepository : RepositoryBase<Position>, IPositionRepository
{
    private readonly Context _context;
    public PositionRepository(Context context) 
        : base(context)
    {
        _context = context;
    }

    public async Task<Position?> GetByCaption(string caption)
    {
        return await _context.Positions.FirstOrDefaultAsync(p => p.Caption == caption);
    }
}

