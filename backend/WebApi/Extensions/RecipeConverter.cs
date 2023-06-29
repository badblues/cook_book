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
        List<(string, string)> urlsAndDescriptions = new List<(string, string)>();
        int counter = 1;
        foreach ((string, string) pair in recipe.StepsImagesAndDescriptions)
        {
            string imageUrl =
                $"{_storagePath}/{recipe.Id}/{Storage.STEPS_IMAGES_PREFIX}{counter++}{Storage.IMAGES_FORMAT}";
            urlsAndDescriptions.Add((imageUrl, pair.Item2));
        }
        return new RecipeDto()
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            MainImagePath =
                $"{_storagePath}/{recipe.Id}/{Storage.MAIN_IMAGE_FILENAME}{Storage.IMAGES_FORMAT}",
            StepsImagePathsAndDescriptions = urlsAndDescriptions
        };
    }
}
