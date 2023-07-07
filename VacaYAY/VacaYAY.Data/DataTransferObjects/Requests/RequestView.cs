using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VacaYAY.Data.DataTransferObjects.Vacations;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Data.DataTransferObjects.Requests;

public class RequestView
{
    [MaxLength(100)]
    public string? SearchInput { get; set; }

    public int? SelectedLeaveTypeID { get; set; }

    public RequestStatus Status { get; set; }

    [DisplayName("Start date")]
    [DataType(DataType.Date)]
    public DateTime? StartDateFilter { get; set; }

    [DisplayName("End date")]
    [DataType(DataType.Date)]
    public DateTime? EndDateFilter { get; set; }

    public IEnumerable<LeaveType> LeaveTypes { get; set; } = Enumerable.Empty<LeaveType>();

    public IEnumerable<Request> Requests { get; set; } = Enumerable.Empty<Request>();

    public CollectiveVacationCreate? CollectiveVacation { get; set; }
}
