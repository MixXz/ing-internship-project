using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects;

public class EmployeeOld
{
    [Required]
    [MaxLength(50)]
    [DisplayName("First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [DisplayName("Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    [DisplayName("Address")]
    public string Address { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [DisplayName("ID number")]
    public string IDNumber { get; set; } = string.Empty;

    [Required]
    [Range(0, 100)]
    [DisplayName("Days off number")]
    public int DaysOffNumber { get; set; }

    [Required]
    [DisplayName("Start date")]
    public DateTime EmployeeStartDate { get; set; }

    [DisplayName("End date")]
    public DateTime? EmployeeEndDate { get; set; }

    [Required]
    [DisplayName("Insert date")]
    public DateTime InsertDate { get; set; }

    [Required]
    public Position Position { get; set; } = new();
}
