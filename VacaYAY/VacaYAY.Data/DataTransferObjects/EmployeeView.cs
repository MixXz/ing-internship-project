using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects;

public class EmployeeView
{
    [MaxLength(100)]
    public string? SearchInput { get; set; }

    [DisplayName("Start date")]
    public DateTime? StartDateFilter { get; set; }

    [DisplayName("End date")]
    public DateTime? EndDateFilter { get; set; }
    public IEnumerable<Employee> Employees { get; set; } = Enumerable.Empty<Employee>();
}
