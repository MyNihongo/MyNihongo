namespace MyNihongo.WebApi.Infrastructure;

internal abstract record PaginationBaseParams
{
	private readonly int _pageIndex,
		_pageSize;

	[Param("pageIndex")]
	public int PageIndex
	{
		get => _pageIndex;
		init
		{
			const int floor = 0;

			if (value < floor)
				value = floor;

			_pageIndex = value;
		}
	}

	[Param("pageSize")]
	public int PageSize
	{
		get => _pageSize;
		init
		{
			const int floor = 10, ceiling = 35;

			if (value is < floor or > ceiling)
				value = ceiling;

			_pageSize = value;
		}
	}
}