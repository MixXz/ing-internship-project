using AutoMapper;
using System.Security.Claims;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.DataTransferObjects.Employees;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Helpers;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Business.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmployeeService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Employee>> GetAll()
    {
        return await _unitOfWork.Employee.GetAll();
    }

    public async Task<Employee?> GetById(string id)
    {
        return await _unitOfWork.Employee.GetById(id);
    }

    public async Task<Employee?> GetByIdDetailed(string id)
    {
        return await _unitOfWork.Employee.GetDetailedById(id);
    }

    public Task<IEnumerable<Employee>> GetByFilters(EmployeeView filters)
    {
        return _unitOfWork.Employee.GetByFilters(filters);
    }

    public async Task<Employee?> GetCurrent(ClaimsPrincipal claims)
    {
        return await _unitOfWork.Employee.GetCurrent(claims);
    }

    public async Task<ServiceResult<Employee>> Register(EmployeeCreate employeeData)
    {
        ServiceResult<Employee> result = new();

        var position = await _unitOfWork.Position.GetById(employeeData.SelectedPositionID);

        if (position is null)
        {
            result.Errors.Add(new()
            {
                Property = nameof(employeeData.SelectedPositionID),
                Text = "Position invalid."
            });

            return result;
        }

        var insertResult = await _unitOfWork.Employee.Insert(employeeData, position);

        if (insertResult.Entity is null)
        {
            result.Errors.AddRange(insertResult.Errors);
            return result;
        }

        await _unitOfWork.SaveChangesAsync();
        result.Entity = insertResult.Entity;

        return result;
    }

    public async Task<int> InsertOldEmployees(string jsonResponse)
    {
        var oldEmployees = _unitOfWork.Employee.ExtractEmployeeData(jsonResponse);

        if (oldEmployees is null)
        {
            return 0;
        }

        foreach (var oldEmployee in oldEmployees)
        {
            var position = await _unitOfWork.Position.GetByCaption(oldEmployee.Position.Caption);

            if (position is null)
            {
                position = new()
                {
                    Caption = oldEmployee.Position.Caption,
                    Description = oldEmployee.Position.Description
                };

                _unitOfWork.Position.Insert(position);
                await _unitOfWork.SaveChangesAsync();
            }

            var employeeData = _mapper.Map<EmployeeCreate>(oldEmployee);
            employeeData.Password = $"{oldEmployee.FirstName}123!";

            await _unitOfWork.Employee.Insert(employeeData, position);
        }

        await _unitOfWork.SaveChangesAsync();

        return oldEmployees.Count();
    }

    public async Task<ServiceResult<Employee>> Edit(EmployeeEdit employeeData)
    {
        ServiceResult<Employee> result = new();
        var position = await _unitOfWork.Position.GetById(employeeData.SelectedPosition.ID);

        if (position is null)
        {
            result.Errors.Add(new()
            {
                Property = nameof(employeeData.SelectedPosition),
                Text = "Position invalid."
            });

            return result;
        }

        employeeData.SelectedPosition = position;
        var updateResult = await _unitOfWork.Employee.Update(employeeData);

        if (updateResult.Entity is null)
        {
            result.Errors.AddRange(updateResult.Errors);
            return result;
        }

        result.Entity = updateResult.Entity;

        return result;
    }

    public async Task<ServiceResult<Employee>> Delete(string id)
    {
        return await _unitOfWork.Employee.Delete(id);
    }

    public Task<bool> IsAdmin(Employee employee)
    {
        return _unitOfWork.Employee.IsAdmin(employee);
    }

    public async Task<bool> IsAuthorized(ClaimsPrincipal userClaims)
    {
        var user = await GetCurrent(userClaims);

        if (user is null)
        {
            return false;
        }

        return await _unitOfWork.Employee.IsAdmin(user);
    }

    public async Task<bool> IsAuthorizedToSee(ClaimsPrincipal userClaims, string authorId)
    {
        return await _unitOfWork.Employee.IsAuthorizedToSee(userClaims, authorId);
    }

    public EmployeeEdit GetEditDto(Employee employee)
    {
        return _mapper.Map<EmployeeEdit>(employee);
    }
}
