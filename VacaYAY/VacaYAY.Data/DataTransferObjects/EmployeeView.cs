using System.ComponentModel.DataAnnotations;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects;

public class EmployeeView
{
    [MaxLength(100)]
    public string? SearchInput { get; set; }
    public DateTime? StartDateFilter { get; set; }
    public DateTime? EndDateFilter { get; set; }
    public IEnumerable<Employee> Employees { get; set; } = Enumerable.Empty<Employee>();
}
