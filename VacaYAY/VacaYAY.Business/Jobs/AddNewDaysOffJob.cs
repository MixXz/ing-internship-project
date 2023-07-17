using Quartz;
using Microsoft.Extensions.Configuration;
using VacaYAY.Business.ServiceContracts;

namespace VacaYAY.Business.Jobs;

public class AddNewDaysOffJob : IJob
{
    private readonly IEmployeeService _employeeService;
    private readonly IConfiguration _configuration;

    public AddNewDaysOffJob(
        IEmployeeService employeeService,
        IConfiguration configuration)
    {
        _employeeService = employeeService;
        _configuration = configuration;
    }

    public Task Execute(IJobExecutionContext context)
    {
        var numOfDays = _configuration.GetValue<int>("AppSettings:Employee:DaysOffNumber");
        return Task.Run(() => _employeeService.AddNewDaysOff(numOfDays));
    }
}
