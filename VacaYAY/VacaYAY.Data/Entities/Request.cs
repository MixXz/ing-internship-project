using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Entities;

public class Request
{
    [Key]
    public int ID { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public string? Comment { get; set; }

    public Employee CreatedBy { get; set; } = new();

    public Response? Response { get; set; }
}