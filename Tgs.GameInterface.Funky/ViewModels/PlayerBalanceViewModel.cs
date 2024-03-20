namespace Tgs.GameInterface.Funky.ViewModels;

public record PlayerBalanceViewModel(
	Uri ApiUrl,
	string UserAgent,
	string Authentication,
	string PlayerCode,
	string Currency);
