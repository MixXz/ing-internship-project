namespace VacaYAY.Data.Helpers;

public class ServiceResult<T> where T : class
{
    public T? Entity { get; set; }
    public List<CustomValidationResult> Errors { get; set; } = new List<CustomValidationResult>();
}
