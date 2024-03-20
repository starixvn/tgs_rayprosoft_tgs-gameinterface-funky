using Tgs.GameInterface.Funky.Protos;

namespace Tgs.GameInterface.Funky.Client;

public interface IGameInterfaceFunkyClient
{
	ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundInAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string currency, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundOutAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string currency, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<bool> FundStatusAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string orderId, CancellationToken cancellationToken = default);

	ValueTask<string> GameUrlAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string currency, string language, string gameCode, string playerIp, string redirectUrl, string sessionId, string userNames, CancellationToken cancellationToken = default);

	ValueTask<string> GameWagerDetailAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string gameCode, string externalWagerId, string language, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameWagerInfo> GameWagerAsync(Uri apiUrl, string userAgent, string authentication, DateTimeOffset startTime, DateTimeOffset endTime, int page, CancellationToken cancellationToken = default);

	ValueTask<decimal> PlayerBalanceAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string currency, CancellationToken cancellationToken = default);
}
