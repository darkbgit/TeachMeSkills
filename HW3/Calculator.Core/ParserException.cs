namespace Calculator.Core;

public class ParserException : Exception
{
    public ParserException(string message)
        : base(message)
    {
    }

    public ParserException(string message, int position)
        : base(message)
    {
        Position = position;
    }

    public int Position { get; }
}
