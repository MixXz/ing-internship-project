using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.DataTransferObjects;

public class ResponseCreate
{
    [Required]
    public int ID { get; set; }

    [Required]
    public bool IsApproved { get; set; } = false;

    [MaxLength(256)]
    public string? Comment { get; set; }
}
