//#define DEBUG
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using iBet.DTO;

namespace iBet.Engine
{
    public class IBetSubEngine : BaseEngine
    {
        private System.Net.CookieContainer _cookies;
        public bool _detectedAccount = false;
        public AccountDTO _highestFollowingAccount;// = new AccountDTO();
        public bool useProxy = false;
        private string _host;
        public float _currentCredit = 0f;
        public string type = "";
        private string _dynamicURL;
        private string _setCookie;
        private System.Collections.Generic.List<BetDTO> _lastListBetList;
        private System.Collections.Generic.List<AccountDTO> _lastAccountOutStandingList;
        public System.Collections.Generic.Dictionary<string, System.DateTime> _OldBetListHistory;
        private Timer _updateDataTimer;

        private int _updateDataInterval = 6000;
        public event EngineDelegate FullDataCompleted;
        public event EngineDelegate GetOutStandingDataCompleted;

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
        public IBetSubEngine(string host, string dynamicURL, string setCookie, System.Net.CookieContainer cookies)
        {
            this._host = host;
            this._setCookie = setCookie;
            this._dynamicURL = dynamicURL;
            this._cookies = cookies;
            this.InitializeObjects();
        }
        public void Start()
        {            
            _highestFollowingAccount = new AccountDTO();
            _detectedAccount = false;
            this.GetSubInfo();
            this._updateDataTimer.Start();
        }
        public void Stop()
        {
            this._updateDataTimer.Interval = this._updateDataInterval;
            this._updateDataTimer.Stop();
            _highestFollowingAccount = new AccountDTO();

        }
        private void InitializeObjects()
        {
            this._updateDataTimer = new Timer();
            this._updateDataTimer.Interval = this._updateDataInterval;
            this._updateDataTimer.Tick += new System.EventHandler(this._updateDataTimer_Tick);
            this._OldBetListHistory = new Dictionary<string, DateTime>();
        }
        private void _updateDataTimer_Tick(object sender, System.EventArgs e)
        {
            if (this.LastUpdateCompleted)
            {
                //this.GetUpdateData();
                if (_detectedAccount)
                    this.GetAccountData(this._highestFollowingAccount);
                else
                    this.GetSubInfo();
            }
        }

        private void GetSubInfo()
        {
            this.LastUpdateCompleted = false;
            try
            {
                string requestUriString = string.Concat(new string[]
				{                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/Common/Outstanding.aspx"
				});                

                string text = CreateWebRequest("GET",
                    requestUriString,
                    "https://" + this._host + "/" + this._dynamicURL + "/_Menu/Menu.aspx",
                    this._cookies,
                    this._host, useProxy, "");

                if (text == null || text == string.Empty)
                {
                    this.On_GetOutStandingDataCompleted(new EngineEventArgs
                    {
                        Type = eEngineEventType.SessionExpired,
                        Data = null
                    });
                }
                else
                {
                    try
                    {
                        this._lastAccountOutStandingList = ConvertAcountsOutStanding(text);
                        this.On_GetOutStandingDataCompleted(new EngineEventArgs
                        {
                            Type = eEngineEventType.Success,
                            Data = this._lastAccountOutStandingList
                        });
                    }
                    catch (System.Exception data)
                    {
                        this.On_GetOutStandingDataCompleted(new EngineEventArgs
                        {
                            Type = eEngineEventType.Error,
                            Data = data
                        });
                    }
                }
            }
            catch
            {

            }
            finally
            {
                this.LastUpdateCompleted = true;
            }
        }
        private void GetSubInfo(AccountDTO account)
        {
            this.LastUpdateCompleted = false;
            try
            {
                string requestUriString = string.Empty;
                if (account.AccountType == "Member")
                {
                    requestUriString = string.Concat(new string[]
				    {                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/BetList/OutstandingDetail.aspx?custid=",
                    account.ID,
                    "&username=",
                    account.Name,
                    "&type=RunByDate_Mem&chkgetall=on&chk_showsb=on&chk_showcasino=off&chk_showrb=on&chk_showng=on&chk_showbi=off"					
				    });

                    //this._getDataRequest.CookieContainer.Add(new System.Uri("http://" + this._host), new System.Net.Cookie(this._setCookie, requestUriString));
                }
                else if (account.AccountType == "Agent")
                {
                    requestUriString = string.Concat(new string[]
				    {                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/Common/Outstanding.aspx?custid=",
                    account.ID,
                    "&roleid=2&sortingcolumn=username&sortingup=true"                    
				    });
                }
                else if (account.AccountType == "Master")
                {
                    requestUriString = string.Concat(new string[]
				    {                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/Common/Outstanding.aspx?custid=",
                    account.ID,
                    "&roleid=3&sortingcolumn=username&sortingup=true"                    
				    });
                }
                string text = CreateWebRequest("GET",
                        requestUriString,
                        "http://" + this._host + "/" + this._dynamicURL + "/_Reports/Common/Outstanding.aspx",
                        this._cookies,
                        this._host,
                        useProxy, "");

                if (text == null || text == string.Empty)
                {
                    this.On_GetOutStandingDataCompleted(new EngineEventArgs
                    {
                        Type = eEngineEventType.SessionExpired,
                        Data = null
                    });
                }
                else
                {
                    try
                    {
                        if (account.AccountType != "Member")
                        {
                            this._lastAccountOutStandingList = ConvertAcountsOutStanding(text);
                            this.On_GetOutStandingDataCompleted(new EngineEventArgs
                            {
                                Type = eEngineEventType.Success,
                                Data = this._lastAccountOutStandingList
                            });
                        }
                        else
                        {

                        }
                    }
                    catch (System.Exception data)
                    {
                        this.On_GetOutStandingDataCompleted(new EngineEventArgs
                        {
                            Type = eEngineEventType.Error,
                            Data = data
                        });
                    }
                }
            }
            catch
            {

            }
            finally
            {
                this.LastUpdateCompleted = true;
            }
        }

        public System.Collections.Generic.List<BetDTO> GetBetsDataFirstTime(AccountDTO account)
        {
            this.LastUpdateCompleted = false;
            System.Collections.Generic.List<BetDTO> list = null;
            try
            {
                string requestUriString = string.Concat(new string[]
				{                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/BetList/OutstandingDetail.aspx?custid=",
                    account.ID,
                    "&username=",
                    account.Name,
                    "&type=RunByDate_Mem&chkgetall=on&chk_showsb=on&chk_showcasino=off&chk_showrb=on&chk_showng=on&chk_showbi=off"					
				});

                string text = CreateWebRequest("GET",
                    requestUriString,
                    "https://" + this._host + "/" + this._dynamicURL + "/_Reports/Common/Outstanding.aspx",
                    this._cookies,
                    this._host,
                    useProxy, "");
                //this._getDataRequest.CookieContainer.Add(new System.Uri("http://" + this._host), new System.Net.Cookie(this._setCookie, requestUriString));
                //string text = ParseRequest(this._getDataRequest, "");

                if (text == null || text == string.Empty)
                {
                    return list;
                }
                else
                {
                    try
                    {
                        return ConvertData(text, _OldBetListHistory, account.Name);
                    }
                    catch (System.Exception data)
                    {
                        //this.On_FullDataCompleted(new EngineEventArgs
                        //{
                        //    Type = eEngineEventType.Error,
                        //    Data = data
                        //});

                        return list;
                    }
                }
            }
            catch (System.Exception data)
            {
                //iBet.Utilities.WriteLog.Write(data.Message);
                //iBet.Utilities.WriteLog.Write(text);
                return list;

                //GetBetsDataFirstTime(account);
            }
            finally
            {
                this.LastUpdateCompleted = true;
            }

        }
        public List<AccountDTO> GetAccountDataFirstTime(AccountDTO account)
        {
            List<AccountDTO> list = new List<AccountDTO>();
            this.LastUpdateCompleted = false;
            try
            {
                string requestUriString = string.Empty;
                if (account.AccountType == "Member")
                {
                    requestUriString = string.Concat(new string[]
				    {                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/BetList/OutstandingDetail.aspx?custid=",
                    account.ID,
                    "&username=",
                    account.Name,
                    "&type=RunByDate_Mem&chkgetall=on&chk_showsb=on&chk_showcasino=off&chk_showrb=on&chk_showng=on&chk_showbi=off"					
				    });

                    //this._getDataRequest.CookieContainer.Add(new System.Uri("http://" + this._host), new System.Net.Cookie(this._setCookie, requestUriString));
                }
                else if (account.AccountType == "Agent")
                {
                    requestUriString = string.Concat(new string[]
				    {                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/Common/Outstanding.aspx?custid=",
                    account.ID,
                    "&roleid=2&sortingcolumn=username&sortingup=true"                    
				    });
                }
                else if (account.AccountType == "Master")
                {
                    requestUriString = string.Concat(new string[]
				    {                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/Common/Outstanding.aspx?custid=",
                    account.ID,
                    "&roleid=3&sortingcolumn=username&sortingup=true"                    
				    });
                }
                string text = CreateWebRequest("GET",
                        requestUriString,
                        "http://" + this._host + "/" + this._dynamicURL + "/_Reports/Common/Outstanding.aspx",
                        this._cookies,
                        this._host,
                        useProxy, "");



                if (text == null || text == string.Empty)
                {

                }
                else
                {
                    try
                    {
                        if (account.AccountType == "Member")
                        {
                            return ConvertAcountsOutStanding(text);
                        }
                        else if (account.AccountType == "Master")
                        {
                            return ConvertAcountsOutStanding(text);
                        }
                        else if (account.AccountType == "Agent")
                        {
                            return ConvertAcountsOutStanding(text);
                        }
                    }
                    catch (System.Exception data)
                    {

                    }
                }
            }
            catch (System.Exception data)
            {
                this.GetAccountDataFirstTime(account);
                throw data;
            }
            finally
            {
                this.LastUpdateCompleted = true;
            }
            return list;
        }

        public void GetAccountData(AccountDTO account)
        {
            this.LastUpdateCompleted = false;
            try
            {
                string requestUriString = string.Empty;
                if (account.AccountType == "Member")
                {
                    requestUriString = string.Concat(new string[]
				    {                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/BetList/OutstandingDetail.aspx?custid=",
                    account.ID,
                    "&username=",
                    account.Name,
                    "&type=RunByDate_Mem&chkgetall=on&chk_showsb=on&chk_showcasino=off&chk_showrb=on&chk_showng=on&chk_showbi=off"					
				    });

                    //this._getDataRequest.CookieContainer.Add(new System.Uri("http://" + this._host), new System.Net.Cookie(this._setCookie, requestUriString));
                }
                else if (account.AccountType == "Agent")
                {
                    requestUriString = string.Concat(new string[]
				    {                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/Common/Outstanding.aspx?custid=",
                    account.ID,
                    "&roleid=2&sortingcolumn=username&sortingup=true"                    
				    });
                }
                else if (account.AccountType == "Master")
                {
                    requestUriString = string.Concat(new string[]
				    {                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/Common/Outstanding.aspx?custid=",
                    account.ID,
                    "&roleid=3&sortingcolumn=username&sortingup=true"                    
				    });
                }
                else if (account.AccountType == "SuperMaster")
                {
                    requestUriString = string.Concat(new string[]
				    {                    
					"https://",
					this._host,
                    "/",
                    this._dynamicURL,
                    "/_Reports/Common/Outstanding.aspx"
				    });
                }
                string text = CreateWebRequest("GET",
                        requestUriString,
                        "https://" + this._host + "/" + this._dynamicURL + "/_Reports/Common/Outstanding.aspx",
                        this._cookies,
                        this._host,
                        useProxy, "");

                //string text = ParseRequest(this._getDataRequest, "");

                if (text == null || text == string.Empty)
                {
                    this.On_FullDataCompleted(new EngineEventArgs
                    {
                        Type = eEngineEventType.SessionExpired,
                        Data = null
                    });
                }
                else
                {
                    try
                    {
                        if (account.AccountType == "Member")
                        {
                            this._lastListBetList = ConvertData(text, _OldBetListHistory, account.Name);
                            this.On_FullDataCompleted(new EngineEventArgs
                            {
                                Type = eEngineEventType.Success,
                                Data = this._lastListBetList
                            });
                        }
                        else
                        {

                            this._lastAccountOutStandingList = ConvertAcountsOutStanding(text);
                            this.On_GetOutStandingDataCompleted(new EngineEventArgs
                            {
                                Type = eEngineEventType.Success,
                                Data = this._lastAccountOutStandingList
                            });
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
            catch (System.Exception data)
            {
                this.GetAccountData(account);
                throw data;
            }
            finally
            {
                this.LastUpdateCompleted = true;
            }
        }


        static System.Collections.Generic.List<BetDTO> ConvertData(string data, System.Collections.Generic.Dictionary<string, System.DateTime> oldBetList, string accountName)
        {
            System.Collections.Generic.List<BetDTO> list = null;
            if (data != string.Empty)
            {
                if (!data.Contains("No information is available"))
                {
                    list = new List<BetDTO>();
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(data);

                    foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table/tr/td/table"))
                    {
                        foreach (HtmlNode row in table.SelectNodes("tr"))
                        {
                            //iBet.Utilities.WriteLog.Write("------------Bet-------------");
                            HtmlNodeCollection cells = row.SelectNodes("td");
                            string refID = cells[1].InnerText.Substring(8, 10);
                            if (row.InnerText.Contains("Soccer"))
                            {
                                //string stringEntry = cells[2].FirstChild.FirstChild.FirstChild.InnerText;
                                string stringEntry = cells[2].FirstChild.FirstChild.NextSibling.InnerText;
                                if (!stringEntry.Contains("Mix Parlay"))
                                {
                                    BetDTO betDTO = new BetDTO();
                                    betDTO.Account = accountName;
                                    betDTO.RefID = refID;
                                    betDTO.DateTime = DateTime.Parse(cells[1].FirstChild.NextSibling.InnerText);

                                    string string1 = string.Empty, string2 = string.Empty, string3 = string.Empty, string4 = string.Empty;
                                    string[] array1 = null, array2 = null;

                                    try
                                    {
                                        if (stringEntry != string.Empty && !stringEntry.Contains("FT.") && !stringEntry.Contains("HT."))
                                        {
                                            betDTO.Dumb = false;
                                            //betDTO.Choice = cells[2].FirstChild.FirstChild.FirstChild.FirstChild.InnerText;
                                            betDTO.Choice = cells[2].FirstChild.FirstChild.NextSibling.FirstChild.InnerText.Trim();
                                            betDTO.Odd = cells[2].FirstChild.FirstChild.NextSibling.FirstChild.NextSibling.InnerText.Trim();

                                            //if (cells[2].FirstChild.FirstChild.FirstChild.FirstChild.NextSibling.LastChild.InnerHtml.Contains("["))
                                            //string1 = cells[2].FirstChild.FirstChild.FirstChild.FirstChild.NextSibling.NextSibling.NextSibling.InnerText.Trim().Replace("[", "").Replace("]", "");
                                            if (stringEntry.Contains("["))
                                            {
                                                array1 = stringEntry.Split(new string[] { "[" }, System.StringSplitOptions.None);
                                                array2 = array1[1].Split(new string[] { "]" }, System.StringSplitOptions.None);
                                                string1 = array2[0];
                                            }

                                            //string1 = cells[2].FirstChild.FirstChild.FirstChild.FirstChild.NextSibling.NextSibling.NextSibling.InnerText.Trim().Replace("[", "").Replace("]", "");
                                            string2 = cells[2].FirstChild.FirstChild.NextSibling.InnerText;
                                            string3 = cells[2].FirstChild.FirstChild.NextSibling.NextSibling.NextSibling.InnerText;
                                            string4 = cells[2].FirstChild.FirstChild.NextSibling.NextSibling.InnerText;

                                            betDTO.League = cells[2].FirstChild.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                                        }
                                        else
                                        {
                                            string stringEntry2 = cells[2].FirstChild.FirstChild.FirstChild.NextSibling.FirstChild.InnerText;
                                            bool _htChoice = stringEntry2.Contains("HT.");
                                            bool _ftChoice = stringEntry2.Contains("FT.");

                                            if (_htChoice || _ftChoice)
                                            {
                                                betDTO.Dumb = false;
                                                betDTO.Choice = stringEntry2.Replace("&nbsp;", "");
                                                betDTO.Odd = "12";
                                                try
                                                {
                                                    string1 = cells[2].FirstChild.FirstChild.FirstChild.NextSibling.FirstChild.NextSibling.InnerText.Replace("[", "").Replace("]", "");
                                                }
                                                catch
                                                {
                                                    string1 = string.Empty;
                                                }
                                                string2 = cells[2].FirstChild.FirstChild.NextSibling.NextSibling.FirstChild.InnerText;
                                                string3 = cells[2].FirstChild.FirstChild.NextSibling.NextSibling.NextSibling.FirstChild.InnerText;
                                                string4 = cells[2].FirstChild.FirstChild.NextSibling.NextSibling.InnerText;
                                                betDTO.League = cells[2].FirstChild.FirstChild.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling.FirstChild.NextSibling.InnerText;
                                            }
                                            else
                                            {
                                                betDTO.Dumb = false;
                                                betDTO.Choice = stringEntry2;
                                                betDTO.Odd = cells[2].FirstChild.FirstChild.NextSibling.FirstChild.NextSibling.InnerText;

                                                if (cells[2].FirstChild.FirstChild.NextSibling.LastChild.InnerHtml.Contains("["))
                                                    string1 = cells[2].FirstChild.FirstChild.FirstChild.NextSibling.FirstChild.NextSibling.NextSibling.NextSibling.InnerText.Trim().Replace("[", "").Replace("]", "");

                                                string2 = cells[2].FirstChild.FirstChild.NextSibling.NextSibling.FirstChild.InnerText;
                                                string3 = cells[2].FirstChild.FirstChild.NextSibling.NextSibling.NextSibling.FirstChild.InnerText;
                                                string4 = cells[2].FirstChild.FirstChild.NextSibling.NextSibling.InnerText;

                                                betDTO.League = cells[2].FirstChild.FirstChild.FirstChild.NextSibling.NextSibling.NextSibling.NextSibling.FirstChild.NextSibling.InnerText.Replace("&nbsp;", "");
                                            }
                                        }
                                        //
                                        if (betDTO.Dumb != true)
                                        {
                                            if (string1 != string.Empty)
                                            {
                                                betDTO.Score = string1;
                                                array1 = string1.Split(new string[] { "-" }, System.StringSplitOptions.None);
                                                betDTO.HomeScore = array1[0];
                                                betDTO.AwayScore = array1[1];
                                                betDTO.Live = true;
                                            }
                                            else
                                            {
                                                betDTO.Score = "?-?";
                                                betDTO.HomeScore = "0";
                                                betDTO.AwayScore = "0";
                                                betDTO.Live = false;
                                            }
                                            if (string3.Contains("nbsp"))
                                            {
                                                array2 = string3.Split(new string[] { "&nbsp;-&nbsp;vs&nbsp;-&nbsp;" }, System.StringSplitOptions.None);
                                                betDTO.HomeTeamName = array2[0];
                                                betDTO.AwayTeamName = array2[1].TrimEnd(' ');
                                            }
                                            else
                                            {
                                                //iBet.Utilities.WriteLog.Write("Can not parse name of match: " + string3);

                                            }
                                            if (string4 != string.Empty)
                                            {
                                                if (string4.Contains("1st Handicap"))
                                                    betDTO.Type = eOddType.FirstHalfHandicap;
                                                else if (string4.Contains("1st Over"))
                                                    betDTO.Type = eOddType.FirstHalfOverUnder;
                                                else if (string4.Contains("Handicap"))
                                                    betDTO.Type = eOddType.FulltimeHandicap;
                                                else if (string4.Contains("Over"))
                                                    betDTO.Type = eOddType.FulltimeOverUnder;
                                                else if (string4.Contains("FT.1X2"))
                                                    betDTO.Type = eOddType.FT;
                                                else if (string4.Contains("1st 1X2"))
                                                    betDTO.Type = eOddType.HT;
                                                else
                                                    betDTO.Type = eOddType.Unknown;
                                            }
                                            betDTO.OddValue = cells[3].FirstChild.InnerText;
                                            betDTO.Stake = int.Parse(cells[4].InnerText.Replace(",", ""));
                                            //if (cells[5].FirstChild.InnerText.StartsWith("Run"))
                                            //    betDTO.Status = true;
                                            //else
                                            //    betDTO.Status = false;
                                            //betDTO.IP = cells[5].FirstChild.NextSibling.InnerText;

                                        }
                                        //
                                    }
                                    catch (Exception ex)
                                    {
                                        betDTO.Dumb = true;
                                        //iBet.Utilities.WriteLog.Write("----bet convert Dumb----");
                                        //iBet.Utilities.WriteLog.Write("Meassge: " + ex);
                                        //iBet.Utilities.WriteLog.Write(row.InnerHtml);
                                        //iBet.Utilities.WriteLog.Write("----end----");
                                    }
                                    if (betDTO.Dumb == false)
                                    {
                                        list.Add(betDTO);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return list;
        }
        static System.Collections.Generic.List<AccountDTO> ConvertAcountsOutStanding(string data)
        {
            System.Collections.Generic.List<AccountDTO> list = null;
            if (data != string.Empty)
            {
                if (!data.Contains("No information is available"))
                {
                    string type = string.Empty;
                    if (data.Contains("Master Outstanding"))
                    {
                        type = "Master";
                    }
                    else if (data.Contains("Agent Outstanding"))
                    {
                        type = "Agent";
                    }
                    else if (data.Contains("Member Outstanding"))
                    {
                        type = "Member";
                    }

                    list = new List<AccountDTO>();
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(data);

                    foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
                    {
                        foreach (HtmlNode row in table.SelectNodes("tr"))
                        {
                            AccountDTO account = new AccountDTO();
                            account.AccountType = type;
                            HtmlNodeCollection cells = row.SelectNodes("td");
                            string stringEntry = cells[0].InnerText;
                            if (!stringEntry.Contains("User") && !stringEntry.Contains("standing") && !stringEntry.Contains("Total"))
                            {
                                account.Name = stringEntry;
                                if (account.AccountType == "Member")
                                {
                                    string[] array = cells[1].InnerHtml.Split(new string[] { "'" }, System.StringSplitOptions.None);
                                    account.ID = array[1];
                                }
                                else if (account.AccountType == "Agent")
                                {
                                    string[] array = cells[0].InnerHtml.Split(new string[] { "'" }, System.StringSplitOptions.None);
                                    account.ID = array[1];
                                }
                                else if (account.AccountType == "Master")
                                {
                                    string[] array = cells[0].InnerHtml.Split(new string[] { "'" }, System.StringSplitOptions.None);
                                    account.ID = array[1];
                                }
                                account.OutStanding = cells[1].InnerText.Trim();
                                list.Add(account);
                            }
                        }
                    }
                }
            }
            return list;
        }

        private void On_FullDataCompleted(EngineEventArgs eventArgs)
        {
            if (this.FullDataCompleted != null)
            {
                this.FullDataCompleted(this, eventArgs);
            }
        }
        private void On_GetOutStandingDataCompleted(EngineEventArgs eventArgs)
        {
            if (this.GetOutStandingDataCompleted != null)
            {
                this.GetOutStandingDataCompleted(this, eventArgs);
            }
        }
    }
}
