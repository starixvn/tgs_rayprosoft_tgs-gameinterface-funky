namespace Tgs.GameInterface.Funky.Core.Models;
public record BetResult(
	string PlayerId,
	DateTimeOffset StatementDate,
	DateTimeOffset BetTime,
	string RefNo,
	string Status,
	string GameCode,
	string GameName,
	string Currency,
	decimal BetAmount,
	decimal EffectiveStake,
	decimal Winloss,
	bool IsTransferGame,
	string AdditionalData);
