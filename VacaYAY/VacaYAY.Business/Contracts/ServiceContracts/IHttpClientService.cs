namespace VacaYAY.Business.Contracts.ServiceContracts;

public interface IHttpClientService
{
    Task<HttpResponseMessage> GetAsync(string controller, string route = "");
}
