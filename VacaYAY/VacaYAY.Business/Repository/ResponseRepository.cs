using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Repository;

public class ResponseRepository : RepositoryBase<Response>, IResponseRepository
{
    private readonly Context _context;
    public ResponseRepository(Context context)
        : base(context)
    {
        _context = context;
    }
}

