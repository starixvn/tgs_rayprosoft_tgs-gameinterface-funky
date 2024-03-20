using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.Funky.Core.ApplicationServices;
using Tgs.GameInterface.Funky.Core.Models;
using Tgs.GameInterface.Funky.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Funky.GrpcServices;

public class GameInterfaceFunkyGrpcService(
	ILogger<GameInterfaceFunkyGrpcService> logger,
	IGetService<FundInQuery, WithdrawResult> fundInService,
	IGetService<FundOutQuery, DepositResult> fundOutService,
	IGetService<FundStatusQuery, bool> fundStatusService,
	IGetService<GameUrlQuery, string> gameUrlService,
	IGetService<GameWagerDetailQuery, string> gameWagerDetailService,
	IQueryService<GameWagerQuery, GameWagerResult> gameWagerService,
	IGetService<PlayerBalanceQuery, decimal> playerBalanceService) : GameInterfaceFunky.GameInterfaceFunkyBase
{
	public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
	{
		logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

		return Task.FromResult(new CheckingServiceReply
		{
			MachineName = Environment.MachineName
		});
	}

	public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
	{
		var query = new FundInQuery(
			ApiUrl: new Uri(request.ApiUrl),
			UserAgent: request.UserAgent,
			Authentication: request.Authentication,
			PlayerCode: request.PlayerCode,
			Currency: request.Currency,
			OrderId: request.OrderId,
			Amount: request.Amount);

		var result = await fundInService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply()
		{
			TxSerialId = result.TxSerialId,
			TxId = result.TxId,
			Balance = result.Balance,
			Res = result.Res
		};
	}

	public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
	{
		var query = new FundOutQuery(
			ApiUrl: new Uri(request.ApiUrl),
			UserAgent: request.UserAgent,
			Authentication: request.Authentication,
			PlayerCode: request.PlayerCode,
			Currency: request.Currency,
			OrderId: request.OrderId,
			Amount: request.Amount);

		var result = await fundOutService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply()
		{
			TxSerialId = result.TxSerialId,
			TxId = result.TxId,
			Balance = result.Balance,
			Res = result.Res
		};
	}

	public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
	{
		var query = new FundStatusQuery(
			ApiUrl: new Uri(request.ApiUrl),
			UserAgent: request.UserAgent,
			Authentication: request.Authentication,
			PlayerCode: request.PlayerCode,
			OrderId: request.OrderId);

		var result = await fundStatusService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply()
		{
			State = result
		};
	}

	public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
	{
		var query = new GameUrlQuery(
			ApiUrl: new Uri(request.ApiUrl),
			UserAgent: request.UserAgent,
			Authentication: request.Authentication,
			PlayerCode: request.PlayerCode,
			Currency: request.Currency,
			Language: request.Language,
			GameCode: request.GameCode,
			PlayerIp: request.PlayerIp,
			RedirectUrl: request.RedirectUrl,
			SessionId: request.SessionId,
			UserNames: request.UserNames);

		var result = await gameUrlService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new GameUrlReply()
		{
			GameUrl = result
		};
	}

	public override async Task<GameWagerReply> GameWager(GameWagerRequest request, ServerCallContext context)
	{
		var query = new GameWagerQuery(
			ApiUrl: new Uri(request.ApiUrl),
			UserAgent: request.UserAgent,
			Authentication: request.Authentication,
			StartTime: request.StartTime.ToDateTimeOffset(),
			EndTime: request.EndTime.ToDateTimeOffset(),
			Page: request.Page);

		var results = await gameWagerService.QueryAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ToArrayAsync(
			context.CancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerResult>();

		var reply = new GameWagerReply();

		if (results.Any())
		{
			foreach (var result in results)
			{
				reply.Items.Add(new GameWagerInfo
				{
					ExternalWagerId = result.ExternalWagerId,
					ExternalGameId = result.ExternalGameId,
					PlayerCode = result.PlayerCode,
					Status = (GameWagerStatus)result.Status,
					ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
					ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
					Bet = result.Bet,
					ValidBet = result.ValidBet,
					Settle = result.Settle,
					WinLoss = result.WinLoss,
					AdditionalData = result.AdditionalData
				});
			}
		}

		return reply;
	}

	public override async Task<GameWagerDetailReply> GameWagerDetail(GameWagerDetailRequest request, ServerCallContext context)
	{
		var query = new GameWagerDetailQuery(
			ApiUrl: new Uri(request.ApiUrl),
			UserAgent: request.UserAgent,
			Authentication: request.Authentication,
			PlayerCode: request.PlayerCode,
			GameCode: request.GameCode,
			ExternalWagerId: request.ExternalWagerId,
			Language: request.Language);

		var result = await gameWagerDetailService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new GameWagerDetailReply()
		{
			GameWagerDetailUrl = result
		};
	}

	public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: new Uri(request.ApiUrl),
			UserAgent: request.UserAgent,
			Authentication: request.Authentication,
			PlayerCode: request.PlayerCode,
			Currency: request.Currency);

		var result = await playerBalanceService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceReply()
		{
			Balance = result
		};
	}
}
