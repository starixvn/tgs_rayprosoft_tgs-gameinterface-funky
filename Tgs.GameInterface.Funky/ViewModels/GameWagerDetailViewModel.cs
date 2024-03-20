namespace Tgs.GameInterface.Funky.ViewModels;

public record GameWagerDetailViewModel(
	Uri ApiUrl,
	string UserAgent,
	string Authentication,
	string PlayerCode,
	string GameCode,
	string ExternalWagerId,
	string Language);
