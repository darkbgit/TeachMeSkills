using System.Text;
using TextAnalyzer.Core.Model.Interfaces;
using TextAnalyzer.IO.Consoles;

namespace TextAnalyzer.IO;

public class Terminal(ICommandLine commandLine, IOutput output) : ITerminal

{
    private readonly string _helpString = GenerateHelpString();

    private readonly ICommandLine _commandLine = commandLine;

    private readonly IOutput _output = output;

    public void Print(string str)
    {
        _output.Print(str);
    }

    public void Print(IText text)
    {
        _output.Print(text);
    }

    public void Print(ISentence sentence)
    {
        _output.Print(sentence);
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
        builder.AppendLine($"[{CommandLineArguments.PrintData}] - вывод модели");
        builder.Append(' ', firstLength);
        builder.AppendLine($"[{CommandLineArguments.PrintWordWithMaxNumbersCount}] - вывод слова с максимальным колличеством цифр");
        builder.Append(' ', firstLength);
        builder.AppendLine($"[{CommandLineArguments.PrintLongestWord}] - вывод вывод самого длинного слова и колличество его повторений в тексте");
        builder.Append(' ', firstLength);
        builder.AppendLine(
            $"[{CommandLineArguments.PrintWordWithSameBeginAndEnd}] - вывести все слова с одинаковым символом в начале и конце");
        builder.Append(' ', firstLength);
        builder.AppendLine(
            $"[{CommandLineArguments.PrintExclamationAndQuestionSentences}] - вывести сначала все восклицательные, а затем все вопросительные предложения");
        builder.Append(' ', firstLength);
        builder.AppendLine(
            $"[{CommandLineArguments.PrintSentenceWithoutComms}] - вывести все предложения без запятых");
        builder.Append(' ', firstLength);
        builder.AppendLine(
            $"[{CommandLineArguments.ExchangeNumbersWithLetters}] - заменить все цифры в словах на их написанние");
        builder.Append(' ', firstLength);
        builder.AppendLine($"[{CommandLineArguments.Exit}] - выход");
        return builder.ToString();
    }
}

