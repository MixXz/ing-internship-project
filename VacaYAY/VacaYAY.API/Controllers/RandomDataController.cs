using Microsoft.AspNetCore.Mvc;
using VacaYAY.API.Contracts;
using VacaYAY.Data.Entities;

namespace VacaYAY.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RandomDataController : ControllerBase
{
    private readonly IDataGenService _genService;
    private readonly IConfiguration _config;
    public RandomDataController(
        IDataGenService genService,
        IConfiguration config)
    {
        _genService = genService;
        _config = config;
    }

    [HttpGet]
    public List<Employee> GetEmployees()
    {
        var numOfEmployees = new Random()
                    .Next(_config.GetValue<int>("GenSettings:EmployeeMin"),
                          _config.GetValue<int>("GenSettings:EmployeeMax"));

        return _genService.GenerateEmployees(numOfEmployees);
    }
}