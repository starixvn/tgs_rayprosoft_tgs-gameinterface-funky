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

public class CheckTransactionUnitTest
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		WriteIndented = false,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = { new JsonStringEnumConverter() }
	};

	[Fact]
	public async Task This_order_exists()
	{
		var apiUrl = new Uri("http://trial-gp-api.funkytest.com");
		var userAgent = "109-PG";
		var authentication = "c69d57e5-93e7-488c-90a4-e20876b495bc";
		var playerId = "4ucmqplxb029";
		var txId = "018db105-3894-0000-0001-07e8d561b174";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSerialNumberGenerator = Substitute.For<ISerialNumberGenerator>();

		var xRequestId = Guid.NewGuid();
		_ = fakeSerialNumberGenerator.GenerateAsync()
			.Returns(xRequestId);

		var parameters = new JsonObject
		{
			["playerId"] = playerId,
			["txId"] = txId
		};

		var response = new JsonObject
		{
			["errorCode"] = 0,
			["errorMessage"] = "No Error",
			["data"] = new JsonObject
			{
				["txId"] = "018db105-5428-0000-0001-07e8d561b174",
				["playerId"] = "4ucmqplxb029",
				["balance"] = 1.0,
				["txType"] = "Debit",
				["txTime"] = "2024-03-18 07:14:55",
				["currency"] = "CNY",
				["TxSerialId"] = "260324",
				["status"] = "Completed",
			}
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiUrl, "Funky/Wallet/CheckTransaction").ToString())
				.WithHeaders("Authentication", authentication)
				.WithHeaders("X-Request-ID", xRequestId.ToString())
				.WithJsonContent(parameters, _SerializerOptions)
				.Respond(HttpStatusCode.OK, JsonContent.Create(response));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameApiClient(
			fakeLog,
			fakeSerialNumberGenerator,
			fakeHttpClient);

		var actual = await sut.CheckTransactionAsync(
			 apiUrl: apiUrl,
			 userAgent: userAgent,
			 authentication: authentication,
			 playerId: playerId,
			 txId: txId);

		Assert.True(actual);
	}

	[Fact]
	public async Task This_order_does_not_exist()
	{
		var apiUrl = new Uri("http://trial-gp-api.funkytest.com");
		var userAgent = "109-PG";
		var authentication = "c69d57e5-93e7-488c-90a4-e20876b495bc";
		var playerId = "4ucmqplxb029";
		var txId = "018db105-5428-0000-0001-07e8d561b184";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSerialNumberGenerator = Substitute.For<ISerialNumberGenerator>();

		var xRequestId = Guid.NewGuid();
		_ = fakeSerialNumberGenerator.GenerateAsync()
			.Returns(xRequestId);

		var parameters = new JsonObject
		{
			["playerId"] = playerId,
			["txId"] = txId
		};

		var response = new JsonObject
		{
			["errorCode"] = 511,
			["errorMessage"] = "TransactionNotFound"
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiUrl, "Funky/Wallet/CheckTransaction").ToString())
				.WithHeaders("Authentication", authentication)
				.WithHeaders("X-Request-ID", xRequestId.ToString())
				.WithJsonContent(parameters, _SerializerOptions)
				.Respond(HttpStatusCode.OK, JsonContent.Create(response));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameApiClient(
			fakeLog,
			fakeSerialNumberGenerator,
			fakeHttpClient);

		var actual = await sut.CheckTransactionAsync(
			 apiUrl: apiUrl,
			 userAgent: userAgent,
			 authentication: authentication,
			 playerId: playerId,
			 txId: txId);

		Assert.False(actual);
	}
}
