using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using VacaYAY.Data.DataServiceContracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Helpers;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Data.Repository;

public class ContractRepository : RepositoryBase<Contract>, IContractRepository
{
    private readonly Context _context;
    private readonly IBlobService _blobService;
    public ContractRepository(
        Context context,
        IBlobService blobService)
        : base(context)
    {
        _context = context;
        _blobService = blobService;
    }

    public override Task<Contract?> GetById(int id)
    {
        return _context.Contracts
                      .Include(e => e.Employee)
                      .Where(e => e.ID == id)
                      .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Contract>> GetByEmployeeId(string employeeId)
    {
        return await _context.Contracts
                      .Include(e => e.Employee)
                      .Where(e => e.Employee.Id == employeeId)
                      .ToListAsync();
    }

    public async Task<string?> GetDocumentUrlByContractId(int contractId)
    {
        return await _context.Contracts
                      .Where(c => c.ID == contractId)
                      .Select(c => c.DocumentURL)
                      .FirstOrDefaultAsync();
    }

    public async Task<ServiceResult<Contract>> Create(ContractCreate data, Employee employee)
    {
        ServiceResult<Contract> result = new();

        result.Errors = Validate(data.ContractNumber,
                                 data.ContractType,
                                 data.StartDate,
                                 data.EndDate,
                                 data.Document);

        if (result.Errors.Any())
        {
            return result;
        }

        var fileUrl = await _blobService.UploadFile(data.Document);

        if (fileUrl is null)
        {
            return result;
        }

        Contract contract = new()
        {
            ContractNumber = data.ContractNumber,
            ContractType = data.ContractType,
            StartDate = data.StartDate,
            EndDate = data.EndDate,
            DocumentURL = fileUrl,
            Employee = employee,
        };

        _context.Contracts.Add(contract);
        result.Entity = contract;

        return result;
    }


    public async Task<ServiceResult<Contract>> Update(ContractEdit data)
    {
        ServiceResult<Contract> result = new();

        result.Errors = Validate(data.ContractNumber,
                                 data.ContractType,
                                 data.StartDate,
                                 data.EndDate,
                                 data.Document);

        var contract = await GetById(data.ID);
        if (contract is null || result.Errors.Any())
        {
            return result;
        }

        contract.ContractNumber = data.ContractNumber;
        contract.ContractType = data.ContractType;
        contract.StartDate = data.StartDate;
        contract.EndDate = data.EndDate;

        if (data.Document is not null)
        {
            await _blobService.DeleteFile(data.DocumentUrl);
            var url = await _blobService.UploadFile(data.Document);

            if (url is null)
            {
                return result;
            }

            contract.DocumentURL = url;
        }

        result.Entity = contract;
        _context.Contracts.Update(contract);

        return result;
    }

    private List<CustomValidationResult> Validate(string contractNumber,
                                                  ContractType type,
                                                  DateTime startDate,
                                                  DateTime? endDate,
                                                  IFormFile? file)
    {
        List<CustomValidationResult> errors = new();
        if (!Regex.IsMatch(contractNumber, @"^\d+$"))
        {
            errors.Add(new()
            {
                Property = $"{nameof(Contract.ContractNumber)}",
                Text = "The contract number must consist of numbers only."
            });
        }

        if (type is not ContractType.OpenEnded
            && endDate is null)
        {
            errors.Add(new()
            {
                Property = $"{nameof(Contract.EndDate)}",
                Text = "The end date is mandatory if the contract type isn't open-ended."
            });
        }

        if (startDate > endDate)
        {
            errors.Add(new()
            {
                Property = $"{nameof(Contract.StartDate)}",
                Text = "The end date cannot be earlier than the start date."
            });
        }

        if (file is not null)
        {
            errors = errors.Concat(ValidateDocument(file)).ToList();
        }

        return errors;
    }

    private List<CustomValidationResult> ValidateDocument(IFormFile file)
    {
        List<CustomValidationResult> errors = new();

        string fileExt = Path.GetExtension(file.FileName);
        int maxFileSizeInBytes = 10 * 1024 * 1024;

        if (fileExt != ".pdf" && fileExt != ".doc" && fileExt != ".docx")
        {
            errors.Add(new()
            {
                Property = $"{nameof(ContractCreate.Document)}",
                Text = "Supported files are PDF and Word."
            });
        }

        if (file.Length > maxFileSizeInBytes)
        {
            errors.Add(new()
            {
                Property = $"{nameof(ContractCreate.Document)}",
                Text = "The maximum allowed file size is 10MB."
            });
        }

        return errors;
    }
}
