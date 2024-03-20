namespace Tgs.GameInterface.Funky.ViewModels;

public record FundViewModel(
	Uri ApiUrl,
	string UserAgent,
	string Authentication,
	string PlayerCode,
	string Currency,
	string OrderId,
	decimal Amount);
