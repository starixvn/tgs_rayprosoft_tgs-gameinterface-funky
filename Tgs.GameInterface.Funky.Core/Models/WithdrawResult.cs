namespace Tgs.GameInterface.Funky.Core.Models;
public record WithdrawResult(
	long TxSerialId,
	string TxId,
	decimal Balance,
	string Res);
