using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace MyNihongo.WebApi.Infrastructure;

internal static class TextUtils
{
	public static readonly ObjectPool<StringBuilder> StringBuilderPool = new DefaultObjectPoolProvider()
		.CreateStringBuilderPool();
}