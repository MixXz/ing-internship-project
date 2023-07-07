using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VacaYAY.Data.DataTransferObjects.Vacations;

public class CollectiveVacationCreate
{
    [DisplayName("Start date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DisplayName("End date")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [MaxLength(512)]
    public string? Comment { get; set; }
}
