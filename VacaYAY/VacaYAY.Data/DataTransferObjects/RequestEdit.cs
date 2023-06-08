using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using VacaYAY.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacaYAY.Data.DataTransferObjects;

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
    public LeaveType LeaveType { get; set; } = new();
}
