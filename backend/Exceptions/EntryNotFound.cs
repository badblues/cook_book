namespace CookBook.Exceptions;

public class EntryNotFound : Exception
{
    public EntryNotFound()
        : base("Entry with that ID Not found.") { }
}
