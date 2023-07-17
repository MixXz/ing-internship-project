using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using VacaYAY.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace VacaYAY.Data.DataTransferObjects.Contracts;

public class ContractCreate
{
    [Required]
    public string EmployeeId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [DisplayName("Contract number")]
    public string ContractNumber { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Contract start date")]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayName("Contract end date")]
    public DateTime? EndDate { get; set; }

    [Required]
    [DisplayName("Contract type")]
    public ContractType ContractType { get; set; }

    [Required]
    public IFormFile Document { get; set; } = null!;
}
