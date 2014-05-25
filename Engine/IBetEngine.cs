//#define DEBUG
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using iBet.DTO;

namespace iBet.Engine
{
    public class IBetEngine : BaseEngine
    {
        public BCCore.IBETAgent ibetAgent;
        public BCCore.IBETAgent ibetAgent2;

        private System.Net.CookieContainer _cookies;
        private string _userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Win64; x64; Trident/4.0; .NET CLR 2.0.50727; SLCC2; .NET4.0C; .NET4.0E)";
        private string _host;
        public float _currentCredit = 0f;
        private string _accountID;
        private string _dynamicFieldName;
        private string _dynamicFieldValue;
        private string _lastDataChangeTime;
        private System.Collections.Generic.List<MatchDTO> _lastListMatch;
        private Timer _updateDataTimer;
        private System.Net.HttpWebRequest _fullDataRequest;
        private System.Net.HttpWebRequest _updateDataRequest;
        private System.Net.HttpWebRequest _betPrepareRequest;
        private System.Net.HttpWebRequest _betConfirmRequest;
        private int _updateDataInterval = 10000;
        public event EngineDelegate FullDataCompleted;
        public event EngineDelegate UpdateCompleted;
        public event EngineDelegate BetPrepareCompleted;
        public event EngineDelegate BetConfirmCompleted;
        public string _winlost;
        public string _commision;
        public int _reject;
        public bool useProxy = false;
        public int CoundDown = 6;
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
        public IBetEngine(string host,
                            string accountID,
                            string dynamicFieldName,
                            string dynamicFieldValue,
                            System.Net.CookieContainer cookies)
        {
            this._host = host;
            this._accountID = accountID;
            this._dynamicFieldName = dynamicFieldName;
            this._dynamicFieldValue = dynamicFieldValue;
            this._cookies = cookies;
            this.InitializeObjects();
        }        
        public IBetEngine(BCCore.BetAccount account, BCCore.Utis.EngineLogger logger, string engineID,
            BCCore.BetAccount account2, BCCore.Utis.EngineLogger logger2, string engineID2)
        {
            this.ibetAgent = new BCCore.IBETAgent(account, logger, engineID);
            this.ibetAgent2 = new BCCore.IBETAgent(account2, logger2, engineID2);

            this.InitializeObjects();
        }
        public IBetEngine(BCCore.BetAccount account, BCCore.Utis.EngineLogger logger, string engineID)
        {
            this.ibetAgent = new BCCore.IBETAgent(account, logger, engineID);
            this.InitializeObjects();
        }
        public IBetEngine(BCCore.IBETAgent ibAgent)
        {
            this.ibetAgent = ibAgent;
            this.InitializeObjects();
        }
        public void Start()
        {
            this.GetFullData();
            this._updateDataTimer.Start();
        }
        public void Start(string haicon)
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
            //if (this.LastUpdateCompleted)
            //{
                this.GetUpdateData();
            //}
        }
        public string GetBetList()
        {
            //float result = 0f;
            try
            {
                string text = CreateWebRequest("GET", "http://" + this._host + "/Betlist.aspx", "http://" + this._host + "/topmenu.aspx", this._cookies, this._host, false, "");
                return text;
            }
            catch
            {
                return "";
            }
        }
        public string GetStatement()
        {
            try
            {
                string text = CreateWebRequest("GET", "http://"
                    + this._host
                    + "/DBetlist.aspx?fdate=" + DateTime.Now.AddHours(-8).ToShortDateString(),
                    "http://" + this._host + "/AllStatement.aspx", this._cookies, this._host, useProxy, "");

                ProcessStatement(text, out this._winlost, out this._commision, out this._reject);
                return text;
            }
            catch
            {
                return "";
            }
        }
        public static void ProcessStatement(string data, out string winlost, out string com, out int reject)
        {
            if (data == null || data == string.Empty)
            {
                winlost = string.Empty;
                com = string.Empty;
                reject = 0;
            }
            else
            {
                try
                {
                    string[] array = data.Split(new string[] { "C6D4F1" }, System.StringSplitOptions.None);
                    string[] array2 = array[1].Split(new string[] { "</td>" }, System.StringSplitOptions.None);
                    string[] array3 = array2[0].Split(new string[] { "</span>" }, System.StringSplitOptions.None);

                    string[] array4 = array3[0].Split(new string[] { "\">" }, System.StringSplitOptions.None);
                    winlost = array4[3];

                    string[] array5 = array3[1].Split(new string[] { "\">" }, System.StringSplitOptions.None);
                    com = array5[2];

                    string wordToSearch = "reject";
                    System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex(wordToSearch, RegexOptions.IgnoreCase);
                    reject = oRegex.Matches(data).Count;
                }
                catch (Exception ex)
                {
                    winlost = string.Empty;
                    com = string.Empty;
                    reject = 0;
                }
            }
        }
        public void PrepareBet(string oddID,
                                string oddType,
                                string oddValue,
                                string stake,
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
            try
            {
                string requestUriString = "http://" + this._host + "/BetProcess_Data.aspx";
                this._betPrepareRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUriString);
                this._betPrepareRequest.CookieContainer = this._cookies;
                //this._betPrepareRequest.CookieContainer.Add(new System.Uri("http://ca88.ibet888.net"), new System.Net.Cookie("ASP.NET_SessionId", "jgakg345nc1a2355milpuryf"));
                this._betPrepareRequest.Headers.Add("Accept-Language", "en-US");
                //this._betPrepareRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                this._betPrepareRequest.Headers.Add("Pragma", "no-cache");
                this._betPrepareRequest.Accept = "image/jpeg, image/gif, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, */*";
                //this._betPrepareRequest.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                this._betPrepareRequest.UserAgent = this._userAgent;
                this._betPrepareRequest.Method = "POST";
                this._betPrepareRequest.Host = this._host;
                this._betPrepareRequest.ContentType = "application/x-www-form-urlencoded";
                this._betPrepareRequest.Referer = "http://" + this._host + "/LeftAllInOne.aspx";
                this._betPrepareRequest.Timeout = 30000;
                this._betPrepareRequest.ServicePoint.Expect100Continue = false;
                this._betPrepareRequest.KeepAlive = true;

                this._betPrepareRequest.AutomaticDecompression = (System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate);
                //bp_Match=15505831_h_0.840&UN=SHB8403111&bp_ssport=&chk_BettingChange=&k361881523=v361881718
                if (oddValue == "1" || oddValue == "-1")
                {
                    oddValue += ".000";
                }
                else
                {
                    oddValue += "0";
                }
                string text = string.Concat(new string[]
				{
					"bp_Match=", 
					oddID, 
					"_", 
					oddType, 
					"_", 
					oddValue, 
					"&UN=", 
					this._accountID, 
					"&bp_ssport=&chk_BettingChange=4&", 
					this._dynamicFieldName, 
					"=", 
					this._dynamicFieldValue
				});
#if DEBUG
                iBet.Utilities.WriteLog.Write("ibet 001 : Prepare text bet, then go to processBet " + text);
#endif
                this._betPrepareRequest.ContentLength = (long)text.Length;
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
                this._betPrepareRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)this._betPrepareRequest.GetResponse();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(httpWebResponse.GetResponseStream());
                string data = streamReader.ReadToEnd();

                IBetEngine.ProcessPrepareBet(data,
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
        public void PrepareBet2(string oddID, string oddType, string oddValue, string stake, out bool allowance, out int minStake, out int maxStake, out string betKindValue, out string homeTeamName, out string awayTeamName, out string newOddValue, out string homescore, out string awayscore)
        {
            allowance = false;
            minStake = 0;
            maxStake = 0;
            betKindValue = string.Empty;
            homeTeamName = string.Empty;
            awayTeamName = string.Empty;
            newOddValue = string.Empty;
            homescore = string.Empty;
            awayscore = string.Empty;
            try
            {
                if (oddValue == "1" || oddValue == "-1")
                    oddValue += ".000";
                else
                    oddValue += "0";                
                
                string text2 = string.Format("{0}_{1}_{2:F3}", oddID, oddType, oddValue);
                text2 = string.Format("bp_Match={0}&UN={1}&bp_ssport=1&chk_BettingChange={2}&{3}&bp_key={4}", new object[]
				{
					text2,
					this.ibetAgent.Config.Account.ToUpper(),
					"4",
					this.ibetAgent.CheckSum + "=" + this.ibetAgent.CheckSumValue,
                    this.ibetAgent.encrypt.GetKey("bet", this.ibetAgent.timeWeb, this.ibetAgent.timeWebStart, this.ibetAgent.CT)
				});

                string data = CreateWebRequest("GET",
                    "http://" + this.ibetAgent.Config.HostName + "/BetProcess_Data.aspx?" + text2,
                    "http://" + this.ibetAgent.Config.HostName + "/LeftAllInOne.aspx",
                    this.ibetAgent.cc,
                    this.ibetAgent.Config.HostName, true, "");
                ProcessPrepareBet2(data, out allowance, out minStake, out maxStake, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out homescore, out awayscore);
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
        public void PrepareBetFor2ndIBET(string oddID, string oddType, string oddValue, string stake, out bool allowance, out int minStake, out int maxStake, out string betKindValue, out string homeTeamName, out string awayTeamName, out string newOddValue, out string homescore, out string awayscore)
        {
            allowance = false;
            minStake = 0;
            maxStake = 0;
            betKindValue = string.Empty;
            homeTeamName = string.Empty;
            awayTeamName = string.Empty;
            newOddValue = string.Empty;
            homescore = string.Empty;
            awayscore = string.Empty;
            try
            {
                if (oddValue == "1" || oddValue == "-1")
                    oddValue += ".000";
                else
                    oddValue += "0";

                string text2 = string.Format("{0}_{1}_{2:F3}", oddID, oddType, oddValue);
                text2 = string.Format("bp_Match={0}&UN={1}&bp_ssport=1&chk_BettingChange={2}&{3}&bp_key={4}", new object[]
				{
					text2,
					this.ibetAgent2.Config.Account.ToUpper(),
					"4",
					this.ibetAgent2.CheckSum + "=" + this.ibetAgent2.CheckSumValue,
                    this.ibetAgent2.encrypt.GetKey("bet", this.ibetAgent2.timeWeb, this.ibetAgent2.timeWebStart, this.ibetAgent2.CT)
				});

                string data = CreateWebRequest("GET",
                    "http://" + this.ibetAgent2.Config.HostName + "/BetProcess_Data.aspx?" + text2,
                    "http://" + this.ibetAgent2.Config.HostName + "/LeftAllInOne.aspx",
                    this.ibetAgent2.cc,
                    this.ibetAgent2.Config.HostName, true, "");
                ProcessPrepareBet2(data, out allowance, out minStake, out maxStake, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out homescore, out awayscore);
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
        public void ConfirmBet(eOddType oddType,
                                    string oddValue,
                                    string stake,
                                    string minStake,
                                    string maxStake,
                                    out bool success)
        {
            success = false;
            try
            {
                string text = "BPstake=" + stake;
                text += "&btnBPSubmit=Process+Bet&HorseBPstake="; //related with cookies
                text += "&stakeRequest=&";
                text += "oddsRequest=" + oddValue;
                text += "&oddChange1=Odds+has+changed+from";
                text += "&oddChange2=to";
                text = text + "&MINBET=" + minStake;
                text = text + "&MAXBET=" + maxStake; // cho nay
                switch (oddType)
                {
                    case eOddType.FulltimeHandicap:
                        {
                            text += "&bettype=1";
                            break;
                        }
                    case eOddType.FulltimeOverUnder:
                        {
                            text += "&bettype=3";
                            break;
                        }
                    case eOddType.FirstHalfHandicap:
                        {
                            text += "&bettype=7";
                            break;
                        }
                    case eOddType.FirstHalfOverUnder:
                        {
                            text += "&bettype=8";
                            break;
                        }
                }
                text += "&lowerminmesg=Your+stake+is+lower+than+minimun+bet%21%21%21";
                text += "&highermaxmesg=Your+stake+is+higher+than+maximum+bet%21%21%21";
                text += "&areyousuremesg=Are+you+sure+you+want+process+the+bet%3F";
                text += "&areyousuremesg1=Your+previous+bet+is+still+processing%2C+are+you+sure+you+want+to+bet+%3F";
                text += "&incorrectStakeMesg=Incorrect+Stake.";
                text += "&oddsWarning=WARNING%21%21%21+WE+HAVE+GIVEN+A+NEW+ODDS+%26+NEW+STAKE%21%21%21";
                text += "&betconfirmmesg=Please+click+OK+to+confirm+the+bet%3F";
                text += "&siteType=";
                text += "&hidStake10=Stake+must+be+in+multiples+of+10";
                text += "&hidStake20=Stake+must+be+in+multiples+of+20";
                text += "&hidStake2=Stake+must+be+in+multiples+of+2";
                text += "&sporttype=1";
                text = text + "&username=" + this.ibetAgent.Config.Account;
                text += "&oddsType=4&cbAcceptBetterOdds=1";            

                string data = CreateWebRequest("POST",
                    "http://" + this.ibetAgent.Config.HostName + "/underover/confirm_bet_data.aspx",
                    "http://" + this.ibetAgent.Config.HostName + "/LeftAllInOne.aspx",
                    this.ibetAgent.cc,
                    this.ibetAgent.Config.HostName, true, text);
                
                IBetEngine.ProcessConfirmBet(data, out success);
            }
            catch (System.Exception ex)
            {
                success = false;
                throw ex;
            }
        }
        public void ConfirmBetFor2ndIBET(eOddType oddType,
                                    string oddValue,
                                    string stake,
                                    string minStake,
                                    string maxStake,
                                    out bool success)
        {
            success = false;
            try
            {
                string text = "BPstake=" + stake;
                text += "&btnBPSubmit=Process+Bet&HorseBPstake="; //related with cookies
                text += "&stakeRequest=&";
                text += "oddsRequest=" + oddValue;
                text += "&oddChange1=Odds+has+changed+from";
                text += "&oddChange2=to";
                text = text + "&MINBET=" + minStake;
                text = text + "&MAXBET=" + maxStake; // cho nay
                switch (oddType)
                {
                    case eOddType.FulltimeHandicap:
                        {
                            text += "&bettype=1";
                            break;
                        }
                    case eOddType.FulltimeOverUnder:
                        {
                            text += "&bettype=3";
                            break;
                        }
                    case eOddType.FirstHalfHandicap:
                        {
                            text += "&bettype=7";
                            break;
                        }
                    case eOddType.FirstHalfOverUnder:
                        {
                            text += "&bettype=8";
                            break;
                        }
                }
                text += "&lowerminmesg=Your+stake+is+lower+than+minimun+bet%21%21%21";
                text += "&highermaxmesg=Your+stake+is+higher+than+maximum+bet%21%21%21";
                text += "&areyousuremesg=Are+you+sure+you+want+process+the+bet%3F";
                text += "&areyousuremesg1=Your+previous+bet+is+still+processing%2C+are+you+sure+you+want+to+bet+%3F";
                text += "&incorrectStakeMesg=Incorrect+Stake.";
                text += "&oddsWarning=WARNING%21%21%21+WE+HAVE+GIVEN+A+NEW+ODDS+%26+NEW+STAKE%21%21%21";
                text += "&betconfirmmesg=Please+click+OK+to+confirm+the+bet%3F";
                text += "&siteType=";
                text += "&hidStake10=Stake+must+be+in+multiples+of+10";
                text += "&hidStake20=Stake+must+be+in+multiples+of+20";
                text += "&hidStake2=Stake+must+be+in+multiples+of+2";
                text += "&sporttype=1";
                text = text + "&username=" + this.ibetAgent2.Config.Account;
                text += "&oddsType=4&cbAcceptBetterOdds=1";

                string data = CreateWebRequest("POST",
                    "http://" + this.ibetAgent2.Config.HostName + "/underover/confirm_bet_data.aspx",
                    "http://" + this.ibetAgent2.Config.HostName + "/LeftAllInOne.aspx",
                    this.ibetAgent2.cc,
                    this.ibetAgent2.Config.HostName, true, text);

                IBetEngine.ProcessConfirmBet(data, out success);
            }
            catch (System.Exception ex)
            {
                success = false;
                throw ex;
            }
        }
        private void GetFullData()
        {
            if (this.ibetAgent2.State == BCCore.AgentState.Running)
                this.ibetAgent2.RefreshLiveOdds();

            this.ibetAgent.RefreshOdds();
            
            this.On_FullDataCompleted(new EngineEventArgs
            {
                Type = eEngineEventType.Success,
                Data = this.ibetAgent.parser.LdicMatches
            });                    
        }
        
        private void GetUpdateData()
        {
            if (this.ibetAgent2.State == BCCore.AgentState.Running)
                this.ibetAgent2.RefreshUpdateLiveOdds();

            this.ibetAgent.RefreshUpdateLiveOdds();
            this.CoundDown--;
            if (this.CoundDown == 0)
            {
                this.ibetAgent.RefreshNonLiveOdds();
                this.CoundDown = 6;
            }

            this.On_UpdateCompleted(new EngineEventArgs
            {
                Type = eEngineEventType.Success,
                Data = this.ibetAgent.parser.LdicMatches
            });
        }
        public float GetCurrentCredit()
        {
            float result = 0f;
            try
            {
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://"
                    + this._host
                    + "/leftAllInOneAccount_data.aspx");
                httpWebRequest.CookieContainer = this._cookies;
                httpWebRequest.UserAgent = this._userAgent;
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Headers.Add("Accept-Language", "en-US");
                //httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWebRequest.Headers.Add("Pragma", "no-cache");
                httpWebRequest.Accept = "image/jpeg, image/gif, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, */*";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.Referer = "http://" + this._host + "/LeftAllInOne.aspx";
                httpWebRequest.Host = this._host;
                string text = "accountUpdate=mini";
                httpWebRequest.ContentLength = (long)text.Length;
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(text);
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(httpWebResponse.GetResponseStream());
                string text2 = streamReader.ReadToEnd();
#if DEBUG
                iBet.Utilities.WriteLog.Write("ibet: text current Credit: " + text2);
#endif
                text2 = text2.Replace("\"", "").Replace(",", "").Replace(";", "").Replace(" ", "").Replace("\r", "").Replace("\n", "");
                int num = text2.IndexOf("vartxt_betcredit=") + 17;
                int num2 = text2.IndexOf("vartxt_credit");
                result = float.Parse(text2.Substring(num, num2 - num));
                this._currentCredit = result;
            }
            catch
            {
                result = 0f;
            }
            return result;
        }

        public string GetWaitingBet()
        {
            string returnText = "";
            //float result = 0f;
            try
            {
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://"
                    + this._host
                    + "/WaitingBets_Data.aspx?IsFromWaitingBtn=yes");

                httpWebRequest.CookieContainer = this._cookies;
                httpWebRequest.UserAgent = this._userAgent;
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Headers.Add("Accept-Language", "en-US");
                //httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                //httpWebRequest.Headers.Add("Pragma", "no-cache");
                httpWebRequest.Accept = "image/jpeg, image/gif, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, */*";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.Referer = "http://" + this._host + "/LeftAllInOne.aspx";
                httpWebRequest.Host = this._host;

                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(httpWebResponse.GetResponseStream());
                returnText = streamReader.ReadToEnd();
#if DEBUG
                iBet.Utilities.WriteLog.Write("ibet: text current Credit: " + text2);
#endif
                return returnText;
            }
            catch
            {
                return "";
            }
        }

        public string GetBetListMini()
        {
            string returnText = "";
            //float result = 0f;
            try
            {
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://"
                    + this._host
                    + "/BetListMini_data.aspx?showBetAcceptedMsg=no");
                //http://r44bq.ibet888.net/BetListMini_data.aspx?showBetAcceptedMsg=no

                httpWebRequest.CookieContainer = this._cookies;
                httpWebRequest.UserAgent = this._userAgent;
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Headers.Add("Accept-Language", "en-US");
                //httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                //httpWebRequest.Headers.Add("Pragma", "no-cache");
                httpWebRequest.Accept = "image/jpeg, image/gif, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, */*";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ServicePoint.Expect100Continue = false;
                httpWebRequest.Referer = "http://" + this._host + "/LeftAllInOne.aspx";
                httpWebRequest.Host = this._host;
                string text = "";
                httpWebRequest.ContentLength = (long)text.Length;
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(text);
                httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(httpWebResponse.GetResponseStream());
                returnText = streamReader.ReadToEnd();
#if DEBUG
                iBet.Utilities.WriteLog.Write("ibet: text current Credit: " + text2);
#endif
                return returnText;
            }
            catch
            {
                return "";
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
        public static System.Collections.Generic.List<MatchDTO> ConvertFullData(string data, out string updateTime)
        {
            System.Collections.Generic.List<MatchDTO> list = null;
            updateTime = string.Empty;
            if (data != string.Empty)
            {
                list = new System.Collections.Generic.List<MatchDTO>();
                data = data.Replace("\n", "");
                string[] array = data.Split(new string[]
				{
					";"
				}, System.StringSplitOptions.None);
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string text = array2[i];
                    if (text.StartsWith("window"))
                    {
                        updateTime = text.Substring(43, 19);
                    }
                    else
                    {
                        if (text.StartsWith("Nl"))
                        {
                            string value = text.Split(new string[]
							{
								"="
							}, System.StringSplitOptions.None)[1];
                            JavaScriptArray javaScriptArray = (JavaScriptArray)JavaScriptConvert.DeserializeObject(value);
                            if (javaScriptArray[3].ToString() == "1")
                            {
                                MatchDTO matchDTO = new MatchDTO();
                                matchDTO.Odds = new System.Collections.Generic.List<OddDTO>();
                                matchDTO.ID = javaScriptArray[0].ToString();
                                if (javaScriptArray[4].ToString() == string.Empty)
                                {
                                    MatchDTO matchDTO2 = list[list.Count - 1];
                                    matchDTO.League = matchDTO2.League;
                                }
                                else
                                {
                                    matchDTO.League = new LeagueDTO();
                                    matchDTO.League.ID = javaScriptArray[4].ToString();
                                    matchDTO.League.Name = javaScriptArray[5].ToString();
                                }
                                matchDTO.HomeTeamName = javaScriptArray[6].ToString();
                                matchDTO.AwayTeamName = javaScriptArray[7].ToString();
                                if (javaScriptArray.Count >= 12)
                                {
                                    if (javaScriptArray[12] != null
                                        && javaScriptArray[12].ToString() != string.Empty
                                        && !javaScriptArray[12].ToString().ToLower().Contains("t")
                                        && !javaScriptArray[12].ToString().ToLower().Contains("live"))
                                    {
                                        string[] array3 = javaScriptArray[12].ToString().ToLower().Split(new string[]
										{
											"h"
										}, System.StringSplitOptions.None);
                                        matchDTO.Half = int.Parse(array3[0]);
                                        matchDTO.Minute = int.Parse(array3[1].Trim().Replace("'", ""));
                                        matchDTO.IsHalfTime = false;
                                        //Utilities.WriteLog.Write(javaScriptArray[12].ToString());
                                    }
                                    else
                                    {
                                        matchDTO.Minute = 0;
                                        matchDTO.Half = 0;
                                        matchDTO.IsHalfTime = true;
                                    }
                                }
                                else
                                {
                                    matchDTO.Minute = 0;
                                    matchDTO.Half = 0;
                                    matchDTO.IsHalfTime = true;
                                }
                                float num = 0f;
                                if (javaScriptArray[24] != null && javaScriptArray[24].ToString() != string.Empty)
                                {
                                    OddDTO oddDTO = new OddDTO();
                                    oddDTO.Type = eOddType.FulltimeHandicap;//FULL TIME
                                    oddDTO.ID = javaScriptArray[24].ToString();
                                    oddDTO.Odd = javaScriptArray[25].ToString();
                                    //Utilities.WriteLog.Write(oddDTO.Odd);
                                    if (float.TryParse(javaScriptArray[26].ToString(), out num))
                                    {
                                        oddDTO.Home = num;
                                    }
                                    else
                                    {
                                        oddDTO.Home = 0f;
                                    }
                                    if (float.TryParse(javaScriptArray[27].ToString(), out num))
                                    {
                                        oddDTO.Away = num;
                                    }
                                    else
                                    {
                                        oddDTO.Away = 0f;
                                    }
                                    if (javaScriptArray[28] == null || javaScriptArray[28].ToString() == string.Empty)
                                    {
                                        oddDTO.HomeFavor = false;
                                        oddDTO.AwayFavor = false;
                                    }
                                    else
                                    {
                                        if (javaScriptArray[28].ToString() == "h")
                                        {
                                            oddDTO.HomeFavor = true;
                                            oddDTO.AwayFavor = false;
                                        }
                                        else
                                        {
                                            if (javaScriptArray[28].ToString() == "a")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = true;
                                            }
                                        }
                                    }
                                    matchDTO.Odds.Add(oddDTO);
#if DDEBUG
                                    Utilities.WriteLog.Write("~o~:ibet:" +
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
                                        + ", odd ID:" 
                                        + oddDTO.ID
                                        );
#endif

                                }
                                if (javaScriptArray[29] != null && javaScriptArray[29].ToString() != string.Empty)
                                {
                                    OddDTO oddDTO = new OddDTO();
                                    oddDTO.Type = eOddType.FulltimeOverUnder; //FULL TIME OVER UNDER
                                    oddDTO.ID = javaScriptArray[29].ToString();
                                    oddDTO.Odd = javaScriptArray[30].ToString();
                                    if (float.TryParse(javaScriptArray[31].ToString(), out num))
                                    {
                                        oddDTO.Home = num;
                                    }
                                    else
                                    {
                                        oddDTO.Home = 0f;
                                    }
                                    if (float.TryParse(javaScriptArray[32].ToString(), out num))
                                    {
                                        oddDTO.Away = num;
                                    }
                                    else
                                    {
                                        oddDTO.Away = 0f;
                                    }
                                    matchDTO.Odds.Add(oddDTO);
#if DDEBUG
                                    Utilities.WriteLog.Write("~o~:ibet:" +
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
                                        + ", odd ID:"
                                        + oddDTO.ID
                                        );
#endif
                                }
                                if (javaScriptArray[37] != null && javaScriptArray[37].ToString() != string.Empty)
                                {
                                    OddDTO oddDTO = new OddDTO();
                                    oddDTO.Type = eOddType.FirstHalfHandicap; // FIRST
                                    oddDTO.ID = javaScriptArray[37].ToString();
                                    oddDTO.Odd = javaScriptArray[38].ToString();
                                    if (float.TryParse(javaScriptArray[39].ToString(), out num))
                                    {
                                        oddDTO.Home = num;
                                    }
                                    else
                                    {
                                        oddDTO.Home = 0f;
                                    }
                                    if (float.TryParse(javaScriptArray[40].ToString(), out num))
                                    {
                                        oddDTO.Away = num;
                                    }
                                    else
                                    {
                                        oddDTO.Away = 0f;
                                    }
                                    if (javaScriptArray[41] == null || javaScriptArray[41].ToString() == string.Empty)
                                    {
                                        oddDTO.HomeFavor = false;
                                        oddDTO.AwayFavor = false;
                                    }
                                    else
                                    {
                                        if (javaScriptArray[41].ToString() == "h")
                                        {
                                            oddDTO.HomeFavor = true;
                                            oddDTO.AwayFavor = false;
                                        }
                                        else
                                        {
                                            if (javaScriptArray[41].ToString() == "a")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = true;
                                            }
                                        }
                                    }
                                    matchDTO.Odds.Add(oddDTO);
#if DDEBUG
                                    Utilities.WriteLog.Write("~o~:ibet:" +
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
                                        + ", odd ID:"
                                        + oddDTO.ID
                                        );
#endif

                                }
                                if (javaScriptArray[42] != null && javaScriptArray[42].ToString() != string.Empty)
                                {
                                    OddDTO oddDTO = new OddDTO();
                                    oddDTO.Type = eOddType.FirstHalfOverUnder;
                                    oddDTO.ID = javaScriptArray[42].ToString();
                                    oddDTO.Odd = javaScriptArray[43].ToString();
                                    if (float.TryParse(javaScriptArray[44].ToString(), out num))
                                    {
                                        oddDTO.Home = num;
                                    }
                                    else
                                    {
                                        oddDTO.Home = 0f;
                                    }
                                    if (float.TryParse(javaScriptArray[45].ToString(), out num))
                                    {
                                        oddDTO.Away = num;
                                    }
                                    else
                                    {
                                        oddDTO.Away = 0f;
                                    }
                                    matchDTO.Odds.Add(oddDTO);
#if DDEBUG
                                    Utilities.WriteLog.Write("~o~:ibet:" +
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
                                        + ", odd ID:"
                                        + oddDTO.ID
                                        );
#endif
                                }
                                list.Add(matchDTO);
#if DDEBUG
                                Utilities.WriteLog.Write("~ M ~:ibet: Added match: "
                                    + matchDTO.HomeTeamName
                                    + " - " + matchDTO.AwayTeamName
                                    + ", with number of odds: " + matchDTO.OddCount);
#endif
                            }
                            else
                            {
                                MatchDTO matchDTO2 = list[list.Count - 1];
                                float num = 0f;
                                if (javaScriptArray[24] != null && javaScriptArray[24].ToString() != string.Empty)
                                {
                                    OddDTO oddDTO = new OddDTO();
                                    oddDTO.Type = eOddType.FulltimeHandicap;
                                    oddDTO.ID = javaScriptArray[24].ToString();
                                    oddDTO.Odd = javaScriptArray[25].ToString();
                                    if (float.TryParse(javaScriptArray[26].ToString(), out num))
                                    {
                                        oddDTO.Home = num;
                                    }
                                    else
                                    {
                                        oddDTO.Home = 0f;
                                    }
                                    if (float.TryParse(javaScriptArray[27].ToString(), out num))
                                    {
                                        oddDTO.Away = num;
                                    }
                                    else
                                    {
                                        oddDTO.Away = 0f;
                                    }
                                    if (javaScriptArray[28] == null || javaScriptArray[28].ToString() == string.Empty)
                                    {
                                        oddDTO.HomeFavor = false;
                                        oddDTO.AwayFavor = false;
                                    }
                                    else
                                    {
                                        if (javaScriptArray[28].ToString() == "h")
                                        {
                                            oddDTO.HomeFavor = true;
                                            oddDTO.AwayFavor = false;
                                        }
                                        else
                                        {
                                            if (javaScriptArray[28].ToString() == "a")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = true;
                                            }
                                        }
                                    }
#if DDEBUG
                                    Utilities.WriteLog.Write("~o~:ibet:" +
                                        matchDTO2.HomeTeamName + "["
                                        + oddDTO.HomeFavor.ToString() + "]"
                                        + " - "
                                        + matchDTO2.AwayTeamName + "["
                                        + oddDTO.AwayFavor.ToString() + "]"
                                        + " >> add odd:"
                                        + oddDTO.Type.ToString()
                                        + ". "
                                        + oddDTO.Odd
                                        + " Price Home:"
                                        + oddDTO.Home.ToString()
                                        + ", Away:"
                                        + oddDTO.Away.ToString());
#endif
                                    matchDTO2.Odds.Add(oddDTO);
                                }
                                if (javaScriptArray[29] != null && javaScriptArray[29].ToString() != string.Empty)
                                {
                                    OddDTO oddDTO = new OddDTO();
                                    oddDTO.Type = eOddType.FulltimeOverUnder;
                                    oddDTO.ID = javaScriptArray[29].ToString();
                                    oddDTO.Odd = javaScriptArray[30].ToString();
                                    if (float.TryParse(javaScriptArray[31].ToString(), out num))
                                    {
                                        oddDTO.Home = num;
                                    }
                                    else
                                    {
                                        oddDTO.Home = 0f;
                                    }
                                    if (float.TryParse(javaScriptArray[32].ToString(), out num))
                                    {
                                        oddDTO.Away = num;
                                    }
                                    else
                                    {
                                        oddDTO.Away = 0f;
                                    }
#if DDEBUG
                                    Utilities.WriteLog.Write("~o~:ibet:" +
                                        matchDTO2.HomeTeamName + "["
                                        + oddDTO.HomeFavor.ToString() + "]"
                                        + " - "
                                        + matchDTO2.AwayTeamName + "["
                                        + oddDTO.AwayFavor.ToString() + "]"
                                        + " >> add odd:"
                                        + oddDTO.Type.ToString()
                                        + ". "
                                        + oddDTO.Odd
                                        + " Price Home:"
                                        + oddDTO.Home.ToString()
                                        + ", Away:"
                                        + oddDTO.Away.ToString());
#endif
                                    matchDTO2.Odds.Add(oddDTO);
                                }
                                if (javaScriptArray[37] != null && javaScriptArray[37].ToString() != string.Empty)
                                {
                                    OddDTO oddDTO = new OddDTO();
                                    oddDTO.Type = eOddType.FirstHalfHandicap;
                                    oddDTO.ID = javaScriptArray[37].ToString();
                                    oddDTO.Odd = javaScriptArray[38].ToString();
                                    if (float.TryParse(javaScriptArray[39].ToString(), out num))
                                    {
                                        oddDTO.Home = num;
                                    }
                                    else
                                    {
                                        oddDTO.Home = 0f;
                                    }
                                    if (float.TryParse(javaScriptArray[40].ToString(), out num))
                                    {
                                        oddDTO.Away = num;
                                    }
                                    else
                                    {
                                        oddDTO.Away = 0f;
                                    }
                                    if (javaScriptArray[41] == null || javaScriptArray[41].ToString() == string.Empty)
                                    {
                                        oddDTO.HomeFavor = false;
                                        oddDTO.AwayFavor = false;
                                    }
                                    else
                                    {
                                        if (javaScriptArray[41].ToString() == "h")
                                        {
                                            oddDTO.HomeFavor = true;
                                            oddDTO.AwayFavor = false;
                                        }
                                        else
                                        {
                                            if (javaScriptArray[41].ToString() == "a")
                                            {
                                                oddDTO.HomeFavor = false;
                                                oddDTO.AwayFavor = true;
                                            }
                                        }
                                    }
#if DDEBUG
                                    Utilities.WriteLog.Write("~o~:ibet:" +
                                        matchDTO2.HomeTeamName + "["
                                        + oddDTO.HomeFavor.ToString() + "]"
                                        + " - "
                                        + matchDTO2.AwayTeamName + "["
                                        + oddDTO.AwayFavor.ToString() + "]"
                                        + " >> add odd:"
                                        + oddDTO.Type.ToString()
                                        + ". "
                                        + oddDTO.Odd
                                        + " Price Home:"
                                        + oddDTO.Home.ToString()
                                        + ", Away:"
                                        + oddDTO.Away.ToString());
#endif
                                    matchDTO2.Odds.Add(oddDTO);
                                }
                                if (javaScriptArray[42] != null && javaScriptArray[42].ToString() != string.Empty)
                                {
                                    OddDTO oddDTO = new OddDTO();
                                    oddDTO.Type = eOddType.FirstHalfOverUnder;
                                    oddDTO.ID = javaScriptArray[42].ToString();
                                    oddDTO.Odd = javaScriptArray[43].ToString();
                                    if (float.TryParse(javaScriptArray[44].ToString(), out num))
                                    {
                                        oddDTO.Home = num;
                                    }
                                    else
                                    {
                                        oddDTO.Home = 0f;
                                    }
                                    if (float.TryParse(javaScriptArray[45].ToString(), out num))
                                    {
                                        oddDTO.Away = num;
                                    }
                                    else
                                    {
                                        oddDTO.Away = 0f;
                                    }
                                    matchDTO2.Odds.Add(oddDTO);
#if DDEBUG
                                    Utilities.WriteLog.Write("~o~:ibet:" +
                                        matchDTO2.HomeTeamName + "["
                                        + oddDTO.HomeFavor.ToString() + "]"
                                        + " - "
                                        + matchDTO2.AwayTeamName + "["
                                        + oddDTO.AwayFavor.ToString() + "]"
                                        + " >> add odd:"
                                        + oddDTO.Type.ToString()
                                        + ". "
                                        + oddDTO.Odd
                                        + " Price Home:"
                                        + oddDTO.Home.ToString()
                                        + ", Away:"
                                        + oddDTO.Away.ToString());
#endif
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }
        public static System.Collections.Generic.List<MatchDTO> ConvertUpdateData(
            string data,
            System.Collections.Generic.List<MatchDTO> originalDataSource,
            out string updateTime)
        {
            System.Collections.Generic.List<MatchDTO> list = BaseDTO.DeepClone<System.Collections.Generic.List<MatchDTO>>(originalDataSource);
            updateTime = string.Empty;
            data = data.Replace("\n", "");
            string[] array = data.Split(new string[]
			{
				";"
			}, System.StringSplitOptions.None);
            MatchDTO matchDTO = null;
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                if (text.Contains("var Dell"))
                {
                    JavaScriptArray javaScriptArray = (JavaScriptArray)JavaScriptConvert.DeserializeObject(text.Split(new string[]
					{
						"="
					}, System.StringSplitOptions.None)[2]);
                    foreach (object current in javaScriptArray)
                    {
                        MatchDTO matchDTO2 = MatchDTO.SearchMatch(current.ToString(), list);
                        if (matchDTO2 != null)
                        {
                            list.Remove(matchDTO2);
                        }
                    }
                }
                else
                {
                    if (text.StartsWith("Insl"))
                    {
                        JavaScriptArray javaScriptArray = (JavaScriptArray)JavaScriptConvert.DeserializeObject(text.Split(new string[]
						{
							"="
						}, System.StringSplitOptions.None)[1]);
                        if (javaScriptArray[3].ToString() == "1")
                        {
                            MatchDTO matchDTO3 = new MatchDTO();
                            matchDTO3.Odds = new System.Collections.Generic.List<OddDTO>();
                            matchDTO3.ID = javaScriptArray[0].ToString();
                            if (javaScriptArray[4].ToString() == string.Empty)
                            {
                                matchDTO3.League = matchDTO.League;
                            }
                            else
                            {
                                matchDTO3.League = new LeagueDTO();
                                matchDTO3.League.ID = javaScriptArray[4].ToString();
                                matchDTO3.League.Name = javaScriptArray[5].ToString();
                            }
                            matchDTO3.HomeTeamName = javaScriptArray[6].ToString();
                            matchDTO3.AwayTeamName = javaScriptArray[7].ToString();
                            if (javaScriptArray.Count >= 12)
                            {
                                if (javaScriptArray[12] != null && javaScriptArray[12].ToString() != string.Empty && !javaScriptArray[12].ToString().ToLower().Contains("t") && !javaScriptArray[12].ToString().ToLower().Contains("live"))
                                {
                                    string[] array3 = javaScriptArray[12].ToString().ToLower().Split(new string[]
									{
										"h"
									}, System.StringSplitOptions.None);
                                    matchDTO3.Half = int.Parse(array3[0]);
                                    matchDTO3.Minute = int.Parse(array3[1].Trim().Replace("'", ""));
                                    matchDTO3.IsHalfTime = false;
                                }
                                else
                                {
                                    matchDTO3.Minute = 0;
                                    matchDTO3.Half = 0;
                                    matchDTO3.IsHalfTime = true;
                                }
                            }
                            else
                            {
                                matchDTO3.Minute = 0;
                                matchDTO3.Half = 0;
                                matchDTO3.IsHalfTime = true;
                            }
                            float num = 0f;
                            if (javaScriptArray[24] != null && javaScriptArray[24].ToString() != string.Empty)
                            {
                                OddDTO oddDTO = new OddDTO();
                                oddDTO.Type = eOddType.FulltimeHandicap;
                                oddDTO.ID = javaScriptArray[24].ToString();
                                oddDTO.Odd = javaScriptArray[25].ToString();
                                if (float.TryParse(javaScriptArray[26].ToString(), out num))
                                {
                                    oddDTO.Home = num;
                                }
                                else
                                {
                                    oddDTO.Home = 0f;
                                }
                                if (float.TryParse(javaScriptArray[27].ToString(), out num))
                                {
                                    oddDTO.Away = num;
                                }
                                else
                                {
                                    oddDTO.Away = 0f;
                                }
                                if (javaScriptArray[28] == null || javaScriptArray[28].ToString() == string.Empty)
                                {
                                    oddDTO.HomeFavor = false;
                                    oddDTO.AwayFavor = false;
                                }
                                else
                                {
                                    if (javaScriptArray[28].ToString() == "h")
                                    {
                                        oddDTO.HomeFavor = true;
                                        oddDTO.AwayFavor = false;
                                    }
                                    else
                                    {
                                        if (javaScriptArray[28].ToString() == "a")
                                        {
                                            oddDTO.HomeFavor = false;
                                            oddDTO.AwayFavor = true;
                                        }
                                    }
                                }
                                matchDTO3.Odds.Add(oddDTO);
                            }
                            if (javaScriptArray[29] != null && javaScriptArray[29].ToString() != string.Empty)
                            {
                                OddDTO oddDTO = new OddDTO();
                                oddDTO.Type = eOddType.FulltimeOverUnder;
                                oddDTO.ID = javaScriptArray[29].ToString();
                                oddDTO.Odd = javaScriptArray[30].ToString();
                                if (float.TryParse(javaScriptArray[31].ToString(), out num))
                                {
                                    oddDTO.Home = num;
                                }
                                else
                                {
                                    oddDTO.Home = 0f;
                                }
                                if (float.TryParse(javaScriptArray[32].ToString(), out num))
                                {
                                    oddDTO.Away = num;
                                }
                                else
                                {
                                    oddDTO.Away = 0f;
                                }
                                matchDTO3.Odds.Add(oddDTO);
                            }
                            if (javaScriptArray[37] != null && javaScriptArray[37].ToString() != string.Empty)
                            {
                                OddDTO oddDTO = new OddDTO();
                                oddDTO.Type = eOddType.FirstHalfHandicap;
                                oddDTO.ID = javaScriptArray[37].ToString();
                                oddDTO.Odd = javaScriptArray[38].ToString();
                                if (float.TryParse(javaScriptArray[39].ToString(), out num))
                                {
                                    oddDTO.Home = num;
                                }
                                else
                                {
                                    oddDTO.Home = 0f;
                                }
                                if (float.TryParse(javaScriptArray[40].ToString(), out num))
                                {
                                    oddDTO.Away = num;
                                }
                                else
                                {
                                    oddDTO.Away = 0f;
                                }
                                if (javaScriptArray[41] == null || javaScriptArray[41].ToString() == string.Empty)
                                {
                                    oddDTO.HomeFavor = false;
                                    oddDTO.AwayFavor = false;
                                }
                                else
                                {
                                    if (javaScriptArray[41].ToString() == "h")
                                    {
                                        oddDTO.HomeFavor = true;
                                        oddDTO.AwayFavor = false;
                                    }
                                    else
                                    {
                                        if (javaScriptArray[41].ToString() == "a")
                                        {
                                            oddDTO.HomeFavor = false;
                                            oddDTO.AwayFavor = true;
                                        }
                                    }
                                }
                                matchDTO3.Odds.Add(oddDTO);
                            }
                            if (javaScriptArray[42] != null && javaScriptArray[42].ToString() != string.Empty)
                            {
                                OddDTO oddDTO = new OddDTO();
                                oddDTO.Type = eOddType.FirstHalfOverUnder;
                                oddDTO.ID = javaScriptArray[42].ToString();
                                oddDTO.Odd = javaScriptArray[43].ToString();
                                if (float.TryParse(javaScriptArray[44].ToString(), out num))
                                {
                                    oddDTO.Home = num;
                                }
                                else
                                {
                                    oddDTO.Home = 0f;
                                }
                                if (float.TryParse(javaScriptArray[45].ToString(), out num))
                                {
                                    oddDTO.Away = num;
                                }
                                else
                                {
                                    oddDTO.Away = 0f;
                                }
                                matchDTO3.Odds.Add(oddDTO);
                            }
                            list.Add(matchDTO3);
                            matchDTO = matchDTO3;
                        }
                        else
                        {
                            float num = 0f;
                            if (javaScriptArray[24] != null && javaScriptArray[24].ToString() != string.Empty)
                            {
                                OddDTO oddDTO = new OddDTO();
                                oddDTO.Type = eOddType.FulltimeHandicap;
                                oddDTO.ID = javaScriptArray[24].ToString();
                                oddDTO.Odd = javaScriptArray[25].ToString();
                                if (float.TryParse(javaScriptArray[26].ToString(), out num))
                                {
                                    oddDTO.Home = num;
                                }
                                else
                                {
                                    oddDTO.Home = 0f;
                                }
                                if (float.TryParse(javaScriptArray[27].ToString(), out num))
                                {
                                    oddDTO.Away = num;
                                }
                                else
                                {
                                    oddDTO.Away = 0f;
                                }
                                if (javaScriptArray[28] == null || javaScriptArray[28].ToString() == string.Empty)
                                {
                                    oddDTO.HomeFavor = false;
                                    oddDTO.AwayFavor = false;
                                }
                                else
                                {
                                    if (javaScriptArray[28].ToString() == "h")
                                    {
                                        oddDTO.HomeFavor = true;
                                        oddDTO.AwayFavor = false;
                                    }
                                    else
                                    {
                                        if (javaScriptArray[28].ToString() == "a")
                                        {
                                            oddDTO.HomeFavor = false;
                                            oddDTO.AwayFavor = true;
                                        }
                                    }
                                }
                                matchDTO.Odds.Add(oddDTO);
                            }
                            if (javaScriptArray[29] != null && javaScriptArray[29].ToString() != string.Empty)
                            {
                                OddDTO oddDTO = new OddDTO();
                                oddDTO.Type = eOddType.FulltimeOverUnder;
                                oddDTO.ID = javaScriptArray[29].ToString();
                                oddDTO.Odd = javaScriptArray[30].ToString();
                                if (float.TryParse(javaScriptArray[31].ToString(), out num))
                                {
                                    oddDTO.Home = num;
                                }
                                else
                                {
                                    oddDTO.Home = 0f;
                                }
                                if (float.TryParse(javaScriptArray[32].ToString(), out num))
                                {
                                    oddDTO.Away = num;
                                }
                                else
                                {
                                    oddDTO.Away = 0f;
                                }
                                matchDTO.Odds.Add(oddDTO);
                            }
                            if (javaScriptArray[37] != null && javaScriptArray[37].ToString() != string.Empty)
                            {
                                OddDTO oddDTO = new OddDTO();
                                oddDTO.Type = eOddType.FirstHalfHandicap;
                                oddDTO.ID = javaScriptArray[37].ToString();
                                oddDTO.Odd = javaScriptArray[38].ToString();
                                if (float.TryParse(javaScriptArray[39].ToString(), out num))
                                {
                                    oddDTO.Home = num;
                                }
                                else
                                {
                                    oddDTO.Home = 0f;
                                }
                                if (float.TryParse(javaScriptArray[40].ToString(), out num))
                                {
                                    oddDTO.Away = num;
                                }
                                else
                                {
                                    oddDTO.Away = 0f;
                                }
                                if (javaScriptArray[41] == null || javaScriptArray[41].ToString() == string.Empty)
                                {
                                    oddDTO.HomeFavor = false;
                                    oddDTO.AwayFavor = false;
                                }
                                else
                                {
                                    if (javaScriptArray[41].ToString() == "h")
                                    {
                                        oddDTO.HomeFavor = true;
                                        oddDTO.AwayFavor = false;
                                    }
                                    else
                                    {
                                        if (javaScriptArray[41].ToString() == "a")
                                        {
                                            oddDTO.HomeFavor = false;
                                            oddDTO.AwayFavor = true;
                                        }
                                    }
                                }
                                matchDTO.Odds.Add(oddDTO);
                            }
                            if (javaScriptArray[42] != null && javaScriptArray[42].ToString() != string.Empty)
                            {
                                OddDTO oddDTO = new OddDTO();
                                oddDTO.Type = eOddType.FirstHalfOverUnder;
                                oddDTO.ID = javaScriptArray[42].ToString();
                                oddDTO.Odd = javaScriptArray[43].ToString();
                                if (float.TryParse(javaScriptArray[44].ToString(), out num))
                                {
                                    oddDTO.Home = num;
                                }
                                else
                                {
                                    oddDTO.Home = 0f;
                                }
                                if (float.TryParse(javaScriptArray[45].ToString(), out num))
                                {
                                    oddDTO.Away = num;
                                }
                                else
                                {
                                    oddDTO.Away = 0f;
                                }
                                matchDTO.Odds.Add(oddDTO);
                            }
                        }
                    }
                    if (text.StartsWith("uLl"))
                    {
                        JavaScriptArray javaScriptArray = (JavaScriptArray)JavaScriptConvert.DeserializeObject(text.Split(new string[]
						{
							"="
						}, System.StringSplitOptions.None)[1]);
                        MatchDTO matchDTO4 = MatchDTO.SearchMatch(javaScriptArray[0].ToString(), list);
                        if (matchDTO4 != null)
                        {
                            string text2 = javaScriptArray[1].ToString().Replace("'", "").Replace(" ", "").ToLower();
                            if (text2.Contains("t") || text2.Contains("live"))
                            {
                                matchDTO4.Half = 0;
                                matchDTO4.Minute = 0;
                                matchDTO4.IsHalfTime = true;
                            }
                            else
                            {
                                matchDTO4.IsHalfTime = false;
                                matchDTO4.Half = int.Parse(text2.Split(new string[]
								{
									"h"
								}, System.StringSplitOptions.None)[0]);
                                matchDTO4.Minute = int.Parse(text2.Split(new string[]
								{
									"h"
								}, System.StringSplitOptions.None)[1]);
                            }
                        }
                    }
                    else
                    {
                        if (text.StartsWith("uOl"))
                        {
                            JavaScriptArray javaScriptArray = (JavaScriptArray)JavaScriptConvert.DeserializeObject(text.Split(new string[]
							{
								"="
							}, System.StringSplitOptions.None)[1]);
                            MatchDTO matchDTO4 = MatchDTO.SearchMatch(javaScriptArray[0].ToString(), list);
                            if (matchDTO4 != null)
                            {
                                OddDTO oddDTO2 = OddDTO.SearchOdd(javaScriptArray[2].ToString(), matchDTO4.Odds);
                                if (oddDTO2 != null)
                                {
                                    matchDTO4.Odds.Remove(oddDTO2);
                                }
                                if (javaScriptArray[3].ToString() != string.Empty && javaScriptArray[4].ToString() != string.Empty && javaScriptArray[5].ToString() != string.Empty)
                                {
                                    string text3 = javaScriptArray[1].ToString();
                                    OddDTO oddDTO = new OddDTO();
                                    oddDTO.ID = javaScriptArray[2].ToString();
                                    oddDTO.Odd = javaScriptArray[3].ToString();
                                    oddDTO.Home = float.Parse(javaScriptArray[4].ToString());
                                    oddDTO.Away = float.Parse(javaScriptArray[5].ToString());
                                    if (javaScriptArray.Count >= 7)
                                    {
                                        if (javaScriptArray[6].ToString().ToLower() == "h")
                                        {
                                            oddDTO.HomeFavor = true;
                                            oddDTO.AwayFavor = false;
                                        }
                                        else
                                        {
                                            if (javaScriptArray[6].ToString().ToLower() == "a")
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
                                    string text4 = text3;
                                    if (text4 == null)
                                    {
                                        goto IL_1134;
                                    }
                                    if (!(text4 == "1"))
                                    {
                                        if (!(text4 == "3"))
                                        {
                                            if (!(text4 == "7"))
                                            {
                                                if (!(text4 == "8"))
                                                {
                                                    goto IL_1134;
                                                }
                                                oddDTO.Type = eOddType.FirstHalfOverUnder;
                                            }
                                            else
                                            {
                                                oddDTO.Type = eOddType.FirstHalfHandicap;
                                            }
                                        }
                                        else
                                        {
                                            oddDTO.Type = eOddType.FulltimeOverUnder;
                                        }
                                    }
                                    else
                                    {
                                        oddDTO.Type = eOddType.FulltimeHandicap;
                                    }
                                IL_113F:
                                    if (oddDTO.Type != eOddType.Unknown)
                                    {
                                        matchDTO4.Odds.Add(oddDTO);
                                    }
                                    goto IL_115F;
                                IL_1134:
                                    oddDTO.Type = eOddType.Unknown;
                                    goto IL_113F;
                                }
                            IL_115F: ;
                            }
                        }
                        else
                        {
                            if (text.StartsWith("window"))
                            {
                                updateTime = text.Substring(43, 19);
                            }
                        }
                    }
                }
            }
            return list;
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
            if (data == null || data == string.Empty)
            {
                allowance = false;
                minStake = 0;
                maxStake = 0;
                betKindValue = string.Empty;
            }
            else
            {
                data = data.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "");
                int num = data.IndexOf("'lblBetKindValue':'") + 19;
                int num2 = data.IndexOf("','lblOddsValue':'");
                betKindValue = data.Substring(num, num2 - num);
                num = data.IndexOf("'hiddenMinBet':'") + 16;
                num2 = data.IndexOf("','hiddenMaxBet':'");
                minStake = int.Parse(data.Substring(num, num2 - num).Replace(",", "").Trim());
                num = data.IndexOf("'hiddenMaxBet':'") + 16;
                num2 = data.IndexOf("','hiddenBetType':'");
                maxStake = int.Parse(data.Substring(num, num2 - num).Replace(",", "").Trim());
                num = data.IndexOf("'hiddenOddsRequest':'") + 21;
                num2 = data.IndexOf("','hiddenMinBet':'");
                string a = data.Substring(num, num2 - num);
                num = data.IndexOf("'lblOddsValue':'") + 16;
                num2 = data.IndexOf("','lblPlaceOddsValue':'");
                string b = data.Substring(num, num2 - num);
                if (a == b)
                {
                    allowance = true;
                }
                else
                {
                    allowance = false;
                }
            }
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
                betKindValue = string.Empty;
            }
            else
            {
                try
                {
                    data = data.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                    int num = data.IndexOf("'lblBetKindValue':'") + 19;
                    int num2 = data.IndexOf("',            'lblOddsValue':'");
                    betKindValue = data.Substring(num, num2 - num);
                    num = data.IndexOf("'hiddenMinBet':'") + 16;
                    num2 = data.IndexOf("',            'hiddenMaxBet':'");
                    minStake = int.Parse(data.Substring(num, num2 - num).Replace(",", "").Trim());
                    num = data.IndexOf("'hiddenMaxBet':'") + 16;
                    num2 = data.IndexOf("',            'hiddenBetType':'");
                    maxStake = int.Parse(data.Substring(num, num2 - num).Replace(",", "").Trim());
                    num = data.IndexOf("'hiddenOddsRequest':'") + 21;
                    num2 = data.IndexOf("',            'hiddenMinBet':'");
                    string a = data.Substring(num, num2 - num);
                    num = data.IndexOf("'lblOddsValue':'") + 16;
                    num2 = data.IndexOf("',            'lblPlaceOddsValue':'");
                    string b = data.Substring(num, num2 - num);
                    num = data.IndexOf("'lblHome':'") + 11;
                    num2 = data.IndexOf("',             'lblAway':'");
                    homeTeamName = data.Substring(num, num2 - num).Replace("'", "");
                    num = data.IndexOf("'lblAway':'") + 10;
                    num2 = data.IndexOf("',            'lblLeaguename':'");
                    awayTeamName = data.Substring(num, num2 - num).Replace("'", "");

                    float numReq = float.Parse(a);
                    float numRes = float.Parse(b);

                    if (a == b)// || (numReq <= numRes && numReq > 0) || (numReq <= numRes && numReq < 0))
                    {
                        allowance = true;
                    }
                    else
                    {
                        allowance = false;
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
                    throw ex;
                }
            }
        }
        public static void ProcessPrepareBet2(string data, out bool allowance, out int minStake, out int maxStake, out string betKindValue, out string homeTeamName, out string awayTeamName, out string newOddValue, out string homescore, out string awayscore)
        {
            allowance = false;
            minStake = 0;
            maxStake = 0;
            betKindValue = string.Empty;
            homeTeamName = string.Empty;
            awayTeamName = string.Empty;
            newOddValue = string.Empty;
            homescore = string.Empty;
            awayscore = string.Empty;
            if (data == null || data == string.Empty || data.Contains("'close'"))
            {
                allowance = false;
                minStake = 0;
                maxStake = 0;
                betKindValue = string.Empty;
            }
            else
            {
                try
                {
                    //Regex regex = new Regex("lblBetKind':'(?<BetKindValue>.*)',\\s*'lblHome':'(?<HomeTeam>.*)',\\s*'lblAway':'(?<AwayTeam>.*)',\\s*'lblOddsValue':'(?<OddsValue>.*)',(\\s*.*){4}'lblScoreValue':'(?<ScoreValue>.*)',(\\s*.*){3}'lblChoiceClass':'(?<ChoiceClass>.*)',\\s*.*lblChoiceValue':'(?<ChoiceValue>.*)',(\\s*.*){3}'hiddenOddsRequest':'(?<OddsRequest>.*)',\\s*.*hiddenMinBet':'(?<MinBetValue>.*)',\\s*.*hiddenMaxBet':'(?<MaxBetValue>.*)',(\\s*.*){3}lbloddsStatus':'(?<oddsStatus>.*)',");
                    //Match match = regex.Match(data);
                    //if (match.Success)
                    //{
                    //    maxStake = int.Parse(match.Groups["MaxBetValue"].Value);
                    //    minStake = int.Parse(match.Groups["MinBetValue"].Value);
                    //    homeTeamName = match.Groups["HomeTeam"].Value;
                    //    awayTeamName = match.Groups["AwayTeam"].Value;
                    //}
                    data = data.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                    int num = data.IndexOf("'lblBetKindValue':'") + 19;
                    int num2 = data.IndexOf("','lblOddsValue':'");
                    betKindValue = data.Substring(num, num2 - num);
                    num = data.IndexOf("'hiddenMinBet':'") + 16;
                    num2 = data.IndexOf("','hiddenMaxBet':'");
                    minStake = int.Parse(data.Substring(num, num2 - num).Replace(",", "").Trim());
                    num = data.IndexOf("'hiddenMaxBet':'") + 16;
                    num2 = data.IndexOf("','hiddenBetType':'");
                    maxStake = int.Parse(data.Substring(num, num2 - num).Replace(",", "").Trim());
                    num = data.IndexOf("'hiddenOddsRequest':'") + 21;
                    num2 = data.IndexOf("','hiddenMinBet':'");
                    string a = data.Substring(num, num2 - num);
                    num = data.IndexOf("'lblOddsValue':'") + 16;
                    num2 = data.IndexOf("','lblPlaceOddsValue':'");
                    string b = data.Substring(num, num2 - num);
                    num = data.IndexOf("'lblHome':'") + 11;
                    num2 = data.IndexOf("', 'lblAway':'");
                    homeTeamName = data.Substring(num, num2 - num).Replace("'", "");
                    num = data.IndexOf("'lblAway':'") + 10;
                    num2 = data.IndexOf("','lblLeaguename':'");
                    awayTeamName = data.Substring(num, num2 - num).Replace("'", "");

                    
                    num = data.IndexOf("'lblScoreValue':'") + 18;
                    num2 = data.IndexOf("','hideTicketBox':'");

                    string score = "";
                    if (num2 - num > 2)
                    {
                        score = data.Substring(num, num2 - num).Replace("'", "").Replace("[", "").Replace("]", "");
                        string[] array = score.Split(new string[] { "-" }, System.StringSplitOptions.None);
                        homescore = array[0];
                        awayscore = array[1];                        
                    }
                    else
                    {
                        homescore = "0";
                        awayscore = "0";
                    }

                    newOddValue = b;

                    float numReq = float.Parse(a);
                    float numRes = float.Parse(b);

                    if (a == b)// || (numReq <= numRes && numReq > 0) || (numReq <= numRes && numReq < 0))
                    {
                        allowance = true;
                    }
                    else
                    {
                        allowance = false;
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
                    newOddValue = string.Empty;
                    throw ex;
                }
            }
        }
        public static void ProcessConfirmBet(string data, out bool success)
        {
            success = false;
            if (data == null || data == string.Empty)
            {
                success = false;
            }
            else
            {
                if (data.ToLower().Contains("success") || data.ToLower().Contains("blistmini"))
                {
                    success = true;
                }
            }
        }
    }
}
