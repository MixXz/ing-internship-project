using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.DataTransferObjects;

public class EmployeeCreate
{
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
    [EmailAddress]
    [DisplayName("Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [DisplayName("Password")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [DisplayName("Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    public int SelectedPositionID { get; set; }

    public bool MakeAdmin { get; set; } = false;
}
