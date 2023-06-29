using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects;

public class EmployeeEdit
{
    [Key]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [DisplayName("First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [DisplayName("Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    [DisplayName("Address")]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

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
    public Position SelectedPosition { get; set; } = new();

    [Required]
    public Contract Contract { get; set; } = new();

    public bool MakeAdmin { get; set; } = false;

    public IEnumerable<Position> Positions { get; set; } = Enumerable.Empty<Position>();
}
