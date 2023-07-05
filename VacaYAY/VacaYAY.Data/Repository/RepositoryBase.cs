using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VacaYAY.Data.RepositoryContracts;

namespace VacaYAY.Data.Repository;
internal abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly Context _context;

    public RepositoryBase(Context context) => _context = context;

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<T?> GetById(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public void Insert(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public List<T>? DeserializeList(string jsonString)
    {
        return JsonConvert.DeserializeObject<List<T>>(jsonString);
    }
}
