namespace Tgs.GameInterface.Funky.Core.ApplicationServices;
public record GameWagerQuery(
	Uri ApiUrl,
	string UserAgent,
	string Authentication,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime,
	int Page);
