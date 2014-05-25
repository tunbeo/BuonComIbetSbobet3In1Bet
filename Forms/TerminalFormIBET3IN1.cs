//#define DEBUG
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WMPLib;
using iBet.App.admin;
using iBet.DTO;
using iBet.Engine;
using iBet.Utilities;

namespace iBet.App
{
	public class TerminalFormIBET3IN1 : RibbonForm
	{
		private const int INTERNET_COOKIE_HTTPONLY = 8192;
		private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
		private IBetEngine _ibetEngine;		
        private ThreeIn1BetEngine _3in1Engine;
		private System.Collections.Generic.List<MatchDTO> _listIBETMatch;
		private System.Collections.Generic.List<MatchDTO> _list3in1betMatch;
		private System.Collections.Generic.List<MatchDTO> _listSameMatch;
		private System.Collections.Generic.List<TransactionDTO> _listTransaction;
		private System.Collections.Generic.Dictionary<string, System.DateTime> _oddTransactionHistory;
		private bool _running = false;
		private bool _comparing = false;
		private bool _compareAgain = false;
		private System.Windows.Forms.Timer _forceRefreshTimer;
        private System.Windows.Forms.Timer _matchesRefreshTimer;
		private System.Windows.Forms.Timer _creditRefreshTimer;
        private string _ibetAccount = string.Empty;
        private string _3in1Account = string.Empty;
        private admin.DataServiceSoapClient _dataService;
		private string _currentUserID;
		private System.DateTime _lastTransactionTime = System.DateTime.Now;
		private System.Uri _ibetDataUrl;
        private System.Uri _3in1DataUrl;
		private MainForm _mainForm;
		private System.ComponentModel.IContainer components = null;
		private RibbonControl ribbonControl1;
		private RibbonPage ribbonPage1;
		private RibbonPageGroup rpg3in1bet;
		private BarStaticItem barStaticItem1;
		private BarStaticItem barStaticItem2;
		private BarStaticItem barStaticItem3;
		private BarButtonItem btn3in1betGetInfo;
		private BarStaticItem lbl3in1betCurrentCredit;
		private BarStaticItem lbl3in1betTotalMatch;
		private BarStaticItem lbl3in1betLastUpdate;
		private RibbonPageGroup rpgIbet;
		private RibbonPageGroup ribbonPageGroup3;
		private BarStaticItem barStaticItem7;
		private BarStaticItem barStaticItem8;
		private BarStaticItem barStaticItem9;
		private BarStaticItem lblIbetCurrentCredit;
		private BarStaticItem lblIbetTotalMatch;
		private BarStaticItem lblIbetLastUpdate;
		private BarButtonItem btnIbetGetInfo;
		private SplitContainerControl splitContainerControl1;
		private XtraTabControl xtraTabControl1;
		private XtraTabPage xtraTabPage1;
		private XtraTabPage xtraTabPage3;
        private XtraTabPage xtraTabPage9;
		private XtraTabControl xtraTabControl2;
		private XtraTabPage xtraTabPage4;
		private XtraTabPage xtraTabPage5;
		private BarStaticItem lblStatus;
		private BarStaticItem lblSameMatch;
		private BarStaticItem lblLastUpdate;
		private BarButtonItem btnStart;
		private BarButtonItem btnStop;
		private RibbonPageGroup ribbonPageGroup4;
		private BarButtonItem btnClear;
		private XtraTabPage xtraTabPage2;
		private PanelControl panelControl3;
		private WebBrowser web3IN1BET;
        
		private PanelControl panelControl2;
		private SimpleButton btn3IN1BETGO;
		private TextEdit txt1IN1BETAddress;
		private LabelControl labelControl1;
		private PanelControl panelControl5;
		private PanelControl panelControl4;
		private SimpleButton btnIBETGO;
		private TextEdit txtIBETAddress;
		private LabelControl labelControl5;
		private GridControl grdSameMatch;
		private GridView gridView1;
		private GridColumn gridColumn15;
		private GridColumn gridColumn16;
		private GridColumn gridColumn17;
		private GridColumn gridColumn18;
		private GridColumn gridColumn19;
		private GroupControl groupControl5;
        private CheckEdit checkEdit9; // sound
        private CheckEdit checkEdit8; // money
        private CheckEdit checkEdit7; // rung
		private CheckEdit checkEdit6;
		private CheckEdit checkEdit5;
		private CheckEdit checkEdit4;
		private CheckEdit checkEdit3;
		private GroupControl groupControl4;
        private GroupControl groupControl6;
		private MemoEdit txtStake;
		private CheckEdit chbRandomStake;
		private SpinEdit txtIBETFixedStake;
		private LabelControl labelControl6;
		private GridControl grdTransaction;
		private GridView gridView2;
		private GridColumn gridColumn1;
		private GridColumn gridColumn2;
		private GridColumn gridColumn3;
		private GridColumn gridColumn6;
		private GridColumn gridColumn4;
		private GridColumn gridColumn5;
		private GridColumn gridColumn7;
		private GridColumn gridColumn8;
		private GridColumn gridColumn9;
		private GridColumn gridColumn10;
		private GridColumn gridColumn11;
		private GridColumn gridColumn12;
		private GridColumn gridColumn13;
		private GridColumn gridColumn20;
		private SpinEdit txt3in1BETFixedStake;
		private LabelControl labelControl2;
		private WebBrowser webIBET;
		private GroupControl groupControl1;
		private SpinEdit txt3IN1BETUpdateInterval;
		private LabelControl labelControl3;
		private SpinEdit txtIBETUpdateInterval;
		private LabelControl labelControl4;
		private GroupControl groupControl2;
		private CheckEdit chbAllowHalftime;
		private SpinEdit txtMaxTimePerHalf;
		private LabelControl labelControl7;
		private GridColumn gridColumn14;
		private SpinEdit txtOddValueDifferenet;
		private LabelControl labelControl8;
		private SimpleButton btnSetUpdateInterval;
		private SpinEdit txtLowestOddValue;
		private LabelControl labelControl9;
		private SpinEdit txtTransactionTimeSpan;
		private LabelControl labelControl10;
		private CheckEdit chbHighRevenueBoost;

        private DevExpress.XtraEditors.CheckedListBoxControl chkListAllowedMatch;        
        
		[System.Runtime.InteropServices.DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, System.Text.StringBuilder pchCookieData, ref uint pcchCookieData, int dwFlags, System.IntPtr lpReserved);

		[System.Runtime.InteropServices.DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, System.IntPtr dwReserved);

		[System.Runtime.InteropServices.DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InternetSetOption(System.IntPtr hInternet, int dwOption, System.IntPtr lpBuffer, int dwBufferLength);        

		private static System.Net.CookieContainer GetUriCookieContainer(string uri)
		{
			System.Net.CookieContainer cookieContainer = null;
			uint num = 256u;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(256);
			System.Net.CookieContainer result;
			if (!TerminalFormIBET3IN1.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
			{
				if (num < 0u)
				{
					result = null;
					return result;
				}
				num = 1024u;
				stringBuilder = new System.Text.StringBuilder(1024);
				if (!TerminalFormIBET3IN1.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
				{
					result = null;
					return result;
				}
			}
			if (stringBuilder.Length > 0)
			{
				cookieContainer = new System.Net.CookieContainer();
				cookieContainer.SetCookies(new System.Uri(uri), stringBuilder.ToString().Replace(';', ','));
			}
			result = cookieContainer;
			return result;
		}
		private static string GetASPNETSessionID(string uri)
		{
			uint num = 256u;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(256);
			string result;
			if (!TerminalFormIBET3IN1.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
			{
				if (num < 0u)
				{
					result = null;
					return result;
				}
				num = 1024u;
				stringBuilder = new System.Text.StringBuilder(1024);
				if (!TerminalFormIBET3IN1.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
				{
					result = null;
					return result;
				}
			}
			if (stringBuilder.Length > 0)
			{
				System.Net.CookieContainer cookieContainer = new System.Net.CookieContainer();
				cookieContainer.SetCookies(new System.Uri(uri), stringBuilder.ToString().Replace(';', ','));
			}
			result = stringBuilder.ToString().Split(new string[]
			{
				"="
			}, System.StringSplitOptions.None)[1];
			return result;
		}
		private static string GetCookieString(string uri)
		{
			uint num = 256u;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(256);
			string result;
			if (!TerminalFormIBET3IN1.InternetGetCookieEx(uri, null, stringBuilder, ref num, 8192, System.IntPtr.Zero))
			{
				if (num < 0u)
				{
					result = null;
					return result;
				}
				num = 1024u;
				stringBuilder = new System.Text.StringBuilder(1024);
				if (!TerminalFormIBET3IN1.InternetGetCookieEx(uri, null, stringBuilder, ref num, 8192, System.IntPtr.Zero))
				{
					result = null;
					return result;
				}
			}
			result = stringBuilder.ToString();
			return result;
		}
		public TerminalFormIBET3IN1(MainForm mainForm, string currentUserID)
		{
			this.InitializeComponent();
			this._mainForm = mainForm;
            this._dataService = new App.admin.DataServiceSoapClient();
			this._dataService.AllowRunCompleted += new EventHandler<AllowRunCompletedEventArgs>(this._dataService_AllowRunCompleted);
			this._currentUserID = currentUserID;
			
            this._forceRefreshTimer = new System.Windows.Forms.Timer();
			this._forceRefreshTimer.Interval = 180000;
			this._forceRefreshTimer.Tick += new System.EventHandler(this._forceRefreshTimer_Tick);

			this._creditRefreshTimer = new System.Windows.Forms.Timer();
			this._creditRefreshTimer.Interval = 180000;
			this._creditRefreshTimer.Tick += new System.EventHandler(this._creditRefreshTimer_Tick);

            //this._matchesRefreshTimer = new System.Windows.Forms.Timer();
            //this._matchesRefreshTimer.Interval = 60000;
            //this._matchesRefreshTimer.Tick += new System.EventHandler(this._matchesRefreshTimer_Tick);

			this._oddTransactionHistory = new System.Collections.Generic.Dictionary<string, System.DateTime>();
			this._listTransaction = new System.Collections.Generic.List<TransactionDTO>();

            //this.web3IN1BET.Url = new System.Uri("http://" + _mainForm._list3in1Host[_mainForm.numTerminal] + ".3in1bet.com", System.UriKind.Absolute);

            //TransactionDTO tranTest = new TransactionDTO(); 
            //tranTest.AwayTeamName = "test"; tranTest.HomeTeamName = "test";
            //this._listTransaction.Add(tranTest);

			this.grdTransaction.DataSource = this._listTransaction;
			this._listSameMatch = new System.Collections.Generic.List<MatchDTO>();
			this.grdSameMatch.DataSource = this._listSameMatch;
		}
		private void _dataService_AllowRunCompleted(object sender, AllowRunCompletedEventArgs e)
		{
			if (e.Result)
			{
#if DEBUG
                //Utilities.WriteLog.Write("+ Response from Server:u're allowed to run this program");
#endif
				this.Start();
				//this._dataService.StartTerminalAsync(this._currentUserID, this.Text);
				this._ibetEngine.UpdateDataInterval = (int)this.txtIBETUpdateInterval.Value * 1000;
                this._3in1Engine.UpdateDataInterval = (int)this.txt3IN1BETUpdateInterval.Value * 1000;				
			}
			else
			{
				this.ShowWarningDialog("Your logged in account is not permitted to start the current betting pair account. \nPlease contact with Administrators.");
			}
		}
        private void btnStart_ItemClick(object sender, ItemClickEventArgs e)
        {            
            this._dataService.AllowRunAsync(this._currentUserID, this._ibetAccount.ToUpper(), this._3in1Account.ToUpper());
        }
		private void _creditRefreshTimer_Tick(object sender, System.EventArgs e)
		{
			BarItem barItem1 = this.lblIbetCurrentCredit;
			float currentCredit = this._ibetEngine.GetCurrentCredit();
            barItem1.Caption = currentCredit.ToString();

            BarItem barItem2 = this.lbl3in1betCurrentCredit;
			
            currentCredit = this._3in1Engine.GetCurrentCredit();
            barItem2.Caption = currentCredit.ToString();
		}
		private void _forceRefreshTimer_Tick(object sender, System.EventArgs e)
		{
			if (this._ibetEngine != null)
			{
				this._ibetEngine.Stop();
				this._ibetEngine.Start();
			}
			
            if (this._3in1Engine != null)
			{
                this._3in1Engine.Stop();
                this._3in1Engine.Start();
			}
		}

        private void _matchesRefreshTimer_Tick(object sender, System.EventArgs e)
        {
            //RefreshAllowedListMatches();
        }

		internal void Start()
		{
			if (this._ibetEngine != null)
			{
				this._ibetEngine.Start();
#if DEBUG
                Utilities.WriteLog.Write("+ iBet is running");
#endif
			}
            this._running = true;
            if (this._3in1Engine != null)
			{				
                this._3in1Engine.Start();
#if DEBUG
                Utilities.WriteLog.Write("+ 3in1 is running");
#endif
			}
			//this._running = true;
			this._forceRefreshTimer.Start();
			this._creditRefreshTimer.Start();
            //this._matchesRefreshTimer.Start();
			this.lblStatus.Caption = "RUNNING";
			this.btnStart.Enabled = false;
			this.btnStop.Enabled = true;
			this.btnClear.Enabled = true;
			this.btnIbetGetInfo.Enabled = false;
			this.btn3in1betGetInfo.Enabled = false;
		}
		internal void StartFromTracking()
		{
			this._dataService.AllowRunAsync(this._currentUserID, this._ibetAccount.ToUpper(), this._3in1Account.ToUpper());
		}
		internal void Stop()
		{
			if (this._ibetEngine != null)
			{
				this._ibetEngine.Stop();
#if DEBUG
                Utilities.WriteLog.Write("+ ibet was stopped!");
#endif
			} 
			
            if (this._3in1Engine != null)
			{
                this._3in1Engine.Stop();
#if DEBUG
                Utilities.WriteLog.Write("+ 3in1bet was stopped!");
#endif
			}
			this._running = false;
			this._forceRefreshTimer.Stop();
			this._creditRefreshTimer.Stop();
            //this._matchesRefreshTimer.Stop();
			this.lblStatus.Caption = "STOPPED";
			this.btnStart.Enabled = true;
			this.btnStop.Enabled = false;
			this.btnClear.Enabled = false;
			this.btnIbetGetInfo.Enabled = true;
			this.btn3in1betGetInfo.Enabled = true;
		}
		private void InitializeIBETEngine()
		{
			if (this._ibetEngine != null)
			{
				this._ibetEngine.Stop();
				System.GC.SuppressFinalize(this._ibetEngine);
			}
			string host = this._ibetDataUrl.Host;
			System.Net.CookieContainer cookieContainer = new System.Net.CookieContainer();
			string[] array = TerminalFormIBET3IN1.GetCookieString(this._ibetDataUrl.AbsoluteUri).Split(new string[]
			{
				";"
			}, System.StringSplitOptions.None);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				string[] array3 = text.Split(new string[]
				{
					"="
				}, System.StringSplitOptions.None);
				if (text.Contains("DispVer"))
				{
					cookieContainer.Add(new System.Uri("http://" + this.webIBET.Url.Host), new System.Net.Cookie("DispVer", "1"));
				}
				else
				{
					cookieContainer.Add(new System.Uri("http://" + this.webIBET.Url.Host), new System.Net.Cookie(array3[0].Trim(), array3[1].Trim()));
				}                                

			}
			string[] array4 = this._ibetDataUrl.Query.Split(new string[]
			{
				"&"
			}, System.StringSplitOptions.None);
			string[] array5 = array4[array4.Length - 1].Split(new string[]
			{
				"="
			}, System.StringSplitOptions.None);
			string dynamicFieldName = array5[0];
			string dynamicFieldValue = array5[1];
			string innerHtml = this.webIBET.Document.Body.Parent.InnerHtml;
			int num = innerHtml.IndexOf("UserName");
			int num2 = innerHtml.IndexOf("imgServerURL");
			string text2 = innerHtml.Substring(num, num2 - num);
			string text3 = text2.Split(new string[]
			{
				";"
			}, System.StringSplitOptions.None)[0].Split(new string[]
			{
				"\""
			}, System.StringSplitOptions.None)[1];
			this.rpgIbet.Text = "IBET - " + text3;
			this._ibetAccount = text3;
			this.Text = string.Concat(new object[]
			{
				this._ibetAccount, 
				" - ", 
				this._3in1Account, 
				" - Version: ", 
				System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
			});
			this._ibetEngine = new IBetEngine(host, text3, dynamicFieldName, dynamicFieldValue, cookieContainer);
			this._ibetEngine.FullDataCompleted += new EngineDelegate(this._ibetEngine_FullDataCompleted);
			this._ibetEngine.UpdateCompleted += new EngineDelegate(this._ibetEngine_UpdateCompleted);            
			this.lblIbetCurrentCredit.Caption = this._ibetEngine.GetCurrentCredit().ToString();
            if (_3in1Engine != null)
            {
                this.btnStart.Enabled = true;
            }
#if DEBUG
            Utilities.WriteLog.Write(".......... IBET engine is started ............");            
#endif        
        }
        private void Initialize3in1Engine()
        {
            if (this._3in1Engine != null)
            {
                this._3in1Engine.Stop();
                System.GC.SuppressFinalize(this._3in1Engine);
            }
            
            string host = this._3in1DataUrl.Host;
            System.Net.CookieContainer cookieContainer = new System.Net.CookieContainer();
            
            string[] array = TerminalFormIBET3IN1.GetCookieString(this._3in1DataUrl.AbsoluteUri).Split(new string[]
			{
				";"
			}, System.StringSplitOptions.None);
                        
#if DEBUG
            //Utilities.WriteLog.Write("3in1 host:" + host + "; Cookies counted:" + array.Length);
#endif
            for (int i = 0; i < array.Length; i++)
            {

                string[] array3 = array[i].Split(new string[]
				{
					"="
				}, System.StringSplitOptions.None);

                cookieContainer.Add(new System.Uri("http://" + host), new System.Net.Cookie(array3[0].Trim(), array3[1].Trim()));
            }
                        
            this._3in1Engine = new ThreeIn1BetEngine(host, "asd", cookieContainer);
            this._3in1Engine.UpdateCompleted += new EngineDelegate(this._3in1betEngine_UpdateCompleted);
            this._3in1Engine.FullDataCompleted += new EngineDelegate(this._3in1betEngine_FullDataCompleted);
            this.lbl3in1betCurrentCredit.Caption = this._3in1Engine.GetCurrentCredit().ToString();
            
            string frame1 = web3IN1BET.Document.Window.Frames[1].Document.Body.OuterHtml;
            string[] parts1 = frame1.Split(new string[] { "var C = \"" }, StringSplitOptions.None);
            if (parts1.Length > 0)
            {
                string[] parts2 = parts1[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                this._3in1Engine._secretNumber = parts2[0];
                
                string[] parts3 = parts1[1].Split(new string[] { "name=rfsBtn>" }, StringSplitOptions.None);
                string[] parts4 = parts3[1].Split(new string[] { "<" }, StringSplitOptions.None);
                this._3in1Account = parts4[0].ToUpper();
                string secretnum = parts2[0];
                this._3in1Engine._secretNumber = secretnum;
#if DEBUG
                Utilities.WriteLog.Write("Secret number of server:" + secretnum);
                Utilities.WriteLog.Write("Username:" + this._3in1Account);
                //testReturn(this._3in1Engine);
#endif
            }
#if DEBUG2
            HtmlWindowCollection frm = web3IN1BET.Document.Window.Frames;
            for (int i = 0; i < frm.Count; i++)
            {
                string str = frm[i].Document.Body.OuterHtml;
                Utilities.WriteLog.Write("+++ FRM:" + i.ToString() + str);
            }            
#endif
            this.Text = string.Concat(new object[]
			{
				this._ibetAccount, 
				" - ", 
				this._3in1Account, 
				" - Version: ", 
				System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
			});
            if (_ibetEngine != null)
            {
                this.btnStart.Enabled = true;
            }
#if DEBUG
            Utilities.WriteLog.Write(":::::::::::::3in1bet engine is started ::::::::::::::::");            
#endif

        }
#if DEBUG
        private void testReturn(object enginetest)
        {
            Utilities.WriteLog.Write(enginetest.GetType().ToString());
            iBet.Engine.ThreeIn1BetEngine _3in1Egn = (ThreeIn1BetEngine) enginetest;
            Utilities.WriteLog.Write(_3in1Egn._secretNumber);
            Utilities.WriteLog.Write(_3in1Egn._currentCredit.ToString());
        }
#endif

		private void webIBET_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			this.txtIBETAddress.Text = this.webIBET.Url.AbsoluteUri;
			if (e.Url.AbsoluteUri.Contains("ibet888.net/UnderOver_data.aspx?Market=l&Sport=1&DT=&RT=W&CT=&Game=0&OrderBy=0"))
			{
				this._ibetDataUrl = e.Url;
			}
		}
		private void web3IN1BET_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			this.txt1IN1BETAddress.Text = this.web3IN1BET.Url.AbsoluteUri;
            this._3in1DataUrl = e.Url;
            //WriteLog.Write(txt1IN1BETAddress.Text);
            //WriteLog.Write(_3in1DataUrl.ToString());
		}

        private void web3in1Bet_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.txt1IN1BETAddress.Text = this.web3IN1BET.Url.AbsoluteUri;
        }
		private void _3in1betEngine_FullDataCompleted(BaseEngine sender, EngineEventArgs eventArgs)
		{
			switch (eventArgs.Type)
			{
				case eEngineEventType.Success:
				{
					if (this._list3in1betMatch != null)
					{
						System.GC.SuppressFinalize(this._list3in1betMatch);
					}
					this._list3in1betMatch = BaseDTO.DeepClone<System.Collections.Generic.List<MatchDTO>>((System.Collections.Generic.List<MatchDTO>)eventArgs.Data);
					this.StartCompareSameMatch();
                    //this.RefreshAllowedListMatches();
					this.lbl3in1betLastUpdate.Caption = System.DateTime.Now.ToString();

					break;
				}
				case eEngineEventType.SessionExpired:
				{
					this.Stop();
					this.lbl3in1betLastUpdate.Caption = "Session Expired";
					break;
				}
			}
		}
        private void _3in1betEngine_UpdateCompleted(BaseEngine sender, EngineEventArgs eventArgs)
        {
            switch (eventArgs.Type)
            {
                case eEngineEventType.Success:
                    {
                        if (this._list3in1betMatch != null)
                        {
                            System.GC.SuppressFinalize(this._list3in1betMatch);
                        }
                        this._list3in1betMatch = BaseDTO.DeepClone<List<MatchDTO>>((List<MatchDTO>)eventArgs.Data);
                        this.lbl3in1betLastUpdate.Caption = System.DateTime.Now.ToString();
                        this.StartCompareSameMatch();                        
                        break;
                    }
                case eEngineEventType.SessionExpired:
                    {
                        this.Stop();
                        this.lbl3in1betLastUpdate.Caption = "Session Expired";
                        break;
                    }
            }
        }
		private void _ibetEngine_UpdateCompleted(BaseEngine sender, EngineEventArgs eventArgs)
		{
			switch (eventArgs.Type)
			{
				case eEngineEventType.Success:
				{
					if (this._listIBETMatch != null)
					{
						System.GC.SuppressFinalize(this._listIBETMatch);
					}
					this._listIBETMatch = BaseDTO.DeepClone<System.Collections.Generic.List<MatchDTO>>((System.Collections.Generic.List<MatchDTO>)eventArgs.Data);
					this.lblIbetLastUpdate.Caption = System.DateTime.Now.ToString();
					break;
				}
				case eEngineEventType.SessionExpired:
				{
					this.Stop();
					this.lblIbetLastUpdate.Caption = "Session Expired";
					break;
				}
			}
		}

        private void RefreshAllowedListMatches()
        {
            lock (_listSameMatch)
            {
                foreach (MatchDTO match in _listSameMatch)
                {
                    if (chkListAllowedMatch.FindString(match.HomeTeamName + " - " + match.AwayTeamName) == -1)
                    {
                        chkListAllowedMatch.Items.Add(match.HomeTeamName + " - " + match.AwayTeamName);
                    }
                }
            }
        }

		private void _ibetEngine_FullDataCompleted(BaseEngine sender, EngineEventArgs eventArgs)
		{
			switch (eventArgs.Type)
			{
				case eEngineEventType.Success:
				{
					if (this._listIBETMatch != null)
					{
						System.GC.SuppressFinalize(this._listIBETMatch);
					}
					this._listIBETMatch = BaseDTO.DeepClone<System.Collections.Generic.List<MatchDTO>>((System.Collections.Generic.List<MatchDTO>)eventArgs.Data);
					this.lblIbetLastUpdate.Caption = System.DateTime.Now.ToString();                    
					break;
				}
				case eEngineEventType.SessionExpired:
				{
					this.Stop();
					this.lblIbetLastUpdate.Caption = "Session Expired";
					break;
				}
			}
		}
		private void StartCompareSameMatch()
		{
			if (this._running)
			{
				if (this._comparing)
				{
					this._compareAgain = true;
				}
				else
				{
					System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.CompareSameMatch));
					thread.Start();
				}
			}
		}
		private void CompareSameMatch()
		{
			this._comparing = true;
#if DEBUG
            Utilities.WriteLog.Write("+ System is now comparing....");
#endif
			lock (this._listSameMatch)
			{
				System.Collections.Generic.List<MatchDTO> listIBETMatch = this._listIBETMatch;
				System.Collections.Generic.List<MatchDTO> list3in1Match = this._list3in1betMatch;
				if (this._listSameMatch != null)
				{
					this._listSameMatch.Clear();
				}
				else
				{
					this._listSameMatch = new System.Collections.Generic.List<MatchDTO>();
				}
				System.DateTime now = System.DateTime.Now;
				System.TimeSpan timeSpan;
				foreach (MatchDTO current in listIBETMatch) //lay tung tran tu list iBET
				{
#if DEBUG
                    //Utilities.WriteLog.Write("+ .");
#endif
                    MatchDTO matchDTO = MatchDTO.SearchMatch(current, list3in1Match);// tim trong list 3in1 
					if (matchDTO != null) // neu tim thay
					{
#if DEBUG
                        //Utilities.WriteLog.Write("+ Found: " + current.HomeTeamName + " - " + matchDTO.AwayTeamName + " in 2 list");
#endif               
                        this._listSameMatch.Add(current); // add vao list tran trung nhau
                        //int numFound = chkListAllowedMatch.FindString(current.HomeTeamName + " - " + current.AwayTeamName);
                        //if (numFound != -1 && chkListAllowedMatch.Items[numFound].CheckState.ToString() == "Checked")
                        //{                            
                            if (this.chbAllowHalftime.Checked || !current.IsHalfTime) //neu chon check hiep 1
                            {
                                if (current.Minute <= (int)this.txtMaxTimePerHalf.Value) // neu chon check max time hiep 1
                                {
                                    foreach (OddDTO current2 in current.Odds) // lay tung odd cua tran ibet CURRENT2:IBET
                                    {
                                        if (this.AllowOddBet(current2.ID)) // neu co the bet (loai tru trung OddID va thoi gian
                                        {
                                            OddDTO oddDTO = OddDTO.SearchOdd(current2, matchDTO.Odds);//tim oddDTO cua 3in1bet trong so cac odd cua ibet
                                            if (oddDTO != null) // neu tim thay                             oddDTO: 3IN1BET
                                            {
                                                if ((checkEdit4.Checked && ((oddDTO.Type == eOddType.FirstHalfOverUnder || oddDTO.Type == eOddType.FulltimeOverUnder) && (current.Minute < 30 || (checkEdit7.Checked && current.Minute >= 30)))) || (checkEdit3.Checked && (oddDTO.Type == eOddType.FirstHalfHandicap || oddDTO.Type == eOddType.FulltimeHandicap)))
                                                {
                                                    timeSpan = System.DateTime.Now - this._lastTransactionTime;// sau 15 giay tu lan bet sau cung moi bet tiep
                                                    if (timeSpan.TotalSeconds > 15.0)
                                                    {
                                                        //neu thoa man dieu kien: odd thap nhat chap nhan
                                                        if (current2.Home >= (float)this.txtLowestOddValue.Value || 0f - current2.Home >= (float)this.txtLowestOddValue.Value)
                                                        {
                                                            //neu thoa man dieu kien odd cach nhau -0.02
                                                            if (OddDTO.IsValidOddPair(current2.Home, oddDTO.Away, (double)this.txtOddValueDifferenet.Value, this.chbHighRevenueBoost.Checked))
                                                            {
#if DEBUG
                                                                Utilities.WriteLog.Write(" + IBET home: " + current2.Home + " and 3IN1BET away: " + oddDTO.Away + "");
#endif
                                                                #region Ibet_Odd_Home_3in1bet_Odd_Away
                                                                TransactionDTO transactionDTO; //tao transaction
                                                                if (this.chbRandomStake.Checked) // neu check random stake
                                                                {
                                                                    int num = 0;
                                                                    while (num == 0)
                                                                    {
                                                                        string strNum = this.txtStake.Lines[new System.Random().Next(this.txtStake.Lines.Length)];
                                                                        int.TryParse(strNum, out num);
                                                                    }
                                                                    
                                                                    int ibetStake = num;
                                                                    int threein1Stake = num;    //cho 2 stake = nhau
                                                                    MatchDTO matchDTOibet = current; //lay ra 2 match
                                                                    MatchDTO matchDTO3in1 = matchDTO;
                                                                    eOddType eOddtype = current2.Type;
                                                                    string ibetOddID = current2.ID; // lay oddID cua ibet
                                                                    string threein1OddID = oddDTO.ID; //lay oodID cua 3in1
                                                                    string ibetOddType = "h";// ibet HOME
                                                                    string threein1OddType = "Away"; //3in1 AWAY															
                                                                    string ibetOddValue = current2.Home.ToString();

                                                                    transactionDTO = this.PlaceBetAllowMaxBet(
                                                                        matchDTOibet,       //MatchDTO ibetMatch
                                                                        matchDTO3in1,       //MatchDTO threein1betMatch, 
                                                                        eOddtype,           //eOddType oddType, 
                                                                        ibetOddID,          //string ibetOddID
                                                                        ibetOddType,        //string ibetOddType
                                                                        ibetOddValue,       //string ibetOddValue
                                                                        oddDTO,             //OddDTO threein1Odd
                                                                        threein1OddType,    //string threein1OddType
                                                                        ibetStake,          //int ibetStake
                                                                        threein1Stake,      //int threein1betStake
                                                                        this._ibetEngine,   //IBetEngine ibetEngine
                                                                        this._3in1Engine);  //ThreeIn1BetEngine threein1betEngine

                                                                    transactionDTO.OddType = current2.Type.ToString() + " - Home / Away";
#if DEBUG
                                                                    Utilities.WriteLog.Write("+++ TERMINAL Adding transaction I: " 
                                                                        + transactionDTO.HomeTeamName + " - " 
                                                                        + transactionDTO.AwayTeamName + ";"
                                                                        + transactionDTO.OddValue
                                                                        + "; Odd iBet ID:" + current2.ID + ",odd: " + current2.Odd
                                                                        + "; Odd 3in1 ID:" + oddDTO.ID + ", odd: " + oddDTO.Odd
                                                                        );
#endif

                                                                    this.AddTransaction(transactionDTO);
                                                                }
                                                                else
                                                                {
                                                                    int ibetStake = (int)this.txtIBETFixedStake.Value;
                                                                    int threein1Stake = (int)this.txt3in1BETFixedStake.Value;
                                                                    MatchDTO matchDTOibet = current; //lay ra 2 match
                                                                    MatchDTO matchDTO3in1 = matchDTO;
                                                                    eOddType eOddtype = current2.Type;
                                                                    string ibetOddID = current2.ID;
                                                                    string threein1OddID = oddDTO.ID;
                                                                    string ibetOddType = "h";//fuck
                                                                    string threein1OddType = "Away";

                                                                    string ibetOddValue = current2.Home.ToString();
                                                                    float threein1OddValue = oddDTO.Away;

                                                                    //dang le cai nay fai sua
                                                                    transactionDTO = this.PlaceBetAllowMaxBet(
                                                                        matchDTOibet,       //MatchDTO ibetMatch
                                                                        matchDTO3in1,       //MatchDTO threein1betMatch, 
                                                                        eOddtype,           //eOddType oddType, 
                                                                        ibetOddID,          //string ibetOddID
                                                                        ibetOddType,        //string ibetOddType
                                                                        ibetOddValue,       //string ibetOddValue
                                                                        oddDTO,             //OddDTO threein1Odd
                                                                        threein1OddType,    //string threein1OddType
                                                                        ibetStake,          //int ibetStake
                                                                        threein1Stake,      //int threein1betStake
                                                                        this._ibetEngine,   //IBetEngine ibetEngine
                                                                        this._3in1Engine);  //ThreeIn1BetEngine threein1betEngine
                                                                    transactionDTO.OddType = current2.Type.ToString() + " - Home / Away";
                                                                    this.AddTransaction(transactionDTO);
#if DEBUG
                                                                    Utilities.WriteLog.Write("+++ TERMINAL +++ Adding transaction: " + transactionDTO.HomeTeamName + " - " + transactionDTO.AwayTeamName);
#endif
                                                                }
                                                                if (transactionDTO != null && transactionDTO.IBETTrade)
                                                                {
                                                                    this.UpdateOddBetHistory(current2.ID);
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                        if (current2.Away >= (float)this.txtLowestOddValue.Value || 0f - current2.Away >= (float)this.txtLowestOddValue.Value)
                                                        {
                                                            if (OddDTO.IsValidOddPair(current2.Away, oddDTO.Home, (double)this.txtOddValueDifferenet.Value, this.chbHighRevenueBoost.Checked))
                                                            {
#if DEBUG
                                                                Utilities.WriteLog.Write(" + IBET away: " + current2.Away + " and 3IN1BET home: " + oddDTO.Home + "");
#endif
                                                                #region Ibet_Odd_Away_3in1bet_Odd_Home
                                                                TransactionDTO transactionDTO;
                                                                if (this.chbRandomStake.Checked)
                                                                {
                                                                    int num = 0;
                                                                    while (num == 0)
                                                                    {
                                                                        string strNum = this.txtStake.Lines[new System.Random().Next(this.txtStake.Lines.Length)];
                                                                        int.TryParse(strNum, out num);
                                                                    }

                                                                    int ibetStake = num;
                                                                    int threein1Stake = num;
                                                                    MatchDTO matchDTOibet = current;
                                                                    MatchDTO matchDTO3in1 = matchDTO;
                                                                    eOddType eOddtype = current2.Type;
                                                                    string ibetOddID = current2.ID;
                                                                    string threein1OddID = oddDTO.ID;
                                                                    string ibetOddType = "a";//fuck
                                                                    string threein1OddType = "Home";
                                                                    string ibetOddValue = current2.Away.ToString();

                                                                    transactionDTO = this.PlaceBetAllowMaxBet(
                                                                        matchDTOibet,       //MatchDTO ibetMatch
                                                                        matchDTO3in1,       //MatchDTO threein1betMatch, 
                                                                        eOddtype,           //eOddType oddType, 
                                                                        ibetOddID,          //string ibetOddID
                                                                        ibetOddType,        //string ibetOddType
                                                                        ibetOddValue,       //string ibetOddValue
                                                                        oddDTO,             //OddDTO threein1Odd
                                                                        threein1OddType,    //string threein1OddType
                                                                        ibetStake,          //int ibetStake
                                                                        threein1Stake,      //int threein1betStake
                                                                        this._ibetEngine,   //IBetEngine ibetEngine
                                                                        this._3in1Engine);  //ThreeIn1BetEngine threein1betEngine


                                                                    transactionDTO.OddType = current2.Type.ToString() + " - Away / Home";
                                                                    this.AddTransaction(transactionDTO);
#if DEBUG
                                                                    Utilities.WriteLog.Write("+++ TERMINAL Adding transaction II: "
                                                                        + transactionDTO.HomeTeamName + " - "
                                                                        + transactionDTO.AwayTeamName + ";"
                                                                        + transactionDTO.OddValue
                                                                        + "; Odd iBet ID:" + current2.ID + ",odd: " + current2.Odd
                                                                        + "; Odd 3in1 ID:" + oddDTO.ID + ", odd: " + oddDTO.Odd
                                                                        );
#endif
                                                                }
                                                                else
                                                                {
                                                                    int ibetStake = (int)this.txtIBETFixedStake.Value;
                                                                    int threein1Stake = (int)this.txt3in1BETFixedStake.Value;
                                                                    MatchDTO matchDTOibet = current;
                                                                    MatchDTO matchDTO3in1 = matchDTO;
                                                                    eOddType eOddtype = current2.Type;
                                                                    string ibetOddID = current2.ID;
                                                                    string threein1OddID = oddDTO.ID;
                                                                    string ibetOddType = "a";//fuck
                                                                    string threein1OddType = "Home";
                                                                    float threein1OddValue = current2.Away;
                                                                    string ibetOddValue = threein1OddValue.ToString();
                                                                    threein1OddValue = oddDTO.Home;

                                                                    transactionDTO = this.PlaceBetAllowMaxBet(
                                                                        matchDTOibet,       //MatchDTO ibetMatch
                                                                        matchDTO3in1,       //MatchDTO threein1betMatch, 
                                                                        eOddtype,           //eOddType oddType, 
                                                                        ibetOddID,          //string ibetOddID
                                                                        ibetOddType,        //string ibetOddType
                                                                        ibetOddValue,       //string ibetOddValue
                                                                        oddDTO,             //OddDTO threein1Odd
                                                                        threein1OddType,    //string threein1OddType
                                                                        ibetStake,          //int ibetStake
                                                                        threein1Stake,      //int threein1betStake
                                                                        this._ibetEngine,   //IBetEngine ibetEngine
                                                                        this._3in1Engine);  //ThreeIn1BetEngine threein1betEngine

                                                                    transactionDTO.OddType = current2.Type.ToString() + " - Away / Home";
                                                                    this.AddTransaction(transactionDTO);
#if DEBUG
                                                                    Utilities.WriteLog.Write("+++ TERMINAL +++ Valid odds pair... Adding transaction: " + transactionDTO.HomeTeamName + " - " + transactionDTO.AwayTeamName);
#endif
                                                                }
                                                                if (transactionDTO != null && transactionDTO.IBETTrade)
                                                                {
                                                                    this.UpdateOddBetHistory(current2.ID);
#if DEBUG
                                                                    Utilities.WriteLog.Write("+++ TERMINAL +++ update bet history:" + current2.ID);
#endif
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        //}                        
					}
				}


				System.DateTime now2 = System.DateTime.Now;
				timeSpan = now2 - now;
				double totalMilliseconds = timeSpan.TotalMilliseconds;
				BarItem arg_648_0 = this.lbl3in1betTotalMatch;
				int count = list3in1Match.Count;
				arg_648_0.Caption = count.ToString();
				BarItem arg_663_0 = this.lblIbetTotalMatch;
				count = listIBETMatch.Count;
				arg_663_0.Caption = count.ToString();
				this.lblSameMatch.Caption = "Total Same Match: " + this._listSameMatch.Count;
#if DEBUG
                Utilities.WriteLog.Write("+++ TERMINAL +++ Total Same Match:" + this._listSameMatch.Count);
#endif
				this.lblLastUpdate.Caption = System.DateTime.Now.ToString();
				lock (this.grdSameMatch)
				{
					this.grdSameMatch.RefreshDataSource();
				}
			}
			this._comparing = false;
			if (this._compareAgain && !this._comparing)
			{
				this._compareAgain = false;
				this.CompareSameMatch();
			}
		}        

		private TransactionDTO PlaceBet(
            MatchDTO ibetMatch,
            MatchDTO threein1betMatch,
            eOddType oddType,
            string ibetOddID,
            string ibetOddType,
            string ibetOddValue,
            OddDTO threein1Odd,
            string threein1OddType,
            int ibetStake,
            int threein1betStake,
            IBetEngine ibetEngine,
            ThreeIn1BetEngine threein1betEngine)
		{
			TransactionDTO transactionDTO = new TransactionDTO();
            string text = string.Empty;
            string text2 = string.Empty;
            string text3 = string.Empty;
			bool flag = false;
			bool flag2 = false;
			int num = 0;
			int num2 = 0;
            string text4 = string.Empty;
            string text5 = string.Empty;
            string text6 = string.Empty;
			bool flag3 = false;
			bool flag4 = false;
			bool sBOBETReTrade = false;
			int num3 = 0;
            
            string text7 = string.Empty;
            string betCount = string.Empty;
            
            string text8 = string.Empty;
			

            bool threein1betAllowance = false;
            int threein1betminStake = 0;
            int threein1betmaxStake = 0;
            string threein1betKindValue = string.Empty;
            string threein1betHomeTeamName = string.Empty;
            string threein1betAwayTeamName = string.Empty;
            string postBODY = string.Empty;  

            if (MatchDTO.IsSameMatch(ibetMatch.HomeTeamName.ToLower(), threein1betMatch.HomeTeamName.ToLower(), ibetMatch.AwayTeamName.ToLower(), threein1betMatch.AwayTeamName.ToLower()))
			{
				try
				{
					ibetEngine.PrepareBet(ibetOddID, ibetOddType, ibetOddValue, ibetStake.ToString(), out flag, out num, out num2, out text4, out text2, out text3);
                    threein1betEngine.PrepareBet(threein1Odd.ID, threein1Odd.Away.ToString(), threein1Odd.Type.ToString().Replace("FulltimeHandicap", "Hdp").Replace("", ""), "Away", threein1Odd.IsHomeGive.ToString().Replace("True", "1").Replace("False", "0"), threein1Odd.Odd, threein1betMatch.HomeScore, threein1betMatch.AwayScore, threein1betMatch.ID, threein1betMatch.IsHalfTime.ToString().Replace("True", "1").Replace("False", "0"), threein1betStake.ToString(),
                        out threein1betAllowance, out threein1betminStake, out threein1betmaxStake, out threein1betKindValue, out threein1betHomeTeamName, out threein1betAwayTeamName, out postBODY);
                    
                    if (MatchDTO.IsSameMatch(text2, text5, text3, text6))
					{
						if (flag && flag3)
						{
							float num4 = float.Parse(text4);
							float num5 = float.Parse(text7);
							if (num4 + num5 == 0f || num4 == num5)
							{
								if (ibetStake <= num2 && threein1betStake <= num3)
								{
									try
									{
										float currentCredit = ibetEngine.GetCurrentCredit();
                                        float currentCredit2 = threein1betEngine.GetCurrentCredit();
										if ((float)ibetStake <= currentCredit && (float)threein1betStake <= currentCredit2)
										{
											try
											{
												ibetEngine.ConfirmBet(oddType, ibetOddValue, ibetStake.ToString(), num.ToString(), num2.ToString(), out flag2);
												if (flag2)
												{
                                                    //threeIn1BetEngine.ConfirmBet(oddType,
                                                    //    threein1betOddValue,
                                                    //    threein1betStake.ToString(), 
                                                    //    betCount, 
                                                    //    text8, 
                                                    //    out flag4);
													if (!flag4)
													{
														try
														{
                                                            //
														}
														catch (System.Exception ex)
														{
															text = "Error while Retrade. Details: " + ex.Message;
														}
													}
													this._lastTransactionTime = System.DateTime.Now;
													object obj = text;
													text = string.Concat(new object[]
													{
														obj, 
														" - Success Transaction. Half: ", 
														ibetMatch.Half, 
														" - Minute: ", 
														ibetMatch.Minute, 
														" - Halftime: ", 
														ibetMatch.IsHalfTime
													});
												}
											}
											catch (System.Exception ex)
											{
												text = "Error while Trading. Details: " + ex.Message;
											}
										}
										else
										{
											text = string.Concat(new object[]
											{
												"Out of Cash. IBET Credit: ", 
												currentCredit, 
												" -  3in1BET Credit: ", 
												currentCredit2
											});
										}
									}
									catch (System.Exception ex)
									{
										text = "Error while getting Credit. Details: " + ex.Message;
									}
								}
								else
								{
									text = string.Concat(new object[]
									{
										"Max Bet. IBET: ", 
										num2, 
										" - 3in1BET: ", 
										num3
									});
								}
							}
							else
							{
								text = "Invalid Odd while Preparing Ticket. IBET Odd: " + text4 + " - 3in1BET Odd:" + text7;
							}
						}
					}
					else
					{
						text = string.Concat(new string[]
						{
							"Not Same Match - Preparing Ticket. IBET: ", 
							text2, 
							" / ", 
							text3, 
							" - 3in1BET: ", 
							text5, 
							" / ", 
							text6
						});
					}
				}
				catch (System.Exception ex)
				{
					text = "Error while Preparing Trade. Details: " + ex.Message;
				}
			}
			else
			{
				text = string.Concat(new string[]
				{
					"Not Same Match - Comparing. IBET: ", 
					ibetMatch.HomeTeamName, 
					" / ", 
					ibetMatch.AwayTeamName, 
					" - 3in1BET: ", 
					threein1betMatch.HomeTeamName, 
					" / ", 
					threein1betMatch.AwayTeamName
				});
			}
            transactionDTO.HomeTeamName = ibetMatch.HomeTeamName + " / " + threein1betMatch.HomeTeamName;
            transactionDTO.AwayTeamName = ibetMatch.AwayTeamName + " / " + threein1betMatch.AwayTeamName;
			transactionDTO.Odd = text4 + " / " + text7;
			transactionDTO.OddKindValue = text4 + " / " + text7;
			transactionDTO.OddValue = ibetOddValue + " / " + threein1Odd.Away;
			transactionDTO.Stake = ibetStake + " / " + threein1betStake;
			transactionDTO.IBETAllow = flag;
			transactionDTO.IBETTrade = flag2;
			transactionDTO.IBETReTrade = false;
			transactionDTO.THREEIN1Allow = flag3;
			transactionDTO.THREEIN1Trade = flag4;
			transactionDTO.THREEIN1ReTrade = sBOBETReTrade;
			transactionDTO.OddType = oddType.ToString();
			transactionDTO.Note = text;
			transactionDTO.DateTime = System.DateTime.Now;
			return transactionDTO;
		}

		private TransactionDTO PlaceBetAllowMaxBet(
            MatchDTO ibetMatch, 
            MatchDTO threein1betMatch, 
            eOddType oddType, 
            string ibetOddID,
            string ibetOddType,             
            string ibetOddValue, 
            OddDTO threein1Odd,
            string threein1OddType,
            int ibetStake, 
            int threein1betStake, 
            IBetEngine ibetEngine, 
            ThreeIn1BetEngine threein1betEngine)
		{
            
			TransactionDTO transactionDTO = new TransactionDTO(); //khoi tao transaction
            string text = string.Empty;			
            bool flagIBETOK = false;
            bool flag3IN1OK = false;
			
			bool threein1ReTrade = false;            
            string betCount = string.Empty;
            
            bool flagibetAllowance = false;
            int ibetminStake = 0;
            int ibetmaxStake = 0;
            string ibetBetKindValue = string.Empty;
            string ibetHomeTeamName = string.Empty;
            string ibetAwayTeamNam = string.Empty;

            bool ibetGoFirst = true;// who is Away or Under
            string threein1OddValue = string.Empty;
            if (threein1OddType == "Away") //neu 3in1 o vi tri away
            {
                threein1OddValue = threein1Odd.Away.ToString();
                if (threein1Odd.Type == eOddType.FirstHalfOverUnder || threein1Odd.Type == eOddType.FulltimeOverUnder) //neu 3in1 la Under
                {
                    ibetGoFirst = false; // cho 3in1 Confirm truoc
                }
                else // bong handicap
                {
                    if (threein1Odd.IsHomeGive) // neu away la doi cua duoi
                    {
                        ibetGoFirst = false;
                    }
                }
            }
            else if (threein1OddType == "Home") // neu 3in1 o vi tri home - ibet away
            {
                threein1OddValue = threein1Odd.Home.ToString();
                if (threein1Odd.Type == eOddType.FirstHalfOverUnder || threein1Odd.Type == eOddType.FulltimeOverUnder) //neu 3in1 la Over
                {
                    ibetGoFirst = true; // cho ibet Confirm truoc (under)
                }
                else // bong handicap
                {
                    if (!threein1Odd.IsHomeGive) // neu 3in1 la doi cua duoi
                    {
                        ibetGoFirst = false;
                    }
                }
            }

            bool flagthreein1betAllowance = false;
            int threein1betminStake = 0;
            int threein1betmaxStake = 0;
            string threein1betKindValue = string.Empty;
            string threein1betHomeTeamName = string.Empty;
            string threein1betAwayTeamName = string.Empty;
            
            string postBODY = string.Empty;            

            if (MatchDTO.IsSameMatch(ibetMatch.HomeTeamName.ToLower(), threein1betMatch.HomeTeamName.ToLower(), ibetMatch.AwayTeamName.ToLower(), threein1betMatch.AwayTeamName.ToLower()))
			{
				try
				{
                    ibetEngine.PrepareBet(ibetOddID, ibetOddType, ibetOddValue, ibetStake.ToString(), 
                        out flagibetAllowance, out ibetminStake, out ibetmaxStake, out ibetBetKindValue, out ibetHomeTeamName, out ibetAwayTeamNam);
                    
                    threein1betEngine.PrepareBet(
                        threein1Odd.ID,
                        threein1OddValue,//odd price
                        threein1Odd.Type.ToString(), 
                        threein1OddType, // "Away"
                        threein1Odd.IsHomeGive.ToString().Replace("True", "1").Replace("False", "0"),
                        threein1Odd.Odd, 
                        threein1betMatch.HomeScore, 
                        threein1betMatch.AwayScore, 
                        threein1betMatch.ID, 
                        threein1betMatch.IsHalfTime.ToString().Replace("True", "1").Replace("False", "0"), 
                        threein1betStake.ToString(),
                        out flagthreein1betAllowance, 
                        out threein1betminStake, 
                        out threein1betmaxStake, 
                        out threein1betKindValue, 
                        out threein1betHomeTeamName, 
                        out threein1betAwayTeamName,
                        out postBODY);
#if DEBUG
                    Utilities.WriteLog.Write("Calling threein1betEngine.PrepareBet");
#endif

                    if (MatchDTO.IsSameMatch(ibetHomeTeamName, threein1betHomeTeamName, ibetAwayTeamNam, threein1betAwayTeamName))
                    {
                        if (flagibetAllowance && flagthreein1betAllowance)
                        {
                            float numIbet = float.Parse(ibetBetKindValue);
                            float num3in1bet = float.Parse(threein1betKindValue);

                            if (numIbet + num3in1bet == 0f || numIbet == num3in1bet)
                            {
#if DEBUG
                                Utilities.WriteLog.Write("+ Valid bet, system is now going to confirm:" + ibetHomeTeamName + " - " + threein1betAwayTeamName);
#endif
                                int numMax = 0; // get minimum of Maximum bet allowed of two system
                                if (ibetmaxStake >= threein1betmaxStake)
                                {
                                    numMax = threein1betmaxStake;                                    
                                }
                                else
                                {
                                    numMax = ibetmaxStake;
                                }

                                if (ibetStake >= numMax) //compare stake with numMax
                                {
                                    ibetStake = numMax;
                                }
                                if (threein1betStake >= numMax)
                                {
                                    threein1betStake = numMax;
                                }

                                try
                                {
                                    float currentCredit = 0;
                                    float currentCredit2 = 0;
                                    if (checkEdit8.Checked)
                                    {
                                        currentCredit = ibetEngine.GetCurrentCredit();
                                        currentCredit2 = threein1betEngine.GetCurrentCredit();
                                    }
                                    else
                                    {
                                        currentCredit = ibetEngine._currentCredit;
                                        currentCredit2 = threein1betEngine._currentCredit;
                                    }
                                    
                                    
                                    if ((float)ibetStake <= currentCredit && (float)threein1betStake <= currentCredit2)
                                    {
                                        if (ibetGoFirst)
                                        {
                                            #region Confirm_IBET_first_then_3in1
#if DEBUG
                                            Utilities.WriteLog.Write("IBET GO FIRST!");
#endif
                                            try
                                            {
                                                ibetEngine.ConfirmBet(oddType, ibetOddValue, ibetStake.ToString(), ibetminStake.ToString(), ibetmaxStake.ToString(), out flagIBETOK);
                                                if (flagIBETOK)// iBet Confirm thanh cong
                                                {
                                                    string odd3in1NEW = string.Empty;
                                                    threein1betEngine.ConfirmBet(postBODY, threein1betStake.ToString(), out flag3IN1OK, out odd3in1NEW);
                                                    if (!flag3IN1OK)
                                                    {
                                                        try
                                                        {
                                                            if (odd3in1NEW != "changed")
                                                            {
                                                                //threein1betEngine.ConfirmBet(postBODY, threein1betStake.ToString(), out flag3IN1OK, out odd3in1NEW);
                                                                this._lastTransactionTime = System.DateTime.Now;
                                                                playSound(false);
                                                                text = "Odd 3in1 changed";
                                                                this.lblIbetCurrentCredit.Caption = this._ibetEngine.GetCurrentCredit().ToString();
                                                                
                                                            }
                                                            else
                                                            {
                                                                text = "Invalid Odd while Retrade.";// IBET Odd: " +text4 + " - SBOBET Odd:" + text7;
                                                                this.lblIbetCurrentCredit.Caption = this._ibetEngine.GetCurrentCredit().ToString();
                                                                playSound(false);
                                                            }
                                                        }
                                                        catch (System.Exception ex)
                                                        {
                                                            text = "Error while Retrade. Details: " + ex.Message;
                                                            playSound(false);

#if DEBUG
                                                            Utilities.WriteLog.Write(text);
#endif
                                                        }
                                                    }
                                                    else
                                                    {
                                                        this.lbl3in1betCurrentCredit.Caption = this._3in1Engine.GetCurrentCredit().ToString();
                                                        this.lblIbetCurrentCredit.Caption = this._ibetEngine.GetCurrentCredit().ToString();
                                                        playSound(true);
                                                    }
                                                    this._lastTransactionTime = System.DateTime.Now;
                                                    object obj = text;
                                                    text = string.Concat(new object[]
												    {
													    obj, 
													    " - Success Transaction. Half: ", 
													    ibetMatch.Half, 
													    " - Minute: ", 
													    ibetMatch.Minute, 
													    " - Halftime: ", 
													    ibetMatch.IsHalfTime
												    });                                                    
                                                }
                                            }
                                            catch (System.Exception ex)
                                            {
                                                text = "Error while Trading. Details: " + ex.Message;
#if DEBUG
                                                Utilities.WriteLog.Write(text);
#endif
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Confirm_3in1_first_then_IBET
#if DEBUG
                                            Utilities.WriteLog.Write("3IN1BET GO FIRST!");
#endif
                                            try
                                            {
                                                string odd3in1NEW = string.Empty;
                                                threein1betEngine.ConfirmBet(postBODY, threein1betStake.ToString(), out flag3IN1OK, out odd3in1NEW);
                                                if (flag3IN1OK)
                                                {
                                                    ibetEngine.ConfirmBet(oddType, ibetOddValue, ibetStake.ToString(), ibetminStake.ToString(), ibetmaxStake.ToString(), out flagIBETOK);
                                                    if (!flagIBETOK)// iBet Confirm thanh cong
                                                    {
                                                        try
                                                        {
                                                            playSound(false);
#if DEBUG
                                                            Utilities.WriteLog.Write("iBet confirm failed.");
#endif
                                                        }

                                                        catch (System.Exception ex)
                                                        {
                                                            text = "Error while Retrade. Details: " + ex.Message;
                                                            playSound(false);
#if DEBUG
                                                            Utilities.WriteLog.Write(text);
#endif
                                                        }
                                                    }
                                                    else
                                                    {
                                                        this.lbl3in1betCurrentCredit.Caption = this._3in1Engine.GetCurrentCredit().ToString();
                                                        this.lblIbetCurrentCredit.Caption = this._ibetEngine.GetCurrentCredit().ToString();
                                                        playSound(true);
                                                    }
                                                    this._lastTransactionTime = System.DateTime.Now;
                                                    object obj = text;
                                                    text = string.Concat(new object[]
												    {
													    obj, 
													    " - Success Transaction. Half: ", 
													    ibetMatch.Half, 
													    " - Minute: ", 
													    ibetMatch.Minute, 
													    " - Halftime: ", 
													    ibetMatch.IsHalfTime
												    });
                                                }
                                            }
                                            catch (System.Exception ex)
                                            {
                                                text = "Error while Trading. Details: " + ex.Message;
#if DEBUG
                                                Utilities.WriteLog.Write(text);
#endif
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        text = string.Concat(new object[]
										{
											"Out of Cash. IBET Credit: ", 
											currentCredit, 
											" -  SBOBET Credit: ", 
											currentCredit2
										});
#if DEBUG
                                        Utilities.WriteLog.Write(text);
#endif
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    text = "Error while getting Credit. Details: " + ex.Message;
#if DEBUG
                                    Utilities.WriteLog.Write(text);
#endif
                                }



                            }
                            else
                            {
                                text = "Invalid Odd while Preparing Ticket. IBET Odd: " + ibetBetKindValue + " - SBOBET Odd:" + threein1betKindValue;
#if DEBUG
                                Utilities.WriteLog.Write(text);
#endif
                            }
                        }
                        else
                        { 
#if DEBUG
                            if (!flagibetAllowance)
                                Utilities.WriteLog.Write("IBET DIDN'T ALLOW");
                            if (!flagthreein1betAllowance)
                                Utilities.WriteLog.Write("3IN1 DIDN'T ALLOW");
#endif

                        }
                    }

                    else
                    {
                        text = string.Concat(new string[]
						{
							"Not Same Match - Preparing Ticket. IBET: ", 
							ibetHomeTeamName, 
							" / ", 
							ibetAwayTeamNam, 
							" - 3in1BET: ", 
							threein1betHomeTeamName, 
							" / ", 
							threein1betAwayTeamName
						});
#if DEBUG
                        Utilities.WriteLog.Write(text);
#endif
                    }

                        
				}
				catch (System.Exception ex)
				{
					text = "Error while Preparing Trade. Details: " + ex.Message;
#if DEBUG
                    WriteLog.Write(" *** " + text);
#endif 
				}
			}
			else
			{
				text = string.Concat(new string[]
				{
					"Not Same Match - Comparing. IBET: ", 
					ibetMatch.HomeTeamName, 
					" / ", 
					ibetMatch.AwayTeamName, 
					" - 3in1BET: ", 
					threein1betMatch.HomeTeamName, 
					" / ", 
					threein1betMatch.AwayTeamName
				});
			}
            transactionDTO.HomeTeamName = ibetMatch.HomeTeamName + " / " + threein1betMatch.HomeTeamName;
            transactionDTO.AwayTeamName = ibetMatch.AwayTeamName + " / " + threein1betMatch.AwayTeamName;
            transactionDTO.Odd = ibetBetKindValue + " / " + threein1betKindValue;
			transactionDTO.OddKindValue = ibetBetKindValue + " / " + threein1betKindValue;
            transactionDTO.OddValue = ibetOddValue + " / " + threein1OddValue;
			transactionDTO.Stake = ibetStake + " / " + threein1betStake;
			transactionDTO.IBETAllow = flagibetAllowance;
            transactionDTO.IBETTrade = flagIBETOK;
			transactionDTO.IBETReTrade = false;
            transactionDTO.THREEIN1Allow = flagthreein1betAllowance;
            transactionDTO.THREEIN1Trade = flag3IN1OK;
			transactionDTO.THREEIN1ReTrade = threein1ReTrade;
			transactionDTO.OddType = oddType.ToString();
			transactionDTO.Note = text;
			transactionDTO.DateTime = System.DateTime.Now;
			return transactionDTO;
		}
		private int GetTradeableStake(int ibetMaxStake, int sbobetMaxStake, int randomStake)
		{
			int num = 0;
			if (ibetMaxStake > sbobetMaxStake)
			{
				num = sbobetMaxStake;
			}
			else
			{
				num = ibetMaxStake;
			}
			if (num > randomStake)
			{
				num = randomStake;
			}
			float currentCredit = this._ibetEngine.GetCurrentCredit();
			float currentCredit2 = this._3in1Engine.GetCurrentCredit();
			this.lblIbetCurrentCredit.Caption = currentCredit.ToString();
			this.lbl3in1betCurrentCredit.Caption = currentCredit2.ToString();
			float num2 = 0f;
			if (currentCredit > currentCredit2)
			{
				num2 = currentCredit2;
			}
			else
			{
				num2 = currentCredit;
			}
			int result;
			if ((float)num > num2)
			{
				result = 0;
			}
			else
			{
				result = num;
			}
			return result;
		}
		private bool AllowOddBet(string oddID)
		{
			return !this._oddTransactionHistory.ContainsKey(oddID) || (System.DateTime.Now - this._oddTransactionHistory[oddID]).Seconds > (int)this.txtTransactionTimeSpan.Value;
            //khong bet lai nhung odd da bet roi. Sau 1 thoi gian time span moi dc bet tiep
		}
		private void UpdateOddBetHistory(string oddID)
		{
			this._oddTransactionHistory.Remove(oddID);
			this._oddTransactionHistory.Add(oddID, System.DateTime.Now);
		}
		private void AddTransaction(TransactionDTO transaction)
		{
			lock (this._listTransaction)
			{
				transaction.ID = (this._listTransaction.Count + 1).ToString();
				this._listTransaction.Add(transaction);
				lock (this.grdTransaction)
				{
					this.grdTransaction.RefreshDataSource();
				}
				transaction.AccountPair = this.Text;
				this._mainForm.AddTransaction(transaction);
			}
		}
		private void TerminalForm_FormClosing(object sender, FormClosingEventArgs e)
		{            
			this.Stop();
			if (this._ibetEngine != null)
			{
				this._ibetEngine = null;
			}
			if (this._3in1Engine != null)
			{
                this._3in1Engine = null;
			}
			this._forceRefreshTimer.Stop();
			this.webIBET.Navigate("http://" + this.webIBET.Url.Host + "/logout.aspx");
		}
		private void btn3IN1BETGO_Click(object sender, System.EventArgs e)
		{
			this.web3IN1BET.Navigate(this.txt1IN1BETAddress.Text);
		}
		private void btnIBETGO_Click(object sender, System.EventArgs e)
		{
			this.webIBET.Navigate(this.txtIBETAddress.Text);
		}       

        private void btn3in1betGetInfo_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{				
                this.Initialize3in1Engine();

			}
			catch (System.Exception ex)
			{
				this.ShowErrorDialog("Error while initialize 3in1BET Engine. \nDetails: " + ex.Message);
			}
		}
		private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{                
				this.InitializeIBETEngine();                
			}
			catch (System.Exception ex)
			{
				this.ShowErrorDialog("Error while initialize IBET Engine. \nDetails: " + ex.Message);
			}
		}
		private void ShowErrorDialog(string message)
		{
			MessageBox.Show(message, "Bet Broker", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		private void ShowWarningDialog(string message)
		{
            MessageBox.Show(message, "Bet Broker", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		
		private void btnStop_ItemClick(object sender, ItemClickEventArgs e)
		{
			this.Stop();
			this._dataService.StopTerminalAsync(this._currentUserID, this.Text);
		}
		private void btnClear_ItemClick(object sender, ItemClickEventArgs e)
		{
            //cho nay
            if (xtraTabControl2.SelectedTabPageIndex == 1)
            {
                lock (this._listTransaction)
                {
                    this._listTransaction.Clear();
                    lock (this.grdTransaction)
                    {
                        this.grdTransaction.RefreshDataSource();
                    }
                }
            }

            else if (xtraTabControl2.SelectedTabPageIndex == 0)
            {
                lock (this.chkListAllowedMatch)
                {
                    this.chkListAllowedMatch.Items.Clear();
                }
            }
		}
		private void chbRandomStake_CheckedChanged(object sender, System.EventArgs e)
		{
		}
		private void txtIBETUpdateInterval_EditValueChanged(object sender, System.EventArgs e)
		{
		}
		private void txt3IN1BETUpdateInterval_EditValueChanged(object sender, System.EventArgs e)
		{
		}
		private void btnSetUpdateInterval_Click(object sender, System.EventArgs e)
		{
            if (_running)
            {
                if (this._ibetEngine != null)
                {
                    this._ibetEngine.Stop();
                    this._ibetEngine.UpdateDataInterval = (int)this.txtIBETUpdateInterval.Value * 1000;
                    this._ibetEngine.Start();
                }
                if (this._3in1Engine != null)
                {
                    this._3in1Engine.Stop();
                    this._3in1Engine.UpdateDataInterval = (int)this.txt3IN1BETUpdateInterval.Value * 1000;
                    this._3in1Engine.Start();
                }
            }
            else
            {
                if (this._ibetEngine != null)
                {
                    this._ibetEngine.Stop();
                    this._ibetEngine.UpdateDataInterval = (int)this.txtIBETUpdateInterval.Value * 1000;                    
                }
                if (this._3in1Engine != null)
                {
                    this._3in1Engine.Stop();
                    this._3in1Engine.UpdateDataInterval = (int)this.txt3IN1BETUpdateInterval.Value * 1000;                    
                }
            }
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(TerminalFormIBET3IN1));
			this.ribbonControl1 = new RibbonControl();
			this.barStaticItem1 = new BarStaticItem();
			this.barStaticItem2 = new BarStaticItem();
			this.barStaticItem3 = new BarStaticItem();
			this.btn3in1betGetInfo = new BarButtonItem();
			this.lbl3in1betCurrentCredit = new BarStaticItem();
			this.lbl3in1betTotalMatch = new BarStaticItem();
			this.lbl3in1betLastUpdate = new BarStaticItem();
			this.barStaticItem7 = new BarStaticItem();
			this.barStaticItem8 = new BarStaticItem();
			this.barStaticItem9 = new BarStaticItem();
			this.lblIbetCurrentCredit = new BarStaticItem();
			this.lblIbetTotalMatch = new BarStaticItem();
			this.lblIbetLastUpdate = new BarStaticItem();
			this.btnIbetGetInfo = new BarButtonItem();
			this.lblStatus = new BarStaticItem();
			this.lblSameMatch = new BarStaticItem();
			this.lblLastUpdate = new BarStaticItem();
			this.btnStart = new BarButtonItem();
			this.btnStop = new BarButtonItem();
			this.btnClear = new BarButtonItem();
			this.ribbonPage1 = new RibbonPage();
			this.rpgIbet = new RibbonPageGroup();
			this.rpg3in1bet = new RibbonPageGroup();
			this.ribbonPageGroup3 = new RibbonPageGroup();
			this.ribbonPageGroup4 = new RibbonPageGroup();
			this.splitContainerControl1 = new SplitContainerControl();
			this.xtraTabControl1 = new XtraTabControl();
			this.xtraTabPage1 = new XtraTabPage();
			this.panelControl5 = new PanelControl();
			this.webIBET = new WebBrowser();
			this.panelControl4 = new PanelControl();
			this.btnIBETGO = new SimpleButton();
			this.txtIBETAddress = new TextEdit();
			this.labelControl5 = new LabelControl();
			this.xtraTabPage2 = new XtraTabPage();
			this.panelControl3 = new PanelControl();
			this.web3IN1BET = new WebBrowser();
			this.panelControl2 = new PanelControl();
			this.btn3IN1BETGO = new SimpleButton();
			this.txt1IN1BETAddress = new TextEdit();
			this.labelControl1 = new LabelControl();
			this.xtraTabPage3 = new XtraTabPage();
            this.xtraTabPage9 = new XtraTabPage();
			this.grdSameMatch = new GridControl();
			this.gridView1 = new GridView();
			this.gridColumn15 = new GridColumn();
			this.gridColumn16 = new GridColumn();
			this.gridColumn14 = new GridColumn();
			this.gridColumn17 = new GridColumn();
			this.gridColumn18 = new GridColumn();
			this.gridColumn19 = new GridColumn();
			this.xtraTabControl2 = new XtraTabControl();
			this.xtraTabPage4 = new XtraTabPage();
			this.groupControl2 = new GroupControl();
			this.txtTransactionTimeSpan = new SpinEdit();
			this.labelControl10 = new LabelControl();
			this.txtMaxTimePerHalf = new SpinEdit();
			this.labelControl7 = new LabelControl();
			this.chbAllowHalftime = new CheckEdit();
			this.groupControl1 = new GroupControl();
			this.btnSetUpdateInterval = new SimpleButton();
			this.txt3IN1BETUpdateInterval = new SpinEdit();
			this.labelControl3 = new LabelControl();
			this.txtIBETUpdateInterval = new SpinEdit();
			this.labelControl4 = new LabelControl();
			this.groupControl5 = new GroupControl();            
			this.chbHighRevenueBoost = new CheckEdit();
			this.txtLowestOddValue = new SpinEdit();
			this.labelControl9 = new LabelControl();
			this.txtOddValueDifferenet = new SpinEdit();
			this.labelControl8 = new LabelControl();
            this.checkEdit9 = new CheckEdit();
            this.checkEdit8 = new CheckEdit();
            this.checkEdit7 = new CheckEdit();
			this.checkEdit6 = new CheckEdit();
			this.checkEdit5 = new CheckEdit();
			this.checkEdit4 = new CheckEdit();
			this.checkEdit3 = new CheckEdit();
			this.groupControl4 = new GroupControl();
            this.groupControl6 = new GroupControl();
			this.txt3in1BETFixedStake = new SpinEdit();
			this.labelControl2 = new LabelControl();
			this.txtStake = new MemoEdit();
			this.chbRandomStake = new CheckEdit();
			this.txtIBETFixedStake = new SpinEdit();
			this.labelControl6 = new LabelControl();
			this.xtraTabPage5 = new XtraTabPage();
			this.grdTransaction = new GridControl();
			this.gridView2 = new GridView();
			this.gridColumn1 = new GridColumn();
			this.gridColumn2 = new GridColumn();
			this.gridColumn3 = new GridColumn();
			this.gridColumn6 = new GridColumn();
			this.gridColumn4 = new GridColumn();
			this.gridColumn5 = new GridColumn();
			this.gridColumn7 = new GridColumn();
			this.gridColumn8 = new GridColumn();
			this.gridColumn9 = new GridColumn();
			this.gridColumn10 = new GridColumn();
			this.gridColumn11 = new GridColumn();
			this.gridColumn12 = new GridColumn();
			this.gridColumn20 = new GridColumn();
			this.gridColumn13 = new GridColumn();

            this.chkListAllowedMatch = new CheckedListBoxControl();

			((System.ComponentModel.ISupportInitialize)this.ribbonControl1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.splitContainerControl1).BeginInit();
			this.splitContainerControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).BeginInit();
			this.xtraTabControl1.SuspendLayout();
			this.xtraTabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.panelControl5).BeginInit();
			this.panelControl5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.panelControl4).BeginInit();
			this.panelControl4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txtIBETAddress.Properties).BeginInit();
			this.xtraTabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.panelControl3).BeginInit();
			this.panelControl3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.panelControl2).BeginInit();
			this.panelControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txt1IN1BETAddress.Properties).BeginInit();
			this.xtraTabPage3.SuspendLayout();

            this.xtraTabPage9.SuspendLayout();

			((System.ComponentModel.ISupportInitialize)this.grdSameMatch).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).BeginInit();
			this.xtraTabControl2.SuspendLayout();
			this.xtraTabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.groupControl2).BeginInit();
			this.groupControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txtTransactionTimeSpan.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaxTimePerHalf.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chbAllowHalftime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.groupControl1).BeginInit();
			this.groupControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txt3IN1BETUpdateInterval.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtIBETUpdateInterval.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.groupControl5).BeginInit();
			this.groupControl5.SuspendLayout();

            ((System.ComponentModel.ISupportInitialize)this.groupControl6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkListAllowedMatch)).BeginInit();
            this.groupControl6.SuspendLayout();

			((System.ComponentModel.ISupportInitialize)this.chbHighRevenueBoost.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtLowestOddValue.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtOddValueDifferenet.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.checkEdit9.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.checkEdit8.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.checkEdit7.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.checkEdit6.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.checkEdit5.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.checkEdit4.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.checkEdit3.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.groupControl4).BeginInit();
			this.groupControl4.SuspendLayout();

            

			((System.ComponentModel.ISupportInitialize)this.txt3in1BETFixedStake.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtStake.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chbRandomStake.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtIBETFixedStake.Properties).BeginInit();
			this.xtraTabPage5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.grdTransaction).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView2).BeginInit();
			base.SuspendLayout();
			this.ribbonControl1.ApplicationButtonText = null;
			this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.ExpandCollapseItem.Name = string.Empty;
			this.ribbonControl1.Items.AddRange(new BarItem[]
			{
				this.ribbonControl1.ExpandCollapseItem, 
				this.barStaticItem1, 
				this.barStaticItem2, 
				this.barStaticItem3, 
				this.btn3in1betGetInfo, 
				this.lbl3in1betCurrentCredit, 
				this.lbl3in1betTotalMatch, 
				this.lbl3in1betLastUpdate, 
				this.barStaticItem7, 
				this.barStaticItem8, 
				this.barStaticItem9, 
				this.lblIbetCurrentCredit, 
				this.lblIbetTotalMatch, 
				this.lblIbetLastUpdate, 
				this.btnIbetGetInfo, 
				this.lblStatus, 
				this.lblSameMatch, 
				this.lblLastUpdate, 
				this.btnStart, 
				this.btnStop, 
				this.btnClear
			});
			this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
			this.ribbonControl1.MaxItemId = 22;
			this.ribbonControl1.Name = "ribbonControl1";
			this.ribbonControl1.Pages.AddRange(new RibbonPage[]
			{
				this.ribbonPage1
			});
			this.ribbonControl1.RibbonStyle = RibbonControlStyle.Office2010;
			this.ribbonControl1.SelectedPage = this.ribbonPage1;
			this.ribbonControl1.ShowCategoryInCaption = false;
			this.ribbonControl1.ShowExpandCollapseButton = DefaultBoolean.False;
			this.ribbonControl1.ShowPageHeadersMode = ShowPageHeadersMode.Hide;
			this.ribbonControl1.ShowToolbarCustomizeItem = false;
			this.ribbonControl1.Size = new System.Drawing.Size(999, 123);
			this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
			this.barStaticItem1.Caption = "Current Credit:";
			this.barStaticItem1.Id = 1;
			this.barStaticItem1.Name = "barStaticItem1";
			this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
			this.barStaticItem2.Caption = "Total Match:";
			this.barStaticItem2.Id = 2;
			this.barStaticItem2.Name = "barStaticItem2";
			this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
			this.barStaticItem3.Caption = "Last Update:";
			this.barStaticItem3.Id = 3;
			this.barStaticItem3.Name = "barStaticItem3";
			this.barStaticItem3.TextAlignment = System.Drawing.StringAlignment.Near;
			this.btn3in1betGetInfo.Caption = "Get Info";
			this.btn3in1betGetInfo.Id = 4;
            //Bitmap bmp = (Bitmap)Image.FromFile("C:\\1.jpg");
            this.btn3in1betGetInfo.LargeGlyph = iBet.App.Properties.Resources.i8;
			this.btn3in1betGetInfo.Name = "btn3in1betGetInfo";
			this.btn3in1betGetInfo.ItemClick += new ItemClickEventHandler(this.btn3in1betGetInfo_ItemClick);
			this.lbl3in1betCurrentCredit.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.lbl3in1betCurrentCredit.Appearance.Options.UseFont = true;
			this.lbl3in1betCurrentCredit.Caption = "-";
			this.lbl3in1betCurrentCredit.Id = 5;
			this.lbl3in1betCurrentCredit.Name = "lbl3in1betCurrentCredit";
			this.lbl3in1betCurrentCredit.TextAlignment = System.Drawing.StringAlignment.Far;
			this.lbl3in1betCurrentCredit.Width = 135;
			this.lbl3in1betTotalMatch.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.lbl3in1betTotalMatch.Appearance.Options.UseFont = true;
			this.lbl3in1betTotalMatch.AutoSize = BarStaticItemSize.None;
			this.lbl3in1betTotalMatch.Caption = "-";
			this.lbl3in1betTotalMatch.Id = 6;
			this.lbl3in1betTotalMatch.Name = "lbl3in1betTotalMatch";
			this.lbl3in1betTotalMatch.TextAlignment = System.Drawing.StringAlignment.Far;
			this.lbl3in1betTotalMatch.Width = 135;
			this.lbl3in1betLastUpdate.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			this.lbl3in1betLastUpdate.Appearance.Options.UseFont = true;
			this.lbl3in1betLastUpdate.Caption = "-";
			this.lbl3in1betLastUpdate.Id = 8;
			this.lbl3in1betLastUpdate.Name = "lbl3in1betLastUpdate";
			this.lbl3in1betLastUpdate.TextAlignment = System.Drawing.StringAlignment.Far;
			this.lbl3in1betLastUpdate.Width = 135;
			this.barStaticItem7.Caption = "Current Credit:";
			this.barStaticItem7.Id = 9;
			this.barStaticItem7.Name = "barStaticItem7";
			this.barStaticItem7.TextAlignment = System.Drawing.StringAlignment.Near;
			this.barStaticItem8.Caption = "Total Match:";
			this.barStaticItem8.Id = 10;
			this.barStaticItem8.Name = "barStaticItem8";
			this.barStaticItem8.TextAlignment = System.Drawing.StringAlignment.Near;
			this.barStaticItem9.Caption = "Last Update:";
			this.barStaticItem9.Id = 11;
			this.barStaticItem9.Name = "barStaticItem9";
			this.barStaticItem9.TextAlignment = System.Drawing.StringAlignment.Near;
			this.lblIbetCurrentCredit.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.lblIbetCurrentCredit.Appearance.Options.UseFont = true;
			this.lblIbetCurrentCredit.Caption = "-";
			this.lblIbetCurrentCredit.Id = 12;
			this.lblIbetCurrentCredit.Name = "lblIbetCurrentCredit";
			this.lblIbetCurrentCredit.TextAlignment = System.Drawing.StringAlignment.Far;
			this.lblIbetCurrentCredit.Width = 135;
			this.lblIbetTotalMatch.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.lblIbetTotalMatch.Appearance.Options.UseFont = true;
			this.lblIbetTotalMatch.Caption = "-";
			this.lblIbetTotalMatch.Id = 13;
			this.lblIbetTotalMatch.Name = "lblIbetTotalMatch";
			this.lblIbetTotalMatch.TextAlignment = System.Drawing.StringAlignment.Far;
			this.lblIbetTotalMatch.Width = 135;
			this.lblIbetLastUpdate.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			this.lblIbetLastUpdate.Appearance.Options.UseFont = true;
			this.lblIbetLastUpdate.Caption = "-";
			this.lblIbetLastUpdate.Id = 14;
			this.lblIbetLastUpdate.Name = "lblIbetLastUpdate";
			this.lblIbetLastUpdate.TextAlignment = System.Drawing.StringAlignment.Far;
			this.lblIbetLastUpdate.Width = 135;
			this.btnIbetGetInfo.Caption = "Get Info";
			this.btnIbetGetInfo.Id = 15;
            this.btnIbetGetInfo.LargeGlyph = iBet.App.Properties.Resources.i8;
			this.btnIbetGetInfo.Name = "btnIbetGetInfo";
			this.btnIbetGetInfo.ItemClick += new ItemClickEventHandler(this.barButtonItem2_ItemClick);
			this.lblStatus.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.lblStatus.Appearance.Options.UseFont = true;
			this.lblStatus.Caption = "STOPPED";
			this.lblStatus.Id = 16;
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.TextAlignment = System.Drawing.StringAlignment.Center;
			this.lblStatus.Width = 135;
			this.lblSameMatch.Caption = "Total Same Match: -";
			this.lblSameMatch.Id = 17;
			this.lblSameMatch.Name = "lblSameMatch";
			this.lblSameMatch.TextAlignment = System.Drawing.StringAlignment.Center;
			this.lblSameMatch.Width = 135;
			this.lblLastUpdate.Caption = "-";
			this.lblLastUpdate.Id = 18;
			this.lblLastUpdate.Name = "lblLastUpdate";
			this.lblLastUpdate.TextAlignment = System.Drawing.StringAlignment.Center;
			this.lblLastUpdate.Width = 135;
			this.btnStart.Caption = "Start";
			this.btnStart.Id = 19;
            this.btnStart.LargeGlyph = iBet.App.Properties.Resources.i5;
			this.btnStart.Name = "btnStart";
			this.btnStart.ItemClick += new ItemClickEventHandler(this.btnStart_ItemClick);
            this.btnStart.Enabled = false;
			this.btnStop.Caption = "Stop";
			this.btnStop.Enabled = false;
			this.btnStop.Id = 20;
            this.btnStop.LargeGlyph = iBet.App.Properties.Resources.i6;
			this.btnStop.Name = "btnStop";
			this.btnStop.ItemClick += new ItemClickEventHandler(this.btnStop_ItemClick);
			this.btnClear.Caption = "Clear";
			this.btnClear.Enabled = false;
			this.btnClear.Id = 21;
            this.btnClear.LargeGlyph = iBet.App.Properties.Resources.i7;
			this.btnClear.Name = "btnClear";
			this.btnClear.ItemClick += new ItemClickEventHandler(this.btnClear_ItemClick);
			this.ribbonPage1.Groups.AddRange(new RibbonPageGroup[]
			{
				this.rpgIbet, 
				this.rpg3in1bet, 
				this.ribbonPageGroup3, 
				this.ribbonPageGroup4
			});
			this.ribbonPage1.Name = "ribbonPage1";
			this.ribbonPage1.Text = "ribbonPage1";
			this.rpgIbet.ItemLinks.Add(this.barStaticItem7);
			this.rpgIbet.ItemLinks.Add(this.barStaticItem8);
			this.rpgIbet.ItemLinks.Add(this.barStaticItem9);
			this.rpgIbet.ItemLinks.Add(this.lblIbetCurrentCredit);
			this.rpgIbet.ItemLinks.Add(this.lblIbetTotalMatch);
			this.rpgIbet.ItemLinks.Add(this.lblIbetLastUpdate);
			this.rpgIbet.ItemLinks.Add(this.btnIbetGetInfo);
			this.rpgIbet.Name = "rpgIbet";
			this.rpgIbet.ShowCaptionButton = false;
			this.rpgIbet.Text = "IBET";
			this.rpg3in1bet.ItemLinks.Add(this.barStaticItem1);
            this.rpg3in1bet.ItemLinks.Add(this.barStaticItem2);
            this.rpg3in1bet.ItemLinks.Add(this.barStaticItem3);
            this.rpg3in1bet.ItemLinks.Add(this.lbl3in1betCurrentCredit);
            this.rpg3in1bet.ItemLinks.Add(this.lbl3in1betTotalMatch);
            this.rpg3in1bet.ItemLinks.Add(this.lbl3in1betLastUpdate);
            this.rpg3in1bet.ItemLinks.Add(this.btn3in1betGetInfo);
            this.rpg3in1bet.Name = "rpg3in1bet";
            this.rpg3in1bet.ShowCaptionButton = false;
            this.rpg3in1bet.Text = "3IN1BET";
			this.ribbonPageGroup3.ItemLinks.Add(this.lblStatus);
			this.ribbonPageGroup3.ItemLinks.Add(this.lblSameMatch);
			this.ribbonPageGroup3.ItemLinks.Add(this.lblLastUpdate);
			this.ribbonPageGroup3.ItemLinks.Add(this.btnStart);
			this.ribbonPageGroup3.ItemLinks.Add(this.btnStop);
			this.ribbonPageGroup3.Name = "ribbonPageGroup3";
			this.ribbonPageGroup3.ShowCaptionButton = false;
			this.ribbonPageGroup3.Text = "Status";
			this.ribbonPageGroup4.ItemLinks.Add(this.btnClear);
			this.ribbonPageGroup4.Name = "ribbonPageGroup4";
			this.ribbonPageGroup4.ShowCaptionButton = false;
			this.ribbonPageGroup4.Text = "Transaction";
			this.splitContainerControl1.Dock = DockStyle.Fill;
			this.splitContainerControl1.FixedPanel = SplitFixedPanel.Panel2;
			this.splitContainerControl1.Horizontal = false;
			this.splitContainerControl1.Location = new System.Drawing.Point(0, 123);
			this.splitContainerControl1.Name = "splitContainerControl1";
			this.splitContainerControl1.Panel1.Controls.Add(this.xtraTabControl1);
			this.splitContainerControl1.Panel1.Text = "Panel1";
			this.splitContainerControl1.Panel2.Controls.Add(this.xtraTabControl2);
			this.splitContainerControl1.Panel2.Text = "Panel2";
			this.splitContainerControl1.Size = new System.Drawing.Size(999, 575);
			this.splitContainerControl1.SplitterPosition = 285;
			this.splitContainerControl1.TabIndex = 1;
			this.splitContainerControl1.Text = "splitContainerControl1";
			this.xtraTabControl1.Dock = DockStyle.Fill;
			this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
			this.xtraTabControl1.Name = "xtraTabControl1";
			this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
			this.xtraTabControl1.Size = new System.Drawing.Size(999, 302);
			this.xtraTabControl1.TabIndex = 0;
			this.xtraTabControl1.TabPages.AddRange(new XtraTabPage[]
			{
				this.xtraTabPage1, 
				this.xtraTabPage2, 
				this.xtraTabPage3,
                this.xtraTabPage9
			});
			this.xtraTabPage1.Controls.Add(this.panelControl5);
			this.xtraTabPage1.Controls.Add(this.panelControl4);
			this.xtraTabPage1.Name = "xtraTabPage1";
			this.xtraTabPage1.Size = new System.Drawing.Size(993, 276);
			this.xtraTabPage1.Text = "IBET";
			this.panelControl5.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.panelControl5.Controls.Add(this.webIBET);
			this.panelControl5.Location = new System.Drawing.Point(3, 38);
			this.panelControl5.Name = "panelControl5";
			this.panelControl5.Size = new System.Drawing.Size(987, 235);
			this.panelControl5.TabIndex = 3;
			this.webIBET.Dock = DockStyle.Fill;
			this.webIBET.Location = new System.Drawing.Point(2, 2);
			this.webIBET.MinimumSize = new System.Drawing.Size(20, 20);
			this.webIBET.Name = "webIBET";
			this.webIBET.Size = new System.Drawing.Size(983, 231);
			this.webIBET.TabIndex = 0;
			this.webIBET.Url = new System.Uri("http://www.ibet888.net", System.UriKind.Absolute);
			this.webIBET.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.webIBET_DocumentCompleted);
			this.panelControl4.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.panelControl4.Controls.Add(this.btnIBETGO);
			this.panelControl4.Controls.Add(this.txtIBETAddress);
			this.panelControl4.Controls.Add(this.labelControl5);
			this.panelControl4.Location = new System.Drawing.Point(3, 3);
			this.panelControl4.Name = "panelControl4";
			this.panelControl4.Size = new System.Drawing.Size(987, 29);
			this.panelControl4.TabIndex = 2;
			this.btnIBETGO.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.btnIBETGO.Location = new System.Drawing.Point(908, 3);
			this.btnIBETGO.Name = "btnIBETGO";
			this.btnIBETGO.Size = new System.Drawing.Size(75, 23);
			this.btnIBETGO.TabIndex = 8;
			this.btnIBETGO.Text = "GO";
			this.btnIBETGO.Click += new System.EventHandler(this.btnIBETGO_Click);
			this.txtIBETAddress.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtIBETAddress.EditValue = "http://www.ibet888.net";
			this.txtIBETAddress.Location = new System.Drawing.Point(54, 6);
			this.txtIBETAddress.Name = "txtIBETAddress";
			this.txtIBETAddress.Size = new System.Drawing.Size(848, 20);
			this.txtIBETAddress.TabIndex = 7;
			this.labelControl5.Location = new System.Drawing.Point(5, 9);
			this.labelControl5.Name = "labelControl5";
			this.labelControl5.Size = new System.Drawing.Size(43, 13);
			this.labelControl5.TabIndex = 6;
			this.labelControl5.Text = "Address:";
			this.xtraTabPage2.Controls.Add(this.panelControl3);
			this.xtraTabPage2.Controls.Add(this.panelControl2);
			this.xtraTabPage2.Name = "xtraTabPage2";
			this.xtraTabPage2.Size = new System.Drawing.Size(993, 276);
			this.xtraTabPage2.Text = "3IN1BET";
			this.panelControl3.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.panelControl3.Controls.Add(this.web3IN1BET);
			this.panelControl3.Location = new System.Drawing.Point(3, 38);
			this.panelControl3.Name = "panelControl3";
			this.panelControl3.Size = new System.Drawing.Size(987, 235);
			this.panelControl3.TabIndex = 6;
			this.web3IN1BET.Dock = DockStyle.Fill;
			this.web3IN1BET.Location = new System.Drawing.Point(2, 2);
			this.web3IN1BET.MinimumSize = new System.Drawing.Size(20, 20);
			this.web3IN1BET.Name = "web3IN1BET";
			this.web3IN1BET.Size = new System.Drawing.Size(983, 231);
			this.web3IN1BET.TabIndex = 0;			
            this.web3IN1BET.Url = new System.Uri("http://www.3in1bet.com", System.UriKind.Absolute);
			this.web3IN1BET.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.web3IN1BET_DocumentCompleted);
			this.panelControl2.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.panelControl2.Controls.Add(this.btn3IN1BETGO);
			this.panelControl2.Controls.Add(this.txt1IN1BETAddress);
			this.panelControl2.Controls.Add(this.labelControl1);
			this.panelControl2.Location = new System.Drawing.Point(3, 3);
			this.panelControl2.Name = "panelControl2";
			this.panelControl2.Size = new System.Drawing.Size(987, 29);
			this.panelControl2.TabIndex = 5;
			this.btn3IN1BETGO.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.btn3IN1BETGO.Location = new System.Drawing.Point(908, 3);
			this.btn3IN1BETGO.Name = "btn3IN1BETGO";
			this.btn3IN1BETGO.Size = new System.Drawing.Size(75, 23);
			this.btn3IN1BETGO.TabIndex = 5;
			this.btn3IN1BETGO.Text = "GO";
			this.btn3IN1BETGO.Click += new System.EventHandler(this.btn3IN1BETGO_Click);
			this.txt1IN1BETAddress.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			//this.txt1IN1BETAddress.EditValue = "http://www.secureiron.com"; day
            this.txt1IN1BETAddress.EditValue = "http://www.3in1bet.com";
			this.txt1IN1BETAddress.Location = new System.Drawing.Point(54, 6);
			this.txt1IN1BETAddress.Name = "txt1IN1BETAddress";
			this.txt1IN1BETAddress.Size = new System.Drawing.Size(848, 20);
			this.txt1IN1BETAddress.TabIndex = 4;
			this.labelControl1.Location = new System.Drawing.Point(5, 9);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(43, 13);
			this.labelControl1.TabIndex = 3;
			this.labelControl1.Text = "Address:";

            this.xtraTabPage9.Name = "xtraTabPage9";
            this.xtraTabPage9.Size = new System.Drawing.Size(993, 276);
            this.xtraTabPage9.Text = "ibet Test";
            //this.xtraTabPage9.Controls.Add(this.grdSameMatch);
            
            this.xtraTabPage3.Controls.Add(this.grdSameMatch);
			this.xtraTabPage3.Name = "xtraTabPage3";
			this.xtraTabPage3.Size = new System.Drawing.Size(993, 276);
			this.xtraTabPage3.Text = "Live Match";
			this.grdSameMatch.Dock = DockStyle.Fill;
			this.grdSameMatch.Location = new System.Drawing.Point(0, 0);
			this.grdSameMatch.MainView = this.gridView1;
			this.grdSameMatch.Name = "grdSameMatch";
			this.grdSameMatch.Size = new System.Drawing.Size(993, 276);
			this.grdSameMatch.TabIndex = 3;
			this.grdSameMatch.ViewCollection.AddRange(new BaseView[]
			{
				this.gridView1
			});
			this.gridView1.Columns.AddRange(new GridColumn[]
			{
				this.gridColumn15, 
				this.gridColumn16, 
				this.gridColumn14, 
				this.gridColumn17, 
				this.gridColumn18, 
				this.gridColumn19
			});
			this.gridView1.GridControl = this.grdSameMatch;
			this.gridView1.Name = "gridView1";
			this.gridView1.OptionsBehavior.Editable = false;
			this.gridView1.OptionsCustomization.AllowGroup = false;
			this.gridView1.OptionsDetail.AllowZoomDetail = false;
			this.gridView1.OptionsDetail.EnableMasterViewMode = false;
			this.gridView1.OptionsDetail.ShowDetailTabs = false;
			this.gridView1.OptionsDetail.SmartDetailExpand = false;
			this.gridView1.OptionsView.ShowAutoFilterRow = true;
			this.gridView1.OptionsView.ShowGroupPanel = false;
			this.gridView1.OptionsView.ShowPreview = true;
			this.gridView1.PreviewFieldName = "LeagueName";
			this.gridView1.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumn18, ColumnSortOrder.Ascending)
			});
			this.gridColumn15.Caption = "Home Team";
			this.gridColumn15.FieldName = "HomeTeamName";            
			this.gridColumn15.Name = "gridColumn15";
			this.gridColumn15.Visible = true;
			this.gridColumn15.VisibleIndex = 0;
			this.gridColumn15.Width = 367;
			this.gridColumn16.Caption = "Away Team";
			this.gridColumn16.FieldName = "AwayTeamName";
			this.gridColumn16.Name = "gridColumn16";
			this.gridColumn16.Visible = true;
			this.gridColumn16.VisibleIndex = 1;
			this.gridColumn16.Width = 368;
			this.gridColumn14.Caption = "Odd Count";
			this.gridColumn14.FieldName = "OddCount";
			this.gridColumn14.Name = "gridColumn14";
			this.gridColumn14.OptionsColumn.FixedWidth = true;
			this.gridColumn14.Visible = true;
			this.gridColumn14.VisibleIndex = 2;
			this.gridColumn14.Width = 70;
			this.gridColumn17.Caption = "Half";
			this.gridColumn17.FieldName = "Half";
			this.gridColumn17.Name = "gridColumn17";
			this.gridColumn17.OptionsColumn.FixedWidth = true;
			this.gridColumn17.Visible = true;
			this.gridColumn17.VisibleIndex = 3;
			this.gridColumn17.Width = 60;
			this.gridColumn18.Caption = "Minute";
			this.gridColumn18.FieldName = "Minute";
			this.gridColumn18.Name = "gridColumn18";
			this.gridColumn18.OptionsColumn.FixedWidth = true;
			this.gridColumn18.SortMode = ColumnSortMode.Value;
			this.gridColumn18.Visible = true;
			this.gridColumn18.VisibleIndex = 4;
			this.gridColumn18.Width = 60;
			this.gridColumn19.Caption = "Half Time";
			this.gridColumn19.FieldName = "IsHalfTime";
			this.gridColumn19.Name = "gridColumn19";
			this.gridColumn19.OptionsColumn.FixedWidth = true;
			this.gridColumn19.UnboundType = UnboundColumnType.Boolean;
			this.gridColumn19.Visible = true;
			this.gridColumn19.VisibleIndex = 5;
			this.gridColumn19.Width = 60;
			this.xtraTabControl2.Dock = DockStyle.Fill;
			this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
			this.xtraTabControl2.Name = "xtraTabControl2";
			this.xtraTabControl2.SelectedTabPage = this.xtraTabPage4;
			this.xtraTabControl2.Size = new System.Drawing.Size(999, 248);
			this.xtraTabControl2.TabIndex = 1;
			this.xtraTabControl2.TabPages.AddRange(new XtraTabPage[]
			{
				this.xtraTabPage4, 
				this.xtraTabPage5
			});
			this.xtraTabPage4.Controls.Add(this.groupControl2);
			this.xtraTabPage4.Controls.Add(this.groupControl1);
			this.xtraTabPage4.Controls.Add(this.groupControl5);
			this.xtraTabPage4.Controls.Add(this.groupControl4);
            this.xtraTabPage4.Controls.Add(this.groupControl6);
			this.xtraTabPage4.Name = "xtraTabPage4";
			this.xtraTabPage4.Size = new System.Drawing.Size(993, 222);
			this.xtraTabPage4.Text = "Settings";
			this.groupControl2.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
			this.groupControl2.Controls.Add(this.txtTransactionTimeSpan);
			this.groupControl2.Controls.Add(this.labelControl10);
			this.groupControl2.Controls.Add(this.txtMaxTimePerHalf);
			this.groupControl2.Controls.Add(this.labelControl7);
			this.groupControl2.Controls.Add(this.chbAllowHalftime);
			this.groupControl2.Location = new System.Drawing.Point(209, 3);
			this.groupControl2.Name = "groupControl2";
			this.groupControl2.Size = new System.Drawing.Size(200, 212);
			this.groupControl2.TabIndex = 14;
			this.groupControl2.Text = "Time Settings";
			this.txtTransactionTimeSpan.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			BaseEdit arg_22D6_0 = this.txtTransactionTimeSpan;
			int[] array = new int[4];
			array[0] = 15;
			arg_22D6_0.EditValue = new decimal(array);
			this.txtTransactionTimeSpan.Location = new System.Drawing.Point(123, 80);
			this.txtTransactionTimeSpan.Name = "txtTransactionTimeSpan";
			this.txtTransactionTimeSpan.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.txtTransactionTimeSpan.Properties.IsFloatValue = false;
			this.txtTransactionTimeSpan.Properties.Mask.EditMask = "N00";
			RepositoryItemSpinEdit arg_2378_0 = this.txtTransactionTimeSpan.Properties;
			array = new int[4];
			array[0] = 20;
			arg_2378_0.MaxValue = new decimal(array);
			RepositoryItemSpinEdit arg_239D_0 = this.txtTransactionTimeSpan.Properties;
			array = new int[4];
			array[0] = 5;
			arg_239D_0.MinValue = new decimal(array);
			this.txtTransactionTimeSpan.Size = new System.Drawing.Size(72, 20);
			this.txtTransactionTimeSpan.TabIndex = 9;
			this.labelControl10.Location = new System.Drawing.Point(5, 81);
			this.labelControl10.Name = "labelControl10";
			this.labelControl10.Size = new System.Drawing.Size(112, 13);
			this.labelControl10.TabIndex = 8;
			this.labelControl10.Text = "Transaction Time Span:";
			this.txtMaxTimePerHalf.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			BaseEdit arg_244C_0 = this.txtMaxTimePerHalf;
			array = new int[4];
			array[0] = 40;
			arg_244C_0.EditValue = new decimal(array);
			this.txtMaxTimePerHalf.Location = new System.Drawing.Point(123, 50);
			this.txtMaxTimePerHalf.Name = "txtMaxTimePerHalf";
			this.txtMaxTimePerHalf.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.txtMaxTimePerHalf.Properties.IsFloatValue = false;
			this.txtMaxTimePerHalf.Properties.Mask.EditMask = "N00";
			this.txtMaxTimePerHalf.Size = new System.Drawing.Size(72, 20);
			this.txtMaxTimePerHalf.TabIndex = 7;
			this.labelControl7.Location = new System.Drawing.Point(5, 53);
			this.labelControl7.Name = "labelControl7";
			this.labelControl7.Size = new System.Drawing.Size(90, 13);
			this.labelControl7.TabIndex = 6;
			this.labelControl7.Text = "Max Time Per Half:";
			this.chbAllowHalftime.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.chbAllowHalftime.EditValue = true;
			this.chbAllowHalftime.Location = new System.Drawing.Point(5, 26);
			this.chbAllowHalftime.Name = "chbAllowHalftime";
			this.chbAllowHalftime.Properties.Caption = "Allow Halftime";
			this.chbAllowHalftime.Size = new System.Drawing.Size(190, 19);
			this.chbAllowHalftime.TabIndex = 5;
			this.groupControl1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
			this.groupControl1.Controls.Add(this.btnSetUpdateInterval);
			this.groupControl1.Controls.Add(this.txt3IN1BETUpdateInterval);
			this.groupControl1.Controls.Add(this.labelControl3);
			this.groupControl1.Controls.Add(this.txtIBETUpdateInterval);
			this.groupControl1.Controls.Add(this.labelControl4);
			this.groupControl1.Location = new System.Drawing.Point(415, 3);
			this.groupControl1.Name = "groupControl1";
			this.groupControl1.Size = new System.Drawing.Size(200, 212);
			this.groupControl1.TabIndex = 13;
			this.groupControl1.Text = "Data Settings";
			this.btnSetUpdateInterval.Location = new System.Drawing.Point(59, 77);
			this.btnSetUpdateInterval.Name = "btnSetUpdateInterval";
			this.btnSetUpdateInterval.Size = new System.Drawing.Size(136, 23);
			this.btnSetUpdateInterval.TabIndex = 6;
			this.btnSetUpdateInterval.Text = "Set Update Interval";
			this.btnSetUpdateInterval.Click += new System.EventHandler(this.btnSetUpdateInterval_Click);
			this.txt3IN1BETUpdateInterval.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			BaseEdit arg_274B_0 = this.txt3IN1BETUpdateInterval;
			array = new int[4];
			array[0] = 8;
			arg_274B_0.EditValue = new decimal(array);
			this.txt3IN1BETUpdateInterval.Location = new System.Drawing.Point(132, 51);
			this.txt3IN1BETUpdateInterval.Name = "txt3IN1BETUpdateInterval";
			this.txt3IN1BETUpdateInterval.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.txt3IN1BETUpdateInterval.Properties.IsFloatValue = false;
			this.txt3IN1BETUpdateInterval.Properties.Mask.EditMask = "N00";
			this.txt3IN1BETUpdateInterval.Size = new System.Drawing.Size(63, 20);
			this.txt3IN1BETUpdateInterval.TabIndex = 5;
			this.txt3IN1BETUpdateInterval.EditValueChanged += new System.EventHandler(this.txt3IN1BETUpdateInterval_EditValueChanged);
			this.labelControl3.Location = new System.Drawing.Point(5, 54);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(121, 13);
			this.labelControl3.TabIndex = 4;
			this.labelControl3.Text = "3IN1BET Update Interval:";
			this.txtIBETUpdateInterval.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			BaseEdit arg_288F_0 = this.txtIBETUpdateInterval;
			array = new int[4];
			array[0] = 8;
			arg_288F_0.EditValue = new decimal(array);
			this.txtIBETUpdateInterval.Location = new System.Drawing.Point(132, 25);
			this.txtIBETUpdateInterval.Name = "txtIBETUpdateInterval";
			this.txtIBETUpdateInterval.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.txtIBETUpdateInterval.Properties.IsFloatValue = false;
			this.txtIBETUpdateInterval.Properties.Mask.EditMask = "N00";
			this.txtIBETUpdateInterval.Size = new System.Drawing.Size(63, 20);
			this.txtIBETUpdateInterval.TabIndex = 1;
			this.txtIBETUpdateInterval.EditValueChanged += new System.EventHandler(this.txtIBETUpdateInterval_EditValueChanged);
			this.labelControl4.Location = new System.Drawing.Point(5, 28);
			this.labelControl4.Name = "labelControl4";
			this.labelControl4.Size = new System.Drawing.Size(105, 13);
			this.labelControl4.TabIndex = 0;
			this.labelControl4.Text = "IBET Update Interval:";

			this.groupControl5.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
			this.groupControl5.Controls.Add(this.chbHighRevenueBoost);
			this.groupControl5.Controls.Add(this.txtLowestOddValue);
			this.groupControl5.Controls.Add(this.labelControl9);
			this.groupControl5.Controls.Add(this.txtOddValueDifferenet);
			this.groupControl5.Controls.Add(this.labelControl8);
            this.groupControl5.Controls.Add(this.checkEdit9);
            this.groupControl5.Controls.Add(this.checkEdit8);
            this.groupControl5.Controls.Add(this.checkEdit7);
			this.groupControl5.Controls.Add(this.checkEdit6);
			this.groupControl5.Controls.Add(this.checkEdit5);
			this.groupControl5.Controls.Add(this.checkEdit4);
			this.groupControl5.Controls.Add(this.checkEdit3);
			this.groupControl5.Location = new System.Drawing.Point(3, 3);
			this.groupControl5.Name = "groupControl5";
			this.groupControl5.Size = new System.Drawing.Size(200, 212);
			this.groupControl5.TabIndex = 13;
			this.groupControl5.Text = "Trading Settings";
			this.chbHighRevenueBoost.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.chbHighRevenueBoost.Location = new System.Drawing.Point(5, 203);
			this.chbHighRevenueBoost.Name = "chbHighRevenueBoost";
			this.chbHighRevenueBoost.Properties.Caption = "High Revenue Boost";
			this.chbHighRevenueBoost.Size = new System.Drawing.Size(190, 19);
			this.chbHighRevenueBoost.TabIndex = 13;
			this.txtLowestOddValue.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtLowestOddValue.EditValue = new decimal(new int[]
			{
				5, 
				0, 
				0, 
				65536
			});
			this.txtLowestOddValue.Location = new System.Drawing.Point(110, 178);
			this.txtLowestOddValue.Name = "txtLowestOddValue";
			this.txtLowestOddValue.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.txtLowestOddValue.Properties.Increment = new decimal(new int[]
			{
				1, 
				0, 
				0, 
				65536
			});
			RepositoryItemSpinEdit arg_2C2D_0 = this.txtLowestOddValue.Properties;
			array = new int[4];
			array[0] = 1;
			arg_2C2D_0.MaxValue = new decimal(array);
			this.txtLowestOddValue.Properties.MinValue = new decimal(new int[]
			{
				1, 
				0, 
				0, 
				131072
			});
			this.txtLowestOddValue.Size = new System.Drawing.Size(85, 20);
			this.txtLowestOddValue.TabIndex = 12;
			this.labelControl9.Location = new System.Drawing.Point(5, 181);
			this.labelControl9.Name = "labelControl9";
			this.labelControl9.Size = new System.Drawing.Size(90, 13);
			this.labelControl9.TabIndex = 11;
			this.labelControl9.Text = "Lowest Odd Value:";
			this.txtOddValueDifferenet.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtOddValueDifferenet.EditValue = new decimal(new int[]
			{
				2, 
				0, 
				0, 
				-2147352576
			});
			this.txtOddValueDifferenet.Location = new System.Drawing.Point(110, 153);
			this.txtOddValueDifferenet.Name = "txtOddValueDifferenet";
			this.txtOddValueDifferenet.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.txtOddValueDifferenet.Properties.Increment = new decimal(new int[]
			{
				1, 
				0, 
				0, 
				-2147352576
			});
			this.txtOddValueDifferenet.Properties.MinValue = new decimal(new int[]
			{
				2, //cho nay
				0, 
				0, 
				-2147352576
			});
			this.txtOddValueDifferenet.Size = new System.Drawing.Size(85, 20);
			this.txtOddValueDifferenet.TabIndex = 10;
			this.labelControl8.Location = new System.Drawing.Point(5, 156);
			this.labelControl8.Name = "labelControl8";
			this.labelControl8.Size = new System.Drawing.Size(99, 13);
			this.labelControl8.TabIndex = 9;
			this.labelControl8.Text = "Odd Value Different:";
			
            this.checkEdit6.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.checkEdit6.EditValue = true;
			this.checkEdit6.Enabled = false;
			this.checkEdit6.Location = new System.Drawing.Point(5, 101);
			this.checkEdit6.Name = "checkEdit6";
			this.checkEdit6.Properties.Caption = "Non-Live";
			this.checkEdit6.Size = new System.Drawing.Size(190, 19);
			this.checkEdit6.TabIndex = 8;
			
            this.checkEdit5.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.checkEdit5.EditValue = true;
			this.checkEdit5.Enabled = false;
			this.checkEdit5.Location = new System.Drawing.Point(5, 76);
			this.checkEdit5.Name = "checkEdit5";
			this.checkEdit5.Properties.Caption = "Live";
			this.checkEdit5.Size = new System.Drawing.Size(190, 19);
			this.checkEdit5.TabIndex = 7;
			
            this.checkEdit4.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.checkEdit4.EditValue = true;
			this.checkEdit4.Enabled = true;
			this.checkEdit4.Location = new System.Drawing.Point(5, 51);
			this.checkEdit4.Name = "checkEdit4";
			this.checkEdit4.Properties.Caption = "Allow Over/Under";
			this.checkEdit4.Size = new System.Drawing.Size(190, 19);
			this.checkEdit4.TabIndex = 6;

			this.checkEdit3.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.checkEdit3.EditValue = true;
			this.checkEdit3.Enabled = true;
			this.checkEdit3.Location = new System.Drawing.Point(5, 26);
			this.checkEdit3.Name = "checkEdit3";
			this.checkEdit3.Properties.Caption = "Allow Handicap";
			this.checkEdit3.Size = new System.Drawing.Size(190, 19);
			this.checkEdit3.TabIndex = 5;

            this.checkEdit7.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            this.checkEdit7.EditValue = true;
            this.checkEdit7.Enabled = true;
            this.checkEdit7.Location = new System.Drawing.Point(5, 126);
            this.checkEdit7.Name = "checkEdit7";
            this.checkEdit7.Properties.Caption = "Allow x.5 Over/Under from min 30";
            this.checkEdit7.Size = new System.Drawing.Size(190, 19);
            this.checkEdit7.TabIndex = 9;

            this.checkEdit8.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            this.checkEdit8.EditValue = false;
            this.checkEdit8.Enabled = true;
            this.checkEdit8.Location = new System.Drawing.Point(5, 228);
            this.checkEdit8.Name = "checkEdit8";
            this.checkEdit8.Properties.Caption = "Check credit before bet";
            this.checkEdit8.Size = new System.Drawing.Size(190, 19);
            this.checkEdit8.TabIndex = 14;

            this.checkEdit9.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            this.checkEdit9.EditValue = false;
            this.checkEdit9.Enabled = true;
            this.checkEdit9.Location = new System.Drawing.Point(5, 252);
            this.checkEdit9.Name = "checkEdit9";
            this.checkEdit9.Properties.Caption = "Play sound";
            this.checkEdit9.Size = new System.Drawing.Size(190, 19);
            this.checkEdit9.TabIndex = 14;

			this.groupControl4.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
			this.groupControl4.Controls.Add(this.txt3in1BETFixedStake);
			this.groupControl4.Controls.Add(this.labelControl2);
			this.groupControl4.Controls.Add(this.txtStake);
			this.groupControl4.Controls.Add(this.chbRandomStake);
			this.groupControl4.Controls.Add(this.txtIBETFixedStake);
			this.groupControl4.Controls.Add(this.labelControl6);
			this.groupControl4.Location = new System.Drawing.Point(621, 3);
			this.groupControl4.Name = "groupControl4";
			this.groupControl4.Size = new System.Drawing.Size(200, 212);
			this.groupControl4.TabIndex = 12;
			this.groupControl4.Text = "Stake Settings";

            

			this.txt3in1BETFixedStake.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			BaseEdit arg_31A4_0 = this.txt3in1BETFixedStake;
			array = new int[4];
			array[0] = 10;
			arg_31A4_0.EditValue = new decimal(array);
            

            this.txt3in1BETFixedStake.Location = new System.Drawing.Point(112, 51);
			this.txt3in1BETFixedStake.Name = "txt3in1BETFixedStake";
			this.txt3in1BETFixedStake.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.txt3in1BETFixedStake.Properties.IsFloatValue = false;
			this.txt3in1BETFixedStake.Properties.Mask.EditMask = "N00";
			this.txt3in1BETFixedStake.Size = new System.Drawing.Size(83, 20);
			this.txt3in1BETFixedStake.TabIndex = 5;
			this.labelControl2.Location = new System.Drawing.Point(5, 54);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(101, 13);
			this.labelControl2.TabIndex = 4;
			this.labelControl2.Text = "3IN1BET Fixed Stake:";
			this.txtStake.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.txtStake.EditValue = "10\r\n15\r\n25\r\n35\r\n50\r\n30\r\n20\r\n30";            


            this.txtStake.Location = new System.Drawing.Point(5, 103);
			this.txtStake.Name = "txtStake";
			this.txtStake.Size = new System.Drawing.Size(190, 104);
			this.txtStake.TabIndex = 3;
			this.chbRandomStake.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.chbRandomStake.EditValue = true;
			this.chbRandomStake.Location = new System.Drawing.Point(3, 78);
			this.chbRandomStake.Name = "chbRandomStake";
			this.chbRandomStake.Properties.Caption = "Random Stake";
			this.chbRandomStake.Size = new System.Drawing.Size(192, 19);
			this.chbRandomStake.TabIndex = 2;
			this.chbRandomStake.CheckedChanged += new System.EventHandler(this.chbRandomStake_CheckedChanged);
			this.txtIBETFixedStake.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			BaseEdit arg_33D2_0 = this.txtIBETFixedStake;
			array = new int[4];
			array[0] = 15;
			arg_33D2_0.EditValue = new decimal(array);
			this.txtIBETFixedStake.Location = new System.Drawing.Point(112, 25);
			this.txtIBETFixedStake.Name = "txtIBETFixedStake";
			this.txtIBETFixedStake.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.txtIBETFixedStake.Properties.IsFloatValue = false;
			this.txtIBETFixedStake.Properties.Mask.EditMask = "N00";
			this.txtIBETFixedStake.Size = new System.Drawing.Size(83, 20);
			this.txtIBETFixedStake.TabIndex = 1;
			this.labelControl6.Location = new System.Drawing.Point(5, 28);
			this.labelControl6.Name = "labelControl6";
			this.labelControl6.Size = new System.Drawing.Size(85, 13);
			this.labelControl6.TabIndex = 0;
			this.labelControl6.Text = "IBET Fixed Stake:";

            #region group6



            this.groupControl6.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
            this.groupControl6.Controls.Add(this.chkListAllowedMatch);
            this.groupControl6.Location = new System.Drawing.Point(825, 3);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.Size = new System.Drawing.Size(295, 212);
            this.groupControl6.TabIndex = 19;
            this.groupControl6.Text = "Allowed Betting Matches";
            //this.groupControl6.Resize 


            

            this.chkListAllowedMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkListAllowedMatch.CheckOnClick = true;
            //this.chkListAllowedMatch.Dock = DockStyle.Fill;
            this.chkListAllowedMatch.HighlightedItemStyle = DevExpress.XtraEditors.HighlightStyle.Skinned;
            this.chkListAllowedMatch.ItemHeight = 16;
            this.chkListAllowedMatch.Location = new System.Drawing.Point(5, 25);
            this.chkListAllowedMatch.Name = "chkListAllowedMatch";
            this.chkListAllowedMatch.Size = new System.Drawing.Size(283, 182);
            this.chkListAllowedMatch.TabIndex = 20;
            //this.chkListAllowedMatch.ItemCheck += new ItemCheckedEventHandler (this.chkListAllowedMatch_ItemCheck);
            #endregion
            
            
            this.xtraTabPage5.Controls.Add(this.grdTransaction);
			this.xtraTabPage5.Name = "xtraTabPage5";
			this.xtraTabPage5.Size = new System.Drawing.Size(993, 222);
			this.xtraTabPage5.Text = "Live Transaction";
			this.grdTransaction.Dock = DockStyle.Fill;
			this.grdTransaction.Location = new System.Drawing.Point(0, 0);
			this.grdTransaction.MainView = this.gridView2;
			this.grdTransaction.Name = "grdTransaction";
			this.grdTransaction.Size = new System.Drawing.Size(993, 222);
			this.grdTransaction.TabIndex = 3;
			this.grdTransaction.ViewCollection.AddRange(new BaseView[]
			{
				this.gridView2
			});
			this.gridView2.Columns.AddRange(new GridColumn[]
			{
				this.gridColumn1, 
				this.gridColumn2, 
				this.gridColumn3, 
				this.gridColumn6, 
				this.gridColumn4, 
				this.gridColumn5, 
				this.gridColumn7, 
				this.gridColumn8, 
				this.gridColumn9, 
				this.gridColumn10, 
				this.gridColumn11, 
				this.gridColumn12, 
				this.gridColumn20, 
				this.gridColumn13
			});
			this.gridView2.GridControl = this.grdTransaction;
			this.gridView2.Name = "gridView2";
			this.gridView2.OptionsBehavior.Editable = false;
			this.gridView2.OptionsCustomization.AllowGroup = false;
			this.gridView2.OptionsView.AutoCalcPreviewLineCount = true;
			this.gridView2.OptionsView.ShowAutoFilterRow = true;
			this.gridView2.OptionsView.ShowFooter = true;
			this.gridView2.OptionsView.ShowGroupPanel = false;
			this.gridView2.OptionsView.ShowPreview = true;
			this.gridView2.PreviewFieldName = "Note";
			this.gridView2.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumn13, ColumnSortOrder.Descending)
			});
			this.gridColumn1.Caption = "ID";
			this.gridColumn1.DisplayFormat.FormatType = FormatType.Numeric;
			this.gridColumn1.FieldName = "ID";
			this.gridColumn1.Name = "gridColumn1";
			this.gridColumn1.OptionsColumn.FixedWidth = true;
			this.gridColumn1.UnboundType = UnboundColumnType.Integer;
			this.gridColumn1.Visible = true;
			this.gridColumn1.VisibleIndex = 0;
			this.gridColumn1.Width = 50;
			this.gridColumn2.Caption = "Home Team";
			this.gridColumn2.FieldName = "HomeTeamName";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.UnboundType = UnboundColumnType.String;
			this.gridColumn2.Visible = true;
			this.gridColumn2.VisibleIndex = 1;
			this.gridColumn2.Width = 20;
			this.gridColumn3.Caption = "Away Team";
			this.gridColumn3.FieldName = "AwayTeamName";
			this.gridColumn3.Name = "gridColumn3";
			this.gridColumn3.UnboundType = UnboundColumnType.String;
			this.gridColumn3.Visible = true;
			this.gridColumn3.VisibleIndex = 2;
			this.gridColumn3.Width = 20;
			this.gridColumn6.Caption = "Type";
			this.gridColumn6.FieldName = "OddType";
			this.gridColumn6.Name = "gridColumn6";
			this.gridColumn6.OptionsColumn.FixedWidth = true;
			this.gridColumn6.UnboundType = UnboundColumnType.String;
			this.gridColumn6.Visible = true;
			this.gridColumn6.VisibleIndex = 3;
			this.gridColumn6.Width = 180;
			this.gridColumn4.Caption = "Odd";
			this.gridColumn4.FieldName = "OddKindValue";
			this.gridColumn4.Name = "gridColumn4";
			this.gridColumn4.OptionsColumn.FixedWidth = true;
			this.gridColumn4.UnboundType = UnboundColumnType.String;
			this.gridColumn4.Visible = true;
			this.gridColumn4.VisibleIndex = 4;
			this.gridColumn5.Caption = "Value";
			this.gridColumn5.FieldName = "OddValue";
			this.gridColumn5.Name = "gridColumn5";
			this.gridColumn5.OptionsColumn.FixedWidth = true;
			this.gridColumn5.UnboundType = UnboundColumnType.String;
			this.gridColumn5.Visible = true;
			this.gridColumn5.VisibleIndex = 5;
			this.gridColumn5.Width = 80;
			this.gridColumn7.Caption = "Stake";
			this.gridColumn7.FieldName = "Stake";
			this.gridColumn7.Name = "gridColumn7";
			this.gridColumn7.OptionsColumn.FixedWidth = true;
			this.gridColumn7.UnboundType = UnboundColumnType.String;
			this.gridColumn7.Visible = true;
			this.gridColumn7.VisibleIndex = 6;
			this.gridColumn7.Width = 100;
			this.gridColumn8.Caption = "I Allow";
			this.gridColumn8.FieldName = "IBETAllow";
			this.gridColumn8.Name = "gridColumn8";
			this.gridColumn8.OptionsColumn.FixedWidth = true;
			this.gridColumn8.UnboundType = UnboundColumnType.Boolean;
			this.gridColumn8.Visible = true;
			this.gridColumn8.VisibleIndex = 7;
			this.gridColumn8.Width = 50;
			this.gridColumn9.Caption = "3 Allow";
            this.gridColumn9.FieldName = "THREEIN1Allow";
			this.gridColumn9.Name = "gridColumn9";
			this.gridColumn9.OptionsColumn.FixedWidth = true;
			this.gridColumn9.UnboundType = UnboundColumnType.Boolean;
			this.gridColumn9.Visible = true;
			this.gridColumn9.VisibleIndex = 8;
			this.gridColumn9.Width = 50;
			this.gridColumn10.Caption = "I Trade";
			this.gridColumn10.FieldName = "IBETTrade";
			this.gridColumn10.Name = "gridColumn10";
			this.gridColumn10.OptionsColumn.FixedWidth = true;
			this.gridColumn10.UnboundType = UnboundColumnType.Boolean;
			this.gridColumn10.Visible = true;
			this.gridColumn10.VisibleIndex = 9;
			this.gridColumn10.Width = 50;
			this.gridColumn11.Caption = "3 Trade";
            this.gridColumn11.FieldName = "THREEIN1Trade";
			this.gridColumn11.Name = "gridColumn11";
			this.gridColumn11.OptionsColumn.FixedWidth = true;
			this.gridColumn11.UnboundType = UnboundColumnType.Boolean;
			this.gridColumn11.Visible = true;
			this.gridColumn11.VisibleIndex = 10;
			this.gridColumn11.Width = 50;
			this.gridColumn12.Caption = "3 Retrade";
            this.gridColumn12.FieldName = "THREEIN1ReTrade";
			this.gridColumn12.Name = "gridColumn12";
			this.gridColumn12.OptionsColumn.FixedWidth = true;
			this.gridColumn12.UnboundType = UnboundColumnType.Boolean;
			this.gridColumn12.Visible = true;
			this.gridColumn12.VisibleIndex = 11;
			this.gridColumn12.Width = 50;
			this.gridColumn20.Caption = "I Retrade";
			this.gridColumn20.FieldName = "IBETReTrade";
			this.gridColumn20.Name = "gridColumn20";
			this.gridColumn20.OptionsColumn.FixedWidth = true;
			this.gridColumn20.UnboundType = UnboundColumnType.Boolean;
			this.gridColumn20.Visible = true;
			this.gridColumn20.VisibleIndex = 12;
			this.gridColumn20.Width = 50;
			this.gridColumn13.Caption = "DateTime";
			this.gridColumn13.DisplayFormat.FormatString = "dd/MM/yyyy hh:mm:ss";
			this.gridColumn13.DisplayFormat.FormatType = FormatType.DateTime;
			this.gridColumn13.FieldName = "DateTime";
			this.gridColumn13.Name = "gridColumn13";
			this.gridColumn13.OptionsColumn.FixedWidth = true;
			this.gridColumn13.SortMode = ColumnSortMode.Value;
			this.gridColumn13.SummaryItem.SummaryType = SummaryItemType.Count;
			this.gridColumn13.UnboundType = UnboundColumnType.DateTime;
			this.gridColumn13.Visible = true;
			this.gridColumn13.VisibleIndex = 13;
			this.gridColumn13.Width = 130;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new System.Drawing.Size(1130, 678);
			base.Controls.Add(this.splitContainerControl1);
			base.Controls.Add(this.ribbonControl1);
///			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "TerminalForm";
			this.Ribbon = this.ribbonControl1;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "IBET vs 3IN1BET";
			((System.ComponentModel.ISupportInitialize)this.ribbonControl1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.splitContainerControl1).EndInit();
			this.splitContainerControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.xtraTabControl1).EndInit();
			this.xtraTabControl1.ResumeLayout(false);
			this.xtraTabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.panelControl5).EndInit();
			this.panelControl5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.panelControl4).EndInit();
			this.panelControl4.ResumeLayout(false);
			this.panelControl4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.txtIBETAddress.Properties).EndInit();
			this.xtraTabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.panelControl3).EndInit();
			this.panelControl3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.panelControl2).EndInit();
			this.panelControl2.ResumeLayout(false);
			this.panelControl2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.txt1IN1BETAddress.Properties).EndInit();

            this.xtraTabPage9.ResumeLayout(false);

			this.xtraTabPage3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.grdSameMatch).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.xtraTabControl2).EndInit();
			this.xtraTabControl2.ResumeLayout(false);
			this.xtraTabPage4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.groupControl2).EndInit();
			this.groupControl2.ResumeLayout(false);
			this.groupControl2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.txtTransactionTimeSpan.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaxTimePerHalf.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chbAllowHalftime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.groupControl1).EndInit();
			this.groupControl1.ResumeLayout(false);
			this.groupControl1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.txt3IN1BETUpdateInterval.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtIBETUpdateInterval.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.groupControl5).EndInit();
			this.groupControl5.ResumeLayout(false);
			this.groupControl5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.chbHighRevenueBoost.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtLowestOddValue.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtOddValueDifferenet.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.checkEdit9.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.checkEdit8.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.checkEdit7.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.checkEdit6.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.checkEdit5.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.checkEdit4.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.checkEdit3.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.groupControl4).EndInit();
			this.groupControl4.ResumeLayout(false);
			this.groupControl4.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)this.chkListAllowedMatch).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.groupControl6).EndInit();
            this.groupControl6.ResumeLayout(false);
            this.groupControl6.PerformLayout();

			((System.ComponentModel.ISupportInitialize)this.txt3in1BETFixedStake.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtStake.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chbRandomStake.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtIBETFixedStake.Properties).EndInit();
			this.xtraTabPage5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.grdTransaction).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView2).EndInit();

            Icon icoMain = Properties.Resources._1;
            this.Icon = icoMain;
			base.ResumeLayout(false);
		}

        private void chkListAllowedMatch_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            bool optionSet = e.State == CheckState.Checked ? true : false;
        }
        private void playSound(bool success)
        {
            bool soundChecked = checkEdit9.Checked;
            if (soundChecked && success)
            {
                WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
                wplayer.URL = "s.mp3";
                wplayer.controls.play();
            }
            if (soundChecked && !success)
            {
                WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
                wplayer.URL = "f.wav";
                wplayer.controls.play();
            }
        }
        
	}
}
