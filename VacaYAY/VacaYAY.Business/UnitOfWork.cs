using VacaYAY.Business.Contracts;
using VacaYAY.Business.Repository;
using VacaYAY.Data;

namespace VacaYAY.Business;
public class UnitOfWork : IUnitOfWork
{
    private readonly Context _context;
    private EmployeeRepository? _employeeRepository;
    private RequestRepository? _requestRepository;
    private ResponseRepository? _responseRepository;
    private PositionRepository? _positionRepository;
    private LeaveTypeRepository? _leaveTypeRepository;

    public IEmployeeRepository Employee
    {
        get
        {
            if (_employeeRepository == null)
            {
                _employeeRepository = new(_context);
            }

            return _employeeRepository;
        }
    }

    public IRequestRepository Request
    {
        get
        {
            if (_requestRepository == null)
            {
                _requestRepository = new(_context);
            }

            return _requestRepository;
        }
    }

    public IResponseRepository Response
    {
        get
        {
            if (_responseRepository == null)
            {
                _responseRepository = new(_context);
            }

            return _responseRepository;
        }
    }

    public IPositionRepository Position
    {
        get
        {
            if (_positionRepository == null)
            {
                _positionRepository = new(_context);
            }

            return _positionRepository;
        }
    }

    public ILeaveTypeRepository LeaveType
    {
        get
        {
            if (_leaveTypeRepository == null)
            {
                _leaveTypeRepository = new(_context);
            }

            return _leaveTypeRepository;
        }
    }


    public UnitOfWork(Context context)
    {
        _context = context;
    }
    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
