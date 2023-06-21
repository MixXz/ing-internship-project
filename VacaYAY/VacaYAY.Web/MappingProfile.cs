using AutoMapper;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EmployeeEdit, Employee>();
        CreateMap<Employee, EmployeeEdit>();
        CreateMap<EmployeeCreate, EmployeeOld>();
        CreateMap<EmployeeOld, EmployeeCreate>();
        CreateMap<Request, RequestEdit>();
        CreateMap<RequestEdit, Request>();
        CreateMap<Response, ResponseEdit>();
        CreateMap<ResponseEdit, Response>();
        CreateMap<Contract, ContractEdit>();
        CreateMap<ContractEdit, Contract>();
    }
}
