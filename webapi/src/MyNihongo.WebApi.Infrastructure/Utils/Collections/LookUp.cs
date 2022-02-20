// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;

internal sealed class LookUp<TKey, TElement> : ILookup<TKey, TElement>
	where TKey : notnull
{
	private readonly Dictionary<TKey, Grouping<TKey, TElement>> _lookup = new();

	public int Count => _lookup.Count;

	public IEnumerable<TElement> this[TKey key] =>
		_lookup[key];

	public void Add(TKey key, TElement element)
	{
		if (_lookup.TryGetValue(key, out var grouping))
		{
			grouping.Add(element);
		}
		else
		{
			grouping = new Grouping<TKey, TElement>(key, element);
			_lookup.Add(key, grouping);
		}
	}

	public IReadOnlyList<TElement> GetItems(TKey key) =>
		_lookup[key].GetItems();

	public bool Contains(TKey key) =>
		_lookup.ContainsKey(key);

	public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator() =>
		_lookup.Values.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();
}