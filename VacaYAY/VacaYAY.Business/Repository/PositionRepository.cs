using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using VacaYAY.Business.Contracts.RepositoryContracts;
using VacaYAY.Data;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

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

    public List<CustomValidationResult> Validate(Position position)
    {
        List<CustomValidationResult> errors = new();

        if (char.IsDigit(position.Caption[0]) || !Regex.IsMatch(position.Caption, @"^(?! )[A-Za-z0-9 ]+$"))
        {
            errors.Add(new()
            {
                Property = nameof(Position.Caption),
                Text = "The position name cannot start with a number and must not contain special characters."
            });
        }

        return errors;
    }
}

