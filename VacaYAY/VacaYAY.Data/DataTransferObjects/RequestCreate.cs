using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using VacaYAY.Data.Enums;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects;

public class RequestCreate
{
    [Required]
    [DisplayName("Start date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required]
    [DisplayName("End date")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [NotMapped]
    [DisplayName("Number of days requested")]
    public int NumOfDaysRequested
    {
        get
        {
            return (int)(EndDate - StartDate).TotalDays;
        }
    }

    [MaxLength(256)]
    public string? Comment { get; set; }

    [Required]
    public int LeaveTypeID { get; set; }

    public IEnumerable<LeaveType> LeaveTypes { get; set; } = Enumerable.Empty<LeaveType>();

    public int OldDaysOff { get; set; }

    public int NewDaysOff { get; set; }

    public int AllDaysOff
    {
        get
        {
            return OldDaysOff + NewDaysOff;
        }
    }
}
