using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model.Symbols.OneSign;

public class Letter(char symbol) : Symbol(symbol, SymbolType.Letter), ISymbol
{
}

