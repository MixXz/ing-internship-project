using Quartz;
using Microsoft.Extensions.Configuration;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Business.Jobs;

public class AddNewDaysOffJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AddNewDaysOffJob(
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public Task Execute(IJobExecutionContext context)
    {
        var numOfDays = _configuration.GetValue<int>("AppSettings:Employee:DaysOffNumber");

        return Task.Run(() => _unitOfWork.Employee.AddNewDaysOff(numOfDays));
    }
}
