using System.Diagnostics;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.Funky.Core.Exceptions;
using Tgs.GameInterface.Funky.Core.Extensions;
using Tgs.GameInterface.Funky.Core.Models;
using Tgs.SerialNumberGenerator;

namespace Tgs.GameInterface.Funky.Core;

internal class GameReportApiClient(
	ILogger<GameReportApiClient> logger,
	ISerialNumberGenerator serialNumberGenerator,
	HttpClient httpClient) : IGameReportApiClient
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		WriteIndented = false,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = { new JsonStringEnumConverter() }
	};

	public async IAsyncEnumerable<BetResult> GetBetListAsync(Uri apiUrl, string userAgent, string authentication, DateTimeOffset startTime, DateTimeOffset endTime, int page, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		apiUrl = new Uri(apiUrl, "Funky/Report/GetBetList");

		var parameters = new JsonObject
		{
			["startTime"] = startTime.DatetimeConvertToString(),
			["endTime"] = endTime.DatetimeConvertToString(),
			["page"] = page
		};
		var jsonNode = await PostAsync(
			apiUrl: apiUrl,
			userAgent: userAgent,
			authentication: authentication,
			parameters: parameters,
			logPrefix: "GetBetList",
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (jsonNode["errorCode"]!.GetValue<int>() != 0)
		{
			throw new ErrorCodeException(jsonNode["errorCode"]!.GetValue<int>());
		}

		var datas = jsonNode["data"]?.AsArray();

		if (datas is null)
		{
			yield break;
		}

		foreach (var data in datas)
		{
			if (data is null)
			{
				continue;
			}

			yield return new BetResult(
				data["playerId"]!.GetValue<string>(),
				data["statementDate"]!.GetValue<string>().StringConvertToDatetime(),
				data["betTime"]!.GetValue<string>().StringConvertToDatetime(),
				data["refNo"]!.GetValue<string>(),
				data["status"]!.GetValue<string>(),
				data["gameCode"]!.GetValue<string>(),
				data["gameName"]!.GetValue<string>(),
				data["currency"]!.GetValue<string>(),
				data["betAmount"]!.GetValue<decimal>(),
				data["effectiveStake"]!.GetValue<decimal>(),
				data["winloss"]!.GetValue<decimal>(),
				data["isTransferGame"]!.GetValue<bool>(),
				data.ToJsonString());
		}
	}

	public async ValueTask<JsonNode> PostAsync(Uri apiUrl, string userAgent, string authentication, JsonNode parameters, string logPrefix, CancellationToken cancellationToken = default)
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
