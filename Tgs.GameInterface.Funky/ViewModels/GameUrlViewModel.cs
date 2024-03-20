namespace Tgs.GameInterface.Funky.ViewModels;

public record GameUrlViewModel(
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
