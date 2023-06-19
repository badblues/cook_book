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
        Recipe recipe = LoadRecipe("recipe1", "description1", IMAGES_PATH);
        Storage storage = new Storage(STORAGE_PATH);

        storage.Save(recipe);
        
        Assert.IsTrue(File.Exists($"{STORAGE_PATH}/{recipe.Id}/{RECIPE_TEXT_FILENAME}"));
    }

    [TestMethod]
    public void Save_ValidInput_CreatesMainImage() {
        Recipe recipe = LoadRecipe("recipe1", "description1", IMAGES_PATH);
        Storage storage = new Storage(STORAGE_PATH);

        storage.Save(recipe);

        Assert.IsTrue(File.Exists($"{STORAGE_PATH}/{recipe.Id}/{MAIN_IMAGE_FILENAME}{IMAGES_FORMAT}"));
    }

    private Recipe LoadRecipe(string name, string description, string directory)
    {
        (string, string)[] recipeSteps = {
            (GetImageBase64String(directory + "/step1.jpg"),"step1"),
            (GetImageBase64String(directory + "/step2.jpg"),"step2"),
            (GetImageBase64String(directory + "/step3.jpg"),"step3")
        };

        Recipe recipe = new Recipe()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            ImageBase64 = GetImageBase64String(directory + "/recipe_image.jpg"),
            StepsImageAndDescription = recipeSteps
        };
        return recipe;
    }

    private string GetImageBase64String(string imagePath)
    {
        Image image = Image.Load(imagePath);
        MemoryStream memoryStream = new MemoryStream();
        image.SaveAsJpeg(memoryStream);
        byte[] imageBytes = memoryStream.ToArray();
        return(Convert.ToBase64String(imageBytes));
    }
}
