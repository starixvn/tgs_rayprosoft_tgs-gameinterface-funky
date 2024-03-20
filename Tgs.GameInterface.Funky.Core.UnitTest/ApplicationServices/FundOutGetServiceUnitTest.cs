using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.Funky.Core.ApplicationServices;
using Tgs.GameInterface.Funky.Core.Models;
using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new FundOutQuery(
			ApiUrl: new Uri("http://trial-gp-api.funkytest.com"),
			UserAgent: "109-PG",
			Authentication: "c69d57e5-93e7-488c-90a4-e20876b495bc",
			PlayerCode: "4ucmqplxb029",
			Currency: "CNY",
			OrderId: "018db105-3894-0000-0001-07e8d561b174",
			Amount: 100.0m);

		var result = new DepositResult(
			TxSerialId: 260336,
			TxId: "018db105-3894-0000-0001-07e8d561b174",
			Balance: 199.0m,
			Res: new JsonObject
			{
				["txId"] = "018db105-3894-0000-0001-07e8d561b174",
				["balance"] = 199.0m,
				["playerId"] = "4ucmqplxb029",
				["currency"] = "CNY",
				["TxSerialId"] = 260336,
			}.ToJsonString());
		_ = fakeGameApiClient.DepositAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			userAgent: Arg.Is(query.UserAgent),
			authentication: Arg.Is(query.Authentication),
			playerId: Arg.Is(query.PlayerCode),
			currency: Arg.Is(query.Currency),
			isTestAccount: Arg.Is(false),
			txId: Arg.Is(query.OrderId),
			amount: Arg.Is(query.Amount),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

		var sut = new FundOutGetService(
			fakeGameApiClient);

		var actual = await sut.GetAsync(
			 query: query);

		Assert.Equal(260336, actual.TxSerialId);
		Assert.Equal("018db105-3894-0000-0001-07e8d561b174", actual.TxId);
		Assert.Equal(199.0m, actual.Balance);
		Assert.Equal(result.Res, actual.Res);
	}
}
