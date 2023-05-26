using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.Contracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;

namespace VacaYAY.Web.Controllers;

public class EmployeeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public EmployeeController(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;   
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] EmployeeCreateDto employeeData)
    {
        if (employeeData is null)
        {
            return BadRequest("FieldsMissing");
        }

        var position = await _unitOfWork.Position.GetById(employeeData.PositionID);

        if(position is null)
        {
            return BadRequest("PositionInvalid");
        }

        var employeeEntity = _mapper.Map<Employee>(employeeData);
        employeeEntity.Position = position;
        employeeEntity.InsertDate = DateTime.Now;

        _unitOfWork.Employee.Insert(employeeEntity);
        await _unitOfWork.SaveChangesAsync();

        return Ok(employeeEntity);
    }

    public IActionResult Index()
    {
        return View();
    }
}
