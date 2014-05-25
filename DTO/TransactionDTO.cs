using System;
namespace iBet.DTO
{
    [System.Serializable]
    public class TransactionDTO : BaseDTO
    {
        public string AccountPair
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
        public bool HomePick
        {
            get;
            set;
        }
        public string FT1X2Pick
        {
            get;
            set;
        }
        public bool Follow
        {
            get;
            set;
        }
        public string FollowRef
        {
            get;
            set;
        }
        public bool IsFollowTypeTrans
        {
            get;
            set;
        }
        public string OddFailed
        {
            get;
            set;
        }
        public string OddFailedReal
        {
            get;
            set;
        }
        public string League
        {
            get;
            set;
        }
        public int StakeFailed
        {
            get;
            set;
        }
        public string OddValueFailed
        {
            get;
            set;
        }
        public string BetTime
        {
            get;
            set;
        }
        public string HomeTeamSBOBET
        {
            get;
            set;
        }
        public string AwayTeamSBOBET
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
        public string OddType
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
        public string Stake
        {
            get;
            set;
        }

        public bool IBETAllow
        {
            get;
            set;
        }
        public bool SBOBETAllow
        {
            get;
            set;
        }
        public bool THREEIN1Allow
        {
            get;
            set;
        }

        public bool IBETTrade
        {
            get;
            set;
        }
        public bool SBOBETTrade
        {
            get;
            set;
        }
        public bool THREEIN1Trade
        {
            get;
            set;
        }

        public bool THREEIN1ReTrade
        {
            get;
            set;
        }
        public bool SBOBETReTrade
        {
            get;
            set;
        }
        public bool IBETReTrade
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
