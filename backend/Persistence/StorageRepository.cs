namespace CookBook.Persistence;

using CookBook.Domain;
using CookBook.RecipeStorage;
using CookBook.Exceptions;

public class StorageRepository : IRepository<Recipe>
{
    private Storage _storage;

    public StorageRepository(string storagePath)
    {
        _storage = new Storage(storagePath);
    }

    public Recipe Get(Guid id)
    {
        return _storage.Get(id);
    }

    public IEnumerable<Recipe> GetAll()
    {
        return _storage.GetAll();
    }

    public void Create(Recipe recipe)
    {
        _storage.Create(recipe);
    }

    public void Update(Recipe recipe)
    {
        _storage.Update(recipe);
    }

    public void Delete(Guid id)
    {
        _storage.Delete(id);
    }
}
