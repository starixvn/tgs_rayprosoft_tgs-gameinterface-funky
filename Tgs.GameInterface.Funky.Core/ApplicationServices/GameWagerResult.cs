namespace Tgs.GameInterface.Funky.Core.ApplicationServices;
public record GameWagerResult(
	string ExternalWagerId,
	string ExternalGameId,
	string PlayerCode,
	GameWagerStatus Status,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal WinLoss,
	string AdditionalData);
