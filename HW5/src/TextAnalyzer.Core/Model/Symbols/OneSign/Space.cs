using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model.Symbols.OneSign;

public class Space : Symbol, ISymbol, ISentenceElement
{
    private const char SPACE_CHAR = ' ';
    public Space() :
        base(SPACE_CHAR, SymbolType.Space)
    {

    }
}

