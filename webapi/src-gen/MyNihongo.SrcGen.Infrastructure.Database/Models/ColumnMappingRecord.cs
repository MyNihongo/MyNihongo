using System.Data;

namespace MyNihongo.SrcGen.Infrastructure.Database.Models;

internal sealed record ColumnMappingRecord(string PropertyName, string ColumnProperty, string ColumnName, ColumnMappingRecord.ColType ColumnType, string? Collation)
{
	public string PropertyName { get; } = PropertyName;

	public string ColumnProperty { get; } = ColumnProperty;

	public string ColumnName { get; } = ColumnName;

	public ColType ColumnType { get; } = ColumnType;

	public string? Collation { get; } = Collation;

	public sealed record ColType(SqlDbType DbType, int? Size)
	{
		public SqlDbType DbType { get; } = DbType;

		public int? Size { get; } = Size;

		public override string ToString()
		{
			var stringBuilder = new StringBuilder(DbType.ToString().ToUpper());

			if (Size.HasValue)
			{
				stringBuilder
					.AppendFormat("({0})", Size.Value);
			}

			return stringBuilder.ToString();
		}
	}
}