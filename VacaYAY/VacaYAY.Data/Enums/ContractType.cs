using System.ComponentModel.DataAnnotations;

namespace VacaYAY.Data.Enums;

public enum ContractType
{
    Definite,
    Indefinite,
    [Display(Name = "Open ended")]
    OpenEnded
}
