using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model.Symbols.OneSign;

public class Undefined(char symbol) : Symbol(symbol, SymbolType.Undefined), ISymbol
{
}

