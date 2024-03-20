using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using NSubstitute;
using Tgs.SerialNumberGenerator;

namespace Tgs.GameInterface.Funky.Core.UnitTest;

public class DepositUnitTest
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
		var isTestAccount = true;
		var txId = "018db105-3894-0000-0001-07e8d561b174";
		var amount = 100.0m;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSerialNumberGenerator = Substitute.For<ISerialNumberGenerator>();

		var xRequestId = Guid.NewGuid();
		_ = fakeSerialNumberGenerator.GenerateAsync()
			.Returns(xRequestId);

		var parameters = new JsonObject
		{
			["playerId"] = playerId,
			["currency"] = currency,
			["isTestAccount"] = isTestAccount,
			["txId"] = txId,
			["amount"] = amount
		};

		var response = new JsonObject
		{
			["errorCode"] = 0,
			["errorMessage"] = "No Error",
			["data"] = new JsonObject
			{
				["txId"] = "018db105-3894-0000-0001-07e8d561b174",
				["balance"] = 199.0m,
				["playerId"] = "4ucmqplxb029",
				["currency"] = "CNY",
				["TxSerialId"] = 260336,
			}
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiUrl, "Funky/Wallet/Deposit").ToString())
				.WithHeaders("Authentication", authentication)
				.WithHeaders("X-Request-ID", xRequestId.ToString())
				.WithJsonContent(parameters, _SerializerOptions)
				.Respond(HttpStatusCode.OK, JsonContent.Create(response));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameApiClient(
			fakeLog,
			fakeSerialNumberGenerator,
			fakeHttpClient);

		var actual = await sut.DepositAsync(
			 apiUrl: apiUrl,
			 userAgent: userAgent,
			 authentication: authentication,
			 playerId: playerId,
			 currency: currency,
			 isTestAccount: isTestAccount,
			 txId: txId,
			 amount: amount);

		Assert.Equal(260336, actual.TxSerialId);
		Assert.Equal("018db105-3894-0000-0001-07e8d561b174", actual.TxId);
		Assert.Equal(199.0m, actual.Balance);
		Assert.Equal(response["data"]!.ToJsonString(), actual.Res);
	}
}
