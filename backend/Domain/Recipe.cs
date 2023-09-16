namespace Domain;

public record Recipe
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string MainImageBase64 { get; set; } = "";
    public List<string> StepsImagesBase64 { get; set; } = new List<string>();
    public List<string> StepsTexts { get; set; } = new List<string>();
}
