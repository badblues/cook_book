namespace CookBook.Domain;

public class Recipe
{
    public Guid Id {get; init;}
    public string Name {get; set;} = "";
    public string Description {get; set;} = "";
    public string MainImageBase64 {get; set;} = "";
    public List<(string, string)> StepsImageAndDescription {get; set;} = new List<(string, string)>();
}

