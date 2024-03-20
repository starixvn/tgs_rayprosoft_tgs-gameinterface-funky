namespace Tgs.GameInterface.Funky.Core.ApplicationServices;

public record FundStatusQuery(
	Uri ApiUrl,
	string UserAgent,
	string Authentication,
	string PlayerCode,
	string OrderId);
