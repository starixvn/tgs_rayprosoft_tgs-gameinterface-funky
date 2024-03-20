namespace Tgs.GameInterface.Funky.ViewModels;

public record FundResultViewModel(
	long TxSerialId,
	string TxId,
	decimal Balance,
	string Res);
