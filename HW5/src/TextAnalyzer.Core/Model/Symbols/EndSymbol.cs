using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model.Symbols;

internal class EndSymbol : Symbol, ISymbol
{
    public EndSymbol()
        : base(SymbolType.End)
    {

    }
}

