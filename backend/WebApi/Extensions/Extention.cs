namespace CookBook.WebApi.Extensions;

using CookBook.Domain;
using CookBook.WebApi.Dtos;

public static class Extension
{

    public static Recipe AsRecipe(this InputRecipeDto inputRecipeDto)
    {
        return new Recipe
        {
            Name = inputRecipeDto.Name,
            Description = inputRecipeDto.Description,
            MainImageBase64 = inputRecipeDto.MainImageBase64,
            StepsImagesBase64 = inputRecipeDto.StepsImagesBase64,
            StepsTexts = inputRecipeDto.StepsTexts
        };
    }
}
