using AutoMapper;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.DataServiceContracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Business.Services;

public class ContractService : IContractService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobService _blobService;
    private readonly IMapper _mapper;

    public ContractService(
        IUnitOfWork unitOfWork,
        IBlobService blobService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _blobService = blobService;
        _mapper = mapper;
    }
    public async Task<Contract?> GetById(int id)
    {
        return await _unitOfWork.Contract.GetById(id);
    }

    public async Task<IEnumerable<Contract>> GetByEmployeeId(string id)
    {
        return await _unitOfWork.Contract.GetByEmployeeId(id);
    }

    public async Task<Stream> GetDocumentStream(string blobUrl)
    {
        return await _blobService.DownloadToPdfStream(blobUrl);
    }

    public async Task<(Stream data, string contentType)> DownloadDocument(string blobUrl)
    {
        return await _blobService.DownloadDocument(blobUrl);
    }

    public async Task<ServiceResult<Contract>> Create(ContractCreate contractData, Employee employee)
    {
        var result = await _unitOfWork.Contract.Create(contractData, employee);

        if (result.Entity is null)
        {
            return result;
        }

        employee.Contracts.Add(result.Entity);

        _unitOfWork.Employee.Update(employee);
        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<ServiceResult<Contract>> Update(ContractEdit contractData)
    {
        var result = await _unitOfWork.Contract.Update(contractData);

        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<ContractEdit> GetEditDto(int id)
    {
        var contract = await GetById(id);

        return _mapper.Map<ContractEdit>(contract);
    }
}
