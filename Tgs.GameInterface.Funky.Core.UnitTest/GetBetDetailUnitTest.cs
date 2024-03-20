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

public class GetBetDetailUnitTest
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
		var gameCode = "10211";
		var refNo = "Funky_tt_4xCaptainMoneyx5199x1359";
		var language = "ZH_CN";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSerialNumberGenerator = Substitute.For<ISerialNumberGenerator>();

		var xRequestId = Guid.NewGuid();
		_ = fakeSerialNumberGenerator.GenerateAsync()
			.Returns(xRequestId);

		var parameters = new JsonObject
		{
			["gameCode"] = gameCode,
			["playerId"] = playerId,
			["lang"] = language,
			["refNo"] = refNo
		};

		var response = new JsonObject
		{
			["errorCode"] = 0,
			["errorMessage"] = "No Error",
			["data"] = new JsonObject
			{
				["url"] = "https://funky-test.ttstations.com/orderid/?orderid=4xCaptainMoneyx5199x1359&gamecode=Fish_CaptainMoney&language=zh-cn",
			}
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiUrl, "Funky/Bet/GetBetDetail").ToString())
				.WithHeaders("Authentication", authentication)
				.WithHeaders("X-Request-ID", xRequestId.ToString())
				.WithJsonContent(parameters, _SerializerOptions)
				.Respond(HttpStatusCode.OK, JsonContent.Create(response));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameApiClient(
			fakeLog,
			fakeSerialNumberGenerator,
			fakeHttpClient);

		var actual = await sut.GetBetDetailAsync(
			 apiUrl: apiUrl,
			 userAgent: userAgent,
			 authentication: authentication,
			 playerId: playerId,
			 gameCode: gameCode,
			 refNo: refNo,
			 language: language);

		Assert.Equal("https://funky-test.ttstations.com/orderid/?orderid=4xCaptainMoneyx5199x1359&gamecode=Fish_CaptainMoney&language=zh-cn", actual);
	}
}
