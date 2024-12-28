using System.Collections;

namespace TextAnalyzer.Core.Analyzer;

internal class CollectionWithMax<T>(int max) : IEnumerable<T>
{
    private readonly ICollection<T> _collection = [];

    private readonly int _max = max;

    internal void Add(T item)
    {
        if (_collection.Count >= _max)
        {
            var skipped = string.Join(string.Empty, _collection);
            _collection.Clear();
            _collection.Add(item);

            throw new ArgumentOutOfRangeException(typeof(T).Name, $"Too big. \"{skipped}\" skipped");
        }
        _collection.Add(item);
    }

    public void Clear()
    {
        _collection.Clear();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_collection).GetEnumerator();
    }
}