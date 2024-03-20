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
using Tgs.GameInterface.Funky.Core.Models;
using NSubstitute;
using Tgs.SerialNumberGenerator;

namespace Tgs.GameInterface.Funky.Core.UnitTest;
public class GetBetListUnitTest
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
		var startTime = new DateTimeOffset(2024, 3, 18, 17, 30, 00, TimeSpan.FromHours(+8));
		var endTime = new DateTimeOffset(2024, 3, 18, 18, 30, 00, TimeSpan.FromHours(+8));
		var page = 1;

		var fakeLog = NullLogger<GameReportApiClient>.Instance;
		var fakeSerialNumberGenerator = Substitute.For<ISerialNumberGenerator>();

		var xRequestId = Guid.NewGuid();
		_ = fakeSerialNumberGenerator.GenerateAsync()
			.Returns(xRequestId);

		var parameters = new JsonObject
		{
			["startTime"] = "2024-03-18 09:30:00",
			["endTime"] = "2024-03-18 10:30:00",
			["page"] = page
		};

		var response = new JsonObject
		{
			["errorCode"] = 0,
			["errorMessage"] = "No Error",
			["totalPage"] = 1,
			["currentPage"] = 1,
			["data"] = new JsonArray
			{
				new JsonObject{
					["playerId"]="4ucmqplxb029",
					["statementDate"]= "2024-03-18",
					["betTime"]= "2024-03-18 09:44:00",
					["refNo"]= "Funky_tt_4xCaptainMoneyx5199x1359",
					["status"]= "L",
					["gameCode"]= "10211",
					["gameName"]= "Captain Money",
					["currency"]="CNY",
					["betAmount"]= 107.1,
					["effectiveStake"]= 77.5,
					["winloss"]= -14.8,
					["isTransferGame"]= true
				}
			}
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiUrl, "Funky/Report/GetBetList").ToString())
				.WithHeaders("Authentication", authentication)
				.WithHeaders("X-Request-ID", xRequestId.ToString())
				.WithJsonContent(parameters, _SerializerOptions)
				.Respond(HttpStatusCode.OK, JsonContent.Create(response));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameReportApiClient(
			fakeLog,
			fakeSerialNumberGenerator,
			fakeHttpClient);

		var actual = await sut.GetBetListAsync(
			 apiUrl: apiUrl,
			 userAgent: userAgent,
			 authentication: authentication,
			 startTime: startTime,
			 endTime: endTime,
			 page: page)
			.ToArrayAsync()
			?? Enumerable.Empty<BetResult>();

		Assert.NotEmpty(actual);
		Assert.Equal("4ucmqplxb029", actual.First().PlayerId);
		Assert.Equal(new DateTimeOffset(2024, 03, 18, 0, 0, 0, TimeSpan.Zero), actual.First().StatementDate);
		Assert.Equal(new DateTimeOffset(2024, 03, 18, 09, 44, 0, TimeSpan.Zero), actual.First().BetTime);
		Assert.Equal("Funky_tt_4xCaptainMoneyx5199x1359", actual.First().RefNo);
		Assert.Equal("L", actual.First().Status);
		Assert.Equal("10211", actual.First().GameCode);
		Assert.Equal("Captain Money", actual.First().GameName);
		Assert.Equal("CNY", actual.First().Currency);
		Assert.Equal(107.1m, actual.First().BetAmount);
		Assert.Equal(77.5m, actual.First().EffectiveStake);
		Assert.Equal(-14.8m, actual.First().Winloss);
		Assert.True(actual.First().IsTransferGame);
	}

	[Fact]
	public async Task This_has_no_data()
	{
		var apiUrl = new Uri("http://trial-gp-api.funkytest.com");
		var userAgent = "109-PG";
		var authentication = "c69d57e5-93e7-488c-90a4-e20876b495bc";
		var startTime = new DateTimeOffset(2024, 3, 18, 17, 30, 00, TimeSpan.FromHours(+8));
		var endTime = new DateTimeOffset(2024, 3, 18, 18, 30, 00, TimeSpan.FromHours(+8));
		var page = 1;

		var fakeLog = NullLogger<GameReportApiClient>.Instance;
		var fakeSerialNumberGenerator = Substitute.For<ISerialNumberGenerator>();

		var xRequestId = Guid.NewGuid();
		_ = fakeSerialNumberGenerator.GenerateAsync()
			.Returns(xRequestId);

		var parameters = new JsonObject
		{
			["startTime"] = "2024-03-18 09:30:00",
			["endTime"] = "2024-03-18 10:30:00",
			["page"] = page
		};

		var response = new JsonObject
		{
			["errorCode"] = 0,
			["errorMessage"] = "No Error",
			["totalPage"] = 1,
			["currentPage"] = 1,
			["data"] = new JsonArray { }
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, new Uri(apiUrl, "Funky/Report/GetBetList").ToString())
				.WithHeaders("Authentication", authentication)
				.WithHeaders("X-Request-ID", xRequestId.ToString())
				.WithJsonContent(parameters, _SerializerOptions)
				.Respond(HttpStatusCode.OK, JsonContent.Create(response));

		var fakeHttpClient = handler.ToHttpClient();

		var sut = new GameReportApiClient(
			fakeLog,
			fakeSerialNumberGenerator,
			fakeHttpClient);

		var actual = await sut.GetBetListAsync(
			 apiUrl: apiUrl,
			 userAgent: userAgent,
			 authentication: authentication,
			 startTime: startTime,
			 endTime: endTime,
			 page: page)
			.ToArrayAsync()
			?? Enumerable.Empty<BetResult>();

		Assert.Empty(actual);
	}
}
