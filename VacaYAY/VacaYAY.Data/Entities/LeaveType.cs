using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VacaYAY.Data.Entities;
public class LeaveType
{
    [Key]
    public int ID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Caption { get; set; } = string.Empty;

    [Required]
    [MaxLength(512)]
    public string Description { get; set; } = string.Empty;

    [JsonIgnore]
    public List<LeaveType> Responses { get; set; } = new();
}
