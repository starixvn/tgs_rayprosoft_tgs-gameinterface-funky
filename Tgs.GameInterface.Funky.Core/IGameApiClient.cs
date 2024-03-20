using Tgs.GameInterface.Funky.Core.Models;

namespace Tgs.GameInterface.Funky.Core;

public interface IGameApiClient
{
	public ValueTask<decimal> GetBalanceByCurrencyAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string currency, CancellationToken cancellationToken = default);

	public ValueTask<DepositResult> DepositAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string currency, bool isTestAccount, string txId, decimal amount, CancellationToken cancellationToken = default);

	public ValueTask<WithdrawResult> WithdrawAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string currency, bool isTestAccount, string txId, decimal amount, CancellationToken cancellationToken = default);

	public ValueTask<bool> CheckTransactionAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string txId, CancellationToken cancellationToken = default);

	public ValueTask<LaunchGameResult> LaunchGameAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string currency, string language, string gameCode, string playerIp, string redirectUrl, string sessionId, string userName, bool isTestAccount, CancellationToken cancellationToken = default);

	public ValueTask<string> GetBetDetailAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string gameCode, string refNo, string language, CancellationToken cancellationToken = default);
}
