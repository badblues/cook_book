namespace CookBook.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using CookBook.WebApi.Dtos;
using CookBook.Persistence;
using CookBook.Domain;
using CookBook.WebApi.Extensions;
using CookBook.Exceptions;

[ApiController]
[Route("recipes")]
public class RecipeController : ControllerBase
{
    private IRepository<Recipe> _repository;
    private RecipeConverter _converter;

    public RecipeController(IRepository<Recipe> repository, RecipeConverter converter)
    {
        this._repository = repository;
        this._converter = converter;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<RecipeDto>> GetRecipes()
    {
        IEnumerable<Recipe> recipes = _repository.GetAll();
        return Ok(recipes.Select(recipe => _converter.ConvertImagesToUrls(recipe)));
    }

    [HttpGet("{id}")]
    public ActionResult<RecipeDto> GetRecipe(Guid id)
    {
        try
        {
            Recipe recipe = _repository.Get(id);
            return Ok(_converter.ConvertImagesToUrls(recipe));
        }
        catch (EntryNotFound)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public ActionResult<RecipeDto> CreateRecipe(InputRecipeDto inputRecipeDto)
    {
        try
        {
            Recipe recipe = inputRecipeDto.AsRecipe();
            recipe.Id = Guid.NewGuid();
            _repository.Create(recipe);
            return Ok(_converter.ConvertImagesToUrls(recipe));
        }
        catch (EntryAlreadyExists err)
        {
            return Conflict(err.Message);
        }
    }

    [HttpPut("{id}")]
    public ActionResult UpdateRecipe(Guid id, InputRecipeDto inputRecipeDto)
    {
        try
        {
            Recipe updatedRecipe = inputRecipeDto.AsRecipe();
            updatedRecipe.Id = id;
            _repository.Update(updatedRecipe);
        }
        catch (EntryNotFound)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteRecipe(Guid id)
    {
        try
        {
            _repository.Delete(id);
        }
        catch (EntryNotFound)
        {
            return NotFound();
        }
        return Ok();
    }
}
