using System.Collections;
using System.Globalization;
using System.Text;
using TextAnalyzer.Core.Model.Interfaces;
using TextAnalyzer.Core.Model.Symbols.OneSign;

namespace TextAnalyzer.Core.Model;

internal class Word(IEnumerable<ISymbol> symbols) : IEnumerable<ISymbol>, ISentenceElement, IWord
{
    private readonly List<ISymbol> _symbols = symbols.ToList();

    public void Replace(int index, string str)
    {
        var symbols = new List<ISymbol>();

        for (int i = 0; i < str.Length; i++)
            symbols.Add(GetLetterOrDigitSymbol(str[i]));

        _symbols.RemoveAt(index);

        _symbols.InsertRange(index, symbols);
    }

    private static ISymbol GetLetterOrDigitSymbol(char c)
    {
        ISymbol symbol = char.GetUnicodeCategory(c) switch
        {
            UnicodeCategory.UppercaseLetter
                or UnicodeCategory.LowercaseLetter => new Letter(c),
            UnicodeCategory.DecimalDigitNumber => new Digit(c),
            _ => new Undefined(c),
        };
        return symbol;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var symbol in _symbols)
        {
            builder.Append(symbol);
        }

        return builder.ToString();
    }

    public IEnumerator<ISymbol> GetEnumerator()
    {
        return ((IEnumerable<ISymbol>)_symbols).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_symbols).GetEnumerator();
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return _symbols.Aggregate(17, (current, symbol) => current * 31 + symbol.GetHashCode());
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is Word other && _symbols.SequenceEqual(other._symbols);
    }
}

