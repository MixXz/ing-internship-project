using Microsoft.AspNetCore.Http;

namespace VacaYAY.Data.DataServiceContracts;

public interface IBlobService
{
    Task<string?> UploadFile(IFormFile file);
    Task<bool> DeleteFile(string blobUrl);
    Task<(Stream data, string contentType)> DownloadDocument(string blobUrl);
    Task<Stream> DownloadToPdfStream(string blobUrl);
}
