using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace MyNihongo.Tests.Integration.Utils;

internal static class SqlServerUtils
{
	public static string GetLocalInstance()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			return GetLocalInstanceOnWindows();

		throw new PlatformNotSupportedException();
	}

	[SupportedOSPlatform("windows")]
	private static string GetLocalInstanceOnWindows()
	{
		var registryView = Environment.Is64BitOperatingSystem
			? RegistryView.Registry64
			: RegistryView.Registry32;

		using var registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
		using var instanceKey = registryKey.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);

		if (instanceKey == null)
			throw new InvalidOperationException("Cannot locate an SQL server instance");

		var machineName = Environment.MachineName;
		var instanceName = instanceKey.GetValueNames()[0];

		return instanceName == "MSSQLSERVER"
			? machineName
			: $"{machineName}\\{instanceName}";
	}
}