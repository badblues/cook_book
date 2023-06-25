namespace CookBook.Persistence;

public interface IRepository<T>
{
    T? Get(Guid id);
    IEnumerable<T> GetAll();
    void Create(T obj);
    void Update(T obj);
    void Delete(Guid id);
}
