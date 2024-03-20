using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.Funky.Protos;

namespace Tgs.GameInterface.Funky.Client;

internal class GameInterfaceFunkyClient(
	GameInterfaceFunky.GameInterfaceFunkyClient client) : IGameInterfaceFunkyClient
{
	public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
	{
		var request = new CheckingServiceRequest
		{
			MachineName = machineName,
		};

		var reply = await client.CheckingServiceAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.MachineName;
	}

	public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string currency, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			UserAgent = userAgent,
			Authentication = authentication,
			PlayerCode = playerCode,
			Currency = currency,
			OrderId = orderId,
			Amount = amount,
		};

		return await client.FundInAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string currency, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			UserAgent = userAgent,
			Authentication = authentication,
			PlayerCode = playerCode,
			Currency = currency,
			OrderId = orderId,
			Amount = amount,
		};

		return await client.FundOutAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<bool> FundStatusAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string orderId, CancellationToken cancellationToken = default)
	{
		var request = new FundStatusRequest
		{
			ApiUrl = apiUrl.ToString(),
			UserAgent = userAgent,
			Authentication = authentication,
			PlayerCode = playerCode,
			OrderId = orderId
		};

		var reply = await client.FundStatusAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.State;
	}

	public async ValueTask<string> GameUrlAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string currency, string language, string gameCode, string playerIp, string redirectUrl, string sessionId, string userNames, CancellationToken cancellationToken = default)
	{
		var request = new GameUrlRequest
		{
			ApiUrl = apiUrl.ToString(),
			UserAgent = userAgent,
			Authentication = authentication,
			PlayerCode = playerCode,
			Currency = currency,
			Language = language,
			GameCode = gameCode,
			PlayerIp = playerIp,
			RedirectUrl = redirectUrl,
			SessionId = sessionId,
			UserNames = userNames
		};

		var reply = await client.GameUrlAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.GameUrl;
	}

	public async IAsyncEnumerable<GameWagerInfo> GameWagerAsync(Uri apiUrl, string userAgent, string authentication, DateTime startTime, DateTime endTime, int page, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var request = new GameWagerRequest
		{
			ApiUrl = apiUrl.ToString(),
			UserAgent = userAgent,
			Authentication = authentication,
			StartTime = startTime.ToTimestamp(),
			EndTime = endTime.ToTimestamp(),
			Page = page
		};

		var reply = await client.GameWagerAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var items = reply.Items ?? Enumerable.Empty<GameWagerInfo>();

		if (!items.Any())
		{
			yield break;
		}

		foreach (var item in items)
		{
			yield return item;
		}
	}

	public async ValueTask<string> GameWagerDetailAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string gameCode, string externalWagerId, string language, CancellationToken cancellationToken = default)
	{
		var request = new GameWagerDetailRequest
		{
			ApiUrl = apiUrl.ToString(),
			UserAgent = userAgent,
			Authentication = authentication,
			PlayerCode = playerCode,
			GameCode = gameCode,
			ExternalWagerId = externalWagerId,
			Language = language
		};

		var reply = await client.GameWagerDetailAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.GameWagerDetailUrl;
	}

	public async ValueTask<decimal> PlayerBalanceAsync(Uri apiUrl, string userAgent, string authentication, string playerCode, string currency, CancellationToken cancellationToken = default)
	{
		var request = new PlayerBalanceRequest
		{
			ApiUrl = apiUrl.ToString(),
			UserAgent = userAgent,
			Authentication = authentication,
			PlayerCode = playerCode,
			Currency = currency
		};

		var reply = await client.PlayerBalanceAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.Balance;
	}
}
