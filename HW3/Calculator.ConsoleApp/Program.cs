using Calculator.ConsoleApp.Helpers;
using Calculator.Core;

AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

var help = """
Calculator can solve equations with operators + - * / sqrt() % ( ).
Example: 12 + 7 / (8 * 9) - sqrt(2)
""";

Console.WriteLine(help);

while (true)
{
    Console.WriteLine("Enter equation:");

    var equationStr = Console.ReadLine();

    try
    {
        var result = EquationSolver.SolveEquation(equationStr);
        Console.WriteLine(result);
    }
    catch (ParserException ex)
    {
        Console.WriteLine(ex.Message);

        ConsoleHelpers.WriteLineWithColoredSymbol(equationStr, ex.Position);
        continue;
    }
    catch (SolveException ex)
    {
        Console.WriteLine(ex.Message);

        continue;
    }
}

static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
{
    Console.WriteLine("Error " + (e.ExceptionObject as Exception)?.Message);
    Console.WriteLine("Application will be terminated. Press any key...");
    Console.ReadKey();
}