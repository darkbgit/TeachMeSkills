namespace Calculator.ConsoleApp.Helpers;

public class ConsoleHelpers
{
    public static void WriteLineWithColoredSymbol(string? text, int position)
    {
        if (string.IsNullOrEmpty(text))
            return;

        Console.Write(text[..position]);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(text[position]);
        Console.ResetColor();

        if (text.Length > position)
            Console.WriteLine(text[(position + 1)..]);
    }
}
