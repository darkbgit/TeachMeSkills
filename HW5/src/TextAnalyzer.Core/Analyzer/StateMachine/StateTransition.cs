using TextAnalyzer.Core.Model.Symbols;

namespace TextAnalyzer.Core.Analyzer.StateMachine;

internal class StateTransition(SymbolType currentSymbol, SymbolType nextSymbol)
{
    private readonly SymbolType _currentSymbol = currentSymbol;
    private readonly SymbolType _nextSymbol = nextSymbol;

    public override int GetHashCode()
    {
        return 17 + 31 * _currentSymbol.GetHashCode() + 31 * _nextSymbol.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is StateTransition other && _currentSymbol == other._currentSymbol && _nextSymbol == other._nextSymbol;
    }
}
