using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.Funky.Core.Exceptions;
using Tgs.GameInterface.Funky.Core.Extensions;
using Tgs.GameInterface.Funky.Core.Models;
using Tgs.SerialNumberGenerator;
using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core;

internal class GameApiClient(
	ILogger<GameApiClient> logger,
	ISerialNumberGenerator serialNumberGenerator,
	HttpClient httpClient) : IGameApiClient
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		WriteIndented = false,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = { new JsonStringEnumConverter() }
	};

	public async ValueTask<bool> CheckTransactionAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string txId, CancellationToken cancellationToken = default)
	{
		apiUrl = new Uri(apiUrl, "Funky/Wallet/CheckTransaction");

		var parameters = new JsonObject
		{
			["playerId"] = playerId,
			["txId"] = txId
		};
		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			userAgent: userAgent,
			authentication: authentication,
			parameters: parameters,
			logPrefix: "CheckTransaction",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["errorCode"]!.GetValue<int>() == 0
			|| (jsonNode["errorCode"]!.GetValue<int>() != 511
				? throw new ErrorCodeException(jsonNode["errorCode"]!.GetValue<int>())
				: false);
	}

	public async ValueTask<DepositResult> DepositAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string currency, bool isTestAccount, string txId, decimal amount, CancellationToken cancellationToken = default)
	{
		apiUrl = new Uri(apiUrl, "Funky/Wallet/Deposit");

		var parameters = new JsonObject
		{
			["playerId"] = playerId,
			["currency"] = currency,
			["isTestAccount"] = isTestAccount,
			["txId"] = txId,
			["amount"] = amount
		};
		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			userAgent: userAgent,
			authentication: authentication,
			parameters: parameters,
			logPrefix: "Deposit",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["errorCode"]!.GetValue<int>() != 0
			? throw new ErrorCodeException(jsonNode["errorCode"]!.GetValue<int>())
			: new DepositResult(
			jsonNode["data"]!["TxSerialId"]!.GetValue<long>(),
			jsonNode["data"]!["txId"]!.GetValue<string>(),
			jsonNode["data"]!["balance"]!.GetValue<decimal>(),
			jsonNode["data"]!.ToJsonString());
	}

	public async ValueTask<decimal> GetBalanceByCurrencyAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string currency, CancellationToken cancellationToken = default)
	{
		apiUrl = new Uri(apiUrl, "Funky/Wallet/GetBalanceByCurrency");

		var parameters = new JsonObject
		{
			["playerId"] = playerId,
			["currency"] = currency
		};
		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			userAgent: userAgent,
			authentication: authentication,
			parameters: parameters,
			logPrefix: "GetBalanceByCurrency",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["errorCode"]!.GetValue<int>() != 0
			? throw new ErrorCodeException(jsonNode["errorCode"]!.GetValue<int>())
			: jsonNode["data"]!["balance"]!.GetValue<decimal>();
	}

	public async ValueTask<LaunchGameResult> LaunchGameAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string currency, string language, string gameCode, string playerIp, string redirectUrl, string sessionId, string userName, bool isTestAccount, CancellationToken cancellationToken = default)
	{
		apiUrl = new Uri(apiUrl, "Funky/Game/LaunchGame");

		var parameters = new JsonObject
		{
			["playerId"] = playerId,
			["currency"] = currency,
			["language"] = language,
			["gameCode"] = gameCode,
			["playerIp"] = playerIp,
			["redirectUrl"] = redirectUrl,
			["sessionId"] = sessionId,
			["userName"] = userName,
			["isTestAccount"] = isTestAccount,
		};
		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			userAgent: userAgent,
			authentication: authentication,
			parameters: parameters,
			logPrefix: "LaunchGame",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["errorCode"]!.GetValue<int>() != 0
			? throw new ErrorCodeException(jsonNode["errorCode"]!.GetValue<int>())
			: jsonNode["data"].Deserialize<LaunchGameResult>(_SerializerOptions)!;
	}

	public async ValueTask<WithdrawResult> WithdrawAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string currency, bool isTestAccount, string txId, decimal amount, CancellationToken cancellationToken = default)
	{
		apiUrl = new Uri(apiUrl, "Funky/Wallet/Withdraw");

		var parameters = new JsonObject
		{
			["playerId"] = playerId,
			["currency"] = currency,
			["isTestAccount"] = isTestAccount,
			["txId"] = txId,
			["amount"] = amount
		};
		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			userAgent: userAgent,
			authentication: authentication,
			parameters: parameters,
			logPrefix: "Withdraw",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["errorCode"]!.GetValue<int>() != 0
			? throw new ErrorCodeException(jsonNode["errorCode"]!.GetValue<int>())
			: new WithdrawResult(
			jsonNode["data"]!["TxSerialId"]!.GetValue<long>(),
			jsonNode["data"]!["txId"]!.GetValue<string>(),
			jsonNode["data"]!["balance"]!.GetValue<decimal>(),
			jsonNode["data"]!.ToJsonString());
	}

	public async ValueTask<string> GetBetDetailAsync(Uri apiUrl, string userAgent, string authentication, string playerId, string gameCode, string refNo, string language, CancellationToken cancellationToken = default)
	{
		apiUrl = new Uri(apiUrl, "Funky/Bet/GetBetDetail");

		var parameters = new JsonObject
		{
			["gameCode"] = gameCode,
			["playerId"] = playerId,
			["lang"] = language,
			["refNo"] = refNo
		};
		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			userAgent: userAgent,
			authentication: authentication,
			parameters: parameters,
			logPrefix: "GetBetDetail",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return jsonNode["errorCode"]!.GetValue<int>() != 0
			? throw new ErrorCodeException(jsonNode["errorCode"]!.GetValue<int>())
			: jsonNode["data"]!["url"]!.ToString();
	}

	private async ValueTask<JsonNode> PostAsync(Uri apiUrl, string userAgent, string authentication, JsonNode parameters, string logPrefix, CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient).ToLower()}.{logPrefix}";

		var requestId = await serialNumberGenerator.GenerateAsync(
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		httpClient.DefaultRequestHeaders.Clear();
		httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

		var requestContent = JsonContent.Create(parameters, options: _SerializerOptions);
		requestContent.Headers.Add("Authentication", authentication);
		requestContent.Headers.Add("X-Request-ID", requestId.ToString());

		var sw = Stopwatch.StartNew();

		var httpResponseMessage = await httpClient.PostAsync(
			requestUri: apiUrl,
			content: requestContent,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);

		using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
		{
			responseMessage = await sr.ReadToEndAsync(
				cancellationToken: cancellationToken)
				.ConfigureAwait(false) ?? string.Empty;
		}

		var apiLogEvent = new
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = (await requestContent.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage.NoLongerThan(4096),
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

		_ = httpResponseMessage.EnsureSuccessStatusCode();

		return JsonNode.Parse(responseMessage)!;
	}
}
