using Quartz;
using VacaYAY.Business.Contracts;
using VacaYAY.Business.Contracts.ServiceContracts;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Jobs;

public class NotifyOfRemainingDaysOffJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotifierSerivice _notifierService;

    public NotifyOfRemainingDaysOffJob(
        IUnitOfWork unitOfWork,
        INotifierSerivice notifierSerivice)
    {
        _unitOfWork = unitOfWork;
        _notifierService = notifierSerivice;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var employees = await _unitOfWork.Employee.GetWithRemainingDaysOff();
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
