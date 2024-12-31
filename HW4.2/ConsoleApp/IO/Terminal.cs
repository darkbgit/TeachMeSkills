using System.Text;
using ConsoleApp.IO.Consoles;

namespace ConsoleApp.IO;

internal class Terminal(ICommandLine commandLine, IOutput output)
{
    private readonly string _helpString = GenerateHelpString();

    private readonly ICommandLine _commandLine = commandLine;

    private readonly IOutput _output = output;

    public void Print(string str)
    {
        _output.Print(str);
    }

    public void Print(int[,] matrix)
    {
        _output.Print(matrix);
    }

    public CommandLineCommand CommandLineArgumentParser(string[] args)
    {
        return _commandLine.CommandLineArgumentParser(args);
    }

    public (CommandLineCommand command, string[] args) CommandLineArgumentParser()
    {
        var args = _commandLine.GetArguments();
        var command = _commandLine.CommandLineArgumentParser(args);

        return (command, args);
    }

    public string[] GetArguments()
    {
        return _commandLine.GetArguments();
    }


    public void PrintHelp()
    {
        _output.Print(_helpString);
    }

    private static string GenerateHelpString()
    {
        var builder = new StringBuilder();
        builder.AppendLine("usage: " + Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName));
        var firstLength = builder.Length;
        builder.Append(' ', firstLength);
        builder.AppendLine($"[{CommandLineArguments.PrintMatrix}] - вывод матрицы");
        builder.Append(' ', firstLength);
        builder.AppendLine($"[{CommandLineArguments.InitMatrix}] - инициализация матрицы");
        builder.Append(' ', firstLength);
        builder.AppendLine($"[{CommandLineArguments.FindAllNumbers}-{CommandLineArguments.FindAllNumbersPositive}] - вывод всех положительных чисел в матрицу");
        builder.Append(' ', firstLength);
        builder.AppendLine($"[{CommandLineArguments.FindAllNumbers}-{CommandLineArguments.FindAllNumbersNegative}] - вывод всех отрицательных чисел в матрицу");
        builder.Append(' ', firstLength);
        builder.AppendLine($"[{CommandLineArguments.SortMatrixRows}-{CommandLineArguments.SortMatrixRowsByDescending}] - сортировка элементов матрицы построчно по возрастанию");
        builder.Append(' ', firstLength);
        builder.AppendLine($"[{CommandLineArguments.SortMatrixRows}-{CommandLineArguments.SortMatrixRowsByAscending}] - сортировка элементов матрицы построчно по убыванию");
        builder.Append(' ', firstLength);
        builder.AppendLine($"[{CommandLineArguments.InverseElementsInRows}] - инверсия элементов матрицы в строках");
        builder.Append(' ', firstLength);
        builder.AppendLine($"[{CommandLineArguments.Exit}] - выход");
        return builder.ToString();
    }
}
