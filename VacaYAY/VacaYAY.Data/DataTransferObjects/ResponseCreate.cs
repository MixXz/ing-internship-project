using System.ComponentModel.DataAnnotations;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects;

public class ResponseCreate
{
    [Required]
    public LeaveType LeaveType { get; set; } = new();

    [Required]
    public bool IsApproved { get; set; } = false;

    [MaxLength(256)]
    public string? Comment { get; set; }
}
