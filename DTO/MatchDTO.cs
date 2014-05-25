using System;
using System.Collections.Generic;
using System.Linq;
namespace iBet.DTO
{
	[System.Serializable]
	public class MatchDTO : BaseDTO
	{
        public string KickOffTime
        {
            get;
            set;
        }
        public string HomeTeamName
		{
			get;
			set;
		}
		public string AwayTeamName
		{
			get;
			set;
		}
        public string HomeScore
        {
            get;
            set;
        }
        public string AwayScore
        {
            get;
            set;
        }
		public int Half
		{
			get;
			set;
		}
		public int Minute
		{
			get;
			set;
		}
		public bool IsHalfTime
		{
			get;
			set;
		}
		public System.Collections.Generic.List<OddDTO> Odds
		{
			get;
			set;
		}
		public LeagueDTO League
		{
			get;
			set;
		}
		public string LeagueName
		{
			get
			{
				string result;
				if (this.League == null)
				{
					result = "";
				}
				else
				{
					result = "League: " + this.League.Name;
				}
				return result;
			}
		}
		public int OddCount
		{
			get
			{
				int result;
				if (this.Odds == null)
				{
					result = 0;
				}
				else
				{
					result = this.Odds.Count;
				}
				return result;
			}
		}
		public static bool IsValidMatch(MatchDTO match)
		{
			return !match.LeagueName.ToLower().Contains("special") && !match.LeagueName.ToLower().Contains("specific") && !match.LeagueName.ToLower().Contains("fantasy") && !match.LeagueName.ToLower().Contains("winner") && (!match.HomeTeamName.ToLower().Contains("+") && !match.HomeTeamName.ToLower().Contains("corner") && !match.HomeTeamName.ToLower().Contains("(et)") && !match.HomeTeamName.ToLower().Contains("(pen)") && !match.HomeTeamName.ToLower().Contains("(winner)")) && !match.HomeTeamName.ToLower().Contains("winner") && (!match.AwayTeamName.ToLower().Contains("+") && !match.AwayTeamName.ToLower().Contains("corner") && !match.AwayTeamName.ToLower().Contains("(et)") && !match.AwayTeamName.ToLower().Contains("(pen)") && !match.AwayTeamName.ToLower().Contains("(winner)")) && !match.AwayTeamName.ToLower().Contains("winner");
		}
		public static MatchDTO SearchMatch(MatchDTO matchToSearch, System.Collections.Generic.List<MatchDTO> dataSource)
		{
			string text = matchToSearch.HomeTeamName.ToLower();
			string text2 = matchToSearch.AwayTeamName.ToLower();
			string text3 = matchToSearch.League.Name.ToLower();
			MatchDTO result;
			if (text3.Contains("special") || text3.Contains("specific") || text3.Contains("fantasy") || text3.Contains("which") || text3.Contains("team") || text3.Contains("advance") || text3.Contains("next") || text3.Contains("winner"))
			{
				result = null;
			}
			else
			{
				if (text.Contains("+") || text.Contains("corner") || text.Contains("(et)") || text.Contains("(pen)") || text.Contains("(winner)") || text.Contains("winner"))
				{
					result = null;
				}
				else
				{
					if (text2.Contains("+") || text2.Contains("corner") || text2.Contains("(et)") || text2.Contains("(pen)") || text2.Contains("(winner)") || text2.Contains("winner"))
					{
						result = null;
					}
					else
					{
						foreach (MatchDTO current in dataSource)
						{
							string text4 = current.HomeTeamName.ToLower();
							string text5 = current.AwayTeamName.ToLower();
							string text6 = current.League.Name.ToLower();
							if (!text6.Contains("special") && !text6.Contains("specific") && !text6.Contains("fantasy") && !text3.Contains("which") && !text3.Contains("team") && !text3.Contains("advance") && !text3.Contains("next") && !text3.Contains("winner"))
							{
								if (!text5.Contains("+") && !text5.Contains("corner") && !text5.Contains("(et)") && !text5.Contains("(pen)") && !text5.Contains("(winner)") && !text5.Contains("winner"))
								{
									if (!text4.Contains("+") && !text4.Contains("corner") && !text4.Contains("(et)") && !text4.Contains("(pen)") && !text4.Contains("(winner)") && !text4.Contains("winner"))
									{
										if ((text.Contains(text4) && text2.Contains(text5)) ||  text5 == text2 || text == text4 )
										{
											result = current;
											return result;
										}
										if (text4.Contains(text) && text5.Contains(text2))
										{
											result = current;
											return result;
										}
									}
								}
							}
						}
						result = null;
					}
				}
			}
			return result;
		}
		public static bool IsSameMatch(string homeTeam1, string homeTeam2, string awayTeam1, string awayTeam2)
		{
			homeTeam1 = homeTeam1.ToLower();
			homeTeam2 = homeTeam2.ToLower();
			awayTeam1 = awayTeam1.ToLower();
			awayTeam2 = awayTeam2.ToLower();
			return !homeTeam1.Contains("+") && !homeTeam1.Contains("corner") && !homeTeam1.Contains("(et)") && !homeTeam1.Contains("(pen)") && !homeTeam1.Contains("(winner)") && !homeTeam1.Contains("winner") && (!awayTeam1.Contains("+") && !awayTeam1.Contains("corner") && !awayTeam1.Contains("(et)") && !awayTeam1.Contains("(pen)") && !awayTeam1.Contains("(winner)")) && !awayTeam1.Contains("winner") && (!homeTeam2.Contains("+") && !homeTeam2.Contains("corner") && !homeTeam2.Contains("(et)") && !homeTeam2.Contains("(pen)") && !homeTeam2.Contains("(winner)")) && !homeTeam2.Contains("winner") && (!awayTeam2.Contains("+") && !awayTeam2.Contains("corner") && !awayTeam2.Contains("(et)") && !awayTeam2.Contains("(pen)") && !awayTeam2.Contains("(winner)")) && !awayTeam2.Contains("winner") && 
                ((homeTeam1.Contains(homeTeam2) && awayTeam1.Contains(awayTeam2)) 
                || (homeTeam2.Contains(homeTeam1) && awayTeam2.Contains(awayTeam1))
                || (awayTeam2 == awayTeam1)
                || (homeTeam1 == homeTeam2));
		}
		public static MatchDTO SearchMatch(string matchID, System.Collections.Generic.List<MatchDTO> dataSource)
		{
			System.Collections.Generic.List<MatchDTO> list = (
				from match in dataSource
				where match.ID == matchID
				select match).ToList<MatchDTO>();
			MatchDTO result;
			if (list.Count == 1)
			{
				result = list[0];
			}
			else
			{
				result = null;
			}
			return result;
		}

        public static MatchDTO SearchMatchFull(MatchDTO matchToSearch, System.Collections.Generic.List<MatchDTO> dataSource)
        {
            string text = matchToSearch.HomeTeamName.ToLower();
            string text2 = matchToSearch.AwayTeamName.ToLower();
            string text3 = matchToSearch.League.Name.ToLower();
            MatchDTO result;

            foreach (MatchDTO current in dataSource)
            {
                string text4 = current.HomeTeamName.ToLower();
                string text5 = current.AwayTeamName.ToLower();
                string text6 = current.League.Name.ToLower();
                if (text5 == text2 || text == text4)
                {
                    result = current;
                    return result;
                }
            }
            result = null;
            return result;
        }

	}
}
