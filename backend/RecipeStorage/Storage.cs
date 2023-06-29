using CookBook.Domain;
using CookBook.Exceptions;
using System.Text;
using System.Text.RegularExpressions;

namespace CookBook.RecipeStorage;

public class Storage
{
    public const string IMAGES_FORMAT = ".jpg";
    public const string MAIN_IMAGE_FILENAME = "recipe_image";
    public const string RECIPE_TEXT_FILENAME = "text.txt";
    public const string STEPS_IMAGES_PREFIX = "step";
    private readonly string _storagePath = "";

    public Storage(string _storagePath)
    {
        this._storagePath = _storagePath;
    }

    public void Create(Recipe recipe)
    {
        string directory = $"{_storagePath}/{recipe.Id}";

        if (Directory.Exists(directory))
            throw new EntryAlreadyExists();

        Directory.CreateDirectory(directory);
        SaveRecipeContents(directory, recipe);
    }

    public Recipe Get(Guid id)
    {
        string directory = $"{_storagePath}/{id}";

        if (!Directory.Exists(directory))
            throw new EntryNotFound();

        return LoadRecipe(id);
    }

    public IEnumerable<Recipe> GetAll()
    {
        IEnumerable<Recipe> recipes = new List<Recipe>();
        IEnumerable<Guid> guids = GetGuids();
        foreach (Guid id in guids)
        {
            Recipe recipe = Get(id);
            recipes = recipes.Append(recipe);
        }
        return recipes;
    }

    public void Update(Recipe recipe)
    {
        Delete(recipe.Id);
        Create(recipe);
    }

    public void Delete(Guid id)
    {
        string directory = $"{_storagePath}/{id}";
        if (!Directory.Exists(directory))
            throw new EntryNotFound();
        Directory.Delete(directory, true);
    }

    private void SaveRecipeContents(string directory, Recipe recipe)
    {
        using FileStream textFileStream = File.Create($"{directory}/{RECIPE_TEXT_FILENAME}");

        SaveText(textFileStream, $"Name:{recipe.Name}\t\n");
        SaveText(textFileStream, $"Description:{recipe.Description}\t\n");

        SaveImage($"{directory}/{MAIN_IMAGE_FILENAME}{IMAGES_FORMAT}", recipe.MainImageBase64);

        int stepCounter = 1;
        foreach ((string, string) pair in recipe.StepsImagesAndDescriptions)
        {
            SaveText(textFileStream, $"Step{stepCounter}:{pair.Item2}\t\n");
            SaveImage($"{directory}/{STEPS_IMAGES_PREFIX}{stepCounter}{IMAGES_FORMAT}", pair.Item1);
            stepCounter++;
        }
    }

    private void SaveText(FileStream fileStream, string text)
    {
        byte[] bytes = new UTF8Encoding(true).GetBytes(text);
        fileStream.Write(bytes, 0, bytes.Length);
    }

    private void SaveImage(string imagePath, string base64)
    {
        byte[] imageBytes = Convert.FromBase64String(base64);
        Image recipeIcon = Image.Load(imageBytes);
        recipeIcon.SaveAsJpeg(imagePath);
    }

    private Recipe LoadRecipe(Guid id)
    {
        string directory = $"{_storagePath}/{id}";
        string mainImagePath = $"{directory}/{MAIN_IMAGE_FILENAME}{IMAGES_FORMAT}";
        string recipeText = LoadTextFileContents($"{directory}/{RECIPE_TEXT_FILENAME}");

        Recipe recipe = new Recipe()
        {
            Id = id,
            Name = GetSubstringMatchingRegex(recipeText, "Name:(.*)\t\n"),
            Description = GetSubstringMatchingRegex(recipeText, "Description:(.*)\t\n"),
            MainImageBase64 = LoadImageBase64String(mainImagePath),
            StepsImagesAndDescriptions = LoadSteps(directory, recipeText)
        };
        return recipe;
    }

    private List<(string, string)> LoadSteps(string directory, string recipeText)
    {
        List<(string, string)> recipeSteps = new List<(string, string)>();
        int stepCounter = 1;
        (string, string) step = LoadStep(directory, recipeText, stepCounter);
        while (step.Item1.Length > 0 && step.Item2.Length > 0)
        {
            recipeSteps.Add(step);
            stepCounter++;
            step = LoadStep(directory, recipeText, stepCounter);
        }
        return recipeSteps;
    }

    private (string, string) LoadStep(string directory, string recipeText, int stepNumber)
    {
        string stepImage = LoadImageBase64String(
            $"{directory}/{STEPS_IMAGES_PREFIX}{stepNumber}{IMAGES_FORMAT}"
        );
        string stepText = GetSubstringMatchingRegex(recipeText, $"Step{stepNumber}:(.*)\t\n");
        return (stepImage, stepText);
    }

    private string LoadTextFileContents(string filePath)
    {
        try
        {
            using FileStream fileStream = File.OpenRead(filePath);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (FileNotFoundException)
        {
            return "";
        }
    }

    private string GetSubstringMatchingRegex(string text, string regexString)
    {
        Regex regex = new Regex(regexString);
        Match match = regex.Match(text);
        GroupCollection groups = match.Groups;
        string result = groups[1].Value;
        return result;
    }

    private string LoadImageBase64String(string imagePath)
    {
        try
        {
            Image image = Image.Load(imagePath);
            MemoryStream memoryStream = new MemoryStream();
            image.SaveAsJpeg(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            return Convert.ToBase64String(imageBytes);
        }
        catch (FileNotFoundException)
        {
            return "";
        }
    }

    private IEnumerable<Guid> GetGuids()
    {
        IEnumerable<Guid> guids = new List<Guid>();
        string[] directories = Directory.GetDirectories(_storagePath);
        foreach (string directory in directories)
        {
            string directoryName = GetSubstringMatchingRegex(
                directory,
                "/(.{8}-.{4}-.{4}-.{4}-.{12})$"
            );
            Guid id = Guid.Parse(directoryName);
            guids = guids.Append(id);
        }
        return guids;
    }
}
