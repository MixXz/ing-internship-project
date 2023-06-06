using VacaYAY.Business.Contracts;

namespace VacaYAY.Business.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetAsync(string controller, string route = "")
    {
        return await _httpClient.GetAsync($"{controller}/{route}");
    }
}
