using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.Funky.Core.ApplicationServices;
using Tgs.GameInterface.Funky.Core.Models;
using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new FundInQuery(
			ApiUrl: new Uri("http://trial-gp-api.funkytest.com"),
			UserAgent: "109-PG",
			Authentication: "c69d57e5-93e7-488c-90a4-e20876b495bc",
			PlayerCode: "4ucmqplxb029",
			Currency: "CNY",
			OrderId: "018daaac-3f58-0000-0001-07e80019c9af",
			Amount: 99.0m);

		var result = new WithdrawResult(
			TxSerialId: 260338,
			TxId: "018daaac-3f58-0000-0001-07e80019c9af",
			Balance: 100,
			Res: new JsonObject
			{
				["txId"] = "018daaac-3f58-0000-0001-07e80019c9af",
				["balance"] = 100.0m,
				["playerId"] = "4ucmqplxb029",
				["currency"] = "CNY",
				["TxSerialId"] = 260338,
			}.ToJsonString());
		_ = fakeGameApiClient.WithdrawAsync(
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

		var sut = new FundInGetService(
			fakeGameApiClient);

		var actual = await sut.GetAsync(
			 query: query);

		Assert.Equal(260338, actual.TxSerialId);
		Assert.Equal("018daaac-3f58-0000-0001-07e80019c9af", actual.TxId);
		Assert.Equal(100.0m, actual.Balance);
		Assert.Equal(result.Res, actual.Res);
	}
}
