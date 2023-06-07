using System.ComponentModel.DataAnnotations;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects;

public class RequestView
{
    [MaxLength(100)]
    public string? SearchInput { get; set; }

    public int? SelectedLeaveTypeID { get; set; }

    public bool ShowPendingOnly { get; set; }

    [DataType(DataType.Date)]
    public DateTime? StartDateFilter { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDateFilter { get; set; }

    public IEnumerable<Request> Requests { get; set; } = Enumerable.Empty<Request>();
}
