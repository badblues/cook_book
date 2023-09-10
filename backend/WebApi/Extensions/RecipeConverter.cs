using CookBook.Domain;
using CookBook.WebApi.Dtos;
using CookBook.RecipeStorage;

namespace CookBook.WebApi.Extensions;

public class RecipeConverter
{
    private string _storagePath;

    public RecipeConverter(string storagePath)
    {
        this._storagePath = storagePath;
    }

    public RecipeDto ConvertImagesToUrls(Recipe recipe)
    {
        List<string> imagePaths = new List<string>();
        for (int i = 0; i < recipe.StepsImagesBase64.Count; i++)
        {
            string imagePath =
                $"{_storagePath}/{recipe.Id}/{Storage.STEPS_IMAGES_PREFIX}{i + 1}{Storage.IMAGES_FORMAT}";
            imagePaths.Add(imagePath);
        }
        return new RecipeDto()
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            MainImagePath =
                $"{_storagePath}/{recipe.Id}/{Storage.MAIN_IMAGE_FILENAME}{Storage.IMAGES_FORMAT}",
            StepsImagesPaths = imagePaths,
            StepsTexts = recipe.StepsTexts
        };
    }
}
