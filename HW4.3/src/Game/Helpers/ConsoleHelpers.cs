using System.Globalization;

namespace Game.Helpers;

public class ConsoleHelpers
{
    public static int GetIntFromConsole(string name)
    {
        Console.WriteLine($"Input {name}:");
        while (true)
        {
            var str = Console.ReadLine();
            if (int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                return value;

            Console.WriteLine($"\"{str}\" - incorrect integer. Try again.");
        }
    }
}