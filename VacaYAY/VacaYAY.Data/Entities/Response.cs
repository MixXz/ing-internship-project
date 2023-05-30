using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Entities;

public class Response
{
    [Key]
    public int ID { get; set; }

    public bool IsApproved { get; set; }

    public string? Comment { get; set; }

    public int RequstID { get; set; }

    public Request Request { get; set; } = new();

    public Employee? ReviewedBy { get; set; }

    public LeaveType? LeaveType { get; set; }
}
