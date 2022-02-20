// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;

internal sealed class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
	where TKey : notnull
{
	private readonly List<TElement> _elements = new();

	public Grouping(TKey key, TElement element)
	{
		Key = key;

		_elements.Add(element);
	}

	public TKey Key { get; }

	public void Add(TElement element) =>
		_elements.Add(element);

	public IReadOnlyList<TElement> GetItems() =>
		_elements;

	public IEnumerator<TElement> GetEnumerator() =>
		_elements.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();
}