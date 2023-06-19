using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using VacaYAY.Business.Contracts.RepositoryContracts;
using VacaYAY.Business.Contracts.ServiceContracts;
using VacaYAY.Data;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VacaYAY.Business.Repository;

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

    public override async Task<IEnumerable<Contract>> GetAll()
    {
        return await _context.Contracts
                       .Include(c => c.Employee)
                       .ToListAsync();
    }

    public async Task<ServiceResult<Contract>> Create(ContractCreate data, Employee employee)
    {
        ServiceResult<Contract> result = new();

        result.Errors = Validate(data);

        var fileUrl = await _blobService.UploadFile(data.Document);

        if (fileUrl is null
            || result.Errors.Any())
        {
            result.Entity = null;
            return result;
        }

        Contract contract = new()
        {
            ContractNumber = data.ContractNumber,
            StartDate = data.StartDate,
            EndDate = data.EndDate,
            DocumentURL = fileUrl,
            Employee = employee,
        };

        _context.Contracts.Add(contract);
        result.Entity = contract;

        return result;
    }

    private List<CustomValidationResult> Validate(ContractCreate data)
    {
        List<CustomValidationResult> errors = new();
        if (!Regex.IsMatch(data.ContractNumber, @"^\d+$"))
        {
            errors.Add(new()
            {
                Property = $"{nameof(Contract)}.{nameof(ContractCreate.ContractNumber)}",
                Text = "The contract number must consist of numbers only."
            });
        }

        if (data.ContractType is not ContractType.OpenEnded
            && data.EndDate is null)
        {
            errors.Add(new()
            {
                Property = $"{nameof(Contract)}.{nameof(ContractCreate.EndDate)}",
                Text = "The end date is mandatory if the contract type isn't open-ended."
            });
        }

        string fileExt = Path.GetExtension(data.Document.FileName);
        if (fileExt != ".pdf" && fileExt != ".doc" && fileExt != ".docx")
        {
            errors.Add(new()
            {
                Property = $"{nameof(Contract)}.{nameof(ContractCreate.Document)}",
                Text = "Supported files are PDF and Word."
            });
        }

        if (data.StartDate > data.EndDate)
        {
            errors.Add(new()
            {
                Property = $"{nameof(Contract)}.{nameof(ContractCreate.StartDate)}",
                Text = "The end date cannot be earlier than the start date."
            });
        }
        
        int maxFileSizeInBytes = 10 * 1024 * 1024;
        if (data.Document.Length > maxFileSizeInBytes)
        {
            errors.Add(new()
            {
                Property = $"{nameof(Contract)}.{nameof(ContractCreate.Document)}",
                Text = "The maximum allowed file size is 10MB."
            });
        }

        return errors;
    }
}
