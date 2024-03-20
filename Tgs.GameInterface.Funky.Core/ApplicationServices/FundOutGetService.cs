
using Tgs.GameInterface.Funky.Core.Models;

using Tgs.ServiceUtility;

using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core.ApplicationServices;

internal class FundOutGetService(
	IGameApiClient client) : IGetService<FundOutQuery, DepositResult>
{
	public ValueTask<DepositResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
		=> client.DepositAsync(
			apiUrl: query.ApiUrl,
			userAgent: query.UserAgent,
			authentication: query.Authentication,
			playerId: query.PlayerCode,
			currency: query.Currency,
			isTestAccount: false,
			txId: query.OrderId,
			amount: query.Amount,
			cancellationToken: cancellationToken);
}
