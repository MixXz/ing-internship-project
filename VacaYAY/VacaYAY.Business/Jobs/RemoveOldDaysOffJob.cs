using Quartz;
using VacaYAY.Business.ServiceContracts;
using VacaYAY.Data.Helpers;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Business.Jobs;

public class RemoveOldDaysOffJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotifierService _notifierService;

    public RemoveOldDaysOffJob(
        IUnitOfWork unitOfWork,
        INotifierService notifierSerivice)
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
