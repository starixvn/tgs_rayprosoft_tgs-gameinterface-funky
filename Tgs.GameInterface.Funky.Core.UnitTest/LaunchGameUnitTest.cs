using System.Net;

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.SerialNumberGenerator;

namespace Tgs.GameInterface.Funky.Core.UnitTest;

public class LaunchGameUnitTest
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		WriteIndented = false,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = { new JsonStringEnumConverter() }
	};

	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://trial-gp-api.funkytest.com");
		var userAgent = "109-PG";
		var authentication = "c69d57e5-93e7-488c-90a4-e20876b495bc";
		var playerId = "4ucmqplxb029";
		var currency = "CNY";
		var language = "en";
		var gameCode = "10211";
		var playerIp = "127.0.0.1";
		var redirectUrl = string.Empty;
		var sessionId = "36a6cdef-094f-eb11-8d88-70b5e82b6b8d";
		var userName = "4ucmqplxb029";
		var isTestAccount = true;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSerialNumberGenerator = Substitute.For<ISerialNumberGenerator>();

		var xRequestId = Guid.NewGuid();
		_ = fakeSerialNumberGenerator.GenerateAsync()
			.Returns(xRequestId);

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

		var response = new JsonObject
		{
			["errorCode"] = 0,
			["errorMessage"] = "No Error",
			["data"] = new JsonObject
			{
				["gameUrl"] = "https://funky-test.ttstations.com/Game/Fish_CaptainMoney/",
				["token"] = "b1e1d229-fb2c-4fcf-bad6-64d37c467bb1&platformId=4&language=en&gameCode=CaptainMoney&hideBackButton=0&reloadOnIdle=0&hideCurrency=0&hideRotateHint=0&hideHistory=0"
			}
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiUrl, "Funky/Game/LaunchGame").ToString())
				.WithHeaders("Authentication", authentication)
				.WithHeaders("X-Request-ID", xRequestId.ToString())
				.WithJsonContent(parameters, _SerializerOptions)
				.Respond(HttpStatusCode.OK, JsonContent.Create(response));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameApiClient(
			fakeLog,
			fakeSerialNumberGenerator,
			fakeHttpClient);

		var actual = await sut.LaunchGameAsync(
			 apiUrl: apiUrl,
			 userAgent: userAgent,
			 authentication: authentication,
			 playerId: playerId,
			 currency: currency,
			 language: language,
			 gameCode: gameCode,
			 playerIp: playerIp,
			 redirectUrl: redirectUrl,
			 sessionId: sessionId,
			 userName: userName,
			 isTestAccount: isTestAccount);

		Assert.Equal(response["data"]!["gameUrl"]!.ToString(), actual.GameUrl);
		Assert.Equal(response["data"]!["token"]!.ToString(), actual.Token);
	}
}
