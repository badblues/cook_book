using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;
public record InputRecipeDto
{
    [Required]
    public string Name { get; set; } = "";
    [Required]
    public string Description { get; set; } = "";
    [Required]
    public string MainImageBase64 { get; set; } = "";
    [Required]
    public List<string> StepsImagesBase64 { get; set; } = new List<string>();
    [Required]
    public List<string> StepsTexts { get; set; } = new List<string>();
}

