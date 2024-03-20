namespace Tgs.GameInterface.Funky.Core.ApplicationServices;

public record FundInQuery(
	Uri ApiUrl,
	string UserAgent,
	string Authentication,
	string PlayerCode,
	string Currency,
	string OrderId,
	decimal Amount);
