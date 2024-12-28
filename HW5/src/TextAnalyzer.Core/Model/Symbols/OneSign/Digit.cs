using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model.Symbols.OneSign;

internal class Digit(char symbol) : Symbol(symbol, SymbolType.Digit), ISymbol
{
}

