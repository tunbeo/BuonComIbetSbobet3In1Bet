//#define DEBUG
//#define DDEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Windows.Forms;
using iBet.DTO;
namespace iBet.Engine
{
    public class ThreeIn1BetEngine : BaseEngine
    {
        private System.Net.CookieContainer _cookies;
        private string _userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Win64; x64; Trident/4.0; .NET CLR 2.0.50727; SLCC2; .NET4.0C; .NET4.0E)";
        private string _host;
        private string _accountID;        
        private string _lastDataChangeTime;
        public string _secretNumber;
        public float _currentCredit = 0f;
        //public string _lastLoadedDataTime;
        private System.Collections.Generic.List<MatchDTO> _lastListMatch;
        private System.Collections.Generic.Dictionary<string, string> _mappingOddwithMatch;
        private Timer _updateDataTimer;
        private System.Net.HttpWebRequest _fullDataRequest;
        private System.Net.HttpWebRequest _updateDataRequest;
        private System.Net.HttpWebRequest _betPrepareRequest;
        private System.Net.HttpWebRequest _betConfirmRequest;
        private int _updateDataInterval = 15000;
        public event EngineDelegate FullDataCompleted;
        public event EngineDelegate UpdateCompleted;
        public event EngineDelegate BetPrepareCompleted;
        public event EngineDelegate BetConfirmCompleted;
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

        public ThreeIn1BetEngine(
            string host, 
            string accountID,             
            System.Net.CookieContainer cookies)
		{
			this._host = host;
			this._accountID = accountID;			
			this._cookies = cookies;
			this.InitializeObjects();
		}
		public void Start()
		{
			this.GetFullData();
			this._updateDataTimer.Start();
		}
		public void Stop()
		{
			this._updateDataTimer.Interval = this._updateDataInterval;
			this._updateDataTimer.Stop();
		}
        
        private void InitializeObjects()
        {
            this._updateDataTimer = new Timer();
            this._updateDataTimer.Interval = this._updateDataInterval;
            this._updateDataTimer.Tick += new System.EventHandler(this._updateDataTimer_Tick);
        }
        private void _updateDataTimer_Tick(object sender, System.EventArgs e)
        {
            if (this.LastUpdateCompleted)
            {
                this.GetUpdateData();
            }
        }

        
        private void GetFullData()
        {
            this.LastUpdateCompleted = false;
            try
            {
                string requestUriString = string.Concat(new string[]
				{
					"http://", 
					this._host, 
					"/Member/BetsView/BetLight/DataOdds.ashx"
				});
                this._fullDataRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUriString);
                this._fullDataRequest.CookieContainer = this._cookies;
                this._fullDataRequest.UserAgent = this._userAgent;
                this._fullDataRequest.Method = "POST";
                this._fullDataRequest.Host = this._host;
                this._fullDataRequest.Accept = "application/json, text/javascript, */*";
                this._fullDataRequest.Referer = "http://" + this._host + "/Member/BetOdds/HdpDouble.aspx?v=1&m1=Today&sports=S_";
                
                string textPost = string.Concat(new string[]
				{
                    "fc=1&m_accType=MY&SystemLanguage=en-US&TimeFilter=0&m_gameType=S_&m_SortByTime=0&m_LeagueList=&SingleDouble=single&clientTime=&c=A&fav="
                });
                this._fullDataRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._fullDataRequest.Headers.Add("Accept-Language", "en-us,en;q=0.5");
                this._fullDataRequest.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
                this._fullDataRequest.Headers.Add("Pragma", "no-cache");
                this._fullDataRequest.Headers.Add("Cache-Control", "no-cache");

                this._fullDataRequest.ContentLength = (long)textPost.Length;
                this._fullDataRequest.KeepAlive = false;
                this._fullDataRequest.ProtocolVersion = HttpVersion.Version11;
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(textPost);
                this._fullDataRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)this._fullDataRequest.GetResponse();
                
                System.IO.Compression.GZipStream gzipReader = new System.IO.Compression.GZipStream(httpWebResponse.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                System.IO.StreamReader streamReader = new System.IO.StreamReader(gzipReader);

                string text = streamReader.ReadToEnd();
                streamReader.Close();
                gzipReader.Close();
                httpWebResponse.Close();
                //iBet.Utilities.WriteLog.Write("trying to parse full data:" + text);
                if (text == null || text == string.Empty)
                {
                    this.On_FullDataCompleted(new EngineEventArgs
                    {
                        Type = eEngineEventType.SessionExpired,
                        Data = null
                    });
#if DEBUG
                    iBet.Utilities.WriteLog.Write("3in1: Error 003: err while get first data from 3in1bet");
#endif
                }
                else
                {
#if DEBUG
                    iBet.Utilities.WriteLog.Write("3in1~FULLDATA~: Loaded");
#endif
#if DDEBUG
                    
                    iBet.Utilities.WriteLog.Write(text);
#endif
                    try
                    {
                        JavaScriptObject javaScriptObj = (JavaScriptObject)JavaScriptConvert.DeserializeObject(text);
                        if (javaScriptObj != null)
                        {
                            if (javaScriptObj["data"] != null)
                            {
                                this._lastListMatch = ThreeIn1BetEngine.ConvertFullData(text, out this._lastDataChangeTime, out this._mappingOddwithMatch);
                                this.On_FullDataCompleted(new EngineEventArgs
                                {
                                    Type = eEngineEventType.Success,
                                    Data = this._lastListMatch
                                });
                                //JavaScriptArray objLiveMatch = (JavaScriptArray)javaScriptObj["data"];
                            }
                            else
                            {
                                this.On_FullDataCompleted(new EngineEventArgs
                                {
                                    Type = eEngineEventType.Error,
                                    Data = null
                                }); 
                            }
                        }                        
                    }
                    catch (System.Exception data)
                    {
                        this.On_FullDataCompleted(new EngineEventArgs
                        {
                            Type = eEngineEventType.Error,
                            Data = data
                        });
                    }
                }
            }
            
            catch (System.Exception data3)
            {
#if DEBUG
                iBet.Utilities.WriteLog.Write("3in1: Error 001: couldn't get first packet data of 3in1Bet, retrying now.." + data3.Message);
#endif
                this.GetFullData();

            }
            finally
            {
                this.LastUpdateCompleted = true;
            }

        }
        private void GetUpdateData()
        {
            this.LastUpdateCompleted = false;
            int timeOut5 = 0;
            try
            {
                string requestUriString = string.Concat(new string[]
				{
					"http://", 
					this._host, 
					"/Member/BetsView/BetLight/DataOdds.ashx"
				});

                this._updateDataRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUriString);
                this._updateDataRequest.CookieContainer = this._cookies;                
                this._updateDataRequest.UserAgent = this._userAgent;
                this._updateDataRequest.Method = "POST";
                this._updateDataRequest.Host = this._host;
                this._updateDataRequest.Accept = "application/json, text/javascript, */*";
                this._updateDataRequest.Referer = "http://" + this._host + "/Member/BetOdds/HdpDouble.aspx?v=1&m1=Today&sports=S_";
                //                                  http://mem21.3in1bet.com/Member/BetOdds/HdpDouble.aspx?v=1&m1=Today&sports=S_
                string textPost = string.Concat(new string[]
				{
                   //fc=5&m_accType=MY&SystemLanguage=en-US&TimeFilter=0&m_gameType=S_&m_SortByTime=0&m_LeagueList=&SingleDouble=single&clientTime=09/05/2011 14:50:16.8004951&c=A&fav=&exlist=0                   
                    "fc=5&m_accType=MY&SystemLanguage=en-US&TimeFilter=0&m_gameType=S_&m_SortByTime=0&m_LeagueList=&SingleDouble=single&clientTime=",
                    this._lastDataChangeTime,
                    "&c=A&fav=&exlist=0"
                });

                this._updateDataRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._updateDataRequest.Headers.Add("Accept-Language", "en-us,en;q=0.5");
                this._updateDataRequest.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
                this._updateDataRequest.Headers.Add("Pragma", "no-cache");
                this._updateDataRequest.Headers.Add("Cache-Control", "no-cache");

                this._updateDataRequest.ContentLength = (long)textPost.Length;
                this._updateDataRequest.KeepAlive = false;
                this._updateDataRequest.ProtocolVersion = HttpVersion.Version11;
                this._updateDataRequest.Timeout = 20000;

                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(textPost);
                Stream postStream = this._updateDataRequest.GetRequestStream();
                postStream.Write(bytes, 0, bytes.Length);
                postStream.Close(); // post done!
                HttpWebResponse httpWebResponse = (HttpWebResponse)this._updateDataRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
                {
                    responseStream = new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                }
                else if (httpWebResponse.ContentEncoding.ToLower().Contains("deflate"))
                {
                    responseStream = new DeflateStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                }
                //GZipStream gzipReader = new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                StreamReader streamReader = new StreamReader(responseStream, Encoding.Default);
                
                string text = streamReader.ReadToEnd();
                
                streamReader.Close();
                responseStream.Close();
                httpWebResponse.Close();

                if (text == null || text == string.Empty)
                {
                    this.On_UpdateCompleted(new EngineEventArgs
                    {
                        Type = eEngineEventType.SessionExpired,
                        Data = null
                    });
                }
                else
                {
                    try
                    {
                        this._lastListMatch = ThreeIn1BetEngine.ConvertUpdateData(text, 
                            this._lastListMatch, 
                            out this._lastDataChangeTime, 
                            this._mappingOddwithMatch,
                            out this._mappingOddwithMatch);
                        this.On_UpdateCompleted(new EngineEventArgs
                        {
                            Type = eEngineEventType.Success,
                            Data = this._lastListMatch
                        });
                        timeOut5 = 0;
#if DEBUG
                        iBet.Utilities.WriteLog.Write("3in1: Read update completed.");
                        //iBet.Utilities.WriteLog.Write(text);
#endif

                    }
                    catch (System.Exception data)
                    {
                        this.On_UpdateCompleted(new EngineEventArgs
                        {
                            Type = eEngineEventType.SessionExpired,
                            Data = data
                        });
#if DEBUG
                        iBet.Utilities.WriteLog.Write("3in1: Error 002: can not convert update" + data);
#endif
                    }
                }
            }
            catch (System.Exception data)
            {
#if DEBUG
                iBet.Utilities.WriteLog.Write("3in1: Error 004: " + data.Message);
#endif                
                timeOut5++;
                if (timeOut5 < 4)
                {
                    this.GetUpdateData();
                }
                else
                {
                    this.Stop();
                    this.On_UpdateCompleted(new EngineEventArgs
                    {
                        Type = eEngineEventType.SessionExpired,
                        Data = data
                    });
#if DEBUG
                    iBet.Utilities.WriteLog.Write("3in1: Error 999: SERVER RESPONSE ERROR:" + data);
#endif
                }
            }

            finally {
                this.LastUpdateCompleted = true;
            }
        }

        public float GetCurrentCredit()
        {
            float result = 0f;
            try {
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://"
                    + this._host
                    + "/Member/BetsView/UserInfoPanelHost.aspx?Ajax=1");
                httpWebRequest.CookieContainer = this._cookies;
                httpWebRequest.UserAgent = this._userAgent;
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Referer = "http://" + this._host + "/Main/AdminLeftFrame.aspx";
                httpWebRequest.Host = this._host;
                string text = "{}";
                httpWebRequest.ContentLength = (long)text.Length;
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(text);
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(httpWebResponse.GetResponseStream());

                string textResponse = streamReader.ReadToEnd();
#if DEBUG
                iBet.Utilities.WriteLog.Write("3in1: From get current credit text response: " + textResponse);
#endif
                JavaScriptObject javaScriptObj = (JavaScriptObject)JavaScriptConvert.DeserializeObject(textResponse);
                if (javaScriptObj != null)
                {
                    result = float.Parse(javaScriptObj["Bet_credit"].ToString());
                    this._currentCredit = result;
                }
#if DEBUG
                //iBet.Utilities.WriteLog.Write(" text response: " + textResponse);
#endif
                return result;
                
            }

            catch
            {
                result = 0f;
            }
            return result;
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

        private void On_BetPrepareCompleted(EngineEventArgs eventArgs)
        {
            if (this.BetPrepareCompleted != null)
            {
                this.BetPrepareCompleted(this, eventArgs);
            }
        }
        private void On_BetConfirmCompleted(EngineEventArgs eventArgs)
        {
            if (this.BetConfirmCompleted != null)
            {
                this.BetConfirmCompleted(this, eventArgs);
            }
        }

        protected static int ConvertSoccerTime(string time) 
        {            
            System.Collections.Generic.List<string> listTime = time.Split(new string[]
			{
				"H"
			}, System.StringSplitOptions.None).ToList<string>();
            if (listTime[1].Contains("+"))
                return 45;
            else
                return Int32.Parse(listTime[1]);
        }        
        
        public static System.Collections.Generic.List<MatchDTO> ConvertFullData(string data, out string updateTime, out Dictionary<string, string> mappingList3in1)
        { 
            System.Collections.Generic.List<MatchDTO> list = null;
            System.Collections.Generic.Dictionary<string, string> map = new Dictionary<string,string>();
            updateTime = string.Empty;
            if (data != string.Empty)
            {
                list = new System.Collections.Generic.List<MatchDTO>();

                JavaScriptObject javaScriptObj = (JavaScriptObject)JavaScriptConvert.DeserializeObject(data);
                if (javaScriptObj != null)
                {
                    string timeUpdate = javaScriptObj["t"].ToString();
                    updateTime = timeUpdate;
#if DEBUG
                    Utilities.WriteLog.Write("3in1: time after get full data:" + timeUpdate);
#endif
                    if (javaScriptObj["data"] != null)
                    {
                        string matchID = string.Empty;

                        JavaScriptArray objLiveMatch = (JavaScriptArray)javaScriptObj["data"];
                        objLiveMatch.RemoveAt(objLiveMatch.Count - 1);
#if DEBUG
                        Utilities.WriteLog.Write("3in1: Number of objects in first get data:" + objLiveMatch.Count.ToString()
                            + ". After remove the last item from [\"data\"]:" + (objLiveMatch.Count - 1).ToString());
#endif
                        using (System.Collections.Generic.List<object>.Enumerator enumerator = objLiveMatch.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {

                                JavaScriptArray objCurrentMatchProcess = (JavaScriptArray)enumerator.Current;
                                string string38 = objCurrentMatchProcess[38].ToString(); // HomeTeamName
                                string string53 = objCurrentMatchProcess[53].ToString(); //Time
                                if (!string38.ToLower().Contains("corner") && !string38.ToLower().Contains(":") && !string38.ToLower().Contains("(et)") && !string38.ToLower().Contains("(pen)"))
                                {
                                    if (objCurrentMatchProcess[34].ToString() != matchID)
                                    {
                                        MatchDTO matchDTO = new MatchDTO();
                                        matchDTO.ID = objCurrentMatchProcess[34].ToString();
                                        matchDTO.HomeTeamName = string38;
                                        matchDTO.AwayTeamName = objCurrentMatchProcess[39].ToString();
                                        matchDTO.League = new LeagueDTO();
                                        matchDTO.League.ID = objCurrentMatchProcess[3].ToString();
                                        matchDTO.League.Name = objCurrentMatchProcess[37].ToString();
                                        matchDTO.HomeScore = objCurrentMatchProcess[7].ToString();
                                        matchDTO.AwayScore = objCurrentMatchProcess[8].ToString();

                                        if (string53 == "HT")
                                        {
                                            matchDTO.Minute = 0;
                                            matchDTO.Half = 0;
                                            matchDTO.IsHalfTime = false;
                                        }
                                        else if (string53.ToLower().Contains("live"))
                                        {
                                            matchDTO.Minute = 0;
                                            matchDTO.Half = 0;
                                            matchDTO.IsHalfTime = false;
                                        }
                                        else if (string53.StartsWith("1H"))
                                        {
                                            matchDTO.Minute = ConvertSoccerTime(string53);
                                            matchDTO.Half = 1;
                                            matchDTO.IsHalfTime = true;
                                        }
                                        else if (string53.StartsWith("2H"))
                                        {
                                            matchDTO.Minute = ConvertSoccerTime(string53);
                                            matchDTO.Half = 2;
                                            matchDTO.IsHalfTime = false;
                                        }
                                        else
                                        {
                                            matchDTO.Minute = 0;
                                            matchDTO.Half = 0;
                                            matchDTO.IsHalfTime = false;
                                        }
                                            

                                        #region add odds
                                        matchDTO.Odds = new System.Collections.Generic.List<OddDTO>();//add Odds
                                        string string24 = objCurrentMatchProcess[24].ToString();
                                        string string9 = objCurrentMatchProcess[9].ToString();
                                        string string13 = objCurrentMatchProcess[13].ToString();
                                        string string0 = objCurrentMatchProcess[0].ToString();

                                        float num = 0f;
                                        if (string9 != null && string9 != "-999")
                                        {
                                            OddDTO oddDTO = new OddDTO();
                                            oddDTO.Type = eOddType.FulltimeHandicap;//FULL TIME !!!!!!!!!
                                            oddDTO.ID = string0 + "^";
                                            oddDTO.Odd = string9.Replace("/", "-");
                                            if (objCurrentMatchProcess[40].ToString() != "-999" && float.TryParse(objCurrentMatchProcess[40].ToString(), out num))
                                            {
                                                oddDTO.Home = num;
                                            }
                                            else
                                            {
                                                oddDTO.Home = 0f;
                                            }
                                            if (objCurrentMatchProcess[41].ToString() != "-999" && float.TryParse(objCurrentMatchProcess[41].ToString(), out num))
                                            {
                                                oddDTO.Away = num;
                                            }
                                            else
                                            {
                                                oddDTO.Away = 0f;
                                            }                                            

                                            if (string24 == "1" && string9 == "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }
                                            else if (string24 == "1" && string9 != "0")
                                            {
                                                oddDTO.HomeFavor = true;
                                                oddDTO.AwayFavor = false;

                                            }
                                            else if (string24 == "0" && string9 == "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }
                                            else if (string24 == "0" && string9 != "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = true;
                                            }
                                            else
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }

                                            oddDTO.IsHomeGive = bool.Parse(string24.Replace("1","true").Replace("0","false"));
                                            matchDTO.Odds.Add(oddDTO);
                                            if (!map.ContainsKey(oddDTO.ID))
                                                map.Add(oddDTO.ID, matchDTO.ID);
#if DDEBUG
                                            Utilities.WriteLog.Write("~o~3in1bet:" +
                                                matchDTO.HomeTeamName + "["
                                                + oddDTO.HomeFavor.ToString() + "]"
                                                + " - "
                                                + matchDTO.AwayTeamName + "["
                                                + oddDTO.AwayFavor.ToString() + "]"
                                                + " >> add odd:"
                                                + oddDTO.Type.ToString()
                                                + ": "
                                                + oddDTO.Odd
                                                + " Price Home:"
                                                + oddDTO.Home.ToString()
                                                + ", Away:"
                                                + oddDTO.Away.ToString()
                                                + ", ID: "
                                                + oddDTO.ID
                                                );
#endif
                                        }
                                        if (objCurrentMatchProcess[11] != null
                                            && objCurrentMatchProcess[11].ToString() != "-999")
                                        {
                                            OddDTO oddDTO = new OddDTO();
                                            oddDTO.Type = eOddType.FulltimeOverUnder;// FULL TIME OVER/UNDER
                                            oddDTO.ID = objCurrentMatchProcess[0].ToString();
                                            oddDTO.Odd = objCurrentMatchProcess[11].ToString().Replace("/", "-");
                                            if (objCurrentMatchProcess[42].ToString() != "-999" &&  float.TryParse(objCurrentMatchProcess[42].ToString(), out num))
                                            {
                                                oddDTO.Home = num;
                                            }
                                            else
                                            {
                                                oddDTO.Home = 0f;
                                            }
                                            if (objCurrentMatchProcess[43].ToString() != "-999" &&  float.TryParse(objCurrentMatchProcess[43].ToString(), out num))
                                            {
                                                oddDTO.Away = num;
                                            }
                                            else
                                            {
                                                oddDTO.Away = 0f;
                                            }

                                            oddDTO.IsHomeGive = bool.Parse(string24.Replace("1","true").Replace("0","false"));
                                            matchDTO.Odds.Add(oddDTO);
                                            if (!map.ContainsKey(oddDTO.ID))
                                                map.Add(oddDTO.ID, matchDTO.ID);
#if DDEBUG
                                            Utilities.WriteLog.Write("~o~:3in1:" +
                                                matchDTO.HomeTeamName + "["
                                                + oddDTO.HomeFavor.ToString() + "]"
                                                + " - "
                                                + matchDTO.AwayTeamName + "["
                                                + oddDTO.AwayFavor.ToString() + "]"
                                                + " >> add odd:"
                                                + oddDTO.Type.ToString()
                                                + ". "
                                                + oddDTO.Odd
                                                + " Price Home:"
                                                + oddDTO.Home.ToString()
                                                + ", Away:"
                                                + oddDTO.Away.ToString()
                                                + ", ID: "
                                                + oddDTO.ID
                                                );
#endif
                                        }
                                        if (objCurrentMatchProcess[13] != null
                                            && objCurrentMatchProcess[13].ToString() != "-999")
                                        {
                                            OddDTO oddDTO = new OddDTO();
                                            oddDTO.Type = eOddType.FirstHalfHandicap; // FIRST HALF HANDICAP
                                            oddDTO.ID = objCurrentMatchProcess[2].ToString() + "^";
                                            oddDTO.Odd = objCurrentMatchProcess[13].ToString().Replace("/", "-");
                                            if (objCurrentMatchProcess[44].ToString() != "-999" &&  float.TryParse(objCurrentMatchProcess[44].ToString(), out num))
                                            {
                                                oddDTO.Home = num;
                                            }
                                            else
                                            {
                                                oddDTO.Home = 0f;
                                            }
                                            if (objCurrentMatchProcess[45].ToString() != "-999" &&  float.TryParse(objCurrentMatchProcess[45].ToString(), out num))
                                            {
                                                oddDTO.Away = num;
                                            }
                                            else
                                            {
                                                oddDTO.Away = 0f;
                                            }
                                            if (string24 == "1" && string13 == "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }
                                            else if (string24 == "1" && string13 != "0")
                                            {
                                                oddDTO.HomeFavor = true;
                                                oddDTO.AwayFavor = false;

                                            }
                                            else if (string24 == "0" && string13 == "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }
                                            else if (string24 == "0" && string13 != "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = true;
                                            }
                                            else
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }

                                            oddDTO.IsHomeGive = bool.Parse(string24.Replace("1","true").Replace("0","false"));
                                            matchDTO.Odds.Add(oddDTO);
                                            if (!map.ContainsKey(oddDTO.ID))
                                                map.Add(oddDTO.ID, matchDTO.ID);
#if DDEBUG
                                            Utilities.WriteLog.Write("~o~:3in1:" +
                                                matchDTO.HomeTeamName + "["
                                                + oddDTO.HomeFavor.ToString() + "]"
                                                + " - "
                                                + matchDTO.AwayTeamName + "["
                                                + oddDTO.AwayFavor.ToString() + "]"
                                                + " >> add odd:"
                                                + oddDTO.Type.ToString()
                                                + ". "
                                                + oddDTO.Odd
                                                + " Price Home:"
                                                + oddDTO.Home.ToString()
                                                + ", Away:"
                                                + oddDTO.Away.ToString()
                                                + ", ID: "
                                                + oddDTO.ID
                                                );
#endif
                                        }
                                        if (objCurrentMatchProcess[15] != null
                                            && objCurrentMatchProcess[15].ToString() != "-999")
                                        {
                                            OddDTO oddDTO = new OddDTO();
                                            oddDTO.Type = eOddType.FirstHalfOverUnder; // FIRST HAFL OVER UNDER
                                            oddDTO.ID = objCurrentMatchProcess[2].ToString();
                                            oddDTO.Odd = objCurrentMatchProcess[15].ToString().Replace("/", "-");
                                            if (objCurrentMatchProcess[46].ToString() != "-999" &&  float.TryParse(objCurrentMatchProcess[46].ToString(), out num))
                                            {
                                                oddDTO.Home = num;
                                            }
                                            else
                                            {
                                                oddDTO.Home = 0f;
                                            }
                                            if (objCurrentMatchProcess[47].ToString() != "-999" && float.TryParse(objCurrentMatchProcess[47].ToString(), out num))
                                            {
                                                oddDTO.Away = num;
                                            }
                                            else
                                            {
                                                oddDTO.Away = 0f;
                                            }
                                            oddDTO.IsHomeGive = bool.Parse(string24.Replace("1","true").Replace("0","false"));
                                            matchDTO.Odds.Add(oddDTO);
                                            if (!map.ContainsKey(oddDTO.ID))
                                                map.Add(oddDTO.ID, matchDTO.ID);
#if DDEBUG
                                            Utilities.WriteLog.Write("~o~:3in1:" +
                                                matchDTO.HomeTeamName + "["
                                                + oddDTO.HomeFavor.ToString() + "]"
                                                + " - "
                                                + matchDTO.AwayTeamName + "["
                                                + oddDTO.AwayFavor.ToString() + "]"
                                                + " >> add odd:"
                                                + oddDTO.Type.ToString()
                                                + ". "
                                                + oddDTO.Odd
                                                + " Price Home:"
                                                + oddDTO.Home.ToString()
                                                + ", Away:"
                                                + oddDTO.Away.ToString()
                                                + ", ID: "
                                                + oddDTO.ID
                                                );
#endif
                                        }
                                        #endregion

                                        matchID = matchDTO.ID;
                                        list.Add(matchDTO);
#if DDEBUG
                                        Utilities.WriteLog.Write("~M~:3in1: Added match: "
                                            + matchDTO.HomeTeamName
                                            + " - " + matchDTO.AwayTeamName
                                            + "with number of odds: "
                                            + matchDTO.OddCount
                                            + ", ID: "
                                            + matchDTO.ID);                                        
#endif
                                    }
                                    else
                                    {
                                        //neu da co matchID thi lay tran dau vua add o tren
                                        MatchDTO matchDTO = list[list.Count - 1];
                                        string string24 = objCurrentMatchProcess[24].ToString();
                                        string string9 = objCurrentMatchProcess[9].ToString();
                                        string string0 = objCurrentMatchProcess[0].ToString();
                                        string string13 = objCurrentMatchProcess[13].ToString();

                                        #region add odds
                                        //matchDTO.Odds = new System.Collections.Generic.List<OddDTO>();//add Odds
                                        float num = 0f;
                                        if (string9 != null && string9 != "-999")
                                        {
                                            OddDTO oddDTO = new OddDTO();
                                            oddDTO.Type = eOddType.FulltimeHandicap;//FULL TIME !!!!!!!!!
                                            oddDTO.ID = objCurrentMatchProcess[0].ToString() + "^";
                                            oddDTO.Odd = objCurrentMatchProcess[9].ToString().Replace("/", "-");
                                            if (objCurrentMatchProcess[40].ToString() != "-999" &&  float.TryParse(objCurrentMatchProcess[40].ToString(), out num))
                                            {
                                                oddDTO.Home = num;
                                            }
                                            else
                                            {
                                                oddDTO.Home = 0f;
                                            }
                                            if (objCurrentMatchProcess[41].ToString() != "-999" && float.TryParse(objCurrentMatchProcess[41].ToString(), out num))
                                            {
                                                oddDTO.Away = num;
                                            }
                                            else
                                            {
                                                oddDTO.Away = 0f;
                                            }
                                            if (string24 == "1" && string9 == "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }
                                            else if (string24 == "1" && string9 != "0")
                                            {
                                                oddDTO.HomeFavor = true;
                                                oddDTO.AwayFavor = false;

                                            }
                                            else if (string24 == "0" && string9 == "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }
                                            else if (string24 == "0" && string9 != "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = true;
                                            }
                                            else
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }

                                            oddDTO.IsHomeGive = bool.Parse(string24.Replace("1","true").Replace("0","false"));
                                            matchDTO.Odds.Add(oddDTO);
                                            if (!map.ContainsKey(oddDTO.ID))
                                                map.Add(oddDTO.ID, matchDTO.ID);
#if DDEBUG
                                        Utilities.WriteLog.Write("~o~:3in1:" +
                                            matchDTO.HomeTeamName + "["
                                            + oddDTO.HomeFavor.ToString() + "]"
                                            + " - "
                                            + matchDTO.AwayTeamName + "["
                                            + oddDTO.AwayFavor.ToString() + "]"
                                            + " >> add odd:"
                                            + oddDTO.Type.ToString()
                                            + ". "
                                            + oddDTO.Odd
                                            + " Price Home:"
                                            + oddDTO.Home.ToString()
                                            + ", Away:"
                                            + oddDTO.Away.ToString()
                                            + ", ID: "
                                            + oddDTO.ID
                                            );
#endif
                                        }
                                        if (objCurrentMatchProcess[11] != null
                                            && objCurrentMatchProcess[11].ToString() != "-999")
                                        {
                                            OddDTO oddDTO = new OddDTO();
                                            oddDTO.Type = eOddType.FulltimeOverUnder; // FULL TIME OVER UNDER
                                            oddDTO.ID = objCurrentMatchProcess[0].ToString();
                                            oddDTO.Odd = objCurrentMatchProcess[11].ToString().Replace("/", "-");
                                            if (objCurrentMatchProcess[42].ToString() != "-999" && float.TryParse(objCurrentMatchProcess[42].ToString(), out num))
                                            {
                                                oddDTO.Home = num;
                                            }
                                            else
                                            {
                                                oddDTO.Home = 0f;
                                            }
                                            if (objCurrentMatchProcess[43].ToString() != "-999" && float.TryParse(objCurrentMatchProcess[43].ToString(), out num))
                                            {
                                                oddDTO.Away = num;
                                            }
                                            else
                                            {
                                                oddDTO.Away = 0f;
                                            }

                                            oddDTO.IsHomeGive = bool.Parse(string24.Replace("1","true").Replace("0","false"));
                                            matchDTO.Odds.Add(oddDTO);
                                            if (!map.ContainsKey(oddDTO.ID))
                                                map.Add(oddDTO.ID, matchDTO.ID);
#if DDEBUG
                                        Utilities.WriteLog.Write("~o~:3in1:" +
                                            matchDTO.HomeTeamName + "["
                                            + oddDTO.HomeFavor.ToString() + "]"
                                            + " - "
                                            + matchDTO.AwayTeamName + "["
                                            + oddDTO.AwayFavor.ToString() + "]"
                                            + " >> add odd:"
                                            + oddDTO.Type.ToString()
                                            + ". "
                                            + oddDTO.Odd
                                            + " Price Home:"
                                            + oddDTO.Home.ToString()
                                            + ", Away:"
                                            + oddDTO.Away.ToString()
                                            + ", ID: "
                                            + oddDTO.ID
                                            );
#endif
                                        }
                                        if (objCurrentMatchProcess[13] != null
                                            && objCurrentMatchProcess[13].ToString() != "-999")
                                        {
                                            OddDTO oddDTO = new OddDTO();
                                            oddDTO.Type = eOddType.FirstHalfHandicap; // FIRST HALF HANDICAP
                                            oddDTO.ID = objCurrentMatchProcess[2].ToString() + "^";
                                            oddDTO.Odd = objCurrentMatchProcess[13].ToString().Replace("/", "-");
                                            if (objCurrentMatchProcess[44].ToString() != "-999" && float.TryParse(objCurrentMatchProcess[44].ToString(), out num))
                                            {
                                                oddDTO.Home = num;
                                            }
                                            else
                                            {
                                                oddDTO.Home = 0f;
                                            }
                                            if (objCurrentMatchProcess[44].ToString() != "-999" && float.TryParse(objCurrentMatchProcess[45].ToString(), out num))
                                            {
                                                oddDTO.Away = num;
                                            }
                                            else
                                            {
                                                oddDTO.Away = 0f;
                                            }
                                            if (string24 == "1" && string13 == "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }
                                            else if (string24 == "1" && string13 != "0")
                                            {
                                                oddDTO.HomeFavor = true;
                                                oddDTO.AwayFavor = false;

                                            }
                                            else if (string24 == "0" && string13 == "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }
                                            else if (string24 == "0" && string13 != "0")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = true;
                                            }
                                            else
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = false;
                                            }

                                            oddDTO.IsHomeGive = bool.Parse(string24.Replace("1","true").Replace("0","false"));
                                            matchDTO.Odds.Add(oddDTO);
                                            if (!map.ContainsKey(oddDTO.ID))
                                                map.Add(oddDTO.ID, matchDTO.ID);
#if DDEBUG
                                        Utilities.WriteLog.Write("~o~:3in1:" +
                                            matchDTO.HomeTeamName + "["
                                            + oddDTO.HomeFavor.ToString() + "]"
                                            + " - "
                                            + matchDTO.AwayTeamName + "["
                                            + oddDTO.AwayFavor.ToString() + "]"
                                            + " >> add odd:"
                                            + oddDTO.Type.ToString()
                                            + ". "
                                            + oddDTO.Odd
                                            + " Price Home:"
                                            + oddDTO.Home.ToString()
                                            + ", Away:"
                                            + oddDTO.Away.ToString()
                                            + ", ID: "
                                            + oddDTO.ID
                                            );
#endif
                                        }
                                        if (objCurrentMatchProcess[15] != null
                                            && objCurrentMatchProcess[15].ToString() != "-999")
                                        {
                                            OddDTO oddDTO = new OddDTO();
                                            oddDTO.Type = eOddType.FirstHalfOverUnder; // FIRST HALF OVER UNDER
                                            oddDTO.ID = objCurrentMatchProcess[2].ToString();
                                            oddDTO.Odd = objCurrentMatchProcess[15].ToString().Replace("/", "-");
                                            if (objCurrentMatchProcess[46].ToString() != "-999" && float.TryParse(objCurrentMatchProcess[46].ToString(), out num))
                                            {
                                                oddDTO.Home = num;
                                            }
                                            else
                                            {
                                                oddDTO.Home = 0f;
                                            }
                                            if (objCurrentMatchProcess[47].ToString() != "-999" && float.TryParse(objCurrentMatchProcess[47].ToString(), out num))
                                            {
                                                oddDTO.Away = num;
                                            }
                                            else
                                            {
                                                oddDTO.Away = 0f;
                                            }

                                            oddDTO.IsHomeGive = bool.Parse(string24.Replace("1","true").Replace("0","false"));
                                            matchDTO.Odds.Add(oddDTO);
                                            if (!map.ContainsKey(oddDTO.ID))
                                                map.Add(oddDTO.ID, matchDTO.ID);
#if DDEBUG
                                        Utilities.WriteLog.Write("~o~:3in1:" +
                                            matchDTO.HomeTeamName + "["
                                            + oddDTO.HomeFavor.ToString() + "]"
                                            + " - "
                                            + matchDTO.AwayTeamName + "["
                                            + oddDTO.AwayFavor.ToString() + "]"
                                            + " >> add odd:"
                                            + oddDTO.Type.ToString()
                                            + ". "
                                            + oddDTO.Odd
                                            + " Price Home:"
                                            + oddDTO.Home.ToString()
                                            + ", Away:"
                                            + oddDTO.Away.ToString()
                                            + ", ID: "
                                            + oddDTO.ID
                                            );
#endif
                                        }
                                        #endregion
#if DDEBUG
                                        Utilities.WriteLog.Write("3in1: " + matchDTO.ID + " was added one more odd");
#endif
                                    }
                                }
                            }
                        }
#if DDEBUG
                        Dictionary<string, string>.KeyCollection keyColl = map.Keys;
                        foreach (string s in keyColl)
                        {
                            Utilities.WriteLog.Write("Add odd ID: " + s + " match ID: " + map[s]);
                        }
                        //foreach (MatchDTO matchVaiLon in matchDTO
#endif
                    }
                    else // [data] is null
                    {
 
                    }
                }             
            }
            mappingList3in1 = map;
            if (list != null)
              return list;
            else 
            {
                MatchDTO mNull = new MatchDTO();
                list.Add(mNull);
                return list;
            }
        }

        public static System.Collections.Generic.List<MatchDTO> ConvertUpdateData(
            string data,
            System.Collections.Generic.List<MatchDTO> originalDataSource,
            out string updateTime,
            Dictionary<string, string> originalMappingOddwithMatch,
            out Dictionary<string, string> mappingOddwithMatch)
        {
            System.Collections.Generic.List<MatchDTO> list = BaseDTO.DeepClone<System.Collections.Generic.List<MatchDTO>>(originalDataSource);
            //Dictionary<string, string> map = null;
            updateTime = string.Empty;
            if (data != string.Empty)
            {
                JavaScriptObject javaScriptObj = (JavaScriptObject)JavaScriptConvert.DeserializeObject(data);
                if (javaScriptObj != null)
                {
                    string timeUpdate = javaScriptObj["t"].ToString();
#if DEBUG
                    Utilities.WriteLog.Write("3in1: Time after lastest updated data:" + timeUpdate);
#endif
                    updateTime = timeUpdate;

                    if (javaScriptObj["data"] != null)
                    {
                        JavaScriptArray objLiveMatch = (JavaScriptArray)javaScriptObj["data"];
                        using (System.Collections.Generic.List<object>.Enumerator enumerator = objLiveMatch.GetEnumerator())
                        {
#if DDEBUG
                            int countNumberOfOddCanBeUpdated = 0;
#endif
                            while (enumerator.MoveNext())
                            {
                                JavaScriptArray objCurrentMatchProcess = (JavaScriptArray)enumerator.Current;

                                if (objCurrentMatchProcess.Count == 8) // update changed odd only!!
                                {
                                    int num = 0;
                                    if (int.TryParse(objCurrentMatchProcess[2].ToString(), out num))
                                    {
                                        string matchID = string.Empty;
                                        string oddID = string.Empty;

                                        if (num == 30 || num == 28)// handicap full time ^
                                        {
#if DDEBUG
                                            Utilities.WriteLog.Write("Proccessing odd ID: " + objCurrentMatchProcess[0].ToString() + "^ , type: " + objCurrentMatchProcess[2].ToString());
#endif
                                            oddID = objCurrentMatchProcess[0].ToString() + "^";
                                        }
                                        else if (num == 40 || num == 38)//handicap first half
                                        {
                                            int i;
                                            if (int.TryParse(objCurrentMatchProcess[0].ToString(), out i))
                                            {
                                                i++;
                                            }
                                            oddID = i.ToString() + "^";
#if DDEBUG
                                            Utilities.WriteLog.Write("Proccessing odd ID: " + oddID + " , type: " + objCurrentMatchProcess[2].ToString());
#endif
                                        }
                                        else if (num == 35 || num == 33 || num == 58 || num == 16 || num == 17) // over under full time, 16, 17: update score
                                        {
                                            oddID = objCurrentMatchProcess[0].ToString();
#if DDEBUG
                                            Utilities.WriteLog.Write("Proccessing odd ID: " + oddID + " , type: " + objCurrentMatchProcess[2].ToString());
#endif
                                        }
                                        else if (num == 43 || num == 45) // over / under first haft
                                        {
                                            int i;
                                            if (int.TryParse(objCurrentMatchProcess[0].ToString(), out i))
                                            {
                                                i++;
                                            }
                                            oddID = i.ToString();
#if DDEBUG
                                            Utilities.WriteLog.Write("Proccessing odd ID: " + oddID + " ,type: " + objCurrentMatchProcess[2].ToString());
#endif
                                        }

                                        if (oddID != string.Empty && originalMappingOddwithMatch.TryGetValue(oddID, out matchID))
                                        {
                                            MatchDTO matchDTO2 = MatchDTO.SearchMatch(matchID, list);
                                            if (matchDTO2 != null)
                                            {
                                                OddDTO oddDTO2 = null;
                                                oddDTO2 = OddDTO.SearchOdd(oddID, matchDTO2.Odds);// khong search dc o cho nay
                                                if (oddDTO2 != null)
                                                {
                                                    #region Update_Odd
                                                    matchDTO2.Odds.Remove(oddDTO2);//xoa odd cu
                                                    OddDTO oddDTO = new OddDTO();// tao odd moi      
                                                    oddDTO.ID = oddID;
                                                    oddDTO.IsHomeGive = oddDTO2.IsHomeGive;
                                                    float oddaway = 0f;
                                                    float oddhome = 0f;
                                                    bool validOddUpdate = Not999(objCurrentMatchProcess[3].ToString());

                                                    if (validOddUpdate)
                                                    {
                                                        if (float.TryParse(objCurrentMatchProcess[3].ToString(), out oddhome))
                                                        {
                                                            oddDTO.Home = oddhome;
                                                        }
                                                        if (float.TryParse(objCurrentMatchProcess[4].ToString(), out oddaway))
                                                        {
                                                            oddDTO.Away = oddaway;
                                                        }
                                                    }

                                                    switch (num)
                                                    {
                                                        case 16:
                                                            {
                                                                oddDTO = oddDTO2;
                                                                matchDTO2.HomeScore = objCurrentMatchProcess[3].ToString();
                                                                break;
                                                            }
                                                        case 17:
                                                            {
                                                                oddDTO = oddDTO2;
                                                                matchDTO2.AwayScore = objCurrentMatchProcess[3].ToString();
                                                                break;
                                                            }
                                                        case 30://full time handicap
                                                            {
                                                                oddDTO.Odd = oddDTO2.Odd;
                                                                oddDTO.Type = eOddType.FulltimeHandicap;
                                                                oddDTO.HomeFavor = oddDTO2.HomeFavor;
                                                                oddDTO.AwayFavor = oddDTO2.AwayFavor;
                                                                break;
                                                            }
                                                        case 35://full time over/under
                                                            {
                                                                oddDTO.Odd = oddDTO2.Odd;
                                                                oddDTO.Type = eOddType.FulltimeOverUnder;
                                                                break;
                                                            }
                                                        case 40://Half time handicap
                                                            {
                                                                oddDTO.Type = eOddType.FirstHalfHandicap;
                                                                oddDTO.Odd = oddDTO2.Odd;
                                                                oddDTO.HomeFavor = oddDTO2.HomeFavor;
                                                                oddDTO.AwayFavor = oddDTO2.AwayFavor;
                                                                break;
                                                            }
                                                        case 45://Half time over / under
                                                            {
                                                                oddDTO.Type = eOddType.FirstHalfOverUnder;
                                                                oddDTO.Odd = oddDTO2.Odd;
                                                                oddDTO.ID = oddID;
                                                                break;
                                                            }
                                                        case 28://full time handicap update ball
                                                            {
                                                                oddDTO.Type = eOddType.FulltimeHandicap;
                                                                if (validOddUpdate)
                                                                {
                                                                    oddDTO.Odd = objCurrentMatchProcess[3].ToString().Replace("/", "-");
                                                                    oddDTO.Home = 0f;
                                                                    oddDTO.Away = 0f;
                                                                }
                                                                else oddDTO.Odd = "clear";
                                                                break;
                                                            }
                                                        case 33://full time over / under update ball
                                                            {
                                                                oddDTO.Type = eOddType.FulltimeOverUnder;
                                                                if (validOddUpdate)
                                                                {
                                                                    oddDTO.Odd = objCurrentMatchProcess[3].ToString().Replace("/", "-");
                                                                    oddDTO.Home = 0f;
                                                                    oddDTO.Away = 0f;
                                                                }
                                                                else oddDTO.Odd = "clear";
                                                                break;
                                                            }
                                                        case 38: // half time handicap update ball
                                                            {
                                                                oddDTO.Type = eOddType.FirstHalfHandicap;
                                                                if (validOddUpdate)
                                                                {
                                                                    oddDTO.Odd = objCurrentMatchProcess[3].ToString().Replace("/", "-");
                                                                    oddDTO.Home = 0f;
                                                                    oddDTO.Away = 0f;
                                                                }
                                                                else oddDTO.Odd = "clear";
                                                                oddDTO.ID = oddID;
                                                                break;
                                                            }
                                                        case 43: // first half over under update ball
                                                            {
                                                                oddDTO.Type = eOddType.FirstHalfOverUnder;
                                                                if (validOddUpdate)
                                                                {
                                                                    oddDTO.Odd = objCurrentMatchProcess[3].ToString().Replace("/", "-");
                                                                    oddDTO.Home = 0f;
                                                                    oddDTO.Away = 0f;
                                                                }
                                                                else oddDTO.Odd = "clear";
                                                                oddDTO.ID = oddID;
                                                                break;
                                                            }
                                                        case 58: // update HOME GIVE full time
                                                            {
                                                                if (objCurrentMatchProcess[0].ToString() == objCurrentMatchProcess[3].ToString())
                                                                {
                                                                    oddDTO = oddDTO2;
                                                                    if (objCurrentMatchProcess[4].ToString() == "1")
                                                                    {
                                                                        oddDTO.HomeFavor = true;
                                                                        oddDTO.AwayFavor = false;
                                                                        oddDTO.IsHomeGive = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        oddDTO.HomeFavor = false;
                                                                        oddDTO.IsHomeGive = false;
                                                                    }
                                                                }
                                                                else if (objCurrentMatchProcess[0].ToString() != objCurrentMatchProcess[3].ToString())
                                                                {
                                                                    oddDTO = oddDTO2;
                                                                    if (objCurrentMatchProcess[4].ToString() == "1")
                                                                    {
                                                                        oddDTO.HomeFavor = true;
                                                                        oddDTO.AwayFavor = false;
                                                                        oddDTO.IsHomeGive = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        oddDTO.HomeFavor = false;
                                                                        oddDTO.IsHomeGive = false;
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                        case 111:
                                                            {

                                                                break;
                                                            }
                                                        default:
                                                            {
                                                                //oddDTO.Away = 0f;
                                                                //oddDTO.Home = 0f;
                                                                //oddDTO.
                                                                break;
                                                            }
                                                    }
                                                    matchDTO2.Odds.Add(oddDTO);//add odd moi cho match
#if DDEBUG
                                                    Utilities.WriteLog.Write("3in1: Update type "
                                                        + objCurrentMatchProcess[2].ToString() + "; "
                                                        + oddDTO.Type.ToString() + " for match:"
                                                        + matchDTO2.HomeTeamName + " - " + matchDTO2.AwayTeamName
                                                        + " - " + oddDTO.Type.ToString()
                                                        + "; new odd: "
                                                        + oddDTO.Odd
                                                        + "; new price is Home:"
                                                        + oddDTO.Home.ToString()
                                                        + ", Away: " + oddDTO.Away.ToString()
                                                        + ", ID:"
                                                        + oddDTO.ID
                                                        );
#endif

                                                    #endregion
                                                }
#if DDEBUG
                                                else
                                                {
                                                    Utilities.WriteLog.Write("01. Can not find " + oddID + " of match: " + matchDTO2.HomeTeamName + " - " + matchDTO2.AwayTeamName);
                                                }
#endif

                                            }
#if DDEBUG
                                            else
                                            {
                                                Utilities.WriteLog.Write("02. Can not find " + matchID + " of odd ID: " + oddID);
                                            }

#endif
                                        }
                                        else
                                        {
#if DDEBUG
                                            Utilities.WriteLog.Write("03. Can not find odd ID:"
                                                + oddID
                                                + " in the Dictionary of Odd, update was type: "
                                                + objCurrentMatchProcess[2].ToString()
                                                + " , for ID: "
                                                + objCurrentMatchProcess[0].ToString()
                                                );
#endif
                                        }
                                    }
#if DDEBUG
                                    countNumberOfOddCanBeUpdated++;
#endif
                                }
                                else if (objCurrentMatchProcess.Count == 65)//system insert new odd
                                {

                                }
                                else if (objCurrentMatchProcess.Count == 7)//something happended
                                {

                                }
                            }
#if DDEBUG
                            Utilities.WriteLog.Write("Update data has " + countNumberOfOddCanBeUpdated + " odds valid update");
#endif
                        }
                    }

                    else
                    { 
                        
                    }

                }
            }
            mappingOddwithMatch = originalMappingOddwithMatch;
            return list;
        }

        public void PrepareBet(
            string oddID,//  threein1Odd.ID,
            string oddValue,//threein1Odd.Away.ToString()
            string oddType,
            string HomeOrAway,
            string isHomeGive,
            string odd,
            string scoreHome,
            string scoreAway,
            string GUID,
            string isHalfTime,
            string stake,
            out bool allowance,
            out int minStake,
            out int maxStake,
            out string betKindValue,
            out string homeTeamName,
            out string awayTeamName,
            out string postBODY)
        {
            allowance = false;
            minStake = 0;
            maxStake = 0;
            betKindValue = string.Empty;
            homeTeamName = string.Empty;
            awayTeamName = string.Empty;
            postBODY = string.Empty;
            try
            {
                string random = MathRandom();
                //make a request to http://mem52.3in1bet.com/Member/BetsView/Bet.asmx/SetData                
                string requestUriString = string.Concat(new string[]
				{
					"http://", 
					this._host, 
					"/Member/BetsView/Bet.asmx/SetData"
				});
                
                this._betPrepareRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUriString);
                this._betPrepareRequest.CookieContainer = this._cookies;
                this._betPrepareRequest.UserAgent = this._userAgent; //ok
                this._betPrepareRequest.Method = "POST"; // ok
                this._betPrepareRequest.Host = this._host; // ok
                this._betPrepareRequest.Accept = "application/json, text/javascript, */*"; // ok
                this._betPrepareRequest.Referer = "http://" + this._host + "/Main/AdminLeftFrame.aspx";
                
                string samestring = string.Concat(new string[]
				{
                    "{\"data\":\"",
                    oddID.Replace("^",""),              //OddID
                    ",",
                    oddValue,                           //OddValue
                    ",",
                    ConvertOddType(oddType,HomeOrAway), //OddPlay - Hdp, Under, Over
                    ",",
                    HomeOrAway,                         //OddType Home, Away
                    ",MY,",
                    isHomeGive,                         //Home Give ? 0-1
                    ",",
                    ConvertOdd(odd),                    //OddHandicap
                    ",",
                    "Running,",
                    scoreHome,                          //Score home
                    ",",
                    scoreAway,                          //Score away
                    ",0,",                              //always zero
                    GUID,                               //GUID
                    ",",
                    IsFirstHalfBet(oddType),            // First half bet ?
                    ",0,",                              //Always zero
                    random,                             //Math.Random
                    "\"",
                    ",\"isAuto\":false,\"s\":\""
				});
                postBODY = samestring;
                string textPost = string.Concat(new string[]
				{
                    samestring,
                    BetCodeGenerate(_secretNumber),     //Bet Code
                    "\"}"
                });

                this._betPrepareRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._betPrepareRequest.Headers.Add("Accept-Language", "en-us,en;q=0.5");
                this._betPrepareRequest.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
                this._betPrepareRequest.Headers.Add("Pragma", "no-cache");
                this._betPrepareRequest.Headers.Add("Cache-Control", "no-cache");                

                this._betPrepareRequest.ContentLength = (long)textPost.Length;
                this._betPrepareRequest.KeepAlive = false;
                this._betPrepareRequest.ProtocolVersion = HttpVersion.Version11;
                this._betPrepareRequest.Timeout = 20000;

                
#if DEBUG
                Utilities.WriteLog.Write("Preparing data for SETDATA:" + textPost);
#endif
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(textPost);
                Stream postStream = this._betPrepareRequest.GetRequestStream();
                postStream.Write(bytes, 0, bytes.Length);
                postStream.Close(); // post done!
                HttpWebResponse httpWebResponse = (HttpWebResponse)this._betPrepareRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
                {
                    responseStream = new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                }
                else if (httpWebResponse.ContentEncoding.ToLower().Contains("deflate"))
                {
                    responseStream = new DeflateStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                }                
                StreamReader streamReader = new StreamReader(responseStream, Encoding.Default);

                string text = streamReader.ReadToEnd();

                streamReader.Close();
                responseStream.Close();
                httpWebResponse.Close();

                ThreeIn1BetEngine.ProcessPrepareBet(
                    text,
                    out allowance,
                    out minStake,
                    out maxStake,
                    out betKindValue,
                    out homeTeamName,
                    out awayTeamName);
            }
            catch (System.Exception ex)
            {
                allowance = false;
                minStake = 0;
                maxStake = 0;
                betKindValue = string.Empty;
                homeTeamName = string.Empty;
                awayTeamName = string.Empty;
                throw ex;
            }
        }

        public static void ProcessPrepareBet(
            string data,
            out bool allowance,
            out int minStake,
            out int maxStake,
            out string betKindValue)
        {
            allowance = false;
            minStake = 0;
            maxStake = 0;
            betKindValue = string.Empty;            

        }

        public static void ProcessPrepareBet(
            string data,
            out bool allowance,
            out int minStake,
            out int maxStake,
            out string betKindValue,
            out string homeTeamName,
            out string awayTeamName)
        {
            allowance = false;
            minStake = 0;
            maxStake = 0;
            betKindValue = string.Empty;
            homeTeamName = string.Empty;
            awayTeamName = string.Empty;

            if (data == null || data == string.Empty)
            {
                allowance = false;
                minStake = 0;
                maxStake = 0;
                betKindValue = "";
            }
            else
            {
#if DEBUG
                Utilities.WriteLog.Write("+ Data resonse after SETDATA:" + data);
#endif
                try
                {

                    JavaScriptObject javaScriptObj = (JavaScriptObject)JavaScriptConvert.DeserializeObject(data);
                    if (javaScriptObj != null)
                    {
                        JavaScriptObject objD = (JavaScriptObject)javaScriptObj["d"];
                        JavaScriptObject objBetData = (JavaScriptObject)objD["BetData"];
                        homeTeamName = objBetData["H"].ToString().Replace("1h-", "");
                        awayTeamName = objBetData["A"].ToString().Replace("1h-", "");
                        minStake = (int)float.Parse(objBetData["IN"].ToString());
                        maxStake = (int)float.Parse(objBetData["AX"].ToString());
                        betKindValue = objBetData["RB"].ToString();
                        if (objD["Message"].ToString() == "")
                        {
                            allowance = true;
#if DEBUG
                            Utilities.WriteLog.Write("+ ALLOWANCE!!!");
#endif
                        }
                    }
                    else
                    {
#if DEBUG
                        Utilities.WriteLog.Write("+ Error 11: Deserialize object after place SetData error");
#endif
                    }
                }
                catch (System.Exception ex)
                {
                    allowance = false;
                    minStake = 0;
                    maxStake = 0;
                    betKindValue = string.Empty;
                    homeTeamName = string.Empty;
                    awayTeamName = string.Empty;
#if DEBUG
                    Utilities.WriteLog.Write("+ Error 10:" + ex);
#endif
                    throw ex;
                }
            }
        }

        public void ConfirmBet(string postBODY,string betStake, out bool success, out string newOddValue)
        {
            success = false;
            newOddValue = string.Empty;
            try
            {
                //http://mem22.3in1bet.com/Member/BetsView/Bet.asmx/BetNow
                string requestUriString = string.Concat(new string[]
				{
					"http://", 
					this._host, 
					"/Member/BetsView/Bet.asmx/BetNow"
				});

                this._betConfirmRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUriString);
                this._betConfirmRequest.CookieContainer = this._cookies;
                this._betConfirmRequest.UserAgent = this._userAgent; //ok
                this._betConfirmRequest.Method = "POST"; // ok
                this._betConfirmRequest.Host = this._host; // ok
                this._betConfirmRequest.Accept = "application/json, text/javascript, */*"; // ok
                this._betConfirmRequest.Referer = "http://" + this._host + "/Main/AdminLeftFrame.aspx";

                string stake = "," + betStake + ",0.";
                string textPost = string.Concat(new string[]
                {
                    postBODY,
                    BetCodeGenerate(_secretNumber),     //Bet Code
                    "\"}"
                });

                textPost = textPost.Replace(",0,0.", stake);

                this._betConfirmRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                this._betConfirmRequest.Headers.Add("Accept-Language", "en-us,en;q=0.5");
                this._betConfirmRequest.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
                this._betConfirmRequest.Headers.Add("Pragma", "no-cache");
                this._betConfirmRequest.Headers.Add("Cache-Control", "no-cache");

                this._betConfirmRequest.ContentLength = (long)textPost.Length;
                this._betConfirmRequest.KeepAlive = false;
                this._betConfirmRequest.ProtocolVersion = HttpVersion.Version11;
                this._betConfirmRequest.Timeout = 20000;

#if DEBUG
                Utilities.WriteLog.Write("Preparing data for BETNOW:" + textPost);
#endif
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(textPost);
                Stream postStream = this._betConfirmRequest.GetRequestStream();
                postStream.Write(bytes, 0, bytes.Length);
                postStream.Close(); // post done!
                HttpWebResponse httpWebResponse = (HttpWebResponse)this._betConfirmRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
                {
                    responseStream = new GZipStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                }
                else if (httpWebResponse.ContentEncoding.ToLower().Contains("deflate"))
                {
                    responseStream = new DeflateStream(httpWebResponse.GetResponseStream(), CompressionMode.Decompress);
                }
                StreamReader streamReader = new StreamReader(responseStream, Encoding.Default);

                string text = streamReader.ReadToEnd();
#if DEBUG
                Utilities.WriteLog.Write("+ BETNOW confirm response" + textPost);
#endif
                streamReader.Close();
                responseStream.Close();
                httpWebResponse.Close();

                ThreeIn1BetEngine.ProcessConfirmBet(text,out success, out newOddValue);
                

            }
            catch (System.Exception ex)
            {
                success = false;
#if DEBUG
                Utilities.WriteLog.Write("+ Error confirm bet:" + ex);
#endif
                throw ex;
            }
        }

        public static void ProcessConfirmBet(string data, out bool success, out string newOddValue)
        {
            newOddValue = string.Empty;
            success = false;
            if (data == null || data == string.Empty)
            {
                success = false;
            }
            else
            {
                if (data.ToLower().Contains("accepted"))
                {
                    success = true;
                }
                else if (data.ToLower().Contains("changed"))
                {
                    newOddValue = "changed";

#if DEBUG
                    Utilities.WriteLog.Write("Odd changed:");
#endif
                }
                else 
                {
                    newOddValue = "changed";
                    success = false;
                }
            }
        }

        private static bool Not999(string str)
        {
            if (str == "-999" || str == "\"-999\"")
                return false;
            return true;
        }

        private static string BetCodeGenerate(string serverCode)
        {
            Random r = new Random();
            char[] arr2 = serverCode.ToCharArray();
            Array.Reverse(arr2);
            StringBuilder builder = new StringBuilder();
            foreach (char value in arr2)
            {
                builder.Append(value);
                builder.Append(r.Next(10).ToString());
            }
            return builder.ToString();
        }

        private static string MathRandom()
        {
            Random r = new Random();            
            return (r.NextDouble().ToString() + r.Next(1, 10).ToString());
        }

        private static string ConvertOdd(string odd)
        {
            if (odd.Contains("-"))
            {
                string[] parts = odd.Split(new string[] { "-" }, StringSplitOptions.None);
                float a1 = (float.Parse(parts[0]) + float.Parse(parts[1])) / 2;
                return a1.ToString();
            }
            
            return odd;
        }

        private static string IsFirstHalfBet(string s)
        {
            if (s.Contains("First"))
                return "1";
            return "0";
        }
        private static string ConvertOddType(string oddtype, string HomeOrAway)
        {
            switch (oddtype)
            {
                case "FulltimeHandicap":
                    {
                        return "Hdp";
                    }
                case "FirstHalfHandicap":
                    {
                        return "Hdp";
                    }
                case "FulltimeOverUnder":
                    {
                        if (HomeOrAway == "Away")
                            return "Under";
                        return "Over";
                    }
                case "FirstHalfOverUnder":
                    {
                        if (HomeOrAway == "Away")
                            return "Under";
                        return "Over";
                    }
                default:
                    {
                        return "0";
                    }
            }
                 
        }

    }
}
