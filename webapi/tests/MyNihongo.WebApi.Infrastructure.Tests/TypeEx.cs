using System.Reflection;

namespace MyNihongo.WebApi.Infrastructure.Tests;

internal static class TypeEx
{
	public static T GetStaticField<T>(this Type @this, string fieldName)
	{
		var field = @this.GetField("ConnectionStrings", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new NullReferenceException($"Cannot find the field `{fieldName}` in `{@this.FullName}`");
		return (T)field.GetValue(null)!;
	}

}