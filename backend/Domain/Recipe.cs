namespace CookBook.Domain;

public class Recipe
{
    Guid id;
    string Name = "";
    string Base64Icon = "";
    string Description = "";
    (string, string)[] StepsImageAndDescription = { };
}
