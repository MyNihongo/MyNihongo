// ReSharper disable once CheckNamespace
namespace MyNihongo.WebApi.Infrastructure;

public sealed partial class StringOptional
{
	public static implicit operator StringOptional(string? value) =>
		value != null
			? new StringOptional { HasValue = true, Value = value }
			: new StringOptional { HasValue = false };

	public static implicit operator string?(StringOptional? @this) =>
		@this?.HasValue == true
			? @this.Value
			: null;
}