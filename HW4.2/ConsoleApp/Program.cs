using ConsoleApp.Extensions;
using ConsoleApp.Helpers;
using ConsoleApp.IO;
using ConsoleApp.IO.Consoles;

AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

var terminal = new Terminal(new CommandLine(), new OutputToConsole());

int[,]? matrix = null;

bool breakFlag = true;
while (breakFlag)
{
    if (matrix == null)
    {
        terminal.Print("Матрица не инициализирована.");
        ConsoleHelpers.InitMatrix(ref matrix);
        args = terminal.GetArguments();
        continue;
    }

    switch (terminal.CommandLineArgumentParser(args))
    {
        case CommandLineCommand.PrintMatrix:
            terminal.Print(matrix!);
            break;
        case CommandLineCommand.InitMatrix:
            matrix = null;
            ConsoleHelpers.InitMatrix(ref matrix);
            break;
        case CommandLineCommand.FindAllPositiveNumbers:
            terminal.Print($"Количество положительных чисел в матрице - {matrix.FindNumbers()}");
            break;
        case CommandLineCommand.FindAllNegativeNumbers:
            terminal.Print($"Количество отрицательных чисел в матрице - {matrix.FindNumbers(false)}");
            break;
        case CommandLineCommand.SortNumbersInRowsDescending:
            matrix.SortRows();
            terminal.Print(matrix);
            break;
        case CommandLineCommand.SortNumbersInRowsAscending:
            matrix!.SortRows(false);
            terminal.Print(matrix!);
            break;
        case CommandLineCommand.InverseElementsInRows:
            matrix.InvertRows();
            terminal.Print(matrix);
            break;
        case CommandLineCommand.Exit:
            breakFlag = false;
            continue;
        case CommandLineCommand.Base:
        case CommandLineCommand.UndefinedCommand:
        default:
            terminal.PrintHelp();
            break;
    }
    args = terminal.GetArguments();
}

static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
{
    Console.WriteLine("Error " + (e.ExceptionObject as Exception)?.Message);
    Console.WriteLine("Application will be terminated. Press any key...");
    Console.ReadKey();
}