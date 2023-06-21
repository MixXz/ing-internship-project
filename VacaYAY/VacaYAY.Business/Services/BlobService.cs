using Aspose.Words;
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

    public async Task<bool> DeleteFile(string blobUrl)
    {
        var blobClient = GetBlobClientFromBlobUrl(blobUrl);

        var response = await blobClient.DeleteAsync();

        if (response.IsError)
        {
            return false;
        }

        return true;
    }

    public async Task<(Stream data, string contentType)> DownloadDocument(string blobUrl)
    {
        var blobClient = GetBlobClientFromBlobUrl(blobUrl);
        var result = await blobClient.DownloadAsync();

        return (result.Value.Content,  result.Value.ContentType);
    }

    public async Task<Stream> DownloadToPdfStream(string blobUrl)
    {
        var extension = Path.GetExtension(blobUrl);
        var blobClient = GetBlobClientFromBlobUrl(blobUrl);

        var stream = await blobClient.OpenReadAsync();

        if (extension == ".pdf")
        {
            return stream;
        }

        Document doc = new Document(stream);
        MemoryStream pdfStream = new();

        doc.Save(pdfStream, SaveFormat.Pdf);

        pdfStream.Position = 0;

        return pdfStream;
    }

    private BlobClient GetBlobClientFromBlobUrl(string blobUrl)
    {
        var blobUri = new Uri(blobUrl);

        string fileName = Path.GetFileName(blobUri.LocalPath);
        return _blobContainerClient.GetBlobClient(fileName);
    }
}
