using VacaYAY.Business.Contracts;
using VacaYAY.Data;
using VacaYAY.Data.Entities;

namespace VacaYAY.Business.Repository;

public class LeaveTypeRepository : RepositoryBase<LeaveType>, ILeaveTypeRepository
{
    public LeaveTypeRepository(Context context) 
        : base(context)
    {
    }
}
