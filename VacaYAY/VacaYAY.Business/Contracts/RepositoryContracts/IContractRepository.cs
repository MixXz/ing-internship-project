using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Contracts.RepositoryContracts;

public interface IContractRepository : IRepositoryBase<Contract>
{
    Task<ServiceResult<Contract>> Create(ContractCreate data, Employee employee);
}
