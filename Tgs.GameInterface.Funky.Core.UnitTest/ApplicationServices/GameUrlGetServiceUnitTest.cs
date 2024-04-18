using System.Web;
using NSubstitute;
using Tgs.GameInterface.Funky.Core.ApplicationServices;
using Tgs.GameInterface.Funky.Core.Models;
using Tgs.GameInterface.Funky.Core;
using static System.Net.WebRequestMethods;

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

	[Fact]
	public async Task Game_Code_130xxx()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new GameUrlQuery(
			ApiUrl: new Uri("http://trial-gp-api.funkytest.com"),
			UserAgent: "109-PG",
			Authentication: "c69d57e5-93e7-488c-90a4-e20876b495bc",
			PlayerCode: "4ucmqplxb029",
			Currency: "CNY",
			Language: "en",
			GameCode: "1302638",
			PlayerIp: "220.133.218.203",
			RedirectUrl: "https://t1.srvptt.com/",
			SessionId: "6cb22ca5-275a-4605-bdf9-c06d4d981c58",
			UserNames: "4ucmqplxb029");

		var result = new LaunchGameResult(
			GameUrl: "https://game.gtmlobby.com/candyboom/?wmode=ss&lang=en&gameId=55&token=eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjExODA4OTcxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkZVTktZQ05ZXzEwOTAwMDlfNHVjbXFwbHhiMDI5IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS9hY2Nlc3Njb250cm9sc2VydmljZS8yMDEwLzA3L2NsYWltcy9pZGVudGl0eXByb3ZpZGVyIjoiQVNQLk5FVCBJZGVudGl0eSIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiNjM1ZmM3M2YtNWEyYy00NGMyLWExNzYtOWY5MTVmMzIwM2JmIiwiQXZhcnRhciI6Imh0dHBzOi8vcmVzLjEybGl2ZS52aXAvYXZhdGFyL2RlZmF1bHQvNy5wbmciLCJWaXBMZXZlbCI6IjAiLCJMYXN0TmFtZSI6IkZVTktZQ05ZXzEwOTAwMDlfNHVjbXFwbHhiMDI5IiwiRmlyc3ROYW1lIjoiRlVOS1lDTllfMTA5MDAwOV80dWNtcXBseGIwMjkiLCJUd29GYWN0b3JFbmFibGVkIjoiRmFsc2UiLCJQYXJlbnRJZCI6Ijk3NTEzMzAiLCJQYXJlbnRVc2VyTmFtZSI6IkZVTktZQ05ZIiwiQWdlbnRMZXZlbCI6IjMiLCJDdXJyZW5jeSI6IkNOWSIsIm5iZiI6MTcxMzQxMTIyMSwiZXhwIjoxNzEzNTg0MDIxLCJpc3MiOiIxMmxpdmUudmlwIiwiYXVkIjoiNDE0ZTE5MjdhMzg4NGY2OGFiYzc5ZjcyODM4MzdmZDEifQ.5g6eeYzaj5p6oI5rRGfHsgQWrjcUfZwThrSxrxX-6QY&redirectUrl=https://t1.srvptt.com/",
			Token: "&redirectUrl=https://t1.srvptt.com/");
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
		builder.Query = parseQuery.ToString();

		Assert.Equal(HttpUtility.UrlDecode(builder.ToString()), actual);
	}
}
