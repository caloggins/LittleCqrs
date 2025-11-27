namespace LittleCqrs;

public class NoSuchHandlerException : Exception
{
    public NoSuchHandlerException()
    {
    }

    public NoSuchHandlerException(string message) : base(message)
    {
    }

    public NoSuchHandlerException(string message, Exception inner) : base(message, inner)
    {
    }
}