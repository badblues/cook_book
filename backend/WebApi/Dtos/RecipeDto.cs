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
    public string MainImageBase64 {get; set;} = "";
    [Required]
    public List<(string, string)> StepsImagesAndDescriptions {get; set;} = new List<(string, string)>();
}

