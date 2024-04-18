using System.Web;

using Tgs.ServiceUtility;

using Tgs.GameInterface.Funky.Core;

namespace Tgs.GameInterface.Funky.Core.ApplicationServices;

internal class GameUrlGetService(IGameApiClient client) : IGetService<GameUrlQuery, string>
{
	public async ValueTask<string> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.LaunchGameAsync(
			apiUrl: query.ApiUrl,
			userAgent: query.UserAgent,
			authentication: query.Authentication,
			playerId: query.PlayerCode,
			currency: query.Currency,
			language: query.Language,
			gameCode: query.GameCode,
			playerIp: query.PlayerIp,
			redirectUrl: query.RedirectUrl,
			sessionId: query.SessionId,
			userName: query.UserNames,
			isTestAccount: false,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
		var path = new Uri(result.GameUrl);
		var builder = new UriBuilder(path);
		var parseQuery = HttpUtility.ParseQueryString(builder.Query);
		if (string.IsNullOrEmpty(parseQuery["token"]?.ToString() ?? string.Empty))
		{
			parseQuery["token"] = result.Token;
		}

		builder.Query = parseQuery.ToString();

		return HttpUtility.UrlDecode(builder.ToString());
	}
}
