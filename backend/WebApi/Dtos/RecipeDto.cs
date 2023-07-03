namespace CookBook.WebApi.Dtos;

using System.ComponentModel.DataAnnotations;

public record RecipeDto
{
    [Required]
    public Guid Id {get; init;}
    [Required]
    public string Name {get; set;} = "";
    [Required]
    public string Description {get; set;} = "";
    [Required]
    public string MainImagePath {get; set;} = "";
    [Required]
    public List<string> StepsImagesBase64 { get; set; } = new List<string>();
    [Required]
    public List<string> StepsTexts { get; set; } = new List<string>();
}

