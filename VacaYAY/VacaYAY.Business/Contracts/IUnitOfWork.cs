namespace VacaYAY.Business.Contracts;
public interface IUnitOfWork
{
    IEmployeeRepository Employee { get; }
    IRequestRepository Request { get; }
    IResponseRepository Response { get; }
    IPositionRepository Position { get; }
    ILeaveTypeRepository LeaveType { get; }
    Task SaveChangesAsync();
}
