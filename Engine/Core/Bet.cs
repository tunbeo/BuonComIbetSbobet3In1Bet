using System;
namespace iBet.App.Engine.Core
{
	public class Bet
	{
		public string Id
		{
			get;
			set;
		}
		public string Home
		{
			get;
			set;
		}
		public string Away
		{
			get;
			set;
		}
		public BetType Type
		{
			get;
			set;
		}
		public int Market
		{
			get;
			set;
		}
		public decimal OddsValue
		{
			get;
			set;
		}
		public Choice Choice
		{
			get;
			set;
		}
		public string Score
		{
			get;
			set;
		}
		public bool IsLive
		{
			get;
			set;
		}
		public BetStatus Status
		{
			get;
			set;
		}
		public decimal MaxBetAllowed
		{
			get;
			set;
		}
		public decimal MinBetAllowed
		{
			get;
			set;
		}
		public decimal Stake
		{
			get;
			set;
		}
		public decimal Handicap
		{
			get;
			set;
		}
		public BetResult Result
		{
			get;
			set;
		}
		public DateTime BetTime
		{
			get;
			set;
		}
		public decimal Step
		{
			get;
			set;
		}
		public bool IsAccident
		{
			get;
			set;
		}
		public string MatchId
		{
			get;
			set;
		}
		public Bet()
		{
			this.Step = 1m;
		}
		public Bet Clone()
		{
			return new Bet
			{
				Id = this.Id,
				Home = this.Home,
				Away = this.Away,
				Type = this.Type,
				OddsValue = this.OddsValue,
				Choice = this.Choice,
				Score = this.Score,
				IsLive = this.IsLive,
				Status = this.Status,
				MaxBetAllowed = this.MaxBetAllowed,
				MinBetAllowed = this.MinBetAllowed,
				Stake = this.Stake,
				Handicap = this.Handicap,
				Result = this.Result,
				BetTime = this.BetTime,
				Step = this.Step,
				IsAccident = this.IsAccident,
				MatchId = this.MatchId
			};
		}
		public string ToString2()
		{
			return string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}%_{9}_{10}", new object[]
			{
				this.Id,
				this.Home,
				this.Away,
				(int)this.Type,
				this.Handicap,
				this.OddsValue,
				this.Choice.ToString().ToLower(),
				this.Stake,
				this.Step * 100m,
				this.MinBetAllowed,
				this.MaxBetAllowed
			});
		}
		public string ToText()
		{
			return string.Format("{1} - vs - {2} : Loại kèo : {3} - Kèo : {4} - Giá : {5} - Chọn : {6} - Số điểm cược : {7}", new object[]
			{
				this.Id,
				this.Home,
				this.Away,
				(int)this.Type,
				this.Handicap,
				this.OddsValue,
				this.Choice.ToString().ToLower(),
				this.Stake
			});
		}
		public string ToText2()
		{
			return string.Format("{1} - vs - {2} : Loại kèo : {3} - Kèo : {4} - Giá : {5} - Chọn : {6}", new object[]
			{
				this.Id,
				this.Home,
				this.Away,
				(int)this.Type,
				this.Handicap,
				this.OddsValue,
				this.Choice.ToString().ToLower()
			});
		}
	}
}
