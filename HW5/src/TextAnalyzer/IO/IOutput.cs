using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.IO;

public interface IOutput
{
    void Print(string str);

    void Print(IText text);

    void Print(ISentence sentence);
}

