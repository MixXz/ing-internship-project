using System.ComponentModel.DataAnnotations;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects;

public class EmployeeCreateDto
{
    [Required]
    [MaxLength(50)]
    public string FristName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(50)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string IDNumber { get; set; } = string.Empty;

    [Required]
    public int DaysOfNumber { get; set; }

    //public DateTime EmployeeStartDate { get; set; }

    [Required]
    public int PositionID { get; set; }
}

