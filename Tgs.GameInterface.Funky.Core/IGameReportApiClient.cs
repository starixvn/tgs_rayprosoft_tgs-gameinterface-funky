using Tgs.GameInterface.Funky.Core.Models;

namespace Tgs.GameInterface.Funky.Core;

public interface IGameReportApiClient
{
	public IAsyncEnumerable<BetResult> GetBetListAsync(Uri apiUrl, string userAgent, string authentication, DateTimeOffset startTime, DateTimeOffset endTime, int page, CancellationToken cancellationToken = default);
}
