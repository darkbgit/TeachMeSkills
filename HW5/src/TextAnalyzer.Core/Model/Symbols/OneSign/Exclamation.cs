using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model.Symbols.OneSign;

internal class Exclamation : Symbol, ISymbol, ISentenceElement
{
    private const char EXCLAMATION_CHAR = '!';
    public Exclamation()
        : base(EXCLAMATION_CHAR, SymbolType.Exclamation)
    {

    }
}

