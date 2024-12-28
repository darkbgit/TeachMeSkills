namespace ConsoleApp.IO.Consoles;

internal class CommandLine : ICommandLine
{
    private const char _separator = '-';

    public CommandLineCommand CommandLineArgumentParser(string[] args)
    {
        if (args.Length == 0)
            return CommandLineCommand.Base;

        return args switch
        {
        [CommandLineArguments.PrintMatrix] => CommandLineCommand.PrintMatrix,
        [CommandLineArguments.InitMatrix] => CommandLineCommand.InitMatrix,
        [CommandLineArguments.FindAllNumbers, CommandLineArguments.FindAllNumbersPositive] => CommandLineCommand.FindAllPositiveNumbers,
        [CommandLineArguments.FindAllNumbers, CommandLineArguments.FindAllNumbersNegative] => CommandLineCommand.FindAllNegativeNumbers,
        [CommandLineArguments.SortMatrixRows, CommandLineArguments.SortMatrixRowsByDescending] => CommandLineCommand.SortNumbersInRowsDescending,
        [CommandLineArguments.SortMatrixRows, CommandLineArguments.SortMatrixRowsByAscending] => CommandLineCommand.SortNumbersInRowsAscending,
        [CommandLineArguments.InverseElementsInRows] => CommandLineCommand.InverseElementsInRows,
        [CommandLineArguments.Exit] => CommandLineCommand.Exit,
            _ => CommandLineCommand.UndefinedCommand
        };
    }

    public string[] GetArguments()
    {
        var line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
            return [];

        return line.Split(_separator);
    }
}
