using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects;

public class RequestEdit
{
    [Key]
    public int ID { get; set; }

    [Required]
    [DisplayName("Start date")]
    public DateTime StartDate { get; set; }

    [Required]
    [DisplayName("End date")]
    public DateTime EndDate { get; set; }

    [MaxLength(256)]
    public string? Comment { get; set; }

    [Required]
    public LeaveType LeaveType { get; set; } = new();
}
