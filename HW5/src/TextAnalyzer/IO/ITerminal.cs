using TextAnalyzer.Core.Model.Interfaces;
using TextAnalyzer.IO.Consoles;

namespace TextAnalyzer.IO;

public interface ITerminal
{
    void PrintHelp();

    void Print(string str);

    void Print(IText text);

    void Print(ISentence sentence);

    CommandLineCommand CommandLineArgumentParser(string[] args);

    (CommandLineCommand command, string[] args) CommandLineArgumentParser();
}

