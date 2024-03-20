using Tgs.ServiceUtility;
using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core.ApplicationServices;

internal class GameWagerDetailGetService(
	IGameApiClient client) : IGetService<GameWagerDetailQuery, string>
{
	public ValueTask<string> GetAsync(GameWagerDetailQuery query, CancellationToken cancellationToken = default)
		=> client.GetBetDetailAsync(
			apiUrl: query.ApiUrl,
			userAgent: query.UserAgent,
			authentication: query.Authentication,
			playerId: query.PlayerCode,
			gameCode: query.GameCode,
			refNo: query.ExternalWagerId,
			language: query.Language,
			cancellationToken: cancellationToken);
}
