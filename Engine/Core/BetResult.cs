using System;
namespace iBet.App.Engine.Core
{
	public enum BetResult
	{
		Undefined,
		Reject,
		WaitingRejected,
		Refund,
		Won,
		Lose,
		Draw,
		Running,
		Waiting,
		Void
	}
}
