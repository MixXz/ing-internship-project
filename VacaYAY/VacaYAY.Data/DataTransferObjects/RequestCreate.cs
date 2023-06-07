using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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

    [MaxLength(256)]
    public string? Comment { get; set; }

    [Required]
    public int LeaveTypeID { get; set; }
}
