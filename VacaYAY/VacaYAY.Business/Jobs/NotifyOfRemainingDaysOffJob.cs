using Quartz;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Jobs;

public class NotifyOfRemainingDaysOffJob : IJob
{
    private readonly IEmployeeService _employeeService;
    private readonly INotifierService _notifierService;

    public NotifyOfRemainingDaysOffJob(
        IEmployeeService employeeService,
        INotifierService notifierService)
    {
        _employeeService = employeeService;
        _notifierService = notifierService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var employees = await _employeeService.GetWithRemainingDaysOff();
        EmployeeEmailTemplates templates = new();

        foreach (var employee in employees)
        {
            templates.Employee = employee;
            await _notifierService.NotifyEmployee(templates.RemainingDaysOff);
        }

        templates.ListOfEmployees = employees;
        await _notifierService.NotifyHRTeam(templates.RemainingDaysOffHR);
    }
}
