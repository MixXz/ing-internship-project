using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Contracts;

public interface IRequestRepository : IRepositoryBase<Request>
{
    Task<IEnumerable<Request>> GetByUser(string userId);
}

