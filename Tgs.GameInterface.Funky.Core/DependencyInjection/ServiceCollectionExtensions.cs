using System.Reflection;
using Tgs.GameInterface.Funky.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameInterfaceCore(
		this IServiceCollection services)
		=> services
		.AddHttpClient<IGameApiClient, GameApiClient>()
		.Services
		.AddHttpClient<IGameReportApiClient, GameReportApiClient>()
		.Services
		.AddSingleton(TimeProvider.System)
		.AddSerialNumberGenerator()
		.RegisterAllServiceUtilityTypes(
			Assembly.GetExecutingAssembly(),
			ServiceLifetime.Transient);
}
