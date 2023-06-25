namespace CookBook.Domain;

public record Recipe
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string MainImageBase64 { get; set; } = "";
    public List<(string, string)> StepsImagesAndDescriptions { get; set; } =
        new List<(string, string)>();
}
