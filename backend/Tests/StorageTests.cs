using System.Text;

using Domain;
using Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeStorage;

namespace Tests;

[TestClass]
public class StorageTests
{
    private const string IMAGES_PATH = "../../../images";
    private const string STORAGE_PATH = "../../../temporary_storage";

    [TestMethod]
    public void Create_ValidInput_CreatesImages()
    {
        Recipe recipe = CreateRecipe("recipe1", "description1", IMAGES_PATH);
        Storage storage = new Storage(STORAGE_PATH);

        storage.Create(recipe);

        Assert.IsTrue(File.Exists($"{STORAGE_PATH}/{recipe.Id}/recipe_image.jpg"));
        Assert.IsTrue(File.Exists($"{STORAGE_PATH}/{recipe.Id}/step1.jpg"));
        Assert.IsTrue(File.Exists($"{STORAGE_PATH}/{recipe.Id}/step2.jpg"));
        Assert.IsTrue(File.Exists($"{STORAGE_PATH}/{recipe.Id}/step3.jpg"));
    }

    [TestMethod]
    public void Create_ValidInput_CreatesText()
    {
        Recipe recipe = CreateRecipe("recipe1", "description1", IMAGES_PATH);
        Storage storage = new Storage(STORAGE_PATH);

        storage.Create(recipe);

        string filePath = $"{STORAGE_PATH}/{recipe.Id}/text.txt";
        Assert.IsTrue(File.Exists(filePath));
        string savedText = LoadTextFileContents(filePath);
        string expectedText =
            "Name:recipe1\t\nDescription:description1\t\nStep1:step1\t\nStep2:step2\t\nStep3:step3\t\n";
        Assert.AreEqual(expectedText, savedText);
    }

    [TestMethod]
    public void Create_EntryAlreadyExists_ThrowsException()
    {
        Recipe recipe = CreateRecipe("recipe1", "description1", IMAGES_PATH);
        Storage storage = new Storage(STORAGE_PATH);

        storage.Create(recipe);
        _ = Assert.ThrowsException<EntryAlreadyExists>(() => storage.Create(recipe));
    }

    [TestMethod]
    public void Create_ArraysLengthsDontMatch_ThrowsException()
    {
        Recipe recipe = CreateRecipe("recipe1", "description1", IMAGES_PATH);
        recipe.StepsTexts.Add("qwe");
        Storage storage = new Storage(STORAGE_PATH);

        _ = Assert.ThrowsException<ArrayLengthsDontMatch>(() => storage.Create(recipe));
    }

    [TestMethod]
    public void Get_ValidInput_LoadsText()
    {
        Recipe originalRecipe = CreateRecipe("recipe1", "description1", IMAGES_PATH);
        Storage storage = new Storage(STORAGE_PATH);

        storage.Create(originalRecipe);
        Recipe loadedRecipe = storage.Get(originalRecipe.Id);

        Assert.AreEqual(originalRecipe.Name, loadedRecipe.Name);
        Assert.AreEqual(originalRecipe.Description, loadedRecipe.Description);
        Assert.AreEqual(3, loadedRecipe.StepsTexts.Count);
        Assert.AreEqual(originalRecipe.StepsTexts[0], loadedRecipe.StepsTexts[0]);
        Assert.AreEqual(originalRecipe.StepsTexts[1], loadedRecipe.StepsTexts[1]);
        Assert.AreEqual(originalRecipe.StepsTexts[2], loadedRecipe.StepsTexts[2]);
    }

    [TestMethod]
    public void Get_ValidInput_LoadsImages()
    {
        Recipe originalRecipe = CreateRecipe("recipe1", "description1", IMAGES_PATH);
        Storage storage = new Storage(STORAGE_PATH);

        storage.Create(originalRecipe);
        Recipe loadedRecipe = storage.Get(originalRecipe.Id);

        Assert.IsTrue(loadedRecipe.MainImageBase64.Length > 0);
        Assert.IsTrue(loadedRecipe.StepsImagesBase64[0].Length > 0);
        Assert.IsTrue(loadedRecipe.StepsImagesBase64[1].Length > 0);
        Assert.IsTrue(loadedRecipe.StepsImagesBase64[2].Length > 0);
    }

    [TestMethod]
    public void Get_EntryDoesntExist_ThrowsException()
    {
        Storage storage = new Storage(STORAGE_PATH);
        Guid id = new Guid();

        _ = Assert.ThrowsException<EntryNotFound>(() => storage.Get(id));
    }

    [TestMethod]
    public void GetAll_ValidInput_LoadsTwoOrMoreRecipes()
    {
        Recipe recipe1 = CreateRecipe("recipe1", "description1", IMAGES_PATH);
        Recipe recipe2 = CreateRecipe("recipe2", "description2", IMAGES_PATH);
        Storage storage = new Storage(STORAGE_PATH);

        storage.Create(recipe1);
        storage.Create(recipe2);

        IEnumerable<Recipe> recipes = storage.GetAll();
        Assert.IsTrue(recipes.Count() >= 2);
    }

    [TestMethod]
    public void Delete_ValidInput_RemovesDirectory()
    {
        Recipe originalRecipe = CreateRecipe("recipe1", "description1", IMAGES_PATH);
        Storage storage = new Storage(STORAGE_PATH);

        storage.Create(originalRecipe);
        storage.Delete(originalRecipe.Id);

        Assert.IsFalse(Directory.Exists($"{STORAGE_PATH}/{originalRecipe.Id}"));
    }

    [TestMethod]
    public void Delete_EntryDoesntExist_ThrowsException()
    {
        Storage storage = new Storage(STORAGE_PATH);
        Guid id = new Guid();

        _ = Assert.ThrowsException<EntryNotFound>(() => storage.Delete(id));
    }

    [TestMethod]
    public void Update_ValidInput_ReplacesText()
    {
        Recipe recipe = CreateRecipe("recipe1", "description1", IMAGES_PATH);
        Storage storage = new Storage(STORAGE_PATH);

        storage.Create(recipe);
        recipe.Name = "changed_name";
        recipe.Description = "changed_description";
        storage.Update(recipe);

        string expectedText =
            "Name:changed_name\t\nDescription:changed_description\t\nStep1:step1\t\nStep2:step2\t\nStep3:step3\t\n";
        Assert.AreEqual(expectedText, LoadTextFileContents($"{STORAGE_PATH}/{recipe.Id}/text.txt"));
    }

    [TestMethod]
    public void Update_EntryDoesntExist_ThrowsException()
    {
        Recipe recipe = new Recipe() { Id = new Guid() };
        Storage storage = new Storage(STORAGE_PATH);

        _ = Assert.ThrowsException<EntryNotFound>(() => storage.Update(recipe));
    }

    private Recipe CreateRecipe(string name, string description, string directory)
    {
        List<string> stepsImagesBase64 = new List<string>();
        stepsImagesBase64.Add(GetImageBase64String(directory + "/step1.jpg"));
        stepsImagesBase64.Add(GetImageBase64String(directory + "/step2.jpg"));
        stepsImagesBase64.Add(GetImageBase64String(directory + "/step3.jpg"));
        List<string> stepsTexts = new List<string>() { "step1", "step2", "step3" };

        Recipe recipe = new Recipe()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            MainImageBase64 = GetImageBase64String(directory + "/recipe_image.jpg"),
            StepsImagesBase64 = stepsImagesBase64,
            StepsTexts = stepsTexts
        };
        return recipe;
    }

    private string GetImageBase64String(string imagePath)
    {
        Image image = Image.Load(imagePath);
        MemoryStream memoryStream = new MemoryStream();
        image.SaveAsJpeg(memoryStream);
        byte[] imageBytes = memoryStream.ToArray();
        return Convert.ToBase64String(imageBytes);
    }

    private string LoadTextFileContents(string filePath)
    {
        using FileStream fileStream = File.OpenRead(filePath);
        byte[] bytes = new byte[fileStream.Length];
        _ = fileStream.Read(bytes, 0, (int)fileStream.Length);
        return Encoding.UTF8.GetString(bytes);
    }
}
