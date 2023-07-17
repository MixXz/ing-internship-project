using VacaYAY.Data.DataTransferObjects.Contracts;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.ServiceContracts;

public interface IContractService
{
    Task<Contract?> GetById(int id);
    Task<IEnumerable<Contract>> GetByEmployeeId(string id);
    Task<Stream> GetDocumentStream(string blobUrl);
    Task<(Stream data, string contentType)> DownloadDocument(string blobUrl);
    Task<ServiceResult<Contract>> Create(ContractCreate contractData, Employee employee);
    Task<ServiceResult<Contract>> Update(ContractEdit contractData);
    Task<ContractEdit> GetEditDto(int id);
}
