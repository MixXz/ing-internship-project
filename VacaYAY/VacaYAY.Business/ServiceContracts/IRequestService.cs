using VacaYAY.Data.DataTransferObjects.Requests;
using VacaYAY.Data.DataTransferObjects.Responses;
using VacaYAY.Data.DataTransferObjects.Vacations;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.ServiceContracts;

public interface IRequestService
{
    Task<IEnumerable<Request>> GetAll();
    Task<Request?> GetById(int id);
    Task<IEnumerable<Request>> GetByFilters(RequestView filters);
    Task<IEnumerable<Request>> GetByEmployeeId(string id);
    Task<Response?> GetResponseById(int id);
    Task<ServiceResult<Request>> Create(RequestCreate requestData, Employee author);
    Task<ServiceResult<Request>> Edit(Request requestEntity, RequestEdit requestData);
    Task<ServiceResult<Request>> Delete(Request request);
    Task<ServiceResult<Request>> CreateResponse(Employee reviewer, Request request, ResponseCreate responseData);
    Task<ServiceResult<Request>> EditResponse(Request request, Response response, ResponseEdit responseData, Employee reviewer);
    Task<ServiceResult<Request>> CreateCollectiveVacation(Employee author, CollectiveVacationCreate data);
    RequestEdit ConvertToEditDto(Request request);
    ResponseEdit ConvertToEditDto(Response response);
}
