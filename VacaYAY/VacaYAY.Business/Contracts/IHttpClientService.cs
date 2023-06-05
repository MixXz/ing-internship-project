namespace VacaYAY.Business.Contracts;

public interface IHttpClientService
{
    Task<HttpResponseMessage> GetAsync(string controller, string route = "");
}
