using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacaYAY.Data.Entities;
public class Employee : IdentityUser
{
    [Required]
    [MaxLength(50)]
    [DisplayName("First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [DisplayName("Last Name")]
    public string LastName { get; set; } = string.Empty;

    [NotMapped]
    [DisplayName("Name")]
    public string Name 
    {
        get
        {
            return $"{FirstName} {LastName}";
        }
     }

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
    [DisplayName("Days of number")]
    public int DaysOfNumber { get; set; }

    [Required]
    [DisplayName("Start date")]
    public DateTime EmployeeStartDate { get; set; }

    [DisplayName("End date")]
    public DateTime? EmployeeEndDate { get; set; }

    [Required]
    [DisplayName("Insert date")]
    public DateTime InsertDate { get; set; }

    public DateTime? DeleteDate { get; set; }

    [Required]
    public Position Position { get; set; } = new();

    public List<Request> LeaveRequests { get; set; } = new();
}