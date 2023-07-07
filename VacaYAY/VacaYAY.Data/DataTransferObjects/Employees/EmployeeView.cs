using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects.Employees;

public class EmployeeView
{
    [MaxLength(100)]
    public string? SearchInput { get; set; }

    [DisplayName("Start date")]
    public DateTime? StartDateFilter { get; set; }

    [DisplayName("End date")]
    public DateTime? EndDateFilter { get; set; }

    public List<int> SelectedPositionIds { get; set; } = new();

    public IEnumerable<Employee> Employees { get; set; } = Enumerable.Empty<Employee>();

    public IEnumerable<Position> Positions { get; set; } = Enumerable.Empty<Position>();
}
