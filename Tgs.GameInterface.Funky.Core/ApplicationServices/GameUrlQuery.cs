namespace Tgs.GameInterface.Funky.Core.ApplicationServices;

public record GameUrlQuery(
	Uri ApiUrl,
	string UserAgent,
	string Authentication,
	string PlayerCode,
	string Currency,
	string Language,
	string GameCode,
	string PlayerIp,
	string RedirectUrl,
	string SessionId,
	string UserNames);
