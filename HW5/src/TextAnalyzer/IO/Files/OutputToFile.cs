using System.Text;
using TextAnalyzer.Core.Model.Interfaces;


namespace TextAnalyzer.IO.Files;

internal class OutputToFile : IOutput
{
    private readonly string _filePath;

    public OutputToFile(string filePath)
    {
        _filePath = filePath;
    }

    public void Print(string str)
    {
        using var file = new FileAssist(_filePath, FileMode.OpenOrCreate, FileAccess.Write);
        using var sw = new StreamWriter(file.FileStream, Encoding.Default);

        sw.WriteLine(str);
    }

    public void Print(ISentence sentence)
    {
        using var file = new FileAssist(_filePath, FileMode.OpenOrCreate, FileAccess.Write);
        using var sw = new StreamWriter(file.FileStream, Encoding.Default);

        var builder = new StringBuilder();

        foreach (var element in sentence)
        {
            builder.Append(element);
        }

        builder.Append(Environment.NewLine);
        sw.Write(builder);
    }

    public void Print(IText text)
    {
        const char SPACE_CHAR = ' ';

        using var file = new FileAssist(_filePath, FileMode.Create, FileAccess.Write);
        using var sw = new StreamWriter(file.FileStream, Encoding.Default);

        var builder = new StringBuilder();

        foreach (var sentence in text)
        {
            foreach (var element in sentence)
            {
                builder.Append(element);
            }

            builder.Append(SPACE_CHAR);
            sw.Write(builder);
            builder.Clear();
        }
    }
}

