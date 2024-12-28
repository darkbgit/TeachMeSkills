using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model.Symbols;

public abstract class Symbol : ISymbol
{
    protected readonly char? _symbol;
    protected Symbol(char symbol, SymbolType type)
    {
        _symbol = symbol;
        Type = type;
    }

    protected Symbol(SymbolType type)
    {
        _symbol = null;
        Type = type;
    }

    public SymbolType Type { get; }

    public char? SymbolChar => _symbol;

    public override string ToString()
    {
        if (_symbol == null || !_symbol.HasValue)
        {
            return string.Empty;
        }
        else
        {
            return _symbol.Value.ToString();
        }
    }

    public override int GetHashCode()
    {
        return 17 + 37 * _symbol.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is Symbol other && _symbol == other._symbol;
    }

    public bool Equals(char c)
    {
        return _symbol == c;
    }
}

