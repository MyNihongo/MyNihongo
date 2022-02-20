// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;

internal static class AsyncEnumerable
{
	public static IAsyncEnumerable<T> Empty<T>() =>
		new Enumerable<T>(new EmptyEnumerator<T>());

	private sealed class Enumerable<T> : IAsyncEnumerable<T>
	{
		private readonly IAsyncEnumerator<T> _enumerator;

		public Enumerable(IAsyncEnumerator<T> enumerator)
		{
			_enumerator = enumerator;
		}

		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
			_enumerator;
	}

	private class EmptyEnumerator<T> : IAsyncEnumerator<T>
	{
		public ValueTask DisposeAsync() =>
			default;

		public ValueTask<bool> MoveNextAsync() =>
			new(false);

		public T Current => default!;
	}
}