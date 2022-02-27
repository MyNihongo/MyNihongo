// ReSharper disable once CheckNamespace
namespace MyNihongo.WebApi.Infrastructure;

public sealed partial class DoubleOptional
{
	public static implicit operator DoubleOptional(double value) =>
		new()
		{
			HasValue = true,
			Value = value
		};

	public static implicit operator double?(DoubleOptional? @this) =>
		@this?.HasValue == true
			? @this.Value
			: null;
}