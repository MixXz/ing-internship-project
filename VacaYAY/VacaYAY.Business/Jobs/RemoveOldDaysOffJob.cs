using Quartz;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.Helpers;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Business.Jobs;

public class RemoveOldDaysOffJob : IJob
{
    private readonly IEmployeeService _employeeService;
    private readonly INotifierService _notifierService;

    public RemoveOldDaysOffJob(
        IEmployeeService employeeService,
        IUnitOfWork unitOfWork,
        INotifierService notifierSerivice)
    {
        _employeeService = employeeService;
        _notifierService = notifierSerivice;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var employees = await _employeeService.GetWithRemainingDaysOff();
        _employeeService.RemoveOldDaysOff();

        EmployeeEmailTemplates templates = new();
        templates.ListOfEmployees = employees;

        await _notifierService.NotifyHRTeam(templates.RemovedOldDaysOffHR);
    }
}
