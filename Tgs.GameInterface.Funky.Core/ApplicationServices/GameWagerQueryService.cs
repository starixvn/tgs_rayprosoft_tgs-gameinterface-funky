using System.Runtime.CompilerServices;
using Tgs.GameInterface.Funky.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Funky.Core.ApplicationServices;

internal class GameWagerQueryService(
	TimeProvider timeProvider,
	IGameReportApiClient client) : IQueryService<GameWagerQuery, GameWagerResult>
{
	public async IAsyncEnumerable<GameWagerResult> QueryAsync(GameWagerQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var results = await client.GetBetListAsync(
			apiUrl: query.ApiUrl,
			userAgent: query.UserAgent,
			authentication: query.Authentication,
			startTime: query.StartTime,
			endTime: query.EndTime,
			page: query.Page,
			cancellationToken: cancellationToken)
			.ToArrayAsync(
			cancellationToken: cancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<BetResult>();

		if (!results.Any())
		{
			yield break;
		}

		foreach (var result in results)
		{
			var settle = result.EffectiveStake + result.Winloss;
			var status = result.Status.ToUpper() switch
			{
				"W" => GameWagerStatus.Won,
				"L" => GameWagerStatus.Loss,
				"C" => GameWagerStatus.Canceled,
				"D" => GameWagerStatus.Draw,
				"V" => GameWagerStatus.Voided,
				_ => GameWagerStatus.Running
			};
			var externalClosedOn = DateTimeOffset.UnixEpoch;
			if (status != GameWagerStatus.Running)
			{
				externalClosedOn = result.BetTime;
			}

			yield return new GameWagerResult(
				ExternalWagerId: result.RefNo,
				ExternalGameId: result.GameCode,
				PlayerCode: result.PlayerId,
				Status: status,
				ExternalCreatedOn: result.BetTime,
				ExternalClosedOn: externalClosedOn,
				Bet: result.BetAmount,
				ValidBet: result.EffectiveStake,
				Settle: settle,
				WinLoss: result.Winloss,
				AdditionalData: result.AdditionalData);
		}
	}
}
