using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects;

public class EmployeeEdit
{
    [Key]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [DisplayName("First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [DisplayName("Last Name")]
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
    [DisplayName("Days of number")]
    public int DaysOfNumber { get; set; }

    [Required]
    [DisplayName("Start date")]
    public DateTime EmployeeStartDate { get; set; }

    [Required]
    [DisplayName("End date")]
    public DateTime? EmployeeEndDate { get; set; }

    [Required]
    public Position Position { get; set; } = new();

    public bool MakeAdmin { get; set; } = false;
}
