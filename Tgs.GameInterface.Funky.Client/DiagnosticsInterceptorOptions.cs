using System.Diagnostics;

namespace Tgs.GameInterface.Funky.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.Funky.Client.DiagnosticsInterceptor");
}
