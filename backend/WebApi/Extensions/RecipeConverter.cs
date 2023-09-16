using Domain;

using RecipeStorage;

using WebApi.Dtos;

namespace WebApi.Extensions;

public class RecipeConverter
{
    private readonly string _storagePath;

    public RecipeConverter(string storagePath)
    {
        _storagePath = storagePath;
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
