namespace VacaYAY.Business.Contracts;
public interface IRepositoryBase<T>
{
    Task<T?> GetById(int id);
    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);
}