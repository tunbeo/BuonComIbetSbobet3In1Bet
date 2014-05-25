using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using iBet.DTO;
using BCCore;
namespace iBet.Engine
{
    public class SbobetEngine : BaseEngine
    {
        public BCCore.SBOAgent sboAgent;
        private string content;
        private int liveVersion;
        private int nonLiveVersion;
        public Dictionary<string, SboMatch> DicMatch;

        private Dictionary<string, ArrayList> _tournamentDictionary;
        private Dictionary<string, ArrayList> _eventDictionary;
        private Dictionary<string, ArrayList> _eventResultDictionary;
        private Dictionary<string, ArrayList> _oddsDictionary;
        private Dictionary<string, ArrayList> _marketGroupDictionary;
        private Dictionary<string, ArrayList> _eventResultExtraDictionary = new Dictionary<string, ArrayList>();

        private System.Net.CookieContainer _cookies;
        private string _userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Win64; x64; Trident/4.0; .NET CLR 2.0.50727; SLCC2; .NET4.0C; .NET4.0E)";
        private string _host;
        private string _loginName;
        public float _currentCredit;
        private System.Net.HttpWebRequest _fullDataRequest;
        private System.Net.HttpWebRequest _prepareBetRequest;
        private System.Net.HttpWebRequest _confirmBetRequest;
        private Timer _fullDataTimer;
        private int _updateDataInterval = 10000;
        public event EngineDelegate FullDataCompleted;
        public event EngineDelegate UpdateCompleted;
        public int UpdateDataInterval
        {
            get
            {
                return this._updateDataInterval;
            }
            set
            {
                this._updateDataInterval = value;
            }
        }
        public bool LastUpdateCompleted
        {
            get;
            set;
        }
        public SbobetEngine(string host, string loginName, System.Net.CookieContainer cookies)
        {
            this._host = host;
            this._loginName = loginName;
            this._cookies = cookies;
            this.InitializeObjects();
            DicMatch = new Dictionary<string,SboMatch>();
        }
        
        public SbobetEngine(BCCore.BetAccount account, BCCore.Utis.EngineLogger logger, string engineID)
        {
            this.sboAgent = new SBOAgent(account, logger, engineID);
            this.InitializeObjects();
            DicMatch = new Dictionary<string, SboMatch>();
        }
        public void Start()
        {
            this.GetFullData();
            this._fullDataTimer.Interval = this._updateDataInterval;
            this._fullDataTimer.Start();
        }
        public void Stop()
        {
            this._fullDataTimer.Stop();
        }
        private void InitializeObjects()
        {
            this._fullDataTimer = new Timer();
            this._fullDataTimer.Interval = this._updateDataInterval;
            this._fullDataTimer.Tick += new System.EventHandler(this._fullDataTimer_Tick);
        }
        private void _fullDataTimer_Tick(object sender, System.EventArgs e)
        {            
            this.GetFullData();
        }
        private void GetFullData()
        {
            sboAgent.RefreshOdds();
            this.On_FullDataCompleted(new EngineEventArgs
            {
                Type = eEngineEventType.Success,
                Data = this.sboAgent.parserLive.dicMatches
            });                    
        }
        public float GetCurrentCredit()
        {
            float result = 0f;
            try
            {
                string text = "";                
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://" + this._host + "/web-root/restricted/top-module/action-data.aspx?action=bet-credit");
                httpWebRequest.UserAgent = this._userAgent;
                httpWebRequest.Method = "GET";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.Accept = "image/jpeg, image/gif, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, */*";
                httpWebRequest.Headers.Add("Accept-Language", "en-US");
                //httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWebRequest.Headers.Add("Pragma", "no-cache");                
                httpWebRequest.Referer = "http://" + this._host + "/web-root/restricted/default.aspx?loginname=" + this._loginName + "&redirect=true";
                httpWebRequest.CookieContainer = this._cookies;
                httpWebRequest.Host = this._host;
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream);
                char[] array = new char[256];
                for (int i = streamReader.Read(array, 0, 256); i > 0; i = streamReader.Read(array, 0, 256))
                {
                    string str = new string(array, 0, i);
                    text = (text += str);
                }
                streamReader.Close();
                responseStream.Close();
                httpWebResponse.Close();
                text = text.Replace("\"", "").Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace(",", "");
                int num = text.IndexOf("Callback('") + 10;
                int num2 = text.IndexOf("');");
                result = float.Parse(text.Substring(num, num2 - num));
                _currentCredit = result;
            }
            catch
            {
                result = 0f;
            }
            return result;
        }

        public string GetBetList()
        {
            //float result = 0f;
            string returnText = string.Empty;
            try
            {
                //string text = "";
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://" + this._host + "/webroot/restricted/bet-list/running-bet-list.aspx");                

                httpWebRequest.UserAgent = this._userAgent;
                httpWebRequest.Method = "GET";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.Accept = "image/jpeg, image/gif, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, */*";
                httpWebRequest.Headers.Add("Accept-Language", "en-US");
                //httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWebRequest.Headers.Add("Pragma", "no-cache");
                httpWebRequest.Referer = "http://" + this._host + "/webroot/restricted/HomeLeft.aspx";
                httpWebRequest.CookieContainer = this._cookies;
                httpWebRequest.Host = this._host;
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream);
                returnText = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                httpWebResponse.Close();
                return returnText;
            }
            catch
            {
                return "";
            }
        }

        public void PrepareBet(string oddID,
                                string oddValue,
                                string type,
                            out string betCount,
                            out int maxStake,
                            out bool allowance,
                            out string betKindValue,
                            out string homeTeamName,
                            out string awayTeamName)
        {
            betCount = "0";
            maxStake = 0;
            allowance = false;
            betKindValue = "";
            homeTeamName = "";
            awayTeamName = "";
            try
            {
                string requestUriString = string.Concat(new string[]
				{
					"http://", 
					this._host, 
					"/webroot/restricted/plain/ticket.aspx?id=", 
					oddID, 
					"&odds=", 
					oddValue, 
					"&op=", 
					type, 
					"&hdpType=&isor=0&hidRefreshOdds=&hidAuto=&loginname=", 
					this._loginName
				});
                this._prepareBetRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUriString);
                this._prepareBetRequest.UserAgent = this._userAgent;
                this._prepareBetRequest.Method = "GET";
                this._prepareBetRequest.KeepAlive = true;
                this._prepareBetRequest.ServicePoint.Expect100Continue = false;
                this._prepareBetRequest.CookieContainer = this._cookies;
                this._prepareBetRequest.Host = this._host;
                //this._prepareBetRequest.Referer = "http://" + this._host + "/webroot/restricted/odds/main.aspx?oddsp=1,1,2,0,0,1,1,0,1";

                this._prepareBetRequest.Referer = "http://" + this._host + "/webroot/restricted/odds/main.aspx?page=2&sport=1";

                this._prepareBetRequest.Timeout = 30000;
                this._prepareBetRequest.Headers.Add("Accept-Language", "en-US");
                //this._prepareBetRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._prepareBetRequest.Headers.Add("Pragma", "no-cache");
                this._prepareBetRequest.Accept = "image/jpeg, image/gif, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, */*";
                this._prepareBetRequest.AutomaticDecompression = (System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate);
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)this._prepareBetRequest.GetResponse();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(httpWebResponse.GetResponseStream());
                string data = streamReader.ReadToEnd();

                SbobetEngine.ProcessPrepareBet(
                    data,
                    out betCount,
                    out maxStake,
                    out allowance,
                    out betKindValue,
                    out homeTeamName,
                    out awayTeamName);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public void ConfirmBet(
            string oddID,
            string oddValue,
            string type,
            string stake,
            string betCount,
            out string newOddValue,
            out bool success)
        {
            newOddValue = "";
            success = false;
            try
            {
                string requestUriString = string.Concat(new string[]
				{
					"http://", 
					this._host, 
					"/webroot/restricted/ticket/confirmbet.aspx?stake=", 
					stake, 
					"&betcount=", 
					betCount, 
					"&betpage=&loginname=", 
					this._loginName, 
					"&stakeInAuto=", 
					stake
				});
                this._confirmBetRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUriString);
                this._confirmBetRequest.UserAgent = this._userAgent;
                this._confirmBetRequest.Method = "GET";
                this._confirmBetRequest.KeepAlive = true;
                this._confirmBetRequest.ServicePoint.Expect100Continue = false;
                this._confirmBetRequest.Headers.Add("Accept-Language", "en-US");
                //this._confirmBetRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._confirmBetRequest.Headers.Add("Pragma", "no-cache");
                this._confirmBetRequest.Accept = "image/jpeg, image/gif, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, */*";

                this._confirmBetRequest.CookieContainer = this._cookies;
                this._confirmBetRequest.Host = this._host;
                this._confirmBetRequest.Referer = "http://" + this._host + "/webroot/restricted/HomeLeft.aspx";
                this._confirmBetRequest.Timeout = 30000;
                this._confirmBetRequest.AutomaticDecompression = (System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate);
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)this._confirmBetRequest.GetResponse();

                SbobetEngine.ProcessConfirmBet(new System.IO.StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd(), out newOddValue, out success);
            }
            catch (System.Exception ex)
            {
#if DEBUG
                Utilities.WriteLog.Write(ex.Message);
#endif
                newOddValue = "";
                success = false;
            }
        }
        private void On_FullDataCompleted(EngineEventArgs eventArgs)
        {
            if (this.FullDataCompleted != null)
            {
                this.FullDataCompleted(this, eventArgs);
            }
        }
        private void On_UpdateCompleted(EngineEventArgs eventArgs)
        {
            if (this.UpdateCompleted != null)
            {
                this.UpdateCompleted(this, eventArgs);
            }
        }
        public static System.Collections.Generic.List<MatchDTO> ConvertFullData(string rawData)
        {
            System.Collections.Generic.List<MatchDTO> result;
            if (rawData.ToLower() == "od_OnEmpty(0);".ToLower())
            {
                result = new System.Collections.Generic.List<MatchDTO>();
            }
            else
            {
                System.Collections.Generic.List<MatchDTO> list = null;
                try
                {
                    if (rawData == string.Empty)
                    {
                        result = list;
                        return result;
                    }
                    //rawData = rawData.Replace("<script>", "");
                    if (rawData.Contains("$M('odds-display').onUpdate"))
                    {
                        int _num = rawData.IndexOf("$M('odds-display').onUpdate") + 30;
                        int _num2 = rawData.IndexOf(";");
                        rawData = rawData.Substring(_num, _num2 - _num);
                    }
                    //rawData = "";


                    list = new System.Collections.Generic.List<MatchDTO>();

                    JavaScriptArray javaScriptArray = (JavaScriptArray)JavaScriptConvert.DeserializeObject(rawData);
                    using (System.Collections.Generic.List<object>.Enumerator enumerator = javaScriptArray.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            JavaScriptArray javaScriptArray2 = (JavaScriptArray)enumerator.Current;
                            JavaScriptArray javaScriptArray3 = (JavaScriptArray)javaScriptArray2[1];
                            if (javaScriptArray3 != null)
                            {
                                JavaScriptArray javaScriptArray4 = (JavaScriptArray)javaScriptArray3[0];
                                if (javaScriptArray4 != null)
                                {
                                    MatchDTO matchDTO = new MatchDTO();
                                    matchDTO.ID = javaScriptArray4[0].ToString();
                                    matchDTO.HomeTeamName = javaScriptArray4[1].ToString();
                                    matchDTO.AwayTeamName = javaScriptArray4[2].ToString();
                                    matchDTO.Odds = new System.Collections.Generic.List<OddDTO>();
                                    JavaScriptArray javaScriptArray5 = (JavaScriptArray)javaScriptArray2[0];
                                    if (javaScriptArray5 != null)
                                    {
                                        matchDTO.League = new LeagueDTO();
                                        matchDTO.League.ID = javaScriptArray5[0].ToString();
                                        matchDTO.League.Name = javaScriptArray5[1].ToString();
                                    }
                                    else
                                    {
                                        MatchDTO matchDTO2 = list[list.Count - 1];
                                        matchDTO.League = matchDTO2.League;
                                    }
                                    if (javaScriptArray3.Count >= 2)
                                    {
                                        JavaScriptArray javaScriptArray6 = (JavaScriptArray)javaScriptArray3[2];
                                        if (javaScriptArray6 == null || javaScriptArray6.Count <= 0)
                                        {
                                            matchDTO.IsHalfTime = true;
                                        }
                                        else
                                        {
                                            if (javaScriptArray6.Count >= 5)
                                            {
                                                if (int.Parse(javaScriptArray6[4].ToString()) == 15)
                                                {
                                                    matchDTO.IsHalfTime = true;
                                                }
                                                else
                                                {
                                                    matchDTO.IsHalfTime = false;
                                                    matchDTO.Half = int.Parse(javaScriptArray6[2].ToString());
                                                    matchDTO.Minute = int.Parse(javaScriptArray6[3].ToString());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        matchDTO.IsHalfTime = true;
                                    }
                                    JavaScriptArray javaScriptArray7 = (JavaScriptArray)javaScriptArray2[2];
                                    if (javaScriptArray7 != null)
                                    {
                                        javaScriptArray7 = (JavaScriptArray)javaScriptArray7[1];
                                        if (javaScriptArray7 != null)
                                        {
                                            using (System.Collections.Generic.List<object>.Enumerator enumerator2 = javaScriptArray7.GetEnumerator())
                                            {
                                                while (enumerator2.MoveNext())
                                                {
                                                    JavaScriptArray javaScriptArray8 = (JavaScriptArray)enumerator2.Current;
                                                    OddDTO oddDTO = new OddDTO();
                                                    if (javaScriptArray8.Count > 3)
                                                    {
                                                        int num = int.Parse(javaScriptArray8[0].ToString());
                                                        switch (num)
                                                        {
                                                            case 1:
                                                                {
                                                                    oddDTO.Type = eOddType.FulltimeHandicap;
                                                                    oddDTO.ID = javaScriptArray8[1].ToString();
                                                                    float num2;
                                                                    if (float.TryParse(javaScriptArray8[2].ToString(), out num2))
                                                                    {
                                                                        oddDTO.Home = num2;
                                                                    }
                                                                    else
                                                                    {
                                                                        oddDTO.Home = 0f;
                                                                    }
                                                                    if (float.TryParse(javaScriptArray8[3].ToString(), out num2))
                                                                    {
                                                                        oddDTO.Away = num2;
                                                                    }
                                                                    else
                                                                    {
                                                                        oddDTO.Away = 0f;
                                                                    }
                                                                    oddDTO.Odd = javaScriptArray8[4].ToString();
                                                                    if (javaScriptArray8[5] != null)
                                                                    {
                                                                        if (javaScriptArray8[5].ToString() == "1")
                                                                        {
                                                                            oddDTO.HomeFavor = true;
                                                                            oddDTO.AwayFavor = false;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (javaScriptArray8[5].ToString() == "2")
                                                                            {
                                                                                oddDTO.HomeFavor = false;
                                                                                oddDTO.AwayFavor = true;
                                                                            }
                                                                            else
                                                                            {
                                                                                oddDTO.HomeFavor = false;
                                                                                oddDTO.AwayFavor = false;
                                                                            }
                                                                        }
                                                                    }
                                                                    break;
                                                                }
                                                            case 2:
                                                                {
                                                                    break;
                                                                }
                                                            case 3:
                                                                {
                                                                    oddDTO.Type = eOddType.FulltimeOverUnder;
                                                                    oddDTO.ID = javaScriptArray8[1].ToString();
                                                                    float num2;
                                                                    if (float.TryParse(javaScriptArray8[2].ToString(), out num2))
                                                                    {
                                                                        oddDTO.Home = num2;
                                                                    }
                                                                    else
                                                                    {
                                                                        oddDTO.Home = 0f;
                                                                    }
                                                                    if (float.TryParse(javaScriptArray8[3].ToString(), out num2))
                                                                    {
                                                                        oddDTO.Away = num2;
                                                                    }
                                                                    else
                                                                    {
                                                                        oddDTO.Away = 0f;
                                                                    }
                                                                    oddDTO.Odd = javaScriptArray8[4].ToString();
                                                                    break;
                                                                }
                                                            default:
                                                                {
                                                                    switch (num)
                                                                    {
                                                                        case 7:
                                                                            {
                                                                                oddDTO.Type = eOddType.FirstHalfHandicap;
                                                                                oddDTO.ID = javaScriptArray8[1].ToString();
                                                                                float num2;
                                                                                if (float.TryParse(javaScriptArray8[2].ToString(), out num2))
                                                                                {
                                                                                    oddDTO.Home = num2;
                                                                                }
                                                                                else
                                                                                {
                                                                                    oddDTO.Home = 0f;
                                                                                }
                                                                                if (float.TryParse(javaScriptArray8[3].ToString(), out num2))
                                                                                {
                                                                                    oddDTO.Away = num2;
                                                                                }
                                                                                else
                                                                                {
                                                                                    oddDTO.Away = 0f;
                                                                                }
                                                                                oddDTO.Odd = javaScriptArray8[4].ToString();
                                                                                if (javaScriptArray8[5] != null)
                                                                                {
                                                                                    if (javaScriptArray8[5].ToString() == "1")
                                                                                    {
                                                                                        oddDTO.HomeFavor = true;
                                                                                        oddDTO.AwayFavor = false;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (javaScriptArray8[5].ToString() == "2")
                                                                                        {
                                                                                            oddDTO.HomeFavor = false;
                                                                                            oddDTO.AwayFavor = true;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            oddDTO.HomeFavor = false;
                                                                                            oddDTO.AwayFavor = false;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                break;
                                                                            }
                                                                        case 9:
                                                                            {
                                                                                oddDTO.Type = eOddType.FirstHalfOverUnder;
                                                                                oddDTO.ID = javaScriptArray8[1].ToString();
                                                                                float num2;
                                                                                if (float.TryParse(javaScriptArray8[2].ToString(), out num2))
                                                                                {
                                                                                    oddDTO.Home = num2;
                                                                                }
                                                                                else
                                                                                {
                                                                                    oddDTO.Home = 0f;
                                                                                }
                                                                                if (float.TryParse(javaScriptArray8[3].ToString(), out num2))
                                                                                {
                                                                                    oddDTO.Away = num2;
                                                                                }
                                                                                else
                                                                                {
                                                                                    oddDTO.Away = 0f;
                                                                                }
                                                                                oddDTO.Odd = javaScriptArray8[4].ToString();
                                                                                break;
                                                                            }
                                                                    }
                                                                    break;
                                                                }
                                                        }
                                                        if (oddDTO.Home != 0f)
                                                            matchDTO.Odds.Add(oddDTO);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    list.Add(matchDTO);
                                }
                            }
                            else
                            {
                                MatchDTO matchDTO = list[list.Count - 1];
                                JavaScriptArray javaScriptArray7 = (JavaScriptArray)javaScriptArray2[2];
                                if (javaScriptArray7 != null)
                                {
                                    javaScriptArray7 = (JavaScriptArray)javaScriptArray7[1];
                                    if (javaScriptArray7 != null)
                                    {
                                        using (System.Collections.Generic.List<object>.Enumerator enumerator2 = javaScriptArray7.GetEnumerator())
                                        {
                                            while (enumerator2.MoveNext())
                                            {
                                                JavaScriptArray javaScriptArray8 = (JavaScriptArray)enumerator2.Current;
                                                OddDTO oddDTO = new OddDTO();
                                                if (javaScriptArray8.Count > 3)
                                                {
                                                    int num = int.Parse(javaScriptArray8[0].ToString());
                                                    switch (num)
                                                    {
                                                        case 1:
                                                            {
                                                                oddDTO.Type = eOddType.FulltimeHandicap;
                                                                oddDTO.ID = javaScriptArray8[1].ToString();
                                                                float num2;
                                                                if (float.TryParse(javaScriptArray8[2].ToString(), out num2))
                                                                {
                                                                    oddDTO.Home = num2;
                                                                }
                                                                else
                                                                {
                                                                    oddDTO.Home = 0f;
                                                                }
                                                                if (float.TryParse(javaScriptArray8[3].ToString(), out num2))
                                                                {
                                                                    oddDTO.Away = num2;
                                                                }
                                                                else
                                                                {
                                                                    oddDTO.Away = 0f;
                                                                }
                                                                oddDTO.Odd = javaScriptArray8[4].ToString();
                                                                if (javaScriptArray8[5] != null)
                                                                {
                                                                    if (javaScriptArray8[5].ToString() == "1")
                                                                    {
                                                                        oddDTO.HomeFavor = true;
                                                                        oddDTO.AwayFavor = false;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (javaScriptArray8[5].ToString() == "2")
                                                                        {
                                                                            oddDTO.HomeFavor = false;
                                                                            oddDTO.AwayFavor = true;
                                                                        }
                                                                        else
                                                                        {
                                                                            oddDTO.HomeFavor = false;
                                                                            oddDTO.AwayFavor = false;
                                                                        }
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                        case 2:
                                                            {
                                                                break;
                                                            }
                                                        case 3:
                                                            {
                                                                oddDTO.Type = eOddType.FulltimeOverUnder;
                                                                oddDTO.ID = javaScriptArray8[1].ToString();
                                                                float num2;
                                                                if (float.TryParse(javaScriptArray8[2].ToString(), out num2))
                                                                {
                                                                    oddDTO.Home = num2;
                                                                }
                                                                else
                                                                {
                                                                    oddDTO.Home = 0f;
                                                                }
                                                                if (float.TryParse(javaScriptArray8[3].ToString(), out num2))
                                                                {
                                                                    oddDTO.Away = num2;
                                                                }
                                                                else
                                                                {
                                                                    oddDTO.Away = 0f;
                                                                }
                                                                oddDTO.Odd = javaScriptArray8[4].ToString();
                                                                break;
                                                            }
                                                        default:
                                                            {
                                                                switch (num)
                                                                {
                                                                    case 7:
                                                                        {
                                                                            oddDTO.Type = eOddType.FirstHalfHandicap;
                                                                            oddDTO.ID = javaScriptArray8[1].ToString();
                                                                            float num2;
                                                                            if (float.TryParse(javaScriptArray8[2].ToString(), out num2))
                                                                            {
                                                                                oddDTO.Home = num2;
                                                                            }
                                                                            else
                                                                            {
                                                                                oddDTO.Home = 0f;
                                                                            }
                                                                            if (float.TryParse(javaScriptArray8[3].ToString(), out num2))
                                                                            {
                                                                                oddDTO.Away = num2;
                                                                            }
                                                                            else
                                                                            {
                                                                                oddDTO.Away = 0f;
                                                                            }
                                                                            oddDTO.Odd = javaScriptArray8[4].ToString();
                                                                            if (javaScriptArray8[5] != null)
                                                                            {
                                                                                if (javaScriptArray8[5].ToString() == "1")
                                                                                {
                                                                                    oddDTO.HomeFavor = true;
                                                                                    oddDTO.AwayFavor = false;
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (javaScriptArray8[5].ToString() == "2")
                                                                                    {
                                                                                        oddDTO.HomeFavor = false;
                                                                                        oddDTO.AwayFavor = true;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        oddDTO.HomeFavor = false;
                                                                                        oddDTO.AwayFavor = false;
                                                                                    }
                                                                                }
                                                                            }
                                                                            break;
                                                                        }
                                                                    case 9:
                                                                        {
                                                                            oddDTO.Type = eOddType.FirstHalfOverUnder;
                                                                            oddDTO.ID = javaScriptArray8[1].ToString();
                                                                            float num2;
                                                                            if (float.TryParse(javaScriptArray8[2].ToString(), out num2))
                                                                            {
                                                                                oddDTO.Home = num2;
                                                                            }
                                                                            else
                                                                            {
                                                                                oddDTO.Home = 0f;
                                                                            }
                                                                            if (float.TryParse(javaScriptArray8[3].ToString(), out num2))
                                                                            {
                                                                                oddDTO.Away = num2;
                                                                            }
                                                                            else
                                                                            {
                                                                                oddDTO.Away = 0f;
                                                                            }
                                                                            oddDTO.Odd = javaScriptArray8[4].ToString();
                                                                            break;
                                                                        }
                                                                }
                                                                break;
                                                            }
                                                    }
                                                    if (oddDTO.Home != 0f)
                                                        matchDTO.Odds.Add(oddDTO);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                result = list;
            }
            return result;
        }
        

        public void Update(string _content)
        {
            this.content = _content.Replace("\\\\u200C", "").Replace("\\u200C", "").Replace("'", "");
            ArrayList arrayList = this.splitDataToArray(0);
            if (this.content.IndexOf("onIdle") >= 0)
            {
                return;
            }
            if (!this.content.Contains("$M(odds-display).onUpdate"))
            {
                throw new InvalidOperationException("Input struct of parser is not right");
            }
            int num = int.Parse(arrayList[0].ToString());
            int num2 = int.Parse(arrayList[1].ToString());
            bool flag = int.Parse(arrayList[2].ToString()) == 1;
            object obj = arrayList[3];
            object arg_BF_0 = arrayList[4];
            object arg_C7_0 = arrayList[5];
            if (num2 == 0)
            {
                if (flag || this.liveVersion < num)
                {
                    this.liveVersion = num;
                    if (obj.GetType().Equals(typeof(ArrayList)))
                    {
                        this.update(flag, (ArrayList)obj);
                        return;
                    }
                }
            }
            else
            {
                if (num2 == 1 && (flag || this.nonLiveVersion < num))
                {
                    this.nonLiveVersion = num;
                    if (obj.GetType().Equals(typeof(ArrayList)))
                    {
                        this.update(flag, (ArrayList)obj);
                    }
                }
            }            
        }
        private bool update(bool clearAll, ArrayList data)
        {
            bool result = false;
            if (clearAll)
            {
                result = true;
                this._tournamentDictionary = new Dictionary<string, ArrayList>();
                this._eventDictionary = new Dictionary<string, ArrayList>();
                this._eventResultDictionary = new Dictionary<string, ArrayList>();
                this._oddsDictionary = new Dictionary<string, ArrayList>();
                this._marketGroupDictionary = new Dictionary<string, ArrayList>();
            }
            if (data.Count >= 8)
            {
                if (data[0].GetType().Equals(typeof(ArrayList)))
                {
                    result = true;
                    this._updateElements(this._tournamentDictionary, (ArrayList)data[0]);
                }
                if (data[1].GetType().Equals(typeof(ArrayList)))
                {
                    result = true;
                    this._updateElements(this._eventDictionary, (ArrayList)data[1]);
                }
                if (data[2].GetType().Equals(typeof(ArrayList)))
                {
                    result = true;
                    this._updateEventResults(this._eventResultDictionary, (ArrayList)data[2]);
                }
                if (data[3].GetType().Equals(typeof(ArrayList)))
                {
                    result = true;
                    this._updateEventResultExtras(this._eventResultExtraDictionary, (ArrayList)data[3]);
                }
                if (data[4].GetType().Equals(typeof(ArrayList)))
                {
                    result = true;
                    this._deleteElements(this._eventResultDictionary, (ArrayList)data[4]);
                }
                if (data[5].GetType().Equals(typeof(ArrayList)))
                {
                    result = true;
                    this._updateOdds(this._oddsDictionary, (ArrayList)data[5]);
                }
                if (data[6].GetType().Equals(typeof(ArrayList)))
                {
                    result = true;
                    this._deleteElements(this._oddsDictionary, (ArrayList)data[6]);
                }
                if (data[7].GetType().Equals(typeof(ArrayList)))
                {
                    result = true;
                    this._updateElements(this._marketGroupDictionary, (ArrayList)data[7]);
                }
            }
            
            return result;
        }
        private System.Collections.Generic.List<MatchDTO> updateToDicNew()
        {
            //this.dicMatches.Clear();
            System.Collections.Generic.List<MatchDTO> result = new List<MatchDTO>();

            foreach (ArrayList current in this._eventResultDictionary.Values)
            {
                ArrayList arrayList = this._eventDictionary[current[1].ToString()];
                MatchDTO sboMatch = new MatchDTO();
                sboMatch.ID = arrayList[0].ToString();
                LeagueDTO league = new LeagueDTO();
                league.ID = arrayList[2].ToString();
                league.Name = this._tournamentDictionary[league.ID][1].ToString();
                sboMatch.League = league;
                sboMatch.HomeTeamName = arrayList[3].ToString();
                sboMatch.AwayTeamName = arrayList[4].ToString();
                result.Add(sboMatch);                
            }
            foreach (ArrayList current2 in this._oddsDictionary.Values)
            {
                string id = current2[0].ToString();
                if (current2[1].GetType().Equals(typeof(ArrayList)))
                {
                    ArrayList arrayList2 = (ArrayList)current2[1];
                    if (current2[2].GetType().Equals(typeof(ArrayList)))
                    {
                        ArrayList arrayList3 = (ArrayList)current2[2];
                        if (this._eventResultDictionary.ContainsKey(arrayList2[0].ToString()))
                        {
                            string text = this._eventResultDictionary[arrayList2[0].ToString()][1].ToString();
                            OddDTO sboOdd = new OddDTO();
                            sboOdd.ID = arrayList2[0].ToString();

                            eOddType type = this.reCal2(arrayList2[1].ToString());

                            sboOdd.Type = type;
                            
                            string text2 = this.reCal(arrayList2[1].ToString());
                                                                                    
                            if ("8731".Contains(text2))                            
                            {
                                sboOdd.Odd = arrayList2[4].ToString();
                                //sboOdd.hdp = decimal.Parse(arrayList2[4].ToString());
                                if ("71".Contains(text2))
                                {
                                    //sboOdd.hdp = -sboOdd.hdp;
                                    sboOdd.HomeFavor = true;
                                    sboOdd.IsHomeGive = true;
                                }
                                sboOdd.Home = float.Parse(arrayList3[0].ToString());
                                sboOdd.Away = float.Parse(arrayList3[1].ToString());
                                foreach (MatchDTO m in result)
                                {
                                    if (m.ID == id)
                                    {
                                        m.Odds.Add(sboOdd);
                                    }
                                }                                
                            }
                        }
                    }
                }
            }
            return result;
        }
        private void updateToDic()
        {
            this.DicMatch.Clear();
            foreach (ArrayList current in this._eventResultDictionary.Values)
            {
                ArrayList arrayList = this._eventDictionary[current[1].ToString()];
                SboMatch sboMatch = new SboMatch();
                sboMatch.matchId = arrayList[0].ToString();
                sboMatch.leagueId = arrayList[2].ToString();
                sboMatch.leagueName = this._tournamentDictionary[sboMatch.leagueId][1].ToString();
                sboMatch.home = arrayList[3].ToString();
                sboMatch.away = arrayList[4].ToString();
                sboMatch.dicOdds = new Dictionary<string, SboOdd>();
                if (!this.DicMatch.ContainsKey(sboMatch.matchId))
                {
                    this.DicMatch.Add(sboMatch.matchId, sboMatch);
                }
            }
            foreach (ArrayList current2 in this._oddsDictionary.Values)
            {
                string id = current2[0].ToString();
                if (current2[1].GetType().Equals(typeof(ArrayList)))
                {
                    ArrayList arrayList2 = (ArrayList)current2[1];
                    if (current2[2].GetType().Equals(typeof(ArrayList)))
                    {
                        ArrayList arrayList3 = (ArrayList)current2[2];
                        if (this._eventResultDictionary.ContainsKey(arrayList2[0].ToString()))
                        {
                            string text = this._eventResultDictionary[arrayList2[0].ToString()][1].ToString();
                            SboOdd sboOdd = new SboOdd();
                            sboOdd.muid = arrayList2[0].ToString();
                            sboOdd.matchId = text;
                            string text2 = this.reCal(arrayList2[1].ToString());
                            sboOdd.oddType = int.Parse(text2);
                            if ("8731".Contains(text2))
                            {
                                sboOdd.hdp = decimal.Parse(arrayList2[4].ToString());
                                if ("71".Contains(text2))
                                {
                                    sboOdd.hdp = -sboOdd.hdp;
                                }
                                sboOdd.Id = id;
                                sboOdd.home = decimal.Parse(arrayList3[0].ToString());
                                sboOdd.away = decimal.Parse(arrayList3[1].ToString());
                                if (this.DicMatch.ContainsKey(text))
                                {
                                    SboMatch sboMatch2 = this.DicMatch[text];
                                    if (!sboMatch2.dicOdds.ContainsKey(sboOdd.Key))
                                    {
                                        sboMatch2.dicOdds.Add(sboOdd.Key, sboOdd);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public string reCal(string oddTypeSbo)
        {
            switch (oddTypeSbo)
            {
                case "1":
                    return "1";
                case "2":
                    return "2";
                case "3":
                    return "3";
                case "5":
                    return "5";
                case "7":
                    return "7";
                case "8":
                    return "15";
                case "9":
                    return "8";
                case "12":
                    return "12";
            }
            return oddTypeSbo;
        }
        public eOddType reCal2(string oddTypeSbo)
        {
            switch (oddTypeSbo)
            {
                case "1":
                    return eOddType.FulltimeHandicap;
                case "2":
                    return eOddType.Unknown;
                case "3":
                    return eOddType.FulltimeOverUnder;
                case "5":
                    return eOddType.Unknown;
                case "7":
                    return eOddType.FirstHalfHandicap;
                case "8":
                    return eOddType.Unknown;
                case "9":
                    return eOddType.FirstHalfOverUnder;
                case "12":
                    return eOddType.Unknown;
            }
            return eOddType.Unknown;
        }
        private void _updateOdds(Dictionary<string, ArrayList> oddsDictionary, ArrayList oddsArray)
        {
            int count = oddsArray.Count;
            for (int i = 0; i < count; i++)
            {
                if (oddsArray[i].GetType().Equals(typeof(ArrayList)))
                {
                    ArrayList arrayList = (ArrayList)oddsArray[i];
                    string key = arrayList[0].ToString();
                    if (oddsDictionary.ContainsKey(key))
                    {
                        ArrayList oldElement = oddsDictionary[key];
                        arrayList = this.updateArray(oldElement, arrayList);
                    }
                    oddsDictionary[key] = arrayList;
                }
            }
        }
        private void _deleteElements(Dictionary<string, ArrayList> elementDictionary, ArrayList elementIdArray)
        {
            int count = elementIdArray.Count;
            for (int i = 0; i < count; i++)
            {
                string text = (string)elementIdArray[i];
                if (!string.IsNullOrEmpty(text) && elementDictionary.ContainsKey(text))
                {
                    elementDictionary.Remove(text);
                }
            }
        }
        private void _updateEventResultExtras(Dictionary<string, ArrayList> eventResultExtraDictionary, ArrayList eventResultExtraArray)
        {
            int count = eventResultExtraArray.Count;
            for (int i = 0; i < count; i++)
            {
                if (eventResultExtraArray[i].GetType().Equals(typeof(ArrayList)))
                {
                    ArrayList arrayList = (ArrayList)eventResultExtraArray[i];
                    if (arrayList[0].GetType().Equals(typeof(string)))
                    {
                        string key = arrayList[0].ToString();
                        if (eventResultExtraDictionary.ContainsKey(key))
                        {
                            ArrayList oldElement = eventResultExtraDictionary[key];
                            arrayList = this.updateArray(oldElement, arrayList);
                        }
                        eventResultExtraDictionary[key] = arrayList;
                    }
                }
            }
        }
        private void _updateEventResults(Dictionary<string, ArrayList> eventResultDictionary, ArrayList eventResultArray)
        {
            int count = eventResultArray.Count;
            for (int i = 0; i < count; i++)
            {
                if (eventResultArray[i].GetType().Equals(typeof(ArrayList)))
                {
                    ArrayList arrayList = (ArrayList)eventResultArray[i];
                    if (arrayList[0].GetType().Equals(typeof(string)) && arrayList[2].GetType().Equals(typeof(string)) && !(arrayList[2].ToString() != "0"))
                    {
                        string key = arrayList[0].ToString();
                        if (eventResultDictionary.ContainsKey(key))
                        {
                            ArrayList arrayList2 = eventResultDictionary[key];
                            arrayList = this.updateArray(arrayList2, arrayList);
                            if (arrayList[3] == arrayList2[3])
                            {
                                object arg_DA_0 = arrayList[4];
                                object arg_D9_0 = arrayList2[4];
                            }
                        }
                        eventResultDictionary[key] = arrayList;
                    }
                }
            }
        }
        private bool _updateElements(Dictionary<string, ArrayList> elementDictionary, ArrayList elementArray)
        {
            if (elementArray.Count == 0)
            {
                return false;
            }
            int count = elementArray.Count;
            for (int i = 0; i < count; i++)
            {
                if (elementArray[i].GetType().Equals(typeof(ArrayList)))
                {
                    ArrayList arrayList = (ArrayList)elementArray[i];
                    string key = arrayList[0].ToString();
                    if (elementDictionary.ContainsKey(key))
                    {
                        ArrayList oldElement = elementDictionary[key];
                        arrayList = this.updateArray(oldElement, arrayList);
                    }
                    elementDictionary[key] = arrayList;
                }
            }
            return true;
        }
        private ArrayList updateArray(ArrayList oldElement, ArrayList element)
        {
            if (element.Count == 0)
            {
                return oldElement;
            }
            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < element.Count; i++)
            {
                if (element[i].GetType().Equals(typeof(ArrayList)))
                {
                    if (oldElement[i].GetType().Equals(typeof(ArrayList)))
                    {
                        arrayList.Add(this.updateArray((ArrayList)oldElement[i], (ArrayList)element[i]));
                    }
                    else
                    {
                        arrayList.Add((ArrayList)element[i]);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(element[i].ToString()))
                    {
                        arrayList.Add(element[i]);
                    }
                    else
                    {
                        if (i < oldElement.Count)
                        {
                            arrayList.Add(oldElement[i]);
                        }
                        else
                        {
                            arrayList.Add(string.Empty);
                        }
                    }
                }
            }
            for (int j = element.Count; j < oldElement.Count; j++)
            {
                arrayList.Add(oldElement[j]);
            }
            return arrayList;
        }
        private static void ProcessPrepareBet(
            string data,
            out string betCount,
            out int maxStake,
            out bool allowance,
            out string betKindValue,
            out string homeTeamName,
            out string awayTeamName)
        {
            betCount = "0";
            maxStake = 0;
            allowance = false;
            betKindValue = "";
            homeTeamName = "";
            awayTeamName = "";
            if (data != null && !(data == string.Empty))
            {
                try
                {
                    System.Collections.Generic.List<string> list = data.Split(new string[]
					{
						";"
					}, System.StringSplitOptions.None).ToList<string>();
                    if (list.Count == 3)
                    {
                        list = list[0].Split(new string[]
						{
							"="
						}, System.StringSplitOptions.None).ToList<string>();
                        if (list.Count == 2)
                        {
                            JavaScriptArray javaScriptArray = (JavaScriptArray)JavaScriptConvert.DeserializeObject(list[1]);
                            if (javaScriptArray[9] == null || javaScriptArray[9].ToString() == string.Empty)
                            {
                                betCount = javaScriptArray[18].ToString();
                                maxStake = int.Parse(javaScriptArray[12].ToString());
                                allowance = true;
                                betKindValue = javaScriptArray[5].ToString().Split(new string[]
								{
									"@"
								}, System.StringSplitOptions.None)[0].Split(new string[]
								{
									":"
								}, System.StringSplitOptions.None)[1];
                                homeTeamName = javaScriptArray[2].ToString().Replace("'", "");
                                awayTeamName = javaScriptArray[3].ToString().Replace("'", "");
                            }
                            else
                            {
                                betCount = javaScriptArray[18].ToString();
                                maxStake = int.Parse(javaScriptArray[12].ToString());
                                betKindValue = javaScriptArray[5].ToString().Split(new string[]
								{
									"@"
								}, System.StringSplitOptions.None)[0].Split(new string[]
								{
									":"
								}, System.StringSplitOptions.None)[1];
                                allowance = false;
                                homeTeamName = javaScriptArray[2].ToString().Replace("'", "");
                                awayTeamName = javaScriptArray[3].ToString().Replace("'", "");
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    betCount = "0";
                    maxStake = 0;
                    allowance = false;
                    betKindValue = "";
                    homeTeamName = "";
                    awayTeamName = "";
                    throw ex;
                }
            }
        }
        private static void ProcessConfirmBet(string data, out string newOddValue, out bool success)
        {
            newOddValue = "";
            success = false;
            if (data != null && !(data == string.Empty))
            {
                if (data.ToLower().Contains("this.location='about:blank'"))
                {
                    success = true;
                }
                else
                {
                    if (data.ToLower().Contains("<script>parent.showPBResult(6);</script>".ToLower()))
                    {
                        success = false;
                    }
                    else
                    {
                        try
                        {
                            JavaScriptArray javaScriptArray = (JavaScriptArray)JavaScriptConvert.DeserializeObject(data.Split(new string[]
							{
								"="
							}, System.StringSplitOptions.None)[1].Split(new string[]
							{
								";"
							}, System.StringSplitOptions.None)[0]);
                            newOddValue = javaScriptArray[8].ToString();
                            success = false;
                        }
                        catch
                        {
                            newOddValue = "";
                            success = false;
                        }
                    }
                }
            }
        }
        private ArrayList splitDataToArray(int idx)
        {
            List<ArrayList> list = new List<ArrayList>();
            ArrayList arrayList = new ArrayList();
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            bool flag = false;
            string text = this.content;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                char c2 = c;
                if (c2 != ',')
                {
                    switch (c2)
                    {
                        case '[':
                            num++;
                            stringBuilder = new StringBuilder();
                            list.Add(arrayList);
                            arrayList = new ArrayList();
                            flag = true;
                            goto IL_EA;
                        case ']':
                            num--;
                            if (flag)
                            {
                                arrayList.Add(stringBuilder.ToString());
                            }
                            list[list.Count - 1].Add(arrayList);
                            arrayList = list[list.Count - 1];
                            list.RemoveAt(list.Count - 1);
                            stringBuilder = new StringBuilder();
                            flag = false;
                            goto IL_EA;
                    }
                    stringBuilder.Append(c);
                }
                else
                {
                    if (flag)
                    {
                        arrayList.Add(stringBuilder.ToString());
                    }
                    stringBuilder = new StringBuilder();
                    flag = true;
                }
            IL_EA: ;
            }
            if (arrayList[0].GetType().Equals(typeof(ArrayList)))
            {
                return (ArrayList)arrayList[0];
            }
            return new ArrayList();
        }
    }

}
