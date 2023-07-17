namespace VacaYAY.Business.ServiceContracts;

public interface IHttpClientService
{
    Task<HttpResponseMessage> GetAsync(string controller, string route = "");
}
