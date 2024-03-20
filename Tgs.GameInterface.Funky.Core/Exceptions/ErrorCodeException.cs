namespace Tgs.GameInterface.Funky.Core.Exceptions;

public class ErrorCodeException(int code) : Exception(ConvertErrorMesaage(code))
{
	private static string ConvertErrorMesaage(int code) => code switch
	{
		0 => "成功",
		400 => "Invalid Input",
		401 => "Player Not Login",
		402 => "Insufficient Balance",
		405 => "Api Suspended",
		511 => "Transaction not found",
		512 => "ransaction already existed",
		513 => "Previous transaction hasn’t completed",
		514 => "Wallet not found",
		3002 => "Report Invalid Input",
		3003 => "Report Page Not Found",
		3004 => "Report GameCode Not Found",
		10005 => "GameCode is not allowed.GameCode is XXXX",
		9999 => "Internal Server Error",
		_ => $"遊戲供應商未提供錯誤代碼{code}"
	};
}
