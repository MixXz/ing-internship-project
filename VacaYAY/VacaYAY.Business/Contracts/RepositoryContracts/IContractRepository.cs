using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Contracts.RepositoryContracts;

public interface IContractRepository : IRepositoryBase<Contract>
{
    Task<IEnumerable<Contract>> GetByEmployeeId(string employeeId);
    Task<ServiceResult<Contract>> Create(ContractCreate data, Employee employee);
    Task<ServiceResult<Contract>> Update(ContractEdit data);
    Task<string?> GetDocumentUrlByContractId(int contractId);
}
