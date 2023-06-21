using VacaYAY.Business.Contracts.RepositoryContracts;

namespace VacaYAY.Business.Contracts;
public interface IUnitOfWork
{
    IEmployeeRepository Employee { get; }
    IRequestRepository Request { get; }
    IResponseRepository Response { get; }
    IPositionRepository Position { get; }
    ILeaveTypeRepository LeaveType { get; }
    IContractRepository Contract { get; }
    Task SaveChangesAsync();
}
