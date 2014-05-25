using System;
namespace iBet.DTO
{
    [System.Serializable]
    public class BetDTO : BaseDTO
    {
        public string Account
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
        public string League
        {
            get;
            set;
        }
        public string RefID
        {
            get;
            set;
        }
        public int Stake
        {
            get;
            set;
        }
        public bool Status
        {
            get;
            set;
        }
        public bool Live
        {
            get;
            set;
        }
        public bool Dumb
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
        public string Score
        {
            get;
            set;
        }
        public string IP
        {
            get;
            set;
        }
        public string Choice
        {
            get;
            set;
        }
        public eOddType Type
        {
            get;
            set;
        }
        public string OddValue
        {
            get;
            set;
        }
        public string OddKindValue
        {
            get;
            set;
        }
        public string Odd
        {
            get;
            set;
        }
        public System.DateTime DateTime
        {
            get;
            set;
        }
        public string Note
        {
            get;
            set;
        }
    }
}
