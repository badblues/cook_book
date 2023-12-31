﻿using System.Text;
using System.Text.RegularExpressions;

using Domain;

using Exceptions;

namespace RecipeStorage;

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
        if (recipe.StepsImagesBase64.Count != recipe.StepsTexts.Count)
        {
            throw new ArrayLengthsDontMatch();
        }

        string directory = $"{_storagePath}/{recipe.Id}";

        if (Directory.Exists(directory))
        {
            throw new EntryAlreadyExists();
        }

        _ = Directory.CreateDirectory(directory);
        SaveRecipeContents(directory, recipe);
    }

    public Recipe Get(Guid id)
    {
        string directory = $"{_storagePath}/{id}";

        return !Directory.Exists(directory) ? throw new EntryNotFound() : LoadRecipe(id);
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
        {
            throw new EntryNotFound();
        }

        Directory.Delete(directory, true);
    }

    private void SaveRecipeContents(string directory, Recipe recipe)
    {
        using FileStream textFileStream = File.Create($"{directory}/{RECIPE_TEXT_FILENAME}");

        SaveText(textFileStream, $"Name:{recipe.Name}\t\n");
        SaveText(textFileStream, $"Description:{recipe.Description}\t\n");

        SaveImage($"{directory}/{MAIN_IMAGE_FILENAME}{IMAGES_FORMAT}", recipe.MainImageBase64);

        for (int i = 0; i < recipe.StepsImagesBase64.Count; i++)
        {
            SaveText(textFileStream, $"Step{i + 1}:{recipe.StepsTexts[i]}\t\n");
            SaveImage(
                $"{directory}/{STEPS_IMAGES_PREFIX}{i + 1}{IMAGES_FORMAT}",
                recipe.StepsImagesBase64[i]
            );
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
            StepsImagesBase64 = LoadStepsImagesBase64(directory),
            StepsTexts = LoadStepsTexts(recipeText)
        };
        return recipe;
    }

    private List<string> LoadStepsImagesBase64(string directory)
    {
        List<string> recipeImages = new List<string>();
        int stepCounter = 1;
        string step = LoadStepImage(directory, stepCounter);
        while (step.Length > 0)
        {
            recipeImages.Add(step);
            stepCounter++;
            step = LoadStepImage(directory, stepCounter);
        }
        return recipeImages;
    }

    private List<string> LoadStepsTexts(string recipeText)
    {
        List<string> recipeTexts = new List<string>();
        int stepCounter = 1;
        string step = LoadStepText(recipeText, stepCounter);
        while (step.Length > 0)
        {
            recipeTexts.Add(step);
            stepCounter++;
            step = LoadStepText(recipeText, stepCounter);
        }
        return recipeTexts;
    }

    private string LoadStepImage(string directory, int stepNumber)
    {
        string stepImage = LoadImageBase64String(
            $"{directory}/{STEPS_IMAGES_PREFIX}{stepNumber}{IMAGES_FORMAT}"
        );
        return stepImage;
    }

    private string LoadStepText(string recipeText, int stepNumber)
    {
        string stepText = GetSubstringMatchingRegex(recipeText, $"Step{stepNumber}:(.*)\t\n");
        return stepText;
    }

    private string LoadTextFileContents(string filePath)
    {
        try
        {
            using FileStream fileStream = File.OpenRead(filePath);
            byte[] bytes = new byte[fileStream.Length];
            _ = fileStream.Read(bytes, 0, (int)fileStream.Length);
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
                "(.{8}-.{4}-.{4}-.{4}-.{12})$"
            );
            Guid id = Guid.Parse(directoryName);
            guids = guids.Append(id);
        }
        return guids;
    }
}
