using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model.Symbols;

internal class BeginSymbol : Symbol, ISymbol
{
    public BeginSymbol()
        : base(SymbolType.Begin)
    {

    }
}

