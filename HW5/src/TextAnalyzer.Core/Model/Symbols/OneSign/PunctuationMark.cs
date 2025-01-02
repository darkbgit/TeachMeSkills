using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model.Symbols.OneSign;

public class PunctuationMark(char c) : Symbol(c, SymbolType.PunctuationMark), ISymbol, ISentenceElement
{
}
