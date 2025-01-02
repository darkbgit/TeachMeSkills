using TextAnalyzer.Core.Model.Symbols;

namespace TextAnalyzer.Core.Model.Interfaces;

public interface ISymbol : IEquatable<char>
{
    SymbolType Type { get; }

    char? SymbolChar { get; }
}

