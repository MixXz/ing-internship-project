using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using VacaYAY.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacaYAY.Data.DataTransferObjects.Requests;

public class RequestEdit
{
    [Key]
    public int ID { get; set; }

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

    public Response? Response { get; set; }

    [Required]
    public int SelectedLeaveTypeID { get; set; }

    public IEnumerable<LeaveType> LeaveTypes { get; set; } = Enumerable.Empty<LeaveType>();
}
