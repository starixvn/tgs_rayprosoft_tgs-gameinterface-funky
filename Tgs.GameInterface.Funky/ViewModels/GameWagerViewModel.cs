namespace Tgs.GameInterface.Funky.ViewModels;

public record GameWagerViewModel(
	Uri ApiUrl,
	string UserAgent,
	string Authentication,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime,
	int Page);
