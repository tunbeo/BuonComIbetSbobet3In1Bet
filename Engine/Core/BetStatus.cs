using System;
namespace iBet.App.Engine.Core
{
	public enum BetStatus
	{
		Error = -1,
		Success,
		IncorrectStake = 2,
		AccountIssue = 4,
		OddClosed,
		UpdatingOdds,
		MaxBetExceeded,
		InsufficientCredit = 9,
		OverBudget,
		HandicapChanged,
		ScoreChanged,
		OddValueChanged,
		MaxPerMatchExceeded = 15,
		ForcedLogout,
		Reversed
	}
}
