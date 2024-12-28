using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Analyzer;

public interface IAnalyzer : IDisposable
{
    IText Analyze();
}

