using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VacaYAY.Data.Enums;

namespace VacaYAY.Data.Entities;

public class Contract
{
    [Key]
    public int ID { get; set; }

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
    public string DocumentURL { get; set; } = string.Empty;

    [Required]
    public Employee Employee { get; set; } = new();
}
