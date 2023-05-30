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
}