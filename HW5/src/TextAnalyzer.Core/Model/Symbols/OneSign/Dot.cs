using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model.Symbols.OneSign;

internal class Dot : Symbol, ISymbol, ISentenceElement
{
    private const char DOT_CHAR = '.';
    public Dot()
        : base(DOT_CHAR, SymbolType.Dot)
    {

    }
}

