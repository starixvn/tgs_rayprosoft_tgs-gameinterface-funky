namespace Tgs.GameInterface.Funky.Core.ApplicationServices;
public record GameWagerDetailQuery(
	Uri ApiUrl,
	string UserAgent,
	string Authentication,
	string PlayerCode,
	string GameCode,
	string ExternalWagerId,
	string Language);
