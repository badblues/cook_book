namespace Exceptions;

public class ArrayLengthsDontMatch : Exception
{
    public ArrayLengthsDontMatch()
        : base("Lengths of arrays in provided recipe don't match.") { }
}
