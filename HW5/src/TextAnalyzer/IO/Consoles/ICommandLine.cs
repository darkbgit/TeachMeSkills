namespace TextAnalyzer.IO.Consoles;

public interface ICommandLine
{
    CommandLineCommand CommandLineArgumentParser(string[] args);

    string[] GetArguments();
}

