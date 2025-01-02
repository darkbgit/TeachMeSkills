using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model.Symbols.OneSign;

public class Question : Symbol, ISymbol, ISentenceElement
{
    private const char QUESTION_CHAR = '?';
    public Question()
        : base(QUESTION_CHAR, SymbolType.Question)
    {

    }
}

