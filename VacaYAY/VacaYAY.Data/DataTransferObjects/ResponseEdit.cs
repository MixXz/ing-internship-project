using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Data.DataTransferObjects;

public class ResponseEdit
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int RequstID { get; set; }

    [Required]
    public LeaveType LeaveType { get; set; } = new();

    [Required]
    public bool IsApproved { get; set; } = false;

    [NotMapped]
    public RequestStatus Status
    {
        get
        {
            return IsApproved ?
                RequestStatus.Approved
                :
                RequestStatus.Rejected;
        }
    }

    [MaxLength(256)]
    public string? Comment { get; set; }
}
