using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.Funky.Core.ApplicationServices;
using Tgs.GameInterface.Funky.Core.Models;
using Tgs.GameInterface.Funky.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Funky.Controllers;

[Route("[controller]")]
[ApiController]
public class FunkyController : ControllerBase
{
	/// <summary>
	/// Funds the in asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///			{
	///				"apiUrl": "http://trial-gp-api.funkytest.com/",
	///				"userAgent": "109-PG",
	///				"authentication": "c69d57e5-93e7-488c-90a4-e20876b495bc",
	///				"playerCode": "4ucmqplxb029",
	///				"currency": "CNY",
	///				"orderId": "018db105-5428-0000-0001-07e8d561b174",
	///				"amount": 1
	///			}
	/// </remarks>
	[HttpPost("fund/in", Name = "FundIn")]
	public async ValueTask<FundResultViewModel> FundInAsync(
		[FromServices] IGetService<FundInQuery, WithdrawResult> service,
		FundViewModel source)
	{
		var query = new FundInQuery(
			ApiUrl: source.ApiUrl,
			UserAgent: source.UserAgent,
			Authentication: source.Authentication,
			PlayerCode: source.PlayerCode,
			Currency: source.Currency,
			OrderId: source.OrderId,
			Amount: source.Amount);

		var result = await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new FundResultViewModel(
			TxSerialId: result.TxSerialId,
			TxId: result.TxId,
			Balance: result.Balance,
			Res: result.Res);
	}

	/// <summary>
	/// Funds the out asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///			{
	///				"apiUrl": "http://trial-gp-api.funkytest.com/",
	///				"userAgent": "109-PG",
	///				"authentication": "c69d57e5-93e7-488c-90a4-e20876b495bc",
	///				"playerCode": "4ucmqplxb029",
	///				"currency": "CNY",
	///				"orderId": "018dce36-76e7-0000-0001-07e8d0dbfd1a",
	///				"amount": 100
	///			}
	/// </remarks>
	[HttpPost("fund/out", Name = "FundOut")]
	public async ValueTask<FundResultViewModel> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, DepositResult> service,
		FundViewModel source)
	{
		var query = new FundOutQuery(
			ApiUrl: source.ApiUrl,
			UserAgent: source.UserAgent,
			Authentication: source.Authentication,
			PlayerCode: source.PlayerCode,
			Currency: source.Currency,
			OrderId: source.OrderId,
			Amount: source.Amount);

		var result = await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new FundResultViewModel(
			TxSerialId: result.TxSerialId,
			TxId: result.TxId,
			Balance: result.Balance,
			Res: result.Res);
	}

	/// <summary>
	/// Funds the status asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///			{
	///				"apiUrl": "http://trial-gp-api.funkytest.com/",
	///				"userAgent": "109-PG",
	///				"authentication": "c69d57e5-93e7-488c-90a4-e20876b495bc",
	///				"playerCode": "4ucmqplxb029",
	///				"orderId": "018db105-5428-0000-0001-07e8d561b174"
	///			}
	/// </remarks>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public async ValueTask<bool> FundStatusAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		FundStatusViewModel source)
	{
		var query = new FundStatusQuery(
			ApiUrl: source.ApiUrl,
			UserAgent: source.UserAgent,
			Authentication: source.Authentication,
			PlayerCode: source.PlayerCode,
			OrderId: source.OrderId);

		return await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Games the wager detail asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///			{
	///				"apiUrl": "http://trial-gp-api.funkytest.com/",
	///				"userAgent": "109-PG",
	///				"authentication": "c69d57e5-93e7-488c-90a4-e20876b495bc",
	///				"playerCode": "4ucmqplxb029",
	///				"gameCode": "10261",
	///				"externalWagerId": "Funky_tt_4xGoldenDynastyx5199x82477",
	///				"language": "ZH_CN"
	///			}
	/// </remarks>
	[HttpPost("game/wager/detail", Name = "GameWagerDetail")]
	public async ValueTask<string> GameWagerDetailAsync(
		[FromServices] IGetService<GameWagerDetailQuery, string> service,
		GameWagerDetailViewModel source)
	{
		var query = new GameWagerDetailQuery(
			ApiUrl: source.ApiUrl,
			UserAgent: source.UserAgent,
			Authentication: source.Authentication,
			PlayerCode: source.PlayerCode,
			GameCode: source.GameCode,
			ExternalWagerId: source.ExternalWagerId,
			Language: source.Language);

		return await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Games the wager asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///			{
	///				"apiUrl": "http://trial-gp-api-report.funkytest.com/",
	///				"userAgent": "109-PG",
	///				"authentication": "c69d57e5-93e7-488c-90a4-e20876b495bc",
	///				"StartTime": "2024-03-19T03:00:00+00:00",
	///				"EndTime": "2024-03-19T04:00:00+00:00",
	///				"Page": 1
	///			}
	/// </remarks>
	[HttpPost("game/wager", Name = "GameWager")]
	public IAsyncEnumerable<GameWagerResult> GameWagerAsync(
		[FromServices] IQueryService<GameWagerQuery, GameWagerResult> service,
		GameWagerViewModel source)
	{
		var query = new GameWagerQuery(
			ApiUrl: source.ApiUrl,
			UserAgent: source.UserAgent,
			Authentication: source.Authentication,
			StartTime: source.StartTime,
			EndTime: source.EndTime,
			Page: source.Page);

		return service.QueryAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Games the URL asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///			{
	///				"apiUrl": "http://trial-gp-api.funkytest.com/",
	///				"userAgent": "109-PG",
	///				"authentication": "c69d57e5-93e7-488c-90a4-e20876b495bc",
	///				"playerCode": "4ucmqplxb029",
	///				"currency": "CNY",
	///				"language": "en",
	///				"gameCode": "10211",
	///				"playerIp": "127.0.0.1",
	///				"redirectUrl": "",
	///				"sessionId": "36a6cdef-094f-eb11-8d88-70b5e82b6b8d",
	///				"userNames": "4ucmqplxb029"
	///			}
	/// </remarks>
	[HttpPost("game/url")]
	public ValueTask<string> GameUrlAsync(
		[FromServices] IGetService<GameUrlQuery, string> service,
		GameUrlViewModel source)
	{
		var query = new GameUrlQuery(
			ApiUrl: source.ApiUrl,
			UserAgent: source.UserAgent,
			Authentication: source.Authentication,
			PlayerCode: source.PlayerCode,
			Currency: source.Currency,
			Language: source.Language,
			GameCode: source.GameCode,
			PlayerIp: source.PlayerIp,
			RedirectUrl: source.RedirectUrl,
			SessionId: source.SessionId,
			UserNames: source.UserNames);

		return service.GetAsync(
			 query: query,
			 cancellationToken: HttpContext.RequestAborted);
	}

	/// <summary>
	/// Players the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///
	///			{
	///				"apiUrl": "http://trial-gp-api.funkytest.com/",
	///				"userAgent": "109-PG",
	///				"authentication": "c69d57e5-93e7-488c-90a4-e20876b495bc",
	///				"playerCode": "4ucmqplxb029",
	///				"currency": "CNY"
	///			}
	/// </remarks>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public async ValueTask<decimal> PlayerBalanceAsync(
		[FromServices] IGetService<PlayerBalanceQuery, decimal> service,
		PlayerBalanceViewModel source)
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: source.ApiUrl,
			UserAgent: source.UserAgent,
			Authentication: source.Authentication,
			PlayerCode: source.PlayerCode,
			Currency: source.Currency);

		return await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);
	}
}
