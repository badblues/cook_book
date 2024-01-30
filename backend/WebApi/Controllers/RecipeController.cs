using Domain;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Serilog;
using WebApi.Dtos;
using WebApi.Extensions;

namespace WebApi.Controllers;
[ApiController]
[Route("recipes")]
public class RecipeController : ControllerBase
{
    private readonly IRepository<Recipe> _repository;
    private readonly RecipeConverter _converter;
    private readonly Serilog.ILogger _logger = Log.ForContext<RecipeController>();

    public RecipeController(IRepository<Recipe> repository, RecipeConverter converter)
    {
        _repository = repository;
        _converter = converter;
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
            _logger.Information($"Creating recipe, id:{recipe.Id}");
            _repository.Create(recipe);
            return Ok(_converter.ConvertImagesToUrls(recipe));
        }
        catch (EntryAlreadyExists err)
        {
            _logger.Error(err.Message);
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
            _logger.Information($"Updating recipe, id:{updatedRecipe.Id}");
            _repository.Update(updatedRecipe);
        }
        catch (EntryNotFound err)
        {
            _logger.Error(err.Message);
            return NotFound(err.Message);
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteRecipe(Guid id)
    {
        try
        {
            _logger.Information($"Deleting recipe, id:{id}");
            _repository.Delete(id);
        }
        catch (EntryNotFound err)
        {
            _logger.Error(err.Message);
            return NotFound(err.Message);
        }
        return Ok();
    }
}
