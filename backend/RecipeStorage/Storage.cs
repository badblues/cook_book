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
        using FileStream textFileStream = File.Create($"{folderName}/{RECIPE_TEXT_FILENAME}");
        
        AddText(textFileStream, $"Name:{recipe.Name}\n");
        AddText(textFileStream, $"Description:{recipe.Description}\n");

        SaveImage(recipe.ImageBase64, $"{folderName}/{MAIN_IMAGE_FILENAME}{IMAGES_FORMAT}");

        int stepNumber = 1;
        foreach((string, string) pair in recipe.StepsImageAndDescription)
        {
            AddText(textFileStream, $"Step {stepNumber++}:{pair.Item2}\n");
            SaveImage(pair.Item1, $"{folderName}/step{stepNumber}{IMAGES_FORMAT}");
        }
    }

    private void AddText(FileStream fs, string value)
    {
        byte[] bytes = new UTF8Encoding(true).GetBytes(value);
        fs.Write(bytes, 0, bytes.Length);
    }

    private void SaveImage(string base64, string imagePath)
    {
        byte[] imageBytes = Convert.FromBase64String(base64);
        Image recipeIcon = Image.Load(imageBytes);
        recipeIcon.SaveAsJpeg(imagePath);
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
