using TextAnalyzer.Core.Model;
using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Analyzer;

internal class AnalyzerBuffer
{
    internal AnalyzerBuffer()
    {
        Symbols = new CollectionWithMax<ISymbol>(Params.MaxSymbolsInWord);
        SentenceElements = new CollectionWithMax<ISentenceElement>(Params.MaxElementsInSentence);
        Sentences = [];
    }

    internal CollectionWithMax<ISymbol> Symbols { get; set; }

    internal CollectionWithMax<ISentenceElement> SentenceElements { get; set; }

    internal ICollection<ISentence> Sentences { get; set; }
}

