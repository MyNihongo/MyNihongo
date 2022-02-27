// ReSharper disable once CheckNamespace
namespace MyNihongo.WebApi.Infrastructure;

public sealed partial class Int32Optional
{
	public static implicit operator Int32Optional(int value) =>
		new()
		{
			HasValue = true,
			Value = value
		};

	public static implicit operator int?(Int32Optional? @this) =>
		@this?.HasValue == true
			? @this.Value
			: null;

	public static implicit operator byte?(Int32Optional? @this) =>
		@this?.HasValue == true && @this.Value is >= byte.MinValue and <= byte.MaxValue
			? (byte)@this.Value
			: null;
}