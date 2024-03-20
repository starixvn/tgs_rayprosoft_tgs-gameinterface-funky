using System.Diagnostics;

namespace Tgs.GameInterface.Funky;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.Funky.DiagnosticsInterceptor");
}
