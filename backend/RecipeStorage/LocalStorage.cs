using CookBook.Domain;

namespace CookBook.RecipeStorage;

public class LocalStorage
{
    private string storagePath = "";

    public LocalStorage(string storagePath)
    {
        this.storagePath = storagePath;
    }

    public void Save(Recipe recipe)
    {
        //TODO
    }

    public Recipe Get(Guid id)
    {
        //TODO
        return new Recipe();
    }

    public void Delete(Guid id)
    {
        //TODO
    }

    public void Update(Guid id, Recipe recipe) {
        //TODO
    }
}
