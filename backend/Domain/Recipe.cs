namespace CookBook.Domain;

public class Recipe
{
    public Guid Id {get; init;}
    public string Name {get; set;} = "";
    public string Description {get; set;} = "";
    public string ImageBase64 {get; set;} = "";
    public (string, string)[] StepsImageAndDescription {get; set;} = Array.Empty<(string, string)>();
}

