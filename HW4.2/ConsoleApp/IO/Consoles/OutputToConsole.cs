using System.Text;
using Microsoft.VisualBasic;

namespace ConsoleApp.IO.Consoles;

public class OutputToConsole : IOutput
{
    public void Print(int[,] matrix)
    {
        var maxElementLength = matrix
            .Cast<int>()
            .Max(i =>
            {
                if (i < 0)
                    return Math.Abs(i) * 10;
                else
                    return i;
            })
            .ToString()
            .Length;

        var builder = new StringBuilder();

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                var elementStr = matrix[i, j].ToString();

                builder.Append(elementStr);
                builder.Append(' ', maxElementLength - elementStr.Length + 1);
            }
            builder.AppendLine();
        }

        Console.WriteLine(builder.ToString());
    }

    public void Print(string str)
    {
        Console.WriteLine(str);
    }
}
