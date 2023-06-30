using System.ComponentModel.DataAnnotations;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects;

public class ResponseEdit
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int SelectedLeaveTypeID { get; set; }

    [Required]
    public bool IsApproved { get; set; } = false;

    [MaxLength(256)]
    public string? Comment { get; set; }

    [Required]
    public Request Request { get; set; } = new();

    public IEnumerable<LeaveType> LeaveTypes { get; set; } = Enumerable.Empty<LeaveType>();
}
