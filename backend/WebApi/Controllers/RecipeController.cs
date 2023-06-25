namespace CookBook.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using CookBook.WebApi.Dtos;
using CookBook.Persistence;
using CookBook.Domain;
using CookBook.WebApi.Extentions;

[ApiController]
[Route("recipes")]
public class RecipeController : ControllerBase
{
    private IRepository<Recipe> _repository;

    public RecipeController(IRepository<Recipe> repository)
    {
        this._repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<RecipeDto>> GetRecipes()
    {
        IEnumerable<Recipe> recipes = _repository.GetAll();
        return Ok(recipes.Select(recipe => recipe.AsDto()));
    }

    [HttpGet("{id}")]
    public ActionResult<RecipeDto> GetRecipe(Guid id)
    {
        Recipe? recipe = _repository.Get(id);
        if (recipe is null)
            return NotFound();
        return Ok(recipe.AsDto());
    }

    [HttpPost]
    public ActionResult<RecipeDto> CreateRecipe(InputRecipeDto inputRecipeDto)
    {
        Recipe recipe = inputRecipeDto.AsRecipe();
        _repository.Create(recipe);
        return Ok(recipe.AsDto());
    }

    [HttpPut("{id}")]
    public ActionResult UpdateRecipe(Guid id, InputRecipeDto inputRecipeDto)
    {
        Recipe? existingRecipe = _repository.Get(id);
        if (existingRecipe is null)
            return NotFound();
        Recipe updatedRecipe = existingRecipe with
        {
            Name = inputRecipeDto.Name,
            Description = inputRecipeDto.Description,
            MainImageBase64 = inputRecipeDto.MainImageBase64,
            StepsImagesAndDescriptions = inputRecipeDto.StepsImagesAndDescriptions
        };
        _repository.Update(updatedRecipe);
        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteRecipe(Guid id)
    {
        if (_repository.Get(id) is null)
            return NotFound();
        _repository.Delete(id);
        return Ok();
    }

}
