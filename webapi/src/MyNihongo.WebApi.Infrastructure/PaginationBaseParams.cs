namespace MyNihongo.WebApi.Infrastructure;

internal abstract record PaginationBaseParams
{
	private const int PageSizeCeiling = 35;
	private readonly int _pageIndex, _pageSize = PageSizeCeiling;

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
			const int floor = 10;

			if (value is < floor or > PageSizeCeiling)
				value = PageSizeCeiling;

			_pageSize = value;
		}
	}
}