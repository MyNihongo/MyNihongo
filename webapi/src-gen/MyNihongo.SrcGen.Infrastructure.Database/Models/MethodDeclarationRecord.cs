using MyNihongo.SrcGen.Infrastructure.Database.Enums;

namespace MyNihongo.SrcGen.Infrastructure.Database.Models;

internal sealed record MethodDeclarationRecord(INamedTypeSymbol ParamsType)
{
	public INamedTypeSymbol ParamsType { get; } = ParamsType;

	public string StoredProcedureName { get; set; } = string.Empty;

	public ExecType ExecType { get; set; }

	public string DeclarationString { get; set; } = string.Empty;

	public bool IsNullableReturnType { get; set; }

	public IReadOnlyList<Parameter> Parameters { get; set; } = Array.Empty<Parameter>();
	
	public IReadOnlyList<Parameter> TempTables { get; set; } = Array.Empty<Parameter>();

	public IReadOnlyList<Result> Results { get; set; } = Array.Empty<Result>();

	public sealed record Parameter(IPropertySymbol Property, string Name)
	{
		public IPropertySymbol Property { get; } = Property;

		public string Name { get; } = Name;
	}

	public sealed record Result(ResultType Type, string PropertyName, IPropertySymbol? KeyProperty)
	{
		public ResultType Type { get; } = Type;

		public string PropertyName { get; } = PropertyName;
	
		public IPropertySymbol? KeyProperty { get; } = KeyProperty;
	}

	public sealed record ResultType(INamedTypeSymbol Value, IReadOnlyDictionary<string, string> Keys)
	{
		public INamedTypeSymbol Value { get; } = Value;

		public IReadOnlyDictionary<string, string> Keys { get; } = Keys;
	}
}