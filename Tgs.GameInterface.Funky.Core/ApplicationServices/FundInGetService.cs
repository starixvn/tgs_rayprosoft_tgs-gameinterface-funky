
using Tgs.GameInterface.Funky.Core.Models;

using Tgs.ServiceUtility;

using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core.ApplicationServices;

internal class FundInGetService(
	IGameApiClient client) : IGetService<FundInQuery, WithdrawResult>
{
	public ValueTask<WithdrawResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
		=> client.WithdrawAsync(
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
