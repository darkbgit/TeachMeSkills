using TextAnalyzer.Core.Model;
using TextAnalyzer.Core.Model.Interfaces;
using TextAnalyzer.Core.Model.Symbols;

namespace TextAnalyzer.Core.Analyzer.StateMachine;

internal class StateMachine : IStateMachine
{
    private readonly Dictionary<StateTransition, Action<ISymbol>> _transitions;

    private readonly AnalyzerBuffer _buffer;

    internal StateMachine(AnalyzerBuffer buffer)
    {
        _buffer = buffer;
        _buffer.Symbols.Clear();
        _buffer.Symbols.Add(new BeginSymbol());

        _transitions = new Dictionary<StateTransition, Action<ISymbol>>
            {
                {new StateTransition(SymbolType.Begin, SymbolType.Letter), AddSymbolAfterBegin},
                {new StateTransition(SymbolType.Begin, SymbolType.Digit), AddSymbolAfterBegin},
                {new StateTransition(SymbolType.Begin, SymbolType.Dot), SkipSymbol},
                {new StateTransition(SymbolType.Begin, SymbolType.PunctuationMark), AddSymbolAfterBegin},
                {new StateTransition(SymbolType.Begin, SymbolType.Exclamation), SkipSymbol},
                {new StateTransition(SymbolType.Begin, SymbolType.Question), SkipSymbol},
                {new StateTransition(SymbolType.Begin, SymbolType.Space), SkipSymbol},
                {new StateTransition(SymbolType.Begin, SymbolType.End), SkipSymbol},

                {new StateTransition(SymbolType.Letter, SymbolType.Letter), AddSymbol},
                {new StateTransition(SymbolType.Letter, SymbolType.Digit), AddSymbol},
                {new StateTransition(SymbolType.Letter, SymbolType.PunctuationMark), MakeWordAndAddSymbol},
                {new StateTransition(SymbolType.Letter, SymbolType.Dot), MakeWordAndAddSymbol},
                {new StateTransition(SymbolType.Letter, SymbolType.Question), MakeWordAndAddSymbol},
                {new StateTransition(SymbolType.Letter, SymbolType.Exclamation), MakeWordAndAddSymbol},
                {new StateTransition(SymbolType.Letter, SymbolType.Space), MakeWordAndAddSymbol},
                {new StateTransition(SymbolType.Letter, SymbolType.End), EndWithLetterOrDigit},

                {new StateTransition(SymbolType.Digit, SymbolType.Letter), AddSymbol},
                {new StateTransition(SymbolType.Digit, SymbolType.Digit), AddSymbol},
                {new StateTransition(SymbolType.Digit, SymbolType.PunctuationMark), MakeWordAndAddSymbol},
                {new StateTransition(SymbolType.Digit, SymbolType.Dot), MakeWordAndAddSymbol},
                {new StateTransition(SymbolType.Digit, SymbolType.Question), MakeWordAndAddSymbol},
                {new StateTransition(SymbolType.Digit, SymbolType.Exclamation), MakeWordAndAddSymbol},
                {new StateTransition(SymbolType.Digit, SymbolType.Space), MakeWordAndAddSymbol},
                {new StateTransition(SymbolType.Digit, SymbolType.End), EndWithLetterOrDigit},

                {new StateTransition(SymbolType.PunctuationMark, SymbolType.Letter), MakeSentenceElementAndAddSymbol},
                {new StateTransition(SymbolType.PunctuationMark, SymbolType.Digit), MakeSentenceElementAndAddSymbol},
                {new StateTransition(SymbolType.PunctuationMark, SymbolType.PunctuationMark), MakeSentenceElementAndAddSymbol},
                {new StateTransition(SymbolType.PunctuationMark, SymbolType.Dot), MakeSentenceElementAndAddSymbol},
                {new StateTransition(SymbolType.PunctuationMark, SymbolType.Question), MakeSentenceElementAndAddSymbol},
                {new StateTransition(SymbolType.PunctuationMark, SymbolType.Exclamation), MakeSentenceElementAndAddSymbol},
                {new StateTransition(SymbolType.PunctuationMark, SymbolType.Space), MakeSentenceElementAndAddSymbol},
                {new StateTransition(SymbolType.PunctuationMark, SymbolType.End), EndMakeSentenceElementAndMakeSentence},

                {new StateTransition(SymbolType.Dot, SymbolType.Letter), MakeSentenceElementAndMakeSentenceAndAddSymbol},
                {new StateTransition(SymbolType.Dot, SymbolType.Digit), MakeSentenceElementAndMakeSentenceAndAddSymbol},
                {new StateTransition(SymbolType.Dot, SymbolType.PunctuationMark), MakeSentenceElementAndMakeSentenceAndAddSymbol},
                {new StateTransition(SymbolType.Dot, SymbolType.Space), MakeSentenceElementAndMakeSentence},
                {new StateTransition(SymbolType.Dot, SymbolType.End), EndMakeSentenceElementAndMakeSentence},

                {new StateTransition(SymbolType.Question, SymbolType.Letter), MakeSentenceElementAndMakeSentenceAndAddSymbol},
                {new StateTransition(SymbolType.Question, SymbolType.Digit), MakeSentenceElementAndMakeSentenceAndAddSymbol},
                {new StateTransition(SymbolType.Question, SymbolType.PunctuationMark), MakeSentenceElementAndMakeSentenceAndAddSymbol},
                {new StateTransition(SymbolType.Question, SymbolType.Space), MakeSentenceElementAndMakeSentence},
                {new StateTransition(SymbolType.Question, SymbolType.End), EndMakeSentenceElementAndMakeSentence},

                {new StateTransition(SymbolType.Exclamation, SymbolType.Letter), MakeSentenceElementAndMakeSentenceAndAddSymbol},
                {new StateTransition(SymbolType.Exclamation, SymbolType.Digit), MakeSentenceElementAndMakeSentenceAndAddSymbol},
                {new StateTransition(SymbolType.Exclamation, SymbolType.PunctuationMark), MakeSentenceElementAndMakeSentenceAndAddSymbol},
                {new StateTransition(SymbolType.Exclamation, SymbolType.Space), MakeSentenceElementAndMakeSentence},
                {new StateTransition(SymbolType.Exclamation, SymbolType.End), EndMakeSentenceElementAndMakeSentence},


                {new StateTransition(SymbolType.Space, SymbolType.Letter), MakeSentenceElementAndAddSymbol},
                {new StateTransition(SymbolType.Space, SymbolType.Digit), MakeSentenceElementAndAddSymbol},
                {new StateTransition(SymbolType.Space, SymbolType.PunctuationMark), MakeSentenceElementAndAddSymbol},
                {new StateTransition(SymbolType.Space, SymbolType.Dot), MakeSentenceElementAndAddSymbol},
                {new StateTransition(SymbolType.Space, SymbolType.Space), SkipSymbol},
                {new StateTransition(SymbolType.Space, SymbolType.End), EndWithSpace}
            };
    }

    private Action<ISymbol> GetNext(SymbolType nextSymbol)
    {
        if (_buffer.Symbols.LastOrDefault() == null)
        {
            throw new NullReferenceException();
        }
        var transition = new StateTransition(_buffer.Symbols.Last().Type, nextSymbol);

        if (!_transitions.TryGetValue(transition, out var command))
            throw new StateMachineException(
                $"Invalid transition: {_buffer.Symbols.Last().Type} -> {nextSymbol} in sentence {_buffer.Sentences.Count + 1}. Symbol skipped");
        return command;
    }

    public Action<ISymbol> MoveNext(ISymbol nextSymbol)
    {
        var command = GetNext(nextSymbol.Type);
        return command;
    }

    private void AddSymbol(ISymbol nextSymbol)
    {
        _buffer.Symbols.Add(nextSymbol);
    }

    private void AddSymbolAfterBegin(ISymbol nextSymbol)
    {
        _buffer.Symbols.Clear();
        _buffer.Symbols.Add(nextSymbol);
    }

    private void SkipSymbol(ISymbol nextSymbol)
    {

    }

    private void MakeSentenceElementAndAddSymbol(ISymbol nextSymbol)
    {
        try
        {
            _buffer.SentenceElements.Add((ISentenceElement)_buffer.Symbols.Last());
        }
        finally
        {
            _buffer.Symbols.Clear();
            _buffer.Symbols.Add(nextSymbol);
        }
    }

    private void MakeSentenceElementAndMakeSentenceAndAddSymbol(ISymbol nextSymbol)
    {
        try
        {
            _buffer.SentenceElements.Add((ISentenceElement)_buffer.Symbols.Last());
        }
        finally
        {
            _buffer.Symbols.Clear();
            _buffer.Symbols.Add(nextSymbol);
            _buffer.Sentences.Add(new Sentence(_buffer.SentenceElements));
            _buffer.SentenceElements.Clear();
        }
    }

    private void MakeSentenceElementAndMakeSentence(ISymbol nextSymbol)
    {
        try
        {
            _buffer.SentenceElements.Add((ISentenceElement)_buffer.Symbols.Last());
        }
        finally
        {
            _buffer.Symbols.Clear();
            _buffer.Symbols.Add(new BeginSymbol());
            _buffer.Sentences.Add(new Sentence(_buffer.SentenceElements));
            _buffer.SentenceElements.Clear();
        }
    }

    private void EndWithSpace(ISymbol nextSymbol)
    {
        if (_buffer.SentenceElements.Any())
        {
            _buffer.Sentences.Add(new Sentence(_buffer.SentenceElements));
        }
        _buffer.SentenceElements.Clear();
    }

    private void MakeWordAndAddSymbol(ISymbol nextSymbol)
    {
        try
        {
            _buffer.SentenceElements.Add(new Word(_buffer.Symbols));
        }
        finally
        {
            _buffer.Symbols.Clear();
            _buffer.Symbols.Add(nextSymbol);
        }
    }

    private void EndMakeSentenceElementAndMakeSentence(ISymbol nextSymbol)
    {
        try
        {
            _buffer.SentenceElements.Add((ISentenceElement)_buffer.Symbols.Last());
        }
        finally
        {
            _buffer.Symbols.Clear();
            _buffer.Sentences.Add(new Sentence(_buffer.SentenceElements));
            _buffer.SentenceElements.Clear();
        }
    }

    private void EndWithLetterOrDigit(ISymbol nextSymbol)
    {
        _buffer.SentenceElements.Add(new Word(_buffer.Symbols));
        _buffer.Symbols.Clear();
        _buffer.Sentences.Add(new Sentence(_buffer.SentenceElements));
        _buffer.SentenceElements.Clear();
    }
}

