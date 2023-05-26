using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Entities;
public class Employee
{
    [Key]
    public int ID { get; set; }

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

    public DateTime EmployeeStartDate { get; set; }

    public DateTime? EmployeeEndDate { get; set; }

    public DateTime InsertDate { get; set; }

    public DateTime? DeleteDate { get; set; }

    public Position Position { get; set; } = new();

    public List<Request> LeaveRequests { get; set; } = new();
}