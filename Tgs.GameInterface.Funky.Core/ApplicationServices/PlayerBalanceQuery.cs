namespace Tgs.GameInterface.Funky.Core.ApplicationServices;

public record PlayerBalanceQuery(
	Uri ApiUrl,
	string UserAgent,
	string Authentication,
	string PlayerCode,
	string Currency);
