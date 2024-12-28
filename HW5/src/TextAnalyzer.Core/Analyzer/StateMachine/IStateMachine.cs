using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Analyzer.StateMachine;

internal interface IStateMachine
{
    Action<ISymbol> MoveNext(ISymbol nextSymbol);
}
