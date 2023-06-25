namespace CookBook.WebApi.Extentions;

using CookBook.Domain;
using CookBook.WebApi.Dtos;

public static class Converter
{
    public static RecipeDto AsDto(this Recipe recipe)
    {
        return new RecipeDto{
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            MainImageBase64 = recipe.MainImageBase64,
            StepsImagesAndDescriptions = recipe.StepsImagesAndDescriptions
        };
    }

    public static Recipe AsRecipe(this InputRecipeDto inputRecipeDto)
    {
        return new Recipe {
            Id = Guid.NewGuid(),
            Name = inputRecipeDto.Name,
            Description = inputRecipeDto.Description,
            MainImageBase64 = inputRecipeDto.MainImageBase64,
            StepsImagesAndDescriptions = inputRecipeDto.StepsImagesAndDescriptions
        };
    }
}