using Quartz;
using VacaYAY.Business.Contracts;
using VacaYAY.Business.Contracts.ServiceContracts;
using VacaYAY.Data.Helpers;

namespace VacaYAY.Business.Jobs;

public class RemoveOldDaysOffJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotifierSerivice _notifierService;

    public RemoveOldDaysOffJob(
        IUnitOfWork unitOfWork,
        INotifierSerivice notifierSerivice)
    {
        _unitOfWork = unitOfWork;
        _notifierService = notifierSerivice;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var employees = await _unitOfWork.Employee.GetWithRemainingDaysOff();
        _unitOfWork.Employee.RemoveOldDaysOff();

        EmployeeEmailTemplates templates = new();
        templates.ListOfEmployees = employees;

        await _notifierService.NotifyHRTeam(templates.RemovedOldDaysOffHR);
    }
}
