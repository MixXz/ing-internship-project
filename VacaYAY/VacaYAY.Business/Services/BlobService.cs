using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using VacaYAY.Business.Contracts.ServiceContracts;

namespace VacaYAY.Business.Services;

public class BlobService : IBlobService
{
    private readonly BlobContainerClient _blobContainerClient;
    private readonly IConfiguration _config;

    public BlobService(IConfiguration config)
    {
        _config = config;
        _blobContainerClient = new BlobContainerClient(_config["Azurite:ConnectionString"], _config["Azurite:ContainerName"]);
    }

    public async Task<string?> UploadFile(IFormFile file)
    {
        string fileExtension = Path.GetExtension(file.FileName);
        string fileName = Guid.NewGuid().ToString() + fileExtension;

        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);


        var response = await blobClient.UploadAsync(file.OpenReadStream());
        blobClient.SetHttpHeaders(new BlobHttpHeaders()
        {
            ContentType = file.ContentType.ToString(),
            ContentDisposition = $"inline; filename=\"{fileName}\""
        });

        if (response.GetRawResponse().Status != 201)
        {
            return null;
        }

        return blobClient.Uri.ToString();
    }
}
