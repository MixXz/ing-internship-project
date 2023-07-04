namespace VacaYAY.Data.RepositoryContracts;

public interface IRepositoryBase<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(int id);
    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);
    List<T>? DeserializeList(string jsonString);
}