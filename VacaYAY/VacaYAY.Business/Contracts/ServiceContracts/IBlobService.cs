using Microsoft.AspNetCore.Http;

namespace VacaYAY.Business.Contracts.ServiceContracts;

public interface IBlobService
{
    Task<string?> UploadFile(IFormFile file);
}
