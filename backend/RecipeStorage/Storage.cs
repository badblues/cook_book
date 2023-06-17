using CookBook.Domain;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Text;

namespace CookBook.RecipeStorage;

public class Storage
{
    private const string IMAGES_FORMAT = ".jpg";
    private const string MAIN_IMAGE_FILENAME = "recipe_image";
    private const string RECIPE_TEXT_FILENAME = "text.txt";
    private const string STEPS_IMAGES_PREFIX = "step";
    private readonly string storagePath = "";

    public Storage(string storagePath)
    {
        this.storagePath = storagePath;
    }

    public void Save(Recipe recipe)
    {
        //TODO: if folder with this guid exists should throw an exeption (probably)
        //TODO: handle exeptions
        string folderName = $"{storagePath}/{recipe.Id}";
        
        Directory.CreateDirectory(folderName);
        using (FileStream textFileStream = File.Create($"{folderName}/{RECIPE_TEXT_FILENAME}"))
        {
            AddText(textFileStream, $"Name:{recipe.Name}\n");
            AddText(textFileStream, $"Description:{recipe.Description}\n");
            for (int i = 1; i <= recipe.StepsImageAndDescription.Length; i++)
            {
                AddText(textFileStream, $"Step {i}:{recipe.StepsImageAndDescription[i].Item2}\n");
            }
        }
    }

    private void AddText(FileStream fs, string value)
    {
        byte[] bytes = new UTF8Encoding(true).GetBytes(value);
        fs.Write(bytes, 0, bytes.Length);
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

    public void Update(Guid id, Recipe recipe)
    {
        //TODO
    }
}
