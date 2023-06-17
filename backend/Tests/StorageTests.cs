using Microsoft.VisualStudio.TestTools.UnitTesting;
using CookBook.RecipeStorage;
using CookBook.Domain;
using SixLabors.ImageSharp;

namespace CookBook.Tests;

[TestClass]
public class StorageTests
{
    private const string IMAGES_PATH = "../../../images";
    private const string STORAGE_PATH = "../../../temporary_storage";
    private const string IMAGES_FORMAT = ".jpg";
    private const string MAIN_IMAGE_FILENAME = "recipe_image";
    private const string RECIPE_TEXT_FILENAME = "text.txt";
    private const string STEPS_IMAGES_PREFIX = "step";

    [TestMethod]
    public void Save_ValidInput_CreatesTextFile() {
        Recipe recipe = LoadRecipe("recipe1", "description1", IMAGES_PATH + "/1.jpg");
        Storage storage = new Storage(STORAGE_PATH);

        storage.Save(recipe);
        
        Assert.IsTrue(File.Exists($"{STORAGE_PATH}/{recipe.Id}/{RECIPE_TEXT_FILENAME}"));
    }

    private Recipe LoadRecipe(string name, string description, string imagePath)
    {
        Image recipeIcon = Image.Load(imagePath);
        MemoryStream memoryStream = new MemoryStream();
        recipeIcon.SaveAsJpeg(memoryStream);
        byte[] imageBytes = memoryStream.ToArray();
        Recipe recipe = new Recipe()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            ImageBase64 = Convert.ToBase64String(imageBytes)
        };
        return recipe;
    }
}
