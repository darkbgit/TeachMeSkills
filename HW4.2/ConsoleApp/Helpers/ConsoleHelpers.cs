using System.Globalization;

namespace ConsoleApp.Helpers;

public class ConsoleHelpers
{
    public static void InitMatrix(ref int[,]? matrix)
    {
        if (matrix != null)
            matrix = null;

        Console.WriteLine("Input matrix size. rows x columns");

        var rows = GetIntFromConsole("rows");
        var columns = GetIntFromConsole("columns");

        matrix = new int[rows, columns];

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = GetIntFromConsole($"Value for {i} row, {j} column");
            }
        }
    }

    private static int GetIntFromConsole(string name)
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