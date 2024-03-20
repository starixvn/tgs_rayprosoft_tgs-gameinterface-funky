namespace Tgs.GameInterface.Funky.ViewModels;

public record FundStatusViewModel(
	Uri ApiUrl,
	string UserAgent,
	string Authentication,
	string PlayerCode,
	string OrderId);
