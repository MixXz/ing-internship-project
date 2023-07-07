using VacaYAY.Data.Entities;

namespace VacaYAY.Data.DataTransferObjects.Contracts;

public class ContractView
{
    public Employee Employee { get; set; } = new();

    public IEnumerable<Contract> Contracts { get; set; } = Enumerable.Empty<Contract>();
}
