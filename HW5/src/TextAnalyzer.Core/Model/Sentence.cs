using System.Collections;
using System.Text;
using TextAnalyzer.Core.Model.Interfaces;

namespace TextAnalyzer.Core.Model;

public class Sentence(IEnumerable<ISentenceElement> elements) : ISentence
{
    private readonly IList<ISentenceElement> _elements = elements.ToList();

    public int WordsCount => _elements.Count(e => e is Word);

    public override string ToString()
    {
        var builder = new StringBuilder();

        foreach (var element in _elements)
        {
            builder.Append(element.ToString());
        }

        return builder.ToString();
    }

    public IEnumerator<ISentenceElement> GetEnumerator()
    {
        return _elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_elements).GetEnumerator();
    }
}
