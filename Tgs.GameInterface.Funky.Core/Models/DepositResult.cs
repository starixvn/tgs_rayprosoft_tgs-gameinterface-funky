namespace Tgs.GameInterface.Funky.Core.Models;
public record DepositResult(
	long TxSerialId,
	string TxId,
	decimal Balance,
	string Res);
