using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.Funky.Core.ApplicationServices;
using Tgs.GameInterface.Funky.Core.Models;

namespace Tgs.GameInterface.Funky.Core.UnitTest.ApplicationServices;

public class GameWagerQueryServiceUnitTest
{
	[Fact]
	public async Task Loss()
	{
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeGameReportApiClient = Substitute.For<IGameReportApiClient>();

		var query = new GameWagerQuery(
			ApiUrl: new Uri("http://trial-gp-api.funkytest.com"),
			UserAgent: "109-PG",
			Authentication: "c69d57e5-93e7-488c-90a4-e20876b495bc",
			StartTime: new DateTimeOffset(2024, 3, 18, 17, 30, 00, TimeSpan.FromHours(+8)),
			EndTime: new DateTimeOffset(2024, 3, 18, 18, 30, 00, TimeSpan.FromHours(+8)),
			Page: 1);

		var currentTime = new DateTimeOffset(2024, 03, 18, 17, 47, 20, TimeSpan.FromHours(8));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var result = new BetResult(
			PlayerId: "4ucmqplxb029",
			StatementDate: new DateTimeOffset(2024, 3, 18, 00, 00, 00, TimeSpan.Zero),
			BetTime: new DateTimeOffset(2024, 03, 18, 09, 44, 00, TimeSpan.Zero),
			RefNo: "Funky_tt_4xCaptainMoneyx5199x1359",
			Status: "L",
			GameCode: "10211",
			GameName: "Captain Money",
			Currency: "CNY",
			BetAmount: 107.1m,
			EffectiveStake: 77.5m,
			Winloss: -14.8m,
			IsTransferGame: true,
			AdditionalData: new JsonObject
			{
				["playerId"] = "4ucmqplxb029",
				["statementDate"] = "2024-03-18",
				["betTime"] = "2024-03-18 09:44:00",
				["refNo"] = "Funky_tt_4xCaptainMoneyx5199x1359",
				["status"] = "L",
				["gameCode"] = "10211",
				["gameName"] = "Captain Money",
				["currency"] = "CNY",
				["betAmount"] = 107.1,
				["effectiveStake"] = 77.5,
				["winloss"] = -14.8,
				["isTransferGame"] = true
			}.ToJsonString());
		_ = fakeGameReportApiClient.GetBetListAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			userAgent: Arg.Is(query.UserAgent),
			authentication: Arg.Is(query.Authentication),
			startTime: Arg.Is(query.StartTime),
			endTime: Arg.Is(query.EndTime),
			page: Arg.Is(query.Page),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { result }.ToAsyncEnumerable());

		var sut = new GameWagerQueryService(
			fakeTimeProvider,
			fakeGameReportApiClient);

		var actual = await sut.QueryAsync(
			 query: query)
			.ToArrayAsync()
			?? Enumerable.Empty<GameWagerResult>();

		Assert.NotEmpty(actual);
		Assert.Equal("4ucmqplxb029", actual.First().PlayerCode);
		Assert.Equal(result.RefNo, actual.First().ExternalWagerId);
		Assert.Equal(result.GameCode, actual.First().ExternalGameId);
		Assert.Equal(GameWagerStatus.Loss, actual.First().Status);
		Assert.Equal(result.BetTime, actual.First().ExternalCreatedOn);
		Assert.Equal(result.BetTime, actual.First().ExternalClosedOn);
		Assert.Equal(107.1m, actual.First().Bet);
		Assert.Equal(77.5m, actual.First().ValidBet);
		Assert.Equal(62.7m, actual.First().Settle);
		Assert.Equal(-14.8m, actual.First().WinLoss);
		Assert.Equal(result.AdditionalData, actual.First().AdditionalData);
	}

	[Fact]
	public async Task Won()
	{
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeGameReportApiClient = Substitute.For<IGameReportApiClient>();

		var query = new GameWagerQuery(
			ApiUrl: new Uri("http://trial-gp-api.funkytest.com"),
			UserAgent: "109-PG",
			Authentication: "c69d57e5-93e7-488c-90a4-e20876b495bc",
			StartTime: new DateTimeOffset(2024, 3, 19, 11, 00, 00, TimeSpan.FromHours(+8)),
			EndTime: new DateTimeOffset(2024, 3, 19, 12, 00, 00, TimeSpan.FromHours(+8)),
			Page: 1);

		var currentTime = new DateTimeOffset(2024, 03, 19, 11, 14, 2, TimeSpan.FromHours(8));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var result = new BetResult(
			PlayerId: "4ucmqplxb029",
			StatementDate: new DateTimeOffset(2024, 3, 19, 00, 00, 00, TimeSpan.Zero),
			BetTime: new DateTimeOffset(2024, 03, 19, 03, 14, 02, TimeSpan.Zero),
			RefNo: "Funky_tt_4xGoldenDynastyx5199x82477",
			Status: "W",
			GameCode: "10261",
			GameName: "Golden Dynasty",
			Currency: "CNY",
			BetAmount: 0.76m,
			EffectiveStake: 0.76m,
			Winloss: 5.24m,
			IsTransferGame: true,
			AdditionalData: new JsonObject
			{
				["playerId"] = "4ucmqplxb029",
				["statementDate"] = "2024-03-19",
				["betTime"] = "2024-03-19 03:14:02",
				["refNo"] = "Funky_tt_4xGoldenDynastyx5199x82477",
				["status"] = "W",
				["gameCode"] = "10261",
				["gameName"] = "Golden Dynasty",
				["currency"] = "CNY",
				["betAmount"] = 0.76,
				["effectiveStake"] = 0.76,
				["winloss"] = 5.24,
				["isTransferGame"] = false
			}.ToJsonString());
		_ = fakeGameReportApiClient.GetBetListAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			userAgent: Arg.Is(query.UserAgent),
			authentication: Arg.Is(query.Authentication),
			startTime: Arg.Is(query.StartTime),
			endTime: Arg.Is(query.EndTime),
			page: Arg.Is(query.Page),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { result }.ToAsyncEnumerable());

		var sut = new GameWagerQueryService(
			fakeTimeProvider,
			fakeGameReportApiClient);

		var actual = await sut.QueryAsync(
			 query: query)
			.ToArrayAsync()
			?? Enumerable.Empty<GameWagerResult>();

		Assert.NotEmpty(actual);
		Assert.Equal("4ucmqplxb029", actual.First().PlayerCode);
		Assert.Equal(result.RefNo, actual.First().ExternalWagerId);
		Assert.Equal(result.GameCode, actual.First().ExternalGameId);
		Assert.Equal(GameWagerStatus.Won, actual.First().Status);
		Assert.Equal(result.BetTime, actual.First().ExternalCreatedOn);
		Assert.Equal(result.BetTime, actual.First().ExternalClosedOn);
		Assert.Equal(0.76m, actual.First().Bet);
		Assert.Equal(0.76m, actual.First().ValidBet);
		Assert.Equal(6m, actual.First().Settle);
		Assert.Equal(5.24m, actual.First().WinLoss);
		Assert.Equal(result.AdditionalData, actual.First().AdditionalData);
	}
}
