namespace Exceptions;

public class EntryAlreadyExists : Exception
{
    public EntryAlreadyExists()
        : base("Entry with that ID already exists.") { }
}
