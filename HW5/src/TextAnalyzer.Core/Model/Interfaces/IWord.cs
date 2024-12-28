namespace TextAnalyzer.Core.Model.Interfaces;

public interface IWord : IEnumerable<ISymbol>, ISentenceElement
{
    void Replace(int index, string str);
}
