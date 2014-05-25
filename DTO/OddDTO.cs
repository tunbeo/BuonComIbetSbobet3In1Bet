#define DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using iBet.Utilities;
namespace iBet.DTO
{
    [System.Serializable]
    public class OddDTO : BaseDTO
    {
        public string Odd
        {
            get;
            set;
        }
        public eOddType Type
        {
            get;
            set;
        }
        public int OddType
        {
            get {
                if (Type == eOddType.FulltimeHandicap)
                    return 1;
                else if (Type == eOddType.FulltimeOverUnder)
                    return 3;
                else if (Type == eOddType.FirstHalfHandicap)
                    return 7;
                else if (Type == eOddType.FirstHalfOverUnder)
                    return 8;
                else
                    return 0;

            }
        }

        public float Home
        {
            get;
            set;
        }
        public float Away
        {
            get;
            set;
        }
        public float Draw
        {
            get;
            set;
        }
        public bool HomeFavor
        {
            get;
            set;
        }
        public bool AwayFavor
        {
            get;
            set;
        }
        public bool IsHomeGive
        {
            get;
            set;
        }
        public static OddDTO SearchOdd(OddDTO oddToSearch, System.Collections.Generic.List<OddDTO> dataSource)
        {
            OddDTO result;
            if (oddToSearch.Type == eOddType.FulltimeHandicap || oddToSearch.Type == eOddType.FirstHalfHandicap)
            {
                string __odd = oddToSearch.Odd.ToLower().Replace("0", "").Replace(" ", "");
                System.Collections.Generic.List<OddDTO> list = (
                    from odd in dataSource
                    where odd.Type == oddToSearch.Type && odd.Odd.ToLower().Replace("0", "").Replace(" ", "") == __odd && odd.HomeFavor == oddToSearch.HomeFavor && odd.AwayFavor == oddToSearch.AwayFavor
                    select odd).ToList<OddDTO>();
                if (list.Count == 1)
                {
                    result = list[0];
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                if (oddToSearch.Type == eOddType.FulltimeOverUnder || oddToSearch.Type == eOddType.FirstHalfOverUnder)
                {
                    string __odd = oddToSearch.Odd.ToLower().Replace("0", "").Replace(" ", "");
                    System.Collections.Generic.List<OddDTO> list = (
                        from odd in dataSource
                        where odd.Type == oddToSearch.Type && odd.Odd.ToLower().Replace("0", "").Replace(" ", "") == __odd
                        select odd).ToList<OddDTO>();
                    if (list.Count == 1)
                    {
                        result = list[0];
                    }
                    else
                    {
                        result = null;
                    }
                }
                else
                {
#if DEBUG
                    iBet.Utilities.WriteLog.Write("Error 005 Can not search: " + oddToSearch);
#endif
                    result = null;
                }
            }
            return result;
        }
        public static OddDTO SearchOdd(string oddID, System.Collections.Generic.List<OddDTO> dataSource)
        {
            OddDTO result;
            try
            {
                System.Collections.Generic.List<OddDTO> list = (
                    from odd in dataSource
                    where odd.ID.Equals(oddID)
                    select odd).ToList<OddDTO>();
                if (list.Count == 1)
                {
                    result = list[0];
                }
                else
                {
                    result = null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                iBet.Utilities.WriteLog.Write("Error 006 Can not search: " + oddID + ",ex: " + ex);
#endif
                result = null;
            }
            return result;

        }
        public static OddDTO SearchOdd(eOddType oddType, string oddtoSearch, System.Collections.Generic.List<OddDTO> dataSource)
        {
            OddDTO result = null;
            string __odd = oddtoSearch.ToLower().Replace("0", "").Replace(" ", "");
            if (dataSource != null)
            {
                try
                {
                    System.Collections.Generic.List<OddDTO> list = (
                        from odd in dataSource
                        where odd.Type == oddType && odd.Odd.ToLower().Replace("0", "").Replace(" ", "").Equals(__odd)
                        select odd).ToList<OddDTO>();
                    if (list.Count == 1)
                    {
                        result = list[0];
                    }
                    else
                    {
                        result = null;
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    iBet.Utilities.WriteLog.Write("Error 006 Can not search: " + oddtoSearch + ",ex: " + ex);
#endif
                    result = null;
                }
            }
            return result;

        }
        public static OddDTO SearchOdd(eOddType oddType, string oddtoSearch, bool a, System.Collections.Generic.List<OddDTO> dataSource)
        {
            OddDTO result = null;
            string __odd = oddtoSearch.ToLower().Trim();
            decimal od = Math.Abs(decimal.Parse(__odd));
            if (dataSource != null)
            {
                try
                {
                    System.Collections.Generic.List<OddDTO> list = (
                        from odd in dataSource
                        where odd.Type == oddType && (float.Parse(ConvertOdd(odd.Odd.ToLower().Trim())) == (float.Parse((od.ToString()))))
                        select odd).ToList<OddDTO>();
                    if (list.Count == 1)
                    {
                        result = list[0];
                    }
                    else
                    {
                        result = null;
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    iBet.Utilities.WriteLog.Write("Error 006 Can not search: " + oddtoSearch + ",ex: " + ex);
#endif
                    result = null;
                }
            }
            return result;

        }
        public static OddDTO SearchOdd(string matchID, string oddID, string oddValue, System.Collections.Generic.List<MatchDTO> dataSource)
        {
            MatchDTO matchDTO = MatchDTO.SearchMatch(matchID, dataSource);
            OddDTO result;
            if (matchDTO != null)
            {
                System.Collections.Generic.List<OddDTO> list = (
                    from odd in matchDTO.Odds
                    where odd.ID.Equals(oddID) && odd.Odd.Equals(oddValue)
                    select odd).ToList<OddDTO>();
                if (list.Count == 1)
                {
                    result = list[0];
                    return result;
                }
            }
            result = null;
            return result;
        }
        public static bool IsValidOddPair(float firstOdd, float secondOdd, double oddValueDifferent, bool highRevenueBoost)
        {
            bool result;
            if (highRevenueBoost)
            {
                if ((firstOdd < 0f && secondOdd > 0f) || (firstOdd > 0f && secondOdd < 0f))
                {
                    result = (System.Math.Round((double)(firstOdd + secondOdd), 3) >= oddValueDifferent);
                }
                else
                {
                    result = (firstOdd < 0f && secondOdd < 0f);
                }
            }
            else
            {
                if ((firstOdd < 0f && secondOdd > 0f) || (firstOdd > 0f && secondOdd < 0f))
                {
                    result = (System.Math.Round((double)(firstOdd + secondOdd), 3) == oddValueDifferent);
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
        private static string ConvertOdd(string odd)
        {
            if (odd.Contains("-"))
            {
                string[] parts = odd.Split(new string[] { "-" }, StringSplitOptions.None);
                float a1 = (float.Parse(parts[0]) + float.Parse(parts[1])) / 2;
                return (Math.Abs((decimal)a1)).ToString();
            }
            return odd;
        }
    }
}
