using VacaYAY.Data.Entities;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Data.Repository;

internal class ResponseRepository : RepositoryBase<Response>, IResponseRepository
{
    public ResponseRepository(Context context)
        : base(context)
    {
    }
}

