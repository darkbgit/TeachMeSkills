namespace TextAnalyzer.IO.Consoles;

public class CommandLine : ICommandLine
{
    public CommandLineCommand CommandLineArgumentParser(string[] args)
    {
        if (args.Length == 0)
        {
            return CommandLineCommand.Base;
        }

        return args.FirstOrDefault() switch
        {
            CommandLineArguments.PrintData => CommandLineCommand.PrintData,
            CommandLineArguments.PrintWordWithMaxNumbersCount => CommandLineCommand.PrintWordWithMaxNumbersCount,
            CommandLineArguments.PrintLongestWord => CommandLineCommand.PrintLongestWord,
            CommandLineArguments.PrintWordWithSameBeginAndEnd => CommandLineCommand.PrintWordWithSameBeginAndEnd,
            CommandLineArguments.PrintExclamationAndQuestionSentences => CommandLineCommand.PrintExclamationAndQuestionSentences,
            CommandLineArguments.PrintSentenceWithoutComms => CommandLineCommand.PrintSentenceWithoutComms,
            CommandLineArguments.ExchangeNumbersWithLetters => CommandLineCommand.ExchangeNumbersWithLetters,
            CommandLineArguments.Exit => CommandLineCommand.Exit,
            _ => throw new ArgumentException($"команды \"{args[0]}\" не существует"),
        };
    }

    public string[] GetArguments()
    {
        const char QUOTATION = '"';
        const char SEPARATOR = ' ';

        var line = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
        {
            return [];
        }

        if (line.Count(ch => ch == QUOTATION) % 2 != 0)
        {
            throw new ArgumentException("Неверный формат командной строки");
        }

        var arguments = line.Split(SEPARATOR).ToList();

        int index = arguments.FindIndex(s => s.First() == QUOTATION);

        while (index != -1)
        {
            int nextIndex = arguments.FindIndex(index, s => s.Last() == QUOTATION);

            if (nextIndex == -1)
            {
                throw new ArgumentException("Неверная расстановка кавычек");
            }

            arguments[index] = arguments[index][1..];
            for (int i = index; i < nextIndex; i++)
            {
                arguments[index] += ' ' + arguments[index + 1];
                arguments.RemoveAt(index + 1);
            }

            arguments[index] = arguments[index][..^1];

            index = arguments.FindIndex(index + 1, s => s.First() == QUOTATION);
        }

        return arguments.ToArray();
    }

}

