
using Tgs.ServiceUtility;

using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core.ApplicationServices;

internal class PlayerBalanceGetService(IGameApiClient client) : IGetService<PlayerBalanceQuery, decimal>
{
	public ValueTask<decimal> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
		=> client.GetBalanceByCurrencyAsync(
			apiUrl: query.ApiUrl,
			userAgent: query.UserAgent,
			authentication: query.Authentication,
			playerId: query.PlayerCode,
			currency: query.Currency,
			cancellationToken: cancellationToken);
}
