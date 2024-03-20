namespace Tgs.GameInterface.Funky.Core.Models;
public record CheckTransactionResult(
	string TxId,
	string PlayerId,
	decimal Amount,
	string TxType,
	DateTimeOffset TxTime,
	string Currency,
	long TxSerialId,
	string Status);
