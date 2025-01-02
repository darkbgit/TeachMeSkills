using System.Collections;
using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model;

internal class Text(IEnumerable<ISentence> sentences) : IText
{
    private readonly List<ISentence> _sentences = sentences.ToList();

    public IEnumerator<ISentence> GetEnumerator()
    {
        return ((IEnumerable<ISentence>)_sentences).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_sentences).GetEnumerator();
    }
}

