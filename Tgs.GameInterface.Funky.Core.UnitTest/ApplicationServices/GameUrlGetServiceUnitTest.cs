using System.Web;
using NSubstitute;
using Tgs.GameInterface.Funky.Core.ApplicationServices;
using Tgs.GameInterface.Funky.Core.Models;
using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new GameUrlQuery(
			ApiUrl: new Uri("http://trial-gp-api.funkytest.com"),
			UserAgent: "109-PG",
			Authentication: "c69d57e5-93e7-488c-90a4-e20876b495bc",
			PlayerCode: "4ucmqplxb029",
			Currency: "CNY",
			Language: "en",
			GameCode: "10211",
			PlayerIp: "127.0.0.1",
			RedirectUrl: string.Empty,
			SessionId: "36a6cdef-094f-eb11-8d88-70b5e82b6b8d",
			UserNames: "4ucmqplxb029");

		var result = new LaunchGameResult(
			GameUrl: "https://funky-test.ttstations.com/Game/Fish_CaptainMoney/",
			Token: "b1e1d229-fb2c-4fcf-bad6-64d37c467bb1&platformId=4&language=en&gameCode=CaptainMoney&hideBackButton=0&reloadOnIdle=0&hideCurrency=0&hideRotateHint=0&hideHistory=0");
		_ = fakeGameApiClient.LaunchGameAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			userAgent: Arg.Is(query.UserAgent),
			authentication: Arg.Is(query.Authentication),
			playerId: Arg.Is(query.PlayerCode),
			currency: Arg.Is(query.Currency),
			language: Arg.Is(query.Language),
			gameCode: Arg.Is(query.GameCode),
			playerIp: Arg.Is(query.PlayerIp),
			redirectUrl: Arg.Is(query.RedirectUrl),
			sessionId: Arg.Is(query.SessionId),
			userName: Arg.Is(query.UserNames),
			isTestAccount: Arg.Is(false),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

		var sut = new GameUrlGetService(
			fakeGameApiClient);

		var actual = await sut.GetAsync(
			 query: query);

		var path = new Uri(result.GameUrl);
		var builder = new UriBuilder(path);
		var parseQuery = HttpUtility.ParseQueryString(builder.Query);
		parseQuery["token"] = result.Token;
		builder.Query = parseQuery.ToString();

		Assert.Equal(HttpUtility.UrlDecode(builder.ToString()), actual);
	}
}
