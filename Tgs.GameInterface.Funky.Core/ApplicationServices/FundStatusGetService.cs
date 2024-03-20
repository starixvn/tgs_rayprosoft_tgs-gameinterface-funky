using Tgs.ServiceUtility;
using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core.ApplicationServices;

internal class FundStatusGetService(IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
	public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
		=> client.CheckTransactionAsync(
			apiUrl: query.ApiUrl,
			userAgent: query.UserAgent,
			authentication: query.Authentication,
			playerId: query.PlayerCode,
			txId: query.OrderId,
			cancellationToken: cancellationToken);
}
