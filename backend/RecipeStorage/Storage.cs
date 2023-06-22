using CookBook.Domain;
using System.Text;
using System.Text.RegularExpressions;

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

        SaveText(textFileStream, $"Name:{recipe.Name}\t\n");
        SaveText(textFileStream, $"Description:{recipe.Description}\t\n");

        SaveImage(recipe.MainImageBase64, $"{folderName}/{MAIN_IMAGE_FILENAME}{IMAGES_FORMAT}");

        int stepCounter = 1;
        foreach ((string, string) pair in recipe.StepsImageAndDescription)
        {
            SaveText(textFileStream, $"Step{stepCounter}:{pair.Item2}\t\n");
            SaveImage(
                pair.Item1,
                $"{folderName}/{STEPS_IMAGES_PREFIX}{stepCounter}{IMAGES_FORMAT}"
            );
            stepCounter++;
        }
    }

    private void SaveText(FileStream fileStream, string value)
    {
        byte[] bytes = new UTF8Encoding(true).GetBytes(value);
        fileStream.Write(bytes, 0, bytes.Length);
    }

    private void SaveImage(string base64, string imagePath)
    {
        byte[] imageBytes = Convert.FromBase64String(base64);
        Image recipeIcon = Image.Load(imageBytes);
        recipeIcon.SaveAsJpeg(imagePath);
    }

    public Recipe Get(Guid id)
    {
        //TODO: if folder with this guid doesn't exist should throw an exeption (probably)
        //TODO: handle exeptions
        string directory = $"{storagePath}/{id}";
        string mainImagePath = $"{directory}/{MAIN_IMAGE_FILENAME}{IMAGES_FORMAT}";

        List<(string, string)> recipeSteps = new List<(string, string)>();

        string mainImage = LoadImageBase64String(mainImagePath);
        string recipeText = LoadTextFileContents($"{directory}/{RECIPE_TEXT_FILENAME}");
        string name = GetSubstringMatchingRegex(recipeText, "Name:(.*)\t\n");
        string description = GetSubstringMatchingRegex(recipeText, "Description:(.*)\t\n");

        int stepCounter = 1;
        string stepText = GetSubstringMatchingRegex(recipeText, $"Step{stepCounter}:(.*)\t\n");
        string stepImage = LoadImageBase64String(
            $"{directory}/{STEPS_IMAGES_PREFIX}{stepCounter}{IMAGES_FORMAT}"
        );
        while (stepText.Length > 0 && stepImage.Length > 0)
        {
            recipeSteps.Add((stepImage, stepText));
            stepCounter++;
            stepText = GetSubstringMatchingRegex(recipeText, $"Step{stepCounter}:(.*)\t\n");
            stepImage = LoadImageBase64String(
                $"{directory}/{STEPS_IMAGES_PREFIX}{stepCounter}{IMAGES_FORMAT}"
            );
        }

        Recipe recipe = new Recipe()
        {
            Id = id,
            Name = name,
            Description = description,
            MainImageBase64 = mainImage,
            StepsImageAndDescription = recipeSteps
        };
        return recipe;
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

    public void Delete(Guid id)
    {
        Directory.Delete($"{storagePath}/{id}", true);
    }

    public void Update(Guid id, Recipe recipe)
    {
        //TODO: ids must be the same
        Delete(id);
        Save(recipe);
    }
}
