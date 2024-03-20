using NSubstitute;
using Tgs.GameInterface.Funky.Core.ApplicationServices;
using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new PlayerBalanceQuery(
			ApiUrl: new Uri("http://trial-gp-api.funkytest.com"),
			UserAgent: "109-PG",
			Authentication: "c69d57e5-93e7-488c-90a4-e20876b495bc",
			PlayerCode: "4ucmqplxb029",
			Currency: "CNY");

		_ = fakeGameApiClient.GetBalanceByCurrencyAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			userAgent: Arg.Is(query.UserAgent),
			authentication: Arg.Is(query.Authentication),
			playerId: Arg.Is(query.PlayerCode),
			currency: Arg.Is(query.Currency),
			Arg.Any<CancellationToken>())
			.Returns(90.0m);

		var sut = new PlayerBalanceGetService(
			fakeGameApiClient);

		var actual = await sut.GetAsync(
			 query: query);

		Assert.Equal(90.0m, actual);
	}
}
