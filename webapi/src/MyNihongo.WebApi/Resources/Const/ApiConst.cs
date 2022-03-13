namespace MyNihongo.WebApi.Resources.Const;

public static class ApiConst
{
	public const string CorsPolicy = "QcqAxTUBJAeoEUyo";
	public const string AuthPolicy = "gyIGrWVZZGmClsKc";

	public static class Headers
	{
		public const string ClientInfo = "x-client-info";
	}

	public static class Cookies
	{
		public const string AccessToken = "x-access-jwt",
			RefreshToken = "x-refresh-jw";
	}

	public static class Claims
	{
		public const string TokenId = "apz",
			UserId = "slb";
	}
}