using Microsoft.Extensions.Configuration;
using TextAnalyzer.Core.Analyzer;
using TextAnalyzer.Core.Loggers;
using TextAnalyzer.Core.Model.Interfaces;
using TextAnalyzer.Core.Tasks;
using TextAnalyzer.IO;
using TextAnalyzer.IO.Consoles;
using TextAnalyzer.IO.Files;


AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

ILogger logger = new ConsoleLogger();

var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

var filePath = config["FilePath"]
    ?? throw new FileNotFoundException("Couldn't get file path from appsettings.json");


IText text;

using (var file = new FileAssist(filePath, FileMode.Open, FileAccess.Read))
{
    using var analyzer = new StreamAnalyzer(file.FileStream, logger);

    text = analyzer.Analyze();
}

IOutput output = new OutputToConsole();

var terminal = new Terminal(new CommandLine(), output);

var worker = new TasksWorker(text, output);

CommandLineCommand command;
try
{
    command = terminal.CommandLineArgumentParser(args);
}
catch (ArgumentException e)
{
    terminal.Print(e.Message);
    command = CommandLineCommand.Base;
}

bool breakFlag = true;
while (breakFlag)
{
    try
    {
        switch (command)
        {
            case CommandLineCommand.PrintData:
                terminal.Print(text);
                break;
            case CommandLineCommand.PrintWordWithMaxNumbersCount:
                worker.GetWordWithMaxNumberOfDigits();
                break;
            case CommandLineCommand.PrintLongestWord:
                worker.GetMostLongestWordAndHowManyTimesItOccurs();
                break;
            case CommandLineCommand.PrintWordWithSameBeginAndEnd:
                worker.GetWordsWithSameBeginAndEnd();
                break;
            case CommandLineCommand.PrintExclamationAndQuestionSentences:
                worker.GetExclamationAndQuestionSentences();
                break;
            case CommandLineCommand.PrintSentenceWithoutComms:
                worker.GetSentencesWithoutCommas();
                break;
            case CommandLineCommand.ExchangeNumbersWithLetters:
                worker.ExchangeNumbersToLetters();
                break;
            case CommandLineCommand.Exit:
                breakFlag = false;
                continue;
            case CommandLineCommand.UndefinedCommand:
            case CommandLineCommand.Base:
            default:
                terminal.PrintHelp();
                break;
        }
    }
    catch (ArgumentException e)
    {
        terminal.Print(e.Message);
    }

    try
    {
        (command, args) = terminal.CommandLineArgumentParser();
    }
    catch (ArgumentException e)
    {
        terminal.Print(e.Message);
        command = CommandLineCommand.Base;
    }
}


static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
{
    Console.WriteLine("Error " + (e.ExceptionObject as Exception)?.Message);
    Console.WriteLine("Application will be terminated. Press any key...");
    Console.ReadKey();
}

