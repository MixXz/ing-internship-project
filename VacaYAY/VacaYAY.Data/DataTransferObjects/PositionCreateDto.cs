using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.DataTransferObjects;

public class PositionCreateDto
{
    [Required]
    [MaxLength(50)]
    public string Caption { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    public string Description { get; set; } = string.Empty;
}
