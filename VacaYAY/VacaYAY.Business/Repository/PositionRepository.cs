using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Repository;

public class PositionRepository : RepositoryBase<Position>, IPositionRepository
{
    public PositionRepository(Context context) 
        : base(context)
    {
    }
}

