using VacaYAY.Data.DataTransferObjects.Contracts;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Data.RepositoryContracts;

public interface IContractRepository : IRepositoryBase<Contract>
{
    Task<IEnumerable<Contract>> GetByEmployeeId(string employeeId);
    Task<ServiceResult<Contract>> Create(ContractCreate data, Employee employee);
    Task<ServiceResult<Contract>> Update(ContractEdit data);
    Task<string?> GetDocumentUrlByContractId(int contractId);
}
