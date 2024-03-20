using NSubstitute;
using Tgs.GameInterface.Funky.Core.ApplicationServices;
using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core.UnitTest.ApplicationServices;

public class GameWagerDetailGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new GameWagerDetailQuery(
			ApiUrl: new Uri("http://trial-gp-api.funkytest.com"),
			UserAgent: "109-PG",
			Authentication: "c69d57e5-93e7-488c-90a4-e20876b495bc",
			PlayerCode: "4ucmqplxb029",
			GameCode: "10211",
			ExternalWagerId: "Funky_tt_4xCaptainMoneyx5199x1359",
			Language: "en");

		var url = "https://funky-test.ttstations.com/orderid/?orderid=4xCaptainMoneyx5199x1359&gamecode=Fish_CaptainMoney&language=zh-cn";
		_ = fakeGameApiClient.GetBetDetailAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			userAgent: Arg.Is(query.UserAgent),
			authentication: Arg.Is(query.Authentication),
			playerId: Arg.Is(query.PlayerCode),
			gameCode: Arg.Is(query.GameCode),
			refNo: Arg.Is(query.ExternalWagerId),
			language: Arg.Is(query.Language),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(url);

		var sut = new GameWagerDetailGetService(
			fakeGameApiClient);

		var actual = await sut.GetAsync(
			 query: query);

		Assert.Equal(url, actual);
	}
}
