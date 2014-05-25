//#define DEBUG
#define TESTMODE
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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using iBet.App.admin;
using iBet.DTO;

using iBet.Engine;
using iBet.Utilities;



namespace iBet.App
{
    public class FollowSub : RibbonForm
    {
        private const int INTERNET_COOKIE_HTTPONLY = 8192;
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
        private IBetEngine _ibetEngine;
        public bool isScanner = false;

        private IBetSubEngine _ibetSubEngine;
        private System.Collections.Generic.List<MatchDTO> _listIBETMatch;
        private System.Collections.Generic.List<MatchDTO> _listIBETMatchNow;

        private System.Collections.Generic.List<MatchDTO> _listIBETMatchNonLive;
        private System.Collections.Generic.List<MatchDTO> _listIBETMatchNonLiveNow;

        private System.Collections.Generic.List<BetDTO> _listBetInSubMatch;
        private System.Collections.Generic.List<BetDTO> _listDoneBetInSub;
        private System.Collections.Generic.List<AccountDTO> _listAccountInSubOutStanding;
        private System.Collections.Generic.List<AccountDTO> _lastAccountInSubOutStanding;

        private System.Collections.Generic.List<TransactionDTO> _listTransaction;
        private System.Collections.Generic.List<TransactionDTO> _listWaitingTransaction;
        private System.Collections.Generic.Dictionary<string, System.DateTime> _OldBetListHistory;
        private System.Collections.Generic.Dictionary<string, System.DateTime> _BetWaitingList;
        private bool _running = false;
        private bool _comparing = false;
        private bool _compareAgain = false;
        private bool _comparingAcc = false;
        private bool _compareAgainAcc = false;
        private bool _betting = false;
        private string _followingAccount = "";
        private System.Windows.Forms.Timer _forceRefreshTimer;

        private System.Windows.Forms.Timer _creditRefreshTimer;

        private string _ibetAccount = "";
        private string _ibetSubAccount = "";
        private admin.DataServiceSoapClient _dataService;
        private string _currentUserID;
        private System.DateTime _lastTransactionTime = System.DateTime.Now;
        private System.DateTime _lastTrans = System.DateTime.Now;
        private System.Uri _ibetDataUrl;
        private MainForm _mainForm;
        private System.ComponentModel.IContainer components = null;
        private RibbonControl ribbonControl1;
        private RibbonPage ribbonPage1;
        private RibbonPageGroup rpgSbobet;
        private BarStaticItem barStaticItem1;
        private BarStaticItem barStaticItem2;
        private BarStaticItem barStaticItem3;
        private BarButtonItem btnSbobetGetInfo;
        private BarStaticItem lblSbobetCurrentCredit;
        private BarStaticItem lblSbobetTotalMatch;
        private BarStaticItem lblSbobetLastUpdate;
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
        private WebBrowser webSBOBET;
        private PanelControl panelControl2;
        private SimpleButton btnSBOBETGO;
        private TextEdit txtSBOBETAddress;
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

        private CheckEdit checkEdit12; // IBET over only // secured bet // check tran // sound // credit
        private CheckEdit checkEdit7; // rung
        private CheckEdit checkEdit6; // non-live
        private CheckEdit checkEdit5; // live
        private CheckEdit checkEdit4; // over/under
        private CheckEdit checkEdit3; // handicap
        private GroupControl groupControl4;
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
        private GridColumn gridColumn10;
        private GridColumn gridColumn13;
        private WebBrowser webIBET;
        private GroupControl groupControl1;
        private SpinEdit txtSBOBETUpdateInterval;
        private LabelControl labelControl3;
        private SpinEdit txtIBETUpdateInterval;
        private LabelControl labelControl4;
        private GroupControl groupControl2;
        private CheckEdit chbAllowHalftime;
        private SpinEdit txtMaxTimePerHalf;
        private LabelControl labelControl7;
        private GridColumn gridColumn14;
        private SpinEdit txtFollowPercent;
        private SimpleButton btnSetUpdateInterval;
        private SpinEdit txtLowestOddValue;
        private LabelControl labelControl9;
        private SpinEdit txtTransactionTimeSpan;
        private LabelControl labelControl10;
        private GroupControl groupControl6;
        private Label label1;
        private GridColumn gridColumn21;
        private GridColumn gridColumn22;
        private LabelControl labelControl11;
        private SpinEdit txtAllowTradeMinValue;
        private BarStaticItem lblIBETWinLost;
        private BarStaticItem lblIBETCom;
        private BarStaticItem lblIBETReject;
        private SimpleButton btnStatus;
        private BarStaticItem barStaticItem4;
        private BarStaticItem barStaticItem5;
        private BarStaticItem barStaticItem6;
        private ComboBoxEdit cbeSignatureTemplate;
        private ComboBoxEdit chooseSBOServer;
        private CheckEdit chkProxy;
        private CheckEdit chkFollowType;
        private CheckEdit chkFollowPercent;
        private XtraTabPage tabBetList;
        private GridControl grdBetList;
        private GridView gridView3;
        private GridColumn gridColumn9;
        private GridColumn gridColumn11;
        private GridColumn gridColumn12;
        private GridColumn gridColumn23;
        private GridColumn gridColumn24;
        private GridColumn gridColumn25;
        private GridColumn gridColumn27;
        private GridColumn gridColumn29;
        private GridColumn gridColumn28;
        private GridColumn gridColumn26;
        private GridColumn gridColumn30;
        private GridColumn gridColumn31;
        private XtraTabPage xtraTabPage6;
        private GridControl gridNonLiveMatch;
        private GridView gridView4;
        private GridColumn gridColumn20;
        private GridColumn gridColumn32;
        private GridColumn gridColumn35;
        private GridColumn gridColumn39;
        private CheckEdit chkEnable;
        private DevExpress.XtraEditors.MemoEdit txtListFollowAccounts;

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
            if (!FollowSub.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
            {
                if (num < 0u)
                {
                    result = null;
                    return result;
                }
                num = 1024u;
                stringBuilder = new System.Text.StringBuilder(1024);
                if (!FollowSub.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
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
            if (!FollowSub.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
            {
                if (num < 0u)
                {
                    result = null;
                    return result;
                }
                num = 1024u;
                stringBuilder = new System.Text.StringBuilder(1024);
                if (!FollowSub.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
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
            if (!FollowSub.InternetGetCookieEx(uri, null, stringBuilder, ref num, 8192, System.IntPtr.Zero))
            {
                if (num < 0u)
                {
                    result = null;
                    return result;
                }
                num = 1024u;
                stringBuilder = new System.Text.StringBuilder(1024);
                if (!FollowSub.InternetGetCookieEx(uri, null, stringBuilder, ref num, 8192, System.IntPtr.Zero))
                {
                    result = null;
                    return result;
                }
            }
            result = stringBuilder.ToString();
            return result;
        }
        public FollowSub(MainForm mainForm, string currentUserID)
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



            //this._OldBetListHistory = new Dictionary<string, DateTime>();
            //this._oddTransactionHistory = new System.Collections.Generic.Dictionary<string, System.DateTime>();
            this._listTransaction = new System.Collections.Generic.List<TransactionDTO>();

            this._listWaitingTransaction = new System.Collections.Generic.List<TransactionDTO>();
            this.grdTransaction.DataSource = this._listTransaction;

            this._listIBETMatchNow = new List<MatchDTO>();
            this.grdSameMatch.DataSource = this._listIBETMatchNow;

            this._listIBETMatchNonLiveNow = new List<MatchDTO>();
            this.gridNonLiveMatch.DataSource = this._listIBETMatchNonLiveNow;

            this._lastAccountInSubOutStanding = new List<AccountDTO>();
            this._listDoneBetInSub = new List<BetDTO>();
            this.grdBetList.DataSource = this._listDoneBetInSub;
            //this.grdBetList.DataSource = this._
        }
        private void _dataService_AllowRunCompleted(object sender, AllowRunCompletedEventArgs e)
        {
            if (e.Result)
            {
                this.Start();
                this._dataService.StartTerminalAsync(this._currentUserID, this.Text);
                //this._ibetEngine.UpdateDataInterval = (int)this.txtIBETUpdateInterval.Value * 1000;
                this._ibetSubEngine.UpdateDataInterval = (int)this.txtSBOBETUpdateInterval.Value * 1000;
            }
            else
            {
                this.ShowWarningDialog("Your logged in account is not permitted to start the current betting pair account. \nPlease contact with Administrators.");
            }
        }
        private void _creditRefreshTimer_Tick(object sender, System.EventArgs e)
        {
            BarItem arg_1A_0 = this.lblIbetCurrentCredit;
            //float currentCredit = this._ibetEngine.GetCurrentCredit();
            //arg_1A_0.Caption = currentCredit.ToString();
            //BarItem arg_39_0 = this.lblSbobetCurrentCredit;
            //currentCredit = this._sbobetEngine.GetCurrentCredit();
            //arg_39_0.Caption = currentCredit.ToString();
            //this._mainForm.InitializeWCFService();

        }
        private void _forceRefreshTimer_Tick(object sender, System.EventArgs e)
        {
#if DEBUG
            Utilities.WriteLog.Write("System is forced to restart");
#endif
            if (this._ibetEngine != null)
            {
                this._ibetEngine.Stop();
                this._ibetEngine.Start();
            }

            //ShowStatus();            
        }
        internal void Start()
        {
            if (this._ibetEngine != null)
            {
                this._ibetEngine.Start();
            }
            if (this._ibetSubEngine != null)
            {
                this._ibetSubEngine.Start();
            }

            this._running = true;
            this._forceRefreshTimer.Start();
            this._creditRefreshTimer.Start();

            //this.lblStatus.Caption = "RUNNING";
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            this.btnClear.Enabled = true;
            this.btnIbetGetInfo.Enabled = false;
            this.btnSbobetGetInfo.Enabled = false;
            //this._listDoneBetInSub.Clear();


            this._listDoneBetInSub.Clear();
            this.initFinished = false;
            this.grdBetList.RefreshDataSource();
            this._followingAccount = this.txtListFollowAccounts.Text;
            _lastAccountInSubOutStanding.Clear();
        }
        internal void StartFromTracking()
        {
            this._dataService.AllowRunAsync(this._currentUserID, this._ibetAccount.ToUpper(), this._ibetSubAccount.ToUpper());
        }
        internal void Stop()
        {
            if (initFinished)
            {
                if (this._ibetEngine != null)
                {
                    this._ibetEngine.Stop();
                }
                if (this._ibetSubEngine != null)
                {
                    this._ibetSubEngine.Stop();
                }
                this._running = false;
                this._forceRefreshTimer.Stop();
                this._creditRefreshTimer.Stop();

                //this.lblStatus.Caption = "STOPPED";
                this.btnStart.Enabled = true;
                this.btnStop.Enabled = false;
                this.btnClear.Enabled = false;
                this.btnIbetGetInfo.Enabled = true;
                this.btnSbobetGetInfo.Enabled = true;

                this._OldBetListHistory.Clear();
                //this._listAccountInSubOutStanding = null;
                //InitializeIBETSubEngine();

            }
            else
            {
                //ShowWarningDialog("Please wait until the bet list done");
            }
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

            string[] array = FollowSub.GetCookieString(this._ibetDataUrl.AbsoluteUri).Split(new string[]
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
                    cookieContainer.Add(new System.Uri("http://" + this.webIBET.Url.Host), new System.Net.Cookie("DispVer", "3"));
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
				this._ibetSubAccount, 
				" - Version: ", 
				System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
			});
            //this._ibetEngine = new IBetEngine(host, text3, dynamicFieldName, dynamicFieldValue, cookieContainer, true);
            //if (chkProxy.Checked)
            //    this._ibetEngine.useProxy = true;

            //this._ibetEngine.FullDataCompleted += new EngineDelegate(this._ibetEngine_FullDataCompleted);
            //this._ibetEngine.UpdateCompleted += new EngineDelegate(this._ibetEngine_UpdateCompleted);
            //this._ibetEngine.FullDataNonLiveCompleted += new EngineDelegate(this._ibetEngine_FullDataCompletedNonLive);

            this._ibetEngine.UpdateDataInterval = (int)this.txtIBETUpdateInterval.Value * 1000;
            float currenCredit = 0f;
            currenCredit = this._ibetEngine.GetCurrentCredit();

            this.lblIbetCurrentCredit.Caption = currenCredit.ToString();
            if (_ibetSubEngine != null)
            {
                this.btnStart.Enabled = true;
                this._mainForm.AddToActiveListAccPair(this._ibetAccount + " - " + this._ibetSubAccount);
            }
        }
        public void RunningThisAccount(string a)
        {
        }
        private void InitializeIBETSubEngine()
        {
            if (this._ibetSubEngine != null)
            {
                this._ibetSubEngine.Stop();
                System.GC.SuppressFinalize(this._ibetSubEngine);
            }
            System.Net.CookieContainer cookieContainer = new System.Net.CookieContainer();
            System.Uri url = this.webSBOBET.Url;
            string[] array = FollowSub.GetCookieString(this.webSBOBET.Document.Url.AbsoluteUri).Split(new string[]
			{
				";"
			}, System.StringSplitOptions.None);
            string[] array2 = url.Host.Split(new string[]
			{
				"."
			}, System.StringSplitOptions.None);
            string[] array3 = array;
            for (int i = 0; i < array3.Length; i++)
            {
                string text = array3[i];
                if (!text.Contains("OutstandingDetail"))
                {
                    string[] array4 = text.Split(new string[]
				    {
					    "="
				    }, System.StringSplitOptions.None);
                    if (array4.Length == 2)
                        cookieContainer.Add(new System.Uri("http://" + url.Host), new System.Net.Cookie(array4[0].Trim(), array4[1].Trim()));
                    else
                        cookieContainer.Add(new System.Uri("http://" + url.Host), new System.Net.Cookie(array4[0].Trim(), ""));
                }
                else
                {
                    string[] array4 = text.Split(new string[]
				    {
					    "="
				    }, System.StringSplitOptions.None);
                    cookieContainer.Add(new System.Uri("http://" + url.Host), new System.Net.Cookie(array4[0].Trim(), text.Replace(array4[0] + "=", "")));
                }
            }
            string URL = this.webSBOBET.Document.Url.AbsoluteUri.Replace("Index.aspx", "");
            string[] array5 = URL.Split(new string[]
			{
				"/"
			}, System.StringSplitOptions.None);
            string dynamicURL = array5[3];
            //string[] array6 = array5[0].Split(new string[]
            //{
            //    "="
            //}, System.StringSplitOptions.None);
            //string dynamicURL = array5[3];
            string host = url.Host;
            string innerHtml = this.webSBOBET.Document.Body.InnerHtml;

            int num = innerHtml.IndexOf("setCookie('");
            int num2 = innerHtml.IndexOf("', currentURL");
            //string text2 = innerHtml.Substring(num + 11, num2 - num - 11);            
            string text2 = "aaa";
            this._ibetSubEngine = new IBetSubEngine(host, dynamicURL, text2, cookieContainer);
            if (this._OldBetListHistory != null)
            {
                this._OldBetListHistory = null;
            }
            if (this._BetWaitingList != null)
            {
                this._BetWaitingList = null;
            }

            if (chkProxy.Checked)
                this._ibetSubEngine.useProxy = true;
            this._ibetSubEngine.UpdateDataInterval = (int)this.txtSBOBETUpdateInterval.Value * 1000;
            this._ibetSubEngine.FullDataCompleted += new EngineDelegate(this._ibetSubEngine_FullDataCompleted);
            this._ibetSubEngine.GetOutStandingDataCompleted += new EngineDelegate(this._ibetSubEngine_GetOutStandingDataCompleted);
            this._ibetSubAccount = txtListFollowAccounts.Text;
            //this.lblSbobetCurrentCredit.Caption = this._sbobetEngine.GetCurrentCredit().ToString();
            //int startIndex = this.webSBOBET.Document.Window.Frames[0].Document.Body.InnerHtml.IndexOf("<SPAN id=changeLoginName>") + 25;
            //string text2 = this.webSBOBET.Document.Window.Frames[0].Document.Body.InnerHtml.Substring(startIndex, 10);
            this.rpgSbobet.Text = "Sub IBET - " + this.txtListFollowAccounts.Text.ToUpper();
            //this._ibetSubAccount = txtFollowAcc.Text.ToUpper();
            this.Text = string.Concat(new object[]
			{
				this._ibetAccount, 
				" - ", 
				this._ibetSubAccount, 
				" - Version: ", 
				System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
			});
            this.btnStart.Enabled = true;
            if (_ibetEngine != null)
            {
                this.btnStart.Enabled = true;
                //this._mainForm.AddToActiveListAccPair(this._ibetAccount + " - " + this._ibetSubAccount);
            }
        }
        private void webIBET_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.txtIBETAddress.Text = this.webIBET.Url.AbsoluteUri;
            if (e.Url.AbsoluteUri.Contains("UnderOver_data.aspx?Market=l&Sport=1&DT=&RT=W&CT=&Game=0&OrderBy=0"))
            {
                this._ibetDataUrl = e.Url;
            }
        }
        private void webSBOBET_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.txtSBOBETAddress.Text = this.webSBOBET.Url.AbsoluteUri;
        }
        private void _ibetSubEngine_FullDataCompleted(BaseEngine sender, EngineEventArgs eventArgs)
        {
            switch (eventArgs.Type)
            {
                case eEngineEventType.Success:
                    {
                        if (this._listAccountInSubOutStanding != null)
                        {
                            System.GC.SuppressFinalize(this._listAccountInSubOutStanding);
                        }
                        this._listBetInSubMatch = BaseDTO.DeepClone<System.Collections.Generic.List<BetDTO>>((System.Collections.Generic.List<BetDTO>)eventArgs.Data);
                        //this._listAccountInSubOutStanding = BaseDTO.DeepClone<System.Collections.Generic.List<AccountDTO>>((System.Collections.Generic.List<AccountDTO>)eventArgs.Data);
                        this.StartCompareSameMatch();
                        this.lblSbobetLastUpdate.Caption = System.DateTime.Now.ToString("hh:mm:ss");
                        //this.RefreshAllowedListMatches();
                        break;
                    }
                case eEngineEventType.SessionExpired:
                    {
                        this.Stop();
                        this.lblSbobetLastUpdate.Caption = "Session Expired";
                        break;
                    }
            }
        }
        private void _ibetSubEngine_GetOutStandingDataCompleted(BaseEngine sender, EngineEventArgs eventArgs)
        {
            switch (eventArgs.Type)
            {
                case eEngineEventType.Success:
                    {
                        if (this._listBetInSubMatch != null)
                        {
                            System.GC.SuppressFinalize(this._listBetInSubMatch);
                        }
                        //this._listBetInSubMatch = BaseDTO.DeepClone<System.Collections.Generic.List<BetDTO>>((System.Collections.Generic.List<BetDTO>)eventArgs.Data);
                        this._listAccountInSubOutStanding = BaseDTO.DeepClone<System.Collections.Generic.List<AccountDTO>>((System.Collections.Generic.List<AccountDTO>)eventArgs.Data);
                        this.StartCompareAccounts();
                        this.lblSbobetLastUpdate.Caption = System.DateTime.Now.ToString("hh:mm:ss");
                        //this.RefreshAllowedListMatches();
                        break;
                    }
                case eEngineEventType.SessionExpired:
                    {
                        this.Stop();
                        this.lblSbobetLastUpdate.Caption = "Session Expired";
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
                        this.lblIbetLastUpdate.Caption = System.DateTime.Now.ToString("hh:mm:ss");
                        //this.StartCompareSameMatch();                        
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
                        this.lblIbetLastUpdate.Caption = System.DateTime.Now.ToString("hh:mm:ss");
                        //this.StartCompareSameMatch();
                        RefreshIbetList();
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

        private void _ibetEngine_FullDataCompletedNonLive(BaseEngine sender, EngineEventArgs eventArgs)
        {
            switch (eventArgs.Type)
            {
                case eEngineEventType.Success:
                    {
                        if (this._listIBETMatchNonLive != null)
                        {
                            System.GC.SuppressFinalize(this._listIBETMatch);
                        }
                        this._listIBETMatchNonLive = BaseDTO.DeepClone<System.Collections.Generic.List<MatchDTO>>((System.Collections.Generic.List<MatchDTO>)eventArgs.Data);
                        this.lblIbetLastUpdate.Caption = System.DateTime.Now.ToString("hh:mm:ss");
                        //this.StartCompareSameMatch();
                        RefreshIbetListNonLive();
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

        internal void GetBetList(out string string1, out string string2)
        {
            string1 = string.Empty;
            string2 = string.Empty;
            try
            {
                string1 = this._ibetEngine.GetBetList();

            }
            catch (Exception ex)
            {
                ShowWarningDialog("Error: " + ex);
            }
        }
        internal void GetStatement(out string string1, out string string2)
        {
            string1 = string.Empty;
            string2 = string.Empty;
            string1 = this._ibetEngine.GetStatement();
        }
        private void SendReportToMainForm(string report)
        {
            this._mainForm.WriteReport(report);
        }
        internal void ForceToRunByServer(string accName)
        {
            if (this._ibetAccount.ToLower().Contains(accName.ToLower()) && this._running)
            {
                SetInterval();
            }
        }
        private bool CheckCurrentAccountsForChange(List<AccountDTO> oldacc, AccountDTO acctoCompare)
        {
            if (oldacc == null || oldacc.Count == 0)
                return true;
            AccountDTO account = AccountDTO.SearchAccountByName(acctoCompare.Name, _lastAccountInSubOutStanding);
            if (account != null)
            {
                if (account.OutStanding == acctoCompare.OutStanding)
                {
                    return false;//khong thay doi
                }
                else
                {
                    float floatOld;
                    float floatNew;
                    if (float.TryParse(account.OutStanding.Replace(",", ""), out floatOld) && float.TryParse(acctoCompare.OutStanding.Replace(",", ""), out floatNew))
                    {
                        if (floatOld < floatNew)
                        {
                            //iBet.Utilities.WriteLog.Write(acctoCompare.AccountType + ":" + acctoCompare.Name + " got new outstanding: " + acctoCompare.OutStanding + ". Old value: " + account.OutStanding);
                            account.OutStanding = acctoCompare.OutStanding;
                            return true;// co thay doi
                        }
                        else
                        {
                            //iBet.Utilities.WriteLog.Write(acctoCompare.Name + ": from " + account.OutStanding + " down to " + acctoCompare.OutStanding);
                            account.OutStanding = acctoCompare.OutStanding;
                            return false;
                        }
                    }
                }
            }
            else
            {
                oldacc.Add((AccountDTO)acctoCompare.Clone());
                return true;
            }

            return false;
        }
        private void StartCompareSameMatch()
        {
            if (this._running && this.initFinished)
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
        private void StartCompareAccounts()
        {
            if (this._running)
            {
                if (this._comparingAcc)
                {
                    this._compareAgainAcc = true;
                }
                else
                {
                    System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.CompareAccountOutStanding));
                    thread.Start();
                }
            }
        }
        private void DetectAccountType()
        {
            if (!_ibetSubEngine._detectedAccount)
            {
                AccountDTO account = AccountDTO.SearchAccountByName(this._followingAccount, _lastAccountInSubOutStanding);
                if (account != null)
                {
                    _ibetSubEngine._highestFollowingAccount.AccountType = account.AccountType;
                    _ibetSubEngine._highestFollowingAccount.ID = account.ID;
                    _ibetSubEngine._highestFollowingAccount.Name = account.Name;
                    _ibetSubEngine._detectedAccount = true;
                    //iBet.Utilities.WriteLog.Write("Detected " + account.Name + " treo " + account.OutStanding);
                }
                else
                {
                    if (AccountDTO.IsMasterOfAllAccount(this._followingAccount, _lastAccountInSubOutStanding))
                    {
                        _ibetSubEngine._highestFollowingAccount.AccountType = "SuperMaster";
                        _ibetSubEngine._highestFollowingAccount.ID = "NULL";
                        _ibetSubEngine._highestFollowingAccount.Name = this._followingAccount;
                        _ibetSubEngine._detectedAccount = true;
                    }
                    else
                    {
                        //iBet.Utilities.WriteLog.Write(_followingAccount + " has no bet at this moment. ");
                    }
                }
            }
        }
        private bool CompareAccountWithFollowingField(AccountDTO account)
        {
            //int i = txtListFollowAccounts.Lines.Length;
            for (int i = 0; i < txtListFollowAccounts.Lines.Length; i++)
            {
                if (account.Name.Contains(txtListFollowAccounts.Lines[i]))
                    return true;
            }
            return false;
        }
        private bool CheckCorrectAccount(AccountDTO account)
        {
            if (account.Name.ToLower().Contains(this._followingAccount.ToLower()) || this._followingAccount.ToLower().Contains(account.Name.ToLower()))
                return true;
            return false;
        }

        private bool initFinished = false;
        private void CompareAccountOutStanding()
        {
            this._comparingAcc = false;
            System.Collections.Generic.List<AccountDTO> listOutstandingAccount = this._listAccountInSubOutStanding;

            if (listOutstandingAccount != null) // neu co tran treo cua account
            {
                if (!this.initFinished && !this._ibetSubEngine._detectedAccount)//neu list cac tran da bet chua duoc khoi tao - lan dau tien
                {
                    if (this._OldBetListHistory == null)
                        this._OldBetListHistory = new Dictionary<string, DateTime>();//khoi tao list da bet
                    foreach (AccountDTO account in listOutstandingAccount)//lay chi tiet cho tung account
                    {
                        if (account.AccountType == "Master")
                        {
                            if (CheckCorrectAccount(account))
                            {
                                List<AccountDTO> accountAgentList = this._ibetSubEngine.GetAccountDataFirstTime(account);
                                if (accountAgentList != null)
                                {
                                    foreach (AccountDTO accAgent in accountAgentList)
                                    {
                                        if (CheckCorrectAccount(accAgent))
                                        {
                                            List<AccountDTO> accountMemberList = this._ibetSubEngine.GetAccountDataFirstTime(accAgent);
                                            if (accountMemberList != null)
                                            {
                                                foreach (AccountDTO acc in accountMemberList)
                                                {
                                                    if (CheckCorrectAccount(acc))
                                                    {
                                                        List<BetDTO> oldbets = this._ibetSubEngine.GetBetsDataFirstTime(acc);//lay cac tran da biet cua account                                                         
                                                        if (oldbets != null)
                                                        {
                                                            AddAlreadyBetRunningToOldBetList(oldbets, acc);
                                                            this._lastAccountInSubOutStanding.Add((AccountDTO)acc.Clone());
                                                            //iBet.Utilities.WriteLog.Write("Added member: " + acc.Name + ":" + acc.OutStanding + " has " + oldbets.Count + " bets");
                                                        }
                                                        else
                                                        {
                                                        }
                                                    }
                                                }
                                            }
                                            this._lastAccountInSubOutStanding.Add((AccountDTO)accAgent.Clone());
                                            //iBet.Utilities.WriteLog.Write("Added agent: " + accAgent.Name + ":" + accAgent.OutStanding);
                                        }
                                    }
                                }
                                this._lastAccountInSubOutStanding.Add((AccountDTO)account.Clone());
                                //iBet.Utilities.WriteLog.Write("Added master: " + account.Name + ":" + account.OutStanding);
                            }
                        }
                        else if (account.AccountType == "Agent")
                        {
                            if (CheckCorrectAccount(account))
                            {
                                List<AccountDTO> accountMemberList = this._ibetSubEngine.GetAccountDataFirstTime(account);
                                if (accountMemberList != null)
                                {
                                    foreach (AccountDTO acc in accountMemberList)
                                    {
                                        if (CheckCorrectAccount(acc))
                                        {
                                            List<BetDTO> oldbets = this._ibetSubEngine.GetBetsDataFirstTime(acc);//lay cac tran da biet cua account
                                            if (oldbets != null)
                                            {
                                                AddAlreadyBetRunningToOldBetList(oldbets, acc);
                                                _lastAccountInSubOutStanding.Add((AccountDTO)acc.Clone());
                                            }
                                        }
                                    }
                                    _lastAccountInSubOutStanding.Add(account);
                                }
                            }
                        }
                        else if (account.AccountType == "Member")
                        {
                            if (CheckCorrectAccount(account))
                            {
                                List<BetDTO> oldbets = this._ibetSubEngine.GetBetsDataFirstTime(account);//lay cac tran da biet cua account
                                if (oldbets != null)
                                {
                                    AddAlreadyBetRunningToOldBetList(oldbets, account);
                                    //if (listOutstandingAccount.Count > 1)//neu nhieu hon 1 account, dat cach 1,5s lay tiep account tiep theo
                                    //    Thread.Sleep(1500);
                                    _lastAccountInSubOutStanding.Add((AccountDTO)account.Clone());
                                }
                            }
                        }
                        else
                        {
                            //ShowWarningDialog("Account type is not defined");
                        }

                        //iBet.Utilities.WriteLog.Write(account.Name + " treo " + account.OutStanding);

                    }
                    initFinished = true;//finish get init data   
                    DetectAccountType();
                    if (_ibetSubEngine._detectedAccount)
                    {
                        //this._lastAccountInSubOutStanding = BaseDTO.DeepClone<System.Collections.Generic.List<AccountDTO>>(listOutstandingAccount);

                        lock (grdBetList)
                        {
                            this.grdBetList.DataSource = this._listDoneBetInSub;
                            this.grdBetList.RefreshDataSource();
                        }
                    }
                    else
                    {
                        ShowWarningDialog("Your following account has no bet(s) at the moment.");
                        //initFinished = true;
                        //this.Stop();
                    }
                }
                else// list cac tran fang truoc da ton tai
                {
                    foreach (AccountDTO account in listOutstandingAccount)//cho chay lan luot
                    {
                        if (CheckCorrectAccount(account)) //|| account.Name.ToUpper().Contains(this.txtListFollowAccounts.Text.ToUpper()))//neu co trong list follow
                        {
                            //iBet.Utilities.WriteLog.Write("004: got here " + account.Name);
                            if (account.AccountType == "Master")
                            {
                                if (CheckCurrentAccountsForChange(_lastAccountInSubOutStanding, account))
                                {
                                    List<AccountDTO> accountAgentList = this._ibetSubEngine.GetAccountDataFirstTime(account);
                                    if (accountAgentList != null)
                                    {
                                        foreach (AccountDTO accAgent in accountAgentList)
                                        {
                                            if (CheckCorrectAccount(accAgent))
                                            {
                                                if (CheckCurrentAccountsForChange(_lastAccountInSubOutStanding, accAgent))
                                                {
                                                    //iBet.Utilities.WriteLog.Write("005: got here " + accAgent.Name );
                                                    List<AccountDTO> accountMemberList = this._ibetSubEngine.GetAccountDataFirstTime(accAgent);
                                                    foreach (AccountDTO acc in accountMemberList)
                                                    {
                                                        if (CheckCorrectAccount(acc))
                                                        {
                                                            if (CheckCurrentAccountsForChange(_lastAccountInSubOutStanding, acc))
                                                            {
                                                                //iBet.Utilities.WriteLog.Write("006: got here " + acc.Name);
                                                                this._ibetSubEngine.GetAccountData(acc);
                                                                //iBet.Utilities.WriteLog.Write("007: after get data details of " + acc.Name);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (account.AccountType == "Agent")
                            {
                                if (CheckCurrentAccountsForChange(_lastAccountInSubOutStanding, account))
                                {
                                    List<AccountDTO> accountMemberList = this._ibetSubEngine.GetAccountDataFirstTime(account);
                                    if (accountMemberList != null)
                                    {
                                        foreach (AccountDTO acc in accountMemberList)
                                        {
                                            if (CheckCorrectAccount(acc))
                                            {
                                                if (CheckCurrentAccountsForChange(_lastAccountInSubOutStanding, acc))
                                                {
                                                    this._ibetSubEngine.GetAccountData(acc);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (account.AccountType == "Member")
                            {
                                if (CheckCorrectAccount(account))
                                {
                                    if (CheckCurrentAccountsForChange(_lastAccountInSubOutStanding, account))
                                    {
                                        this._ibetSubEngine.GetAccountData(account);
                                    }
                                }
                            }
                            else
                            {
                                //ShowWarningDialog("Account type is not defined.");
                                iBet.Utilities.WriteLog.Write("Account type is not defined.");
                            }
                        }
                    }
                    DetectAccountType();
                }
            }
            else//master khong ton tai tran treo nao
            {
                this._OldBetListHistory = new Dictionary<string, DateTime>();//khoi tao list 
                initFinished = true;
            }

            this._comparingAcc = false;
            if (this._compareAgainAcc && !this._comparingAcc && !this.initFinished)
            {
                this._compareAgainAcc = false;
                this.CompareAccountOutStanding();
            }
        }
        private void AddAlreadyBetRunningToOldBetList(List<BetDTO> listBeted, AccountDTO account)
        {
            if (listBeted != null)
            {
                if (_listDoneBetInSub == null)
                    _listDoneBetInSub = new List<BetDTO>();
                foreach (BetDTO bet in listBeted)
                {
                    if (!this._OldBetListHistory.ContainsKey(bet.RefID))
                    {
                        bet.ID = (this._listDoneBetInSub.Count + 1).ToString();
                        this._OldBetListHistory.Add(bet.RefID, DateTime.Now);//add vao list da bet
                        this._listDoneBetInSub.Add(bet);
                    }
                    //this._ibetSubEngine._OldBetListHistory.Add(bet.RefID, DateTime.Now);//add vao engine
                }
            }
            else
            {
                ShowErrorDialog("Cannot parse bet list of " + account.Name);
            }

        }
        private void RefreshIbetList()
        {
            lock (this._listIBETMatchNow)
            {
                if (this._listIBETMatchNow != null)
                    this._listIBETMatchNow.Clear();
                else
                    this._listIBETMatchNow = new System.Collections.Generic.List<MatchDTO>();
                foreach (MatchDTO current in this._listIBETMatch) // current = IBET
                {
                    this._listIBETMatchNow.Add(current);
                }
            }
            lock (this.grdSameMatch)
            {
                this.grdSameMatch.RefreshDataSource();
            }
        }
        private void RefreshIbetListNonLive()
        {
            lock (this._listIBETMatchNonLive)
            {
                if (this._listIBETMatchNonLiveNow != null)
                    this._listIBETMatchNonLiveNow.Clear();
                else
                    this._listIBETMatchNonLive = new System.Collections.Generic.List<MatchDTO>();
                foreach (MatchDTO current in this._listIBETMatchNonLive) // current = IBET
                {
                    this._listIBETMatchNonLiveNow.Add(current);
                }
            }
            lock (this.gridNonLiveMatch)
            {
                this.gridNonLiveMatch.RefreshDataSource();
            }
        }
        private void CompareSameMatch()
        {
            //iBet.Utilities.WriteLog.Write("003: Start to comapre now");
            this._comparing = true;

            System.Collections.Generic.List<MatchDTO> listIBETMatch = this._listIBETMatch;
            System.Collections.Generic.List<MatchDTO> listIBETMatchNonLive = this._listIBETMatchNonLive;
            System.Collections.Generic.List<BetDTO> listSubIbetMatch = this._listBetInSubMatch;

            if (this._BetWaitingList == null)
            {
                this._BetWaitingList = new Dictionary<string, DateTime>();
            }
            if (listSubIbetMatch != null)
            {
                foreach (BetDTO bet in listSubIbetMatch)
                {
                    if (!this._OldBetListHistory.ContainsKey(bet.RefID))
                    {
                        if (!_listDoneBetInSub.Any(beted => beted.RefID == bet.RefID))
                        {
                            bet.ID = (_listDoneBetInSub.Count + 1).ToString();
                            this._listDoneBetInSub.Add(bet);
                        }

                        if (bet.Stake >= (int)txtAllowTradeMinValue.Value)
                        {
                            TransactionDTO trans = new TransactionDTO();

                            MatchDTO match = new MatchDTO();
                            match.HomeTeamName = bet.HomeTeamName;
                            match.AwayTeamName = bet.AwayTeamName;
                            match.League = new LeagueDTO();
                            match.League.Name = bet.League;

                            MatchDTO matchDTO = new MatchDTO();
                            if (bet.Live)
                            {
                                matchDTO = MatchDTO.SearchMatchFull(match, listIBETMatch);
                            }
                            else
                            {
                                matchDTO = MatchDTO.SearchMatchFull(match, listIBETMatchNonLive);
                                if (matchDTO == null)
                                    matchDTO = MatchDTO.SearchMatchFull(match, listIBETMatch);//tim trong list live, vi co the tran day chuyen trang thai
                            }

                            if (matchDTO != null)
                            {
                                matchDTO.HomeScore = bet.HomeScore;
                                matchDTO.AwayScore = bet.AwayScore;
                                //_mainForm.ad

                                #region Fang

                                string stake = string.Empty;
                                if (chkFollowPercent.Checked)
                                {
                                    int stk = (int)((bet.Stake * (float)txtFollowPercent.Value) / 100);
                                    if (stk < 5)
                                        stk = 5;
                                    stake = stk.ToString();
                                }
                                else
                                {
                                    stake = txtIBETFixedStake.Value.ToString();
                                }

                                string ibetOddType = string.Empty;
                                bool follow = chkFollowType.Checked;
                                if (bet.Type == eOddType.HT || bet.Type == eOddType.FT)
                                {
                                    if (bet.Choice.Contains(".1"))
                                        ibetOddType = "1";
                                    else if (bet.Choice.Contains(".2"))
                                        ibetOddType = "2";
                                    else if (bet.Choice.Contains(".x"))
                                        ibetOddType = "X";
                                    if (follow)
                                    {
                                        trans = PlaceSingleIBET(matchDTO, bet.Odd, bet.OddValue, bet.Type, ibetOddType, stake, bet.HomeScore, bet.AwayScore, this._ibetEngine, follow, bet.RefID, bet.Account);
                                        //this._mainForm.AddLocalSingleIBET(match, bet.Odd, bet.OddValue, bet.Type, ibetOddType, bet.Stake.ToString(), bet.HomeScore, bet.AwayScore, follow, bet.RefID, bet.Account, bet.Live);
                                    }
                                }
                                else
                                {
                                    if (bet.Choice.Contains("Over"))
                                    {
                                        if (follow)
                                            ibetOddType = "h";
                                        else
                                            ibetOddType = "a";
                                    }
                                    else if (bet.Choice.Contains("Under"))
                                    {
                                        if (follow)
                                            ibetOddType = "a";
                                        else
                                            ibetOddType = "h";
                                    }
                                    else if (bet.HomeTeamName.Contains(bet.Choice))
                                    {
                                        if (follow)
                                            ibetOddType = "h";
                                        else
                                            ibetOddType = "a";
                                    }
                                    else
                                    {
                                        if (chkFollowType.Checked)
                                            ibetOddType = "a";
                                        else
                                            ibetOddType = "h";
                                    }
                                    trans = PlaceSingleIBET(matchDTO, bet.Odd, bet.OddValue, bet.Type, ibetOddType, stake, bet.HomeScore, bet.AwayScore, this._ibetEngine, follow, bet.RefID, bet.Account);
                                    //this._mainForm.AddLocalSingleIBET(match, bet.Odd, bet.OddValue, bet.Type, ibetOddType, bet.Stake.ToString(), bet.HomeScore, bet.AwayScore, follow, bet.RefID, bet.Account, bet.Live);
                                }
                                #endregion
                                if (trans.IBETTrade)
                                {
                                    trans.SBOBETTrade = true;
                                    this._OldBetListHistory.Add(bet.RefID, DateTime.Now);
                                }
                                else if (trans.Note == "Score changed.")
                                {
                                    trans.IBETTrade = true;
                                    trans.Note += " Cancel this bet.";
                                    this._OldBetListHistory.Add(bet.RefID, DateTime.Now);
                                }
                                else
                                {
                                    trans.Note += " Add this bet to pending list.";
                                }
                                this.AddTransaction(trans);
                                if (listSubIbetMatch.Count > 1)
                                    Thread.Sleep(800);
                            }
                        }
                        else
                        {
                            this._OldBetListHistory.Add(bet.RefID, DateTime.Now);
                        }
                    }
                }
                lock (grdBetList)
                {
                    this.grdBetList.RefreshDataSource();
                }
            }
            else
            {
                //iBet.Utilities.WriteLog.Write("list bet just parsed is null"); 
            }

            BarItem arg_648_0 = this.lblSbobetTotalMatch;
            int count;
            if (_listBetInSubMatch != null)
            {
                count = _listBetInSubMatch.Count;
                arg_648_0.Caption = count.ToString();
                this.lblSameMatch.Caption = "Total Waiting Bet: " + this._BetWaitingList.Count;
            }
            else
                arg_648_0.Caption = "No Bet";
            //BarItem arg_663_0 = this.lblIbetTotalMatch;
            //count = listIBETMatch.Count;
            //arg_663_0.Caption = count.ToString();

            this.lblLastUpdate.Caption = System.DateTime.Now.ToString();

            this._comparing = false;
            if (this._compareAgain && !this._comparing)
            {
                this._compareAgain = false;
                this.CompareSameMatch();
            }
        }
        internal bool ReleaseWaitingBet(TransactionDTO tran)
        {
            bool status = false;
            if (this._running)
            {
                eOddType oddtype;
                string ibetOddType = string.Empty;
                if (tran.OddType.StartsWith("FulltimeOverUnder"))
                    oddtype = eOddType.FulltimeOverUnder;
                else if (tran.OddType.StartsWith("FirstHalfOverUnder"))
                    oddtype = eOddType.FirstHalfOverUnder;
                else if (tran.OddType.StartsWith("FulltimeHandicap"))
                    oddtype = eOddType.FulltimeHandicap;
                else if (tran.OddType.StartsWith("FirstHalfHandicap"))
                    oddtype = eOddType.FirstHalfHandicap;
                else if (tran.OddType.StartsWith("FT"))
                    oddtype = eOddType.FT;
                else if (tran.OddType.StartsWith("HT"))
                    oddtype = eOddType.HT;
                else
                    oddtype = eOddType.Unknown;

                MatchDTO matchDTO = new MatchDTO();
                matchDTO.HomeTeamName = tran.HomeTeamName;
                matchDTO.AwayTeamName = tran.AwayTeamName;
                matchDTO.League = new LeagueDTO();
                matchDTO.League.Name = tran.League;

                if (oddtype != eOddType.HT && oddtype != eOddType.FT)
                {
                    if (chkFollowType.Checked)
                    {
                        if (tran.HomePick)
                            ibetOddType = "h";
                        else
                            ibetOddType = "a";
                    }
                    else
                    {
                        if (tran.HomePick)
                            ibetOddType = "a";
                        else
                            ibetOddType = "h";
                    }
                    //return PlaceSingleIBET(matchDTO, tran.Odd, tran.OddValue, oddtype, ibetOddType, tran.Stake, tran.HomeScore, tran.AwayScore, this._ibetEngine);
                }
                else
                {
                    if (chkFollowType.Checked)
                    {
                        ibetOddType = tran.FT1X2Pick;
                        //return PlaceSingleIBET(matchDTO, tran.Odd, tran.OddValue, oddtype, ibetOddType, tran.Stake, tran.HomeScore, tran.AwayScore, this._ibetEngine);
                    }
                }
            }

            return status;
        }
        private TransactionDTO PlaceSingleIBET(MatchDTO ibetMatch, string odd, string oddValue, eOddType oddType, string ibetOddType, string stake, string homeScore, string awayScore, IBetEngine ibetEngine, bool followtype, string followref, string accountName)
        {
            this._betting = true;
            TransactionDTO transactionDTO = new TransactionDTO();
            string following = string.Empty;
            if (followtype)
                following = "following";
            else
                following = "unfollowing";

            string betKindValue = string.Empty;
            string homeTeamName = string.Empty;
            string awayTeamName = string.Empty;
            string newOddValue = string.Empty;
            string newHomeScore = string.Empty;
            string newAwayScore = string.Empty;

            bool flag = false;
            bool flag2 = false;
            string text = string.Empty;

            System.Collections.Generic.List<MatchDTO> listIbetMatch = this._listIBETMatch;
            //MatchDTO matchDTO = MatchDTO.SearchMatchFull(ibetMatch, listIbetMatch);
            if (ibetMatch != null && ibetMatch.HomeTeamName != string.Empty)
            {
                OddDTO oddDTO = OddDTO.SearchOdd(oddType, odd, true, ibetMatch.Odds);
                if (oddDTO != null)
                {
                    string ibetOddNow = string.Empty;
                    if (ibetOddType == "a")
                        ibetOddNow = oddDTO.Away.ToString();
                    else
                        ibetOddNow = oddDTO.Home.ToString();
                    if (followtype || (!followtype && ((float.Parse(ibetOddNow) >= (float)txtLowestOddValue.Value && float.Parse(ibetOddNow) > 0)) || float.Parse(ibetOddNow) < 0))
                    {
                        int maxBet = 0;
                        int minBet = 0;
                        string ibetOddID = oddDTO.ID;
                        try
                        {
                            ibetEngine.PrepareBet2(ibetOddID, ibetOddType, ibetOddNow.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                            if (homeTeamName == ibetMatch.HomeTeamName)
                            {
                                if (newHomeScore == homeScore && newAwayScore == awayScore)
                                {
                                    float num1 = float.Parse(odd);
                                    float num2 = float.Parse(betKindValue);
                                    if (num1 + num2 == 0f || num1 == num2)
                                    {
                                        if (int.Parse(stake) > maxBet)
                                            stake = maxBet.ToString();

                                        float newIbetOddValue = float.Parse(newOddValue);
                                        float oldIbetOddValue = float.Parse(oddValue);

                                        //if (OddDTO.IsValidOddPair(newIbetOddValue, oldIbetOddValue, (float)txtAllowTradeMinValue.Value, true))
                                        //{
                                        ibetEngine.ConfirmBet(oddType, newOddValue, stake, minBet.ToString(), maxBet.ToString(), out flag2);
                                        if (!flag2)
                                        {
                                            //this._lastTransactionTime = System.DateTime.Now;
                                            //SendReportToMainForm("- Follow Bet: " + this._ibetAccount + " >> " + matchDTO.HomeTeamName + " - " + matchDTO.AwayTeamName + ": Bet confirmed failed. ");
                                            text = "Confirm bet failed";
                                        }
                                        else
                                        {
                                            this._lastTransactionTime = DateTime.Now;
                                            text = " - Success " + following + " " + this._ibetSubAccount + ". Follow: " + ibetOddType + " > " + accountName;
                                        }
                                        //}
                                        //else
                                        //{
                                        //    SendReportToMainForm("Follow Bet: " + this._ibetAccount + " >> " + matchDTO.HomeTeamName + " - " + matchDTO.AwayTeamName + ": New odd value: " + newOddValue + " is lower than allowed number ");
                                        //}
                                    }
                                    else
                                        text = "Invalid Odd while Preparing Ticket. IBET Odd: " + betKindValue;
                                }
                                else
                                {
                                    text = "Score changed.";
                                }
                            }
                            else
                            {
                                text = "Not same match. Comparing: " + ibetMatch.HomeTeamName + " - " + ibetMatch.AwayTeamName;
                            }
                        }
                        catch (Exception ex)
                        {
                            text = ex.Message;
                        }
                    }
                    else
                    {
                        text = "Odd value is smaller than lowest accepted:" + ibetOddNow;
                    }
                }
                else
                {
                    string text2 = string.Empty;
                    //foreach (OddDTO o in ibetMatch.Odds)
                    //{
                    //    text2 += o.Odd + ","; 
                    //}
                    text = "Odd not found.";
                }
            }
            else
                text = "Match not found ";


            transactionDTO.HomeTeamName = homeTeamName;
            transactionDTO.AwayTeamName = awayTeamName;
            transactionDTO.HomeScore = homeScore;
            transactionDTO.AwayScore = awayScore;
            transactionDTO.Score = homeScore + "-" + awayScore;
            transactionDTO.AccountPair = this._ibetAccount + " - " + this._ibetSubAccount;
            transactionDTO.OddType = oddType.ToString();
            transactionDTO.Odd = betKindValue;
            transactionDTO.OddKindValue = betKindValue;
            transactionDTO.OddValue = newOddValue;
            transactionDTO.Stake = stake;
            transactionDTO.Note = text;
            transactionDTO.IBETTrade = flag2;
            transactionDTO.IBETAllow = flag;
            transactionDTO.IsFollowTypeTrans = true;
            transactionDTO.DateTime = DateTime.Now;
            transactionDTO.FollowRef = followref;

            this._betting = false;
            return transactionDTO;
        }
        private bool AllowOddBet(string oddID)
        {
            TimeSpan timespan = System.DateTime.Now - this._lastTransactionTime;
            return (timespan.Seconds > (int)this.txtTransactionTimeSpan.Value);
        }
        private bool AllowTrans()
        {
            return true;
            //TimeSpan timespan = System.DateTime.Now - this._lastTrans;
            //return (timespan.Seconds >= 1);
        }
        private void UpdateOddBetHistory(string oddID)
        {
            //this._oddTransactionHistory.Remove(oddID);
            //this._oddTransactionHistory.Add(oddID, System.DateTime.Now);
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
        private void btnSBOBETGO_Click(object sender, System.EventArgs e)
        {
            this.webSBOBET.Navigate(this.txtSBOBETAddress.Text);
        }
        private void btnIBETGO_Click(object sender, System.EventArgs e)
        {
            this.webIBET.Navigate(this.txtIBETAddress.Text);
        }
        private void btnSbobetGetInfo_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (txtListFollowAccounts.Text != "")
                    this.InitializeIBETSubEngine();
                else
                    this.ShowErrorDialog("Please enter your following account(s)");
            }
            catch (System.Exception ex)
            {
                this.ShowErrorDialog("Error while initialize Sub IBET Engine. \nDetails: " + ex.Message);
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
        private void btnStart_ItemClick(object sender, ItemClickEventArgs e)
        {
            this._dataService.AllowRunAsync(this._currentUserID, this._ibetAccount.ToUpper(), this._ibetSubAccount.ToUpper());
        }
        private void btnStop_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Stop();
            this._dataService.StopTerminalAsync(this._currentUserID, this.Text);
        }
        private void btnClear_ItemClick(object sender, ItemClickEventArgs e)
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
        private void btnSetUpdateInterval_Click(object sender, System.EventArgs e)
        {
            SetInterval();
        }
        internal void SetInterval()
        {
            if (this._ibetEngine != null)
            {
                this._ibetEngine.Stop();
                this._ibetEngine.UpdateDataInterval = (int)this.txtIBETUpdateInterval.Value * 1000;
                this._ibetEngine.Start();
            }
            if (this._ibetSubEngine != null)
            {
                this._ibetSubEngine.Stop();
                this._ibetSubEngine.UpdateDataInterval = (int)this.txtSBOBETUpdateInterval.Value * 1000;
                this._ibetSubEngine.Start();
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
        private void chkListAllowedMatch_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            bool optionSet = e.State == CheckState.Checked ? true : false;
        }
        private void TerminalFormIBETSBO_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close?", "Bet Broker", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        private void btnStatus_Click_1(object sender, EventArgs e)
        {
            ShowStatus();
            //ConvertData("");
        }
        private void ShowStatus()
        {
            if (this._ibetEngine != null && this._ibetSubEngine != null)
            {
                string ibetStatement = string.Empty;
                string sbobetStatement = string.Empty;
                GetStatement(out ibetStatement, out sbobetStatement);
                BarItem bar = this.lblIBETWinLost;
                bar.Caption = this._ibetEngine._winlost;
                bar = this.lblIBETCom;
                bar.Caption = this._ibetEngine._commision;
                bar = this.lblIBETReject;
                bar.Caption = this._ibetEngine._reject.ToString();
            }
            else
            {
                ShowWarningDialog("Please get info before you can view status");
            }
        }
        private void InitializeComponent()
        {
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.btnSbobetGetInfo = new DevExpress.XtraBars.BarButtonItem();
            this.lblSbobetCurrentCredit = new DevExpress.XtraBars.BarStaticItem();
            this.lblSbobetTotalMatch = new DevExpress.XtraBars.BarStaticItem();
            this.lblSbobetLastUpdate = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem7 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem8 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem9 = new DevExpress.XtraBars.BarStaticItem();
            this.lblIbetCurrentCredit = new DevExpress.XtraBars.BarStaticItem();
            this.lblIbetTotalMatch = new DevExpress.XtraBars.BarStaticItem();
            this.lblIbetLastUpdate = new DevExpress.XtraBars.BarStaticItem();
            this.btnIbetGetInfo = new DevExpress.XtraBars.BarButtonItem();
            this.lblStatus = new DevExpress.XtraBars.BarStaticItem();
            this.lblSameMatch = new DevExpress.XtraBars.BarStaticItem();
            this.lblLastUpdate = new DevExpress.XtraBars.BarStaticItem();
            this.btnStart = new DevExpress.XtraBars.BarButtonItem();
            this.btnStop = new DevExpress.XtraBars.BarButtonItem();
            this.btnClear = new DevExpress.XtraBars.BarButtonItem();
            this.lblIBETWinLost = new DevExpress.XtraBars.BarStaticItem();
            this.lblIBETCom = new DevExpress.XtraBars.BarStaticItem();
            this.lblIBETReject = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem4 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem5 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem6 = new DevExpress.XtraBars.BarStaticItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rpgIbet = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgSbobet = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.webIBET = new System.Windows.Forms.WebBrowser();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.cbeSignatureTemplate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnIBETGO = new DevExpress.XtraEditors.SimpleButton();
            this.txtIBETAddress = new DevExpress.XtraEditors.TextEdit();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.webSBOBET = new System.Windows.Forms.WebBrowser();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.chooseSBOServer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnSBOBETGO = new DevExpress.XtraEditors.SimpleButton();
            this.txtSBOBETAddress = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.grdSameMatch = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn22 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn19 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabPage6 = new DevExpress.XtraTab.XtraTabPage();
            this.gridNonLiveMatch = new DevExpress.XtraGrid.GridControl();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn32 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn35 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabBetList = new DevExpress.XtraTab.XtraTabPage();
            this.grdBetList = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn23 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn24 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn25 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn27 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn29 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn28 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn26 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn30 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn31 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.txtTransactionTimeSpan = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.txtMaxTimePerHalf = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.chbAllowHalftime = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.chkProxy = new DevExpress.XtraEditors.CheckEdit();
            this.btnStatus = new DevExpress.XtraEditors.SimpleButton();
            this.btnSetUpdateInterval = new DevExpress.XtraEditors.SimpleButton();
            this.txtSBOBETUpdateInterval = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtIBETUpdateInterval = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
            this.chkEnable = new DevExpress.XtraEditors.CheckEdit();
            this.chkFollowType = new DevExpress.XtraEditors.CheckEdit();
            this.txtLowestOddValue = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.checkEdit6 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit5 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit7 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit12 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit4 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit3 = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.chkFollowPercent = new DevExpress.XtraEditors.CheckEdit();
            this.txtIBETFixedStake = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtAllowTradeMinValue = new DevExpress.XtraEditors.SpinEdit();
            this.txtFollowPercent = new DevExpress.XtraEditors.SpinEdit();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.txtListFollowAccounts = new DevExpress.XtraEditors.MemoEdit();
            this.xtraTabPage5 = new DevExpress.XtraTab.XtraTabPage();
            this.grdTransaction = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn39 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbeSignatureTemplate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETAddress.Properties)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chooseSBOServer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBOBETAddress.Properties)).BeginInit();
            this.xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSameMatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.xtraTabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridNonLiveMatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            this.tabBetList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBetList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).BeginInit();
            this.xtraTabControl2.SuspendLayout();
            this.xtraTabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransactionTimeSpan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxTimePerHalf.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAllowHalftime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkProxy.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBOBETUpdateInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETUpdateInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
            this.groupControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFollowType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLowestOddValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit6.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit5.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit7.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit12.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkFollowPercent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETFixedStake.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAllowTradeMinValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowPercent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtListFollowAccounts.Properties)).BeginInit();
            this.xtraTabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTransaction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ApplicationButtonText = null;
            // 
            // 
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.ExpandCollapseItem.Name = "";
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barStaticItem1,
            this.barStaticItem2,
            this.barStaticItem3,
            this.btnSbobetGetInfo,
            this.lblSbobetCurrentCredit,
            this.lblSbobetTotalMatch,
            this.lblSbobetLastUpdate,
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
            this.btnClear,
            this.lblIBETWinLost,
            this.lblIBETCom,
            this.lblIBETReject,
            this.barStaticItem4,
            this.barStaticItem5,
            this.barStaticItem6});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 37;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010;
            this.ribbonControl1.SelectedPage = this.ribbonPage1;
            this.ribbonControl1.ShowCategoryInCaption = false;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(1130, 125);
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "Current Credit:";
            this.barStaticItem1.Id = 1;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "Total Match:";
            this.barStaticItem2.Id = 2;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = "Last Update:";
            this.barStaticItem3.Id = 3;
            this.barStaticItem3.Name = "barStaticItem3";
            this.barStaticItem3.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // btnSbobetGetInfo
            // 
            this.btnSbobetGetInfo.Caption = "Get Info";
            this.btnSbobetGetInfo.Id = 4;
            this.btnSbobetGetInfo.LargeGlyph = global::iBet.App.Properties.Resources.i8;
            this.btnSbobetGetInfo.Name = "btnSbobetGetInfo";
            this.btnSbobetGetInfo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSbobetGetInfo_ItemClick);
            // 
            // lblSbobetCurrentCredit
            // 
            this.lblSbobetCurrentCredit.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSbobetCurrentCredit.Appearance.Options.UseFont = true;
            this.lblSbobetCurrentCredit.Caption = "-";
            this.lblSbobetCurrentCredit.Id = 5;
            this.lblSbobetCurrentCredit.Name = "lblSbobetCurrentCredit";
            this.lblSbobetCurrentCredit.TextAlignment = System.Drawing.StringAlignment.Far;
            this.lblSbobetCurrentCredit.Width = 135;
            // 
            // lblSbobetTotalMatch
            // 
            this.lblSbobetTotalMatch.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSbobetTotalMatch.Appearance.Options.UseFont = true;
            this.lblSbobetTotalMatch.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
            this.lblSbobetTotalMatch.Caption = "-";
            this.lblSbobetTotalMatch.Id = 6;
            this.lblSbobetTotalMatch.Name = "lblSbobetTotalMatch";
            this.lblSbobetTotalMatch.TextAlignment = System.Drawing.StringAlignment.Far;
            this.lblSbobetTotalMatch.Width = 135;
            // 
            // lblSbobetLastUpdate
            // 
            this.lblSbobetLastUpdate.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSbobetLastUpdate.Appearance.Options.UseFont = true;
            this.lblSbobetLastUpdate.Caption = "-";
            this.lblSbobetLastUpdate.Id = 8;
            this.lblSbobetLastUpdate.Name = "lblSbobetLastUpdate";
            this.lblSbobetLastUpdate.TextAlignment = System.Drawing.StringAlignment.Far;
            this.lblSbobetLastUpdate.Width = 135;
            // 
            // barStaticItem7
            // 
            this.barStaticItem7.Caption = "Current Credit:";
            this.barStaticItem7.Id = 9;
            this.barStaticItem7.Name = "barStaticItem7";
            this.barStaticItem7.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem8
            // 
            this.barStaticItem8.Caption = "Total Already Running:";
            this.barStaticItem8.Id = 10;
            this.barStaticItem8.Name = "barStaticItem8";
            this.barStaticItem8.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem9
            // 
            this.barStaticItem9.Caption = "Last Update:";
            this.barStaticItem9.Id = 11;
            this.barStaticItem9.Name = "barStaticItem9";
            this.barStaticItem9.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // lblIbetCurrentCredit
            // 
            this.lblIbetCurrentCredit.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIbetCurrentCredit.Appearance.Options.UseFont = true;
            this.lblIbetCurrentCredit.Caption = "-";
            this.lblIbetCurrentCredit.Id = 12;
            this.lblIbetCurrentCredit.Name = "lblIbetCurrentCredit";
            this.lblIbetCurrentCredit.TextAlignment = System.Drawing.StringAlignment.Far;
            this.lblIbetCurrentCredit.Width = 135;
            // 
            // lblIbetTotalMatch
            // 
            this.lblIbetTotalMatch.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIbetTotalMatch.Appearance.Options.UseFont = true;
            this.lblIbetTotalMatch.Caption = "-";
            this.lblIbetTotalMatch.Id = 13;
            this.lblIbetTotalMatch.Name = "lblIbetTotalMatch";
            this.lblIbetTotalMatch.TextAlignment = System.Drawing.StringAlignment.Far;
            this.lblIbetTotalMatch.Width = 135;
            // 
            // lblIbetLastUpdate
            // 
            this.lblIbetLastUpdate.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIbetLastUpdate.Appearance.Options.UseFont = true;
            this.lblIbetLastUpdate.Caption = "-";
            this.lblIbetLastUpdate.Id = 14;
            this.lblIbetLastUpdate.Name = "lblIbetLastUpdate";
            this.lblIbetLastUpdate.TextAlignment = System.Drawing.StringAlignment.Far;
            this.lblIbetLastUpdate.Width = 135;
            // 
            // btnIbetGetInfo
            // 
            this.btnIbetGetInfo.Caption = "Get Info";
            this.btnIbetGetInfo.Id = 15;
            this.btnIbetGetInfo.LargeGlyph = global::iBet.App.Properties.Resources.i8;
            this.btnIbetGetInfo.Name = "btnIbetGetInfo";
            this.btnIbetGetInfo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // lblStatus
            // 
            this.lblStatus.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Appearance.Options.UseFont = true;
            this.lblStatus.Caption = "STOPPED";
            this.lblStatus.Id = 16;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.TextAlignment = System.Drawing.StringAlignment.Center;
            this.lblStatus.Width = 135;
            // 
            // lblSameMatch
            // 
            this.lblSameMatch.Caption = "Total waiting list: -";
            this.lblSameMatch.Id = 17;
            this.lblSameMatch.Name = "lblSameMatch";
            this.lblSameMatch.TextAlignment = System.Drawing.StringAlignment.Center;
            this.lblSameMatch.Width = 135;
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.Caption = "-";
            this.lblLastUpdate.Id = 18;
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.TextAlignment = System.Drawing.StringAlignment.Center;
            this.lblLastUpdate.Width = 135;
            // 
            // btnStart
            // 
            this.btnStart.Caption = "Start";
            this.btnStart.Enabled = false;
            this.btnStart.Id = 19;
            this.btnStart.LargeGlyph = global::iBet.App.Properties.Resources.i5;
            this.btnStart.Name = "btnStart";
            this.btnStart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStart_ItemClick);
            // 
            // btnStop
            // 
            this.btnStop.Caption = "Stop";
            this.btnStop.Enabled = false;
            this.btnStop.Id = 20;
            this.btnStop.LargeGlyph = global::iBet.App.Properties.Resources.i6;
            this.btnStop.Name = "btnStop";
            this.btnStop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStop_ItemClick);
            // 
            // btnClear
            // 
            this.btnClear.Caption = "Clear";
            this.btnClear.Enabled = false;
            this.btnClear.Id = 21;
            this.btnClear.LargeGlyph = global::iBet.App.Properties.Resources.i7;
            this.btnClear.Name = "btnClear";
            this.btnClear.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClear_ItemClick);
            // 
            // lblIBETWinLost
            // 
            this.lblIBETWinLost.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblIBETWinLost.Appearance.Options.UseFont = true;
            this.lblIBETWinLost.Caption = "-";
            this.lblIBETWinLost.Id = 25;
            this.lblIBETWinLost.Name = "lblIBETWinLost";
            this.lblIBETWinLost.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // lblIBETCom
            // 
            this.lblIBETCom.Caption = "-";
            this.lblIBETCom.Id = 26;
            this.lblIBETCom.Name = "lblIBETCom";
            this.lblIBETCom.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // lblIBETReject
            // 
            this.lblIBETReject.Caption = "-";
            this.lblIBETReject.Id = 27;
            this.lblIBETReject.Name = "lblIBETReject";
            this.lblIBETReject.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem4
            // 
            this.barStaticItem4.Caption = "Win:";
            this.barStaticItem4.Id = 31;
            this.barStaticItem4.Name = "barStaticItem4";
            this.barStaticItem4.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem5
            // 
            this.barStaticItem5.Caption = "Com:";
            this.barStaticItem5.Id = 32;
            this.barStaticItem5.Name = "barStaticItem5";
            this.barStaticItem5.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem6
            // 
            this.barStaticItem6.Caption = "Reject:";
            this.barStaticItem6.Id = 33;
            this.barStaticItem6.Name = "barStaticItem6";
            this.barStaticItem6.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rpgIbet,
            this.rpgSbobet,
            this.ribbonPageGroup3,
            this.ribbonPageGroup4});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            // 
            // rpgIbet
            // 
            this.rpgIbet.ItemLinks.Add(this.barStaticItem7);
            this.rpgIbet.ItemLinks.Add(this.barStaticItem8);
            this.rpgIbet.ItemLinks.Add(this.barStaticItem9);
            this.rpgIbet.ItemLinks.Add(this.lblIbetCurrentCredit);
            this.rpgIbet.ItemLinks.Add(this.lblIbetTotalMatch);
            this.rpgIbet.ItemLinks.Add(this.lblIbetLastUpdate);
            this.rpgIbet.ItemLinks.Add(this.btnIbetGetInfo);
            this.rpgIbet.ItemLinks.Add(this.barStaticItem4);
            this.rpgIbet.ItemLinks.Add(this.barStaticItem5);
            this.rpgIbet.ItemLinks.Add(this.barStaticItem6);
            this.rpgIbet.ItemLinks.Add(this.lblIBETWinLost);
            this.rpgIbet.ItemLinks.Add(this.lblIBETCom);
            this.rpgIbet.ItemLinks.Add(this.lblIBETReject);
            this.rpgIbet.Name = "rpgIbet";
            this.rpgIbet.ShowCaptionButton = false;
            this.rpgIbet.Text = "IBET";
            // 
            // rpgSbobet
            // 
            this.rpgSbobet.ItemLinks.Add(this.barStaticItem1);
            this.rpgSbobet.ItemLinks.Add(this.barStaticItem2);
            this.rpgSbobet.ItemLinks.Add(this.barStaticItem3);
            this.rpgSbobet.ItemLinks.Add(this.lblSbobetCurrentCredit);
            this.rpgSbobet.ItemLinks.Add(this.lblSbobetTotalMatch);
            this.rpgSbobet.ItemLinks.Add(this.lblSbobetLastUpdate);
            this.rpgSbobet.ItemLinks.Add(this.btnSbobetGetInfo);
            this.rpgSbobet.Name = "rpgSbobet";
            this.rpgSbobet.ShowCaptionButton = false;
            this.rpgSbobet.Text = "SUB IBET";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.lblStatus);
            this.ribbonPageGroup3.ItemLinks.Add(this.lblSameMatch);
            this.ribbonPageGroup3.ItemLinks.Add(this.lblLastUpdate);
            this.ribbonPageGroup3.ItemLinks.Add(this.btnStart);
            this.ribbonPageGroup3.ItemLinks.Add(this.btnStop);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.ShowCaptionButton = false;
            this.ribbonPageGroup3.Text = "Status";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.btnClear);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.ShowCaptionButton = false;
            this.ribbonPageGroup4.Text = "Transaction";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 125);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.xtraTabControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.xtraTabControl2);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1130, 617);
            this.splitContainerControl1.SplitterPosition = 179;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1130, 433);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3,
            this.xtraTabPage6,
            this.tabBetList});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.panelControl5);
            this.xtraTabPage1.Controls.Add(this.panelControl4);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1124, 407);
            this.xtraTabPage1.Text = "IBET";
            // 
            // panelControl5
            // 
            this.panelControl5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl5.Controls.Add(this.webIBET);
            this.panelControl5.Location = new System.Drawing.Point(3, 38);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(1118, 366);
            this.panelControl5.TabIndex = 3;
            // 
            // webIBET
            // 
            this.webIBET.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webIBET.Location = new System.Drawing.Point(2, 2);
            this.webIBET.MinimumSize = new System.Drawing.Size(20, 20);
            this.webIBET.Name = "webIBET";
            this.webIBET.Size = new System.Drawing.Size(1114, 362);
            this.webIBET.TabIndex = 0;
            this.webIBET.Url = new System.Uri("http://www.653366.com", System.UriKind.Absolute);
            this.webIBET.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webIBET_DocumentCompleted);
            // 
            // panelControl4
            // 
            this.panelControl4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl4.Controls.Add(this.cbeSignatureTemplate);
            this.panelControl4.Controls.Add(this.label1);
            this.panelControl4.Controls.Add(this.btnIBETGO);
            this.panelControl4.Controls.Add(this.txtIBETAddress);
            this.panelControl4.Location = new System.Drawing.Point(3, 3);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(1118, 29);
            this.panelControl4.TabIndex = 2;
            // 
            // cbeSignatureTemplate
            // 
            this.cbeSignatureTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbeSignatureTemplate.EditValue = "Choose Server";
            this.cbeSignatureTemplate.Location = new System.Drawing.Point(871, 5);
            this.cbeSignatureTemplate.Name = "cbeSignatureTemplate";
            this.cbeSignatureTemplate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.cbeSignatureTemplate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeSignatureTemplate.Properties.DropDownRows = 10;
            this.cbeSignatureTemplate.Properties.Items.AddRange(new object[] {
            "http://www.653366.com",
            "http://www.ibet888.net"});
            this.cbeSignatureTemplate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbeSignatureTemplate.Size = new System.Drawing.Size(162, 20);
            this.cbeSignatureTemplate.TabIndex = 10;
            this.cbeSignatureTemplate.SelectedIndexChanged += new System.EventHandler(this.cbeSignatureTemplate_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Address:";
            // 
            // btnIBETGO
            // 
            this.btnIBETGO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIBETGO.Location = new System.Drawing.Point(1039, 3);
            this.btnIBETGO.Name = "btnIBETGO";
            this.btnIBETGO.Size = new System.Drawing.Size(75, 23);
            this.btnIBETGO.TabIndex = 8;
            this.btnIBETGO.Text = "GO";
            this.btnIBETGO.Click += new System.EventHandler(this.btnIBETGO_Click);
            // 
            // txtIBETAddress
            // 
            this.txtIBETAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIBETAddress.EditValue = "http://www.653366.com";
            this.txtIBETAddress.Location = new System.Drawing.Point(54, 5);
            this.txtIBETAddress.Name = "txtIBETAddress";
            this.txtIBETAddress.Size = new System.Drawing.Size(811, 20);
            this.txtIBETAddress.TabIndex = 7;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.panelControl3);
            this.xtraTabPage2.Controls.Add(this.panelControl2);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1124, 407);
            this.xtraTabPage2.Text = "SUB";
            // 
            // panelControl3
            // 
            this.panelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl3.Controls.Add(this.webSBOBET);
            this.panelControl3.Location = new System.Drawing.Point(3, 38);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1117, 410);
            this.panelControl3.TabIndex = 6;
            // 
            // webSBOBET
            // 
            this.webSBOBET.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webSBOBET.Location = new System.Drawing.Point(2, 2);
            this.webSBOBET.MinimumSize = new System.Drawing.Size(20, 20);
            this.webSBOBET.Name = "webSBOBET";
            this.webSBOBET.Size = new System.Drawing.Size(1113, 406);
            this.webSBOBET.TabIndex = 0;
            this.webSBOBET.Url = new System.Uri("http://www.b88ag.com/", System.UriKind.Absolute);
            this.webSBOBET.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webSBOBET_DocumentCompleted);
            // 
            // panelControl2
            // 
            this.panelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl2.Controls.Add(this.chooseSBOServer);
            this.panelControl2.Controls.Add(this.btnSBOBETGO);
            this.panelControl2.Controls.Add(this.txtSBOBETAddress);
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Location = new System.Drawing.Point(3, 3);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1118, 29);
            this.panelControl2.TabIndex = 5;
            // 
            // chooseSBOServer
            // 
            this.chooseSBOServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chooseSBOServer.EditValue = "Choose Server";
            this.chooseSBOServer.Location = new System.Drawing.Point(871, 5);
            this.chooseSBOServer.Name = "chooseSBOServer";
            this.chooseSBOServer.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.chooseSBOServer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.chooseSBOServer.Properties.DropDownRows = 10;
            this.chooseSBOServer.Properties.Items.AddRange(new object[] {
            "http://www.b88ag.com/"});
            this.chooseSBOServer.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.chooseSBOServer.Size = new System.Drawing.Size(162, 20);
            this.chooseSBOServer.TabIndex = 11;
            this.chooseSBOServer.SelectedIndexChanged += new System.EventHandler(this.chooseSBOServer_SelectedIndexChanged);
            // 
            // btnSBOBETGO
            // 
            this.btnSBOBETGO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSBOBETGO.Location = new System.Drawing.Point(1039, 3);
            this.btnSBOBETGO.Name = "btnSBOBETGO";
            this.btnSBOBETGO.Size = new System.Drawing.Size(75, 23);
            this.btnSBOBETGO.TabIndex = 5;
            this.btnSBOBETGO.Text = "GO";
            this.btnSBOBETGO.Click += new System.EventHandler(this.btnSBOBETGO_Click);
            // 
            // txtSBOBETAddress
            // 
            this.txtSBOBETAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSBOBETAddress.EditValue = "http://www.b88ag.com/";
            this.txtSBOBETAddress.Location = new System.Drawing.Point(54, 5);
            this.txtSBOBETAddress.Name = "txtSBOBETAddress";
            this.txtSBOBETAddress.Size = new System.Drawing.Size(808, 20);
            this.txtSBOBETAddress.TabIndex = 4;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(5, 8);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(43, 13);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Address:";
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.grdSameMatch);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(1124, 407);
            this.xtraTabPage3.Text = "Live Match";
            // 
            // grdSameMatch
            // 
            this.grdSameMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSameMatch.Location = new System.Drawing.Point(0, 0);
            this.grdSameMatch.MainView = this.gridView1;
            this.grdSameMatch.Name = "grdSameMatch";
            this.grdSameMatch.Size = new System.Drawing.Size(1124, 407);
            this.grdSameMatch.TabIndex = 3;
            this.grdSameMatch.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn15,
            this.gridColumn16,
            this.gridColumn21,
            this.gridColumn22,
            this.gridColumn14,
            this.gridColumn17,
            this.gridColumn18,
            this.gridColumn19});
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
            this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn18, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "Home Team";
            this.gridColumn15.FieldName = "HomeTeamName";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 0;
            this.gridColumn15.Width = 367;
            // 
            // gridColumn16
            // 
            this.gridColumn16.Caption = "Away Team";
            this.gridColumn16.FieldName = "AwayTeamName";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 3;
            this.gridColumn16.Width = 368;
            // 
            // gridColumn21
            // 
            this.gridColumn21.Caption = "HomeScore";
            this.gridColumn21.FieldName = "HomeScore";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 1;
            // 
            // gridColumn22
            // 
            this.gridColumn22.Caption = "AwayScore";
            this.gridColumn22.FieldName = "AwayScore";
            this.gridColumn22.Name = "gridColumn22";
            this.gridColumn22.Visible = true;
            this.gridColumn22.VisibleIndex = 2;
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "Odd Count";
            this.gridColumn14.FieldName = "OddCount";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.OptionsColumn.FixedWidth = true;
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 4;
            this.gridColumn14.Width = 70;
            // 
            // gridColumn17
            // 
            this.gridColumn17.Caption = "Half";
            this.gridColumn17.FieldName = "Half";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.OptionsColumn.FixedWidth = true;
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 5;
            this.gridColumn17.Width = 60;
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "Minute";
            this.gridColumn18.FieldName = "Minute";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.OptionsColumn.FixedWidth = true;
            this.gridColumn18.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 6;
            this.gridColumn18.Width = 60;
            // 
            // gridColumn19
            // 
            this.gridColumn19.Caption = "Half Time";
            this.gridColumn19.FieldName = "IsHalfTime";
            this.gridColumn19.Name = "gridColumn19";
            this.gridColumn19.OptionsColumn.FixedWidth = true;
            this.gridColumn19.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn19.Visible = true;
            this.gridColumn19.VisibleIndex = 7;
            this.gridColumn19.Width = 60;
            // 
            // xtraTabPage6
            // 
            this.xtraTabPage6.Controls.Add(this.gridNonLiveMatch);
            this.xtraTabPage6.Name = "xtraTabPage6";
            this.xtraTabPage6.Size = new System.Drawing.Size(1124, 407);
            this.xtraTabPage6.Text = "Non-Live Match";
            // 
            // gridNonLiveMatch
            // 
            this.gridNonLiveMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridNonLiveMatch.Location = new System.Drawing.Point(0, 0);
            this.gridNonLiveMatch.MainView = this.gridView4;
            this.gridNonLiveMatch.Name = "gridNonLiveMatch";
            this.gridNonLiveMatch.Size = new System.Drawing.Size(1124, 407);
            this.gridNonLiveMatch.TabIndex = 4;
            this.gridNonLiveMatch.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView4});
            // 
            // gridView4
            // 
            this.gridView4.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn20,
            this.gridColumn32,
            this.gridColumn35});
            this.gridView4.GridControl = this.gridNonLiveMatch;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsBehavior.Editable = false;
            this.gridView4.OptionsCustomization.AllowGroup = false;
            this.gridView4.OptionsDetail.AllowZoomDetail = false;
            this.gridView4.OptionsDetail.EnableMasterViewMode = false;
            this.gridView4.OptionsDetail.ShowDetailTabs = false;
            this.gridView4.OptionsDetail.SmartDetailExpand = false;
            this.gridView4.OptionsView.ShowAutoFilterRow = true;
            this.gridView4.OptionsView.ShowGroupPanel = false;
            this.gridView4.OptionsView.ShowPreview = true;
            this.gridView4.PreviewFieldName = "LeagueName";
            // 
            // gridColumn20
            // 
            this.gridColumn20.Caption = "Home Team";
            this.gridColumn20.FieldName = "HomeTeamName";
            this.gridColumn20.Name = "gridColumn20";
            this.gridColumn20.Visible = true;
            this.gridColumn20.VisibleIndex = 0;
            this.gridColumn20.Width = 367;
            // 
            // gridColumn32
            // 
            this.gridColumn32.Caption = "Away Team";
            this.gridColumn32.FieldName = "AwayTeamName";
            this.gridColumn32.Name = "gridColumn32";
            this.gridColumn32.Visible = true;
            this.gridColumn32.VisibleIndex = 1;
            this.gridColumn32.Width = 368;
            // 
            // gridColumn35
            // 
            this.gridColumn35.Caption = "Odd Count";
            this.gridColumn35.FieldName = "OddCount";
            this.gridColumn35.Name = "gridColumn35";
            this.gridColumn35.OptionsColumn.FixedWidth = true;
            this.gridColumn35.Visible = true;
            this.gridColumn35.VisibleIndex = 2;
            this.gridColumn35.Width = 70;
            // 
            // tabBetList
            // 
            this.tabBetList.Controls.Add(this.grdBetList);
            this.tabBetList.Name = "tabBetList";
            this.tabBetList.Size = new System.Drawing.Size(1124, 407);
            this.tabBetList.Text = "SUB Bet List";
            // 
            // grdBetList
            // 
            this.grdBetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdBetList.Location = new System.Drawing.Point(0, 0);
            this.grdBetList.MainView = this.gridView3;
            this.grdBetList.Name = "grdBetList";
            this.grdBetList.Size = new System.Drawing.Size(1124, 407);
            this.grdBetList.TabIndex = 4;
            this.grdBetList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn9,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn23,
            this.gridColumn24,
            this.gridColumn25,
            this.gridColumn27,
            this.gridColumn29,
            this.gridColumn28,
            this.gridColumn26,
            this.gridColumn30,
            this.gridColumn31});
            this.gridView3.GridControl = this.grdBetList;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsBehavior.Editable = false;
            this.gridView3.OptionsCustomization.AllowGroup = false;
            this.gridView3.OptionsDetail.AllowZoomDetail = false;
            this.gridView3.OptionsDetail.EnableMasterViewMode = false;
            this.gridView3.OptionsDetail.ShowDetailTabs = false;
            this.gridView3.OptionsDetail.SmartDetailExpand = false;
            this.gridView3.OptionsView.ShowAutoFilterRow = true;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            this.gridView3.OptionsView.ShowPreview = true;
            this.gridView3.PreviewFieldName = "League";
            this.gridView3.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn25, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Home Team";
            this.gridColumn9.FieldName = "HomeTeamName";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 3;
            this.gridColumn9.Width = 198;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Away Team";
            this.gridColumn11.FieldName = "AwayTeamName";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 5;
            this.gridColumn11.Width = 214;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "Score";
            this.gridColumn12.FieldName = "Score";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 4;
            this.gridColumn12.Width = 40;
            // 
            // gridColumn23
            // 
            this.gridColumn23.Caption = "Odd";
            this.gridColumn23.FieldName = "Odd";
            this.gridColumn23.Name = "gridColumn23";
            this.gridColumn23.OptionsColumn.FixedWidth = true;
            this.gridColumn23.Visible = true;
            this.gridColumn23.VisibleIndex = 8;
            this.gridColumn23.Width = 40;
            // 
            // gridColumn24
            // 
            this.gridColumn24.Caption = "Odd Value";
            this.gridColumn24.FieldName = "OddValue";
            this.gridColumn24.Name = "gridColumn24";
            this.gridColumn24.OptionsColumn.FixedWidth = true;
            this.gridColumn24.Visible = true;
            this.gridColumn24.VisibleIndex = 9;
            this.gridColumn24.Width = 60;
            // 
            // gridColumn25
            // 
            this.gridColumn25.Caption = "Time";
            this.gridColumn25.DisplayFormat.FormatString = "hh:mm:ss";
            this.gridColumn25.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn25.FieldName = "DateTime";
            this.gridColumn25.Name = "gridColumn25";
            this.gridColumn25.OptionsColumn.FixedWidth = true;
            this.gridColumn25.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.gridColumn25.Visible = true;
            this.gridColumn25.VisibleIndex = 11;
            this.gridColumn25.Width = 80;
            // 
            // gridColumn27
            // 
            this.gridColumn27.Caption = "Ref ID";
            this.gridColumn27.FieldName = "RefID";
            this.gridColumn27.Name = "gridColumn27";
            this.gridColumn27.Visible = true;
            this.gridColumn27.VisibleIndex = 1;
            this.gridColumn27.Width = 68;
            // 
            // gridColumn29
            // 
            this.gridColumn29.Caption = "Choose";
            this.gridColumn29.FieldName = "Choice";
            this.gridColumn29.Name = "gridColumn29";
            this.gridColumn29.Visible = true;
            this.gridColumn29.VisibleIndex = 6;
            this.gridColumn29.Width = 100;
            // 
            // gridColumn28
            // 
            this.gridColumn28.Caption = "Odd Type";
            this.gridColumn28.FieldName = "Type";
            this.gridColumn28.Name = "gridColumn28";
            this.gridColumn28.Visible = true;
            this.gridColumn28.VisibleIndex = 7;
            this.gridColumn28.Width = 103;
            // 
            // gridColumn26
            // 
            this.gridColumn26.Caption = "Stake";
            this.gridColumn26.FieldName = "Stake";
            this.gridColumn26.Name = "gridColumn26";
            this.gridColumn26.Visible = true;
            this.gridColumn26.VisibleIndex = 10;
            this.gridColumn26.Width = 42;
            // 
            // gridColumn30
            // 
            this.gridColumn30.Caption = "Account";
            this.gridColumn30.FieldName = "Account";
            this.gridColumn30.Name = "gridColumn30";
            this.gridColumn30.Visible = true;
            this.gridColumn30.VisibleIndex = 2;
            this.gridColumn30.Width = 46;
            // 
            // gridColumn31
            // 
            this.gridColumn31.Caption = "ID";
            this.gridColumn31.FieldName = "ID";
            this.gridColumn31.Name = "gridColumn31";
            this.gridColumn31.Visible = true;
            this.gridColumn31.VisibleIndex = 0;
            this.gridColumn31.Width = 30;
            // 
            // xtraTabControl2
            // 
            this.xtraTabControl2.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.xtraTabControl2.Appearance.Options.UseBackColor = true;
            this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl2.Name = "xtraTabControl2";
            this.xtraTabControl2.SelectedTabPage = this.xtraTabPage4;
            this.xtraTabControl2.Size = new System.Drawing.Size(1130, 179);
            this.xtraTabControl2.TabIndex = 1;
            this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage4,
            this.xtraTabPage5});
            // 
            // xtraTabPage4
            // 
            this.xtraTabPage4.Controls.Add(this.groupControl2);
            this.xtraTabPage4.Controls.Add(this.groupControl1);
            this.xtraTabPage4.Controls.Add(this.groupControl5);
            this.xtraTabPage4.Controls.Add(this.groupControl4);
            this.xtraTabPage4.Controls.Add(this.groupControl6);
            this.xtraTabPage4.Name = "xtraTabPage4";
            this.xtraTabPage4.Size = new System.Drawing.Size(1124, 153);
            this.xtraTabPage4.Text = "Settings";
            // 
            // groupControl2
            // 
            this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupControl2.Controls.Add(this.txtTransactionTimeSpan);
            this.groupControl2.Controls.Add(this.labelControl10);
            this.groupControl2.Controls.Add(this.txtMaxTimePerHalf);
            this.groupControl2.Controls.Add(this.labelControl7);
            this.groupControl2.Controls.Add(this.chbAllowHalftime);
            this.groupControl2.Location = new System.Drawing.Point(209, 3);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(200, 147);
            this.groupControl2.TabIndex = 14;
            this.groupControl2.Text = "Time - Type Settings";
            // 
            // txtTransactionTimeSpan
            // 
            this.txtTransactionTimeSpan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTransactionTimeSpan.EditValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtTransactionTimeSpan.Location = new System.Drawing.Point(123, 80);
            this.txtTransactionTimeSpan.Name = "txtTransactionTimeSpan";
            this.txtTransactionTimeSpan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtTransactionTimeSpan.Properties.IsFloatValue = false;
            this.txtTransactionTimeSpan.Properties.Mask.EditMask = "N00";
            this.txtTransactionTimeSpan.Size = new System.Drawing.Size(72, 20);
            this.txtTransactionTimeSpan.TabIndex = 9;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(5, 81);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(112, 13);
            this.labelControl10.TabIndex = 8;
            this.labelControl10.Text = "Transaction Time Span:";
            // 
            // txtMaxTimePerHalf
            // 
            this.txtMaxTimePerHalf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaxTimePerHalf.EditValue = new decimal(new int[] {
            45,
            0,
            0,
            0});
            this.txtMaxTimePerHalf.Enabled = false;
            this.txtMaxTimePerHalf.Location = new System.Drawing.Point(123, 50);
            this.txtMaxTimePerHalf.Name = "txtMaxTimePerHalf";
            this.txtMaxTimePerHalf.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtMaxTimePerHalf.Properties.IsFloatValue = false;
            this.txtMaxTimePerHalf.Properties.Mask.EditMask = "N00";
            this.txtMaxTimePerHalf.Size = new System.Drawing.Size(72, 20);
            this.txtMaxTimePerHalf.TabIndex = 7;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(5, 53);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(90, 13);
            this.labelControl7.TabIndex = 6;
            this.labelControl7.Text = "Max Time Per Half:";
            // 
            // chbAllowHalftime
            // 
            this.chbAllowHalftime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chbAllowHalftime.EditValue = true;
            this.chbAllowHalftime.Enabled = false;
            this.chbAllowHalftime.Location = new System.Drawing.Point(5, 26);
            this.chbAllowHalftime.Name = "chbAllowHalftime";
            this.chbAllowHalftime.Properties.Caption = "Allow Halftime";
            this.chbAllowHalftime.Size = new System.Drawing.Size(190, 19);
            this.chbAllowHalftime.TabIndex = 5;
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupControl1.Controls.Add(this.chkProxy);
            this.groupControl1.Controls.Add(this.btnStatus);
            this.groupControl1.Controls.Add(this.btnSetUpdateInterval);
            this.groupControl1.Controls.Add(this.txtSBOBETUpdateInterval);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.txtIBETUpdateInterval);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Location = new System.Drawing.Point(415, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(200, 147);
            this.groupControl1.TabIndex = 13;
            this.groupControl1.Text = "Data Settings";
            // 
            // chkProxy
            // 
            this.chkProxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkProxy.EditValue = true;
            this.chkProxy.Location = new System.Drawing.Point(5, 108);
            this.chkProxy.Name = "chkProxy";
            this.chkProxy.Properties.Caption = "Use Proxy";
            this.chkProxy.Size = new System.Drawing.Size(190, 19);
            this.chkProxy.TabIndex = 33;
            // 
            // btnStatus
            // 
            this.btnStatus.Location = new System.Drawing.Point(6, 77);
            this.btnStatus.Name = "btnStatus";
            this.btnStatus.Size = new System.Drawing.Size(89, 23);
            this.btnStatus.TabIndex = 31;
            this.btnStatus.Text = "Show status";
            this.btnStatus.Click += new System.EventHandler(this.btnStatus_Click_1);
            // 
            // btnSetUpdateInterval
            // 
            this.btnSetUpdateInterval.Location = new System.Drawing.Point(106, 77);
            this.btnSetUpdateInterval.Name = "btnSetUpdateInterval";
            this.btnSetUpdateInterval.Size = new System.Drawing.Size(89, 23);
            this.btnSetUpdateInterval.TabIndex = 6;
            this.btnSetUpdateInterval.Text = "Set Interval";
            this.btnSetUpdateInterval.Click += new System.EventHandler(this.btnSetUpdateInterval_Click);
            // 
            // txtSBOBETUpdateInterval
            // 
            this.txtSBOBETUpdateInterval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSBOBETUpdateInterval.EditValue = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.txtSBOBETUpdateInterval.Location = new System.Drawing.Point(132, 51);
            this.txtSBOBETUpdateInterval.Name = "txtSBOBETUpdateInterval";
            this.txtSBOBETUpdateInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSBOBETUpdateInterval.Properties.IsFloatValue = false;
            this.txtSBOBETUpdateInterval.Properties.Mask.EditMask = "N00";
            this.txtSBOBETUpdateInterval.Properties.MaxValue = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.txtSBOBETUpdateInterval.Properties.MinValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtSBOBETUpdateInterval.Size = new System.Drawing.Size(63, 20);
            this.txtSBOBETUpdateInterval.TabIndex = 5;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(5, 54);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(102, 13);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "SUB Update Interval:";
            // 
            // txtIBETUpdateInterval
            // 
            this.txtIBETUpdateInterval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIBETUpdateInterval.EditValue = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.txtIBETUpdateInterval.Location = new System.Drawing.Point(132, 25);
            this.txtIBETUpdateInterval.Name = "txtIBETUpdateInterval";
            this.txtIBETUpdateInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtIBETUpdateInterval.Properties.IsFloatValue = false;
            this.txtIBETUpdateInterval.Properties.Mask.EditMask = "N00";
            this.txtIBETUpdateInterval.Properties.MaxValue = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.txtIBETUpdateInterval.Properties.MinValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.txtIBETUpdateInterval.Size = new System.Drawing.Size(63, 20);
            this.txtIBETUpdateInterval.TabIndex = 1;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(5, 28);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(105, 13);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "IBET Update Interval:";
            // 
            // groupControl5
            // 
            this.groupControl5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupControl5.Controls.Add(this.chkEnable);
            this.groupControl5.Controls.Add(this.chkFollowType);
            this.groupControl5.Controls.Add(this.txtLowestOddValue);
            this.groupControl5.Controls.Add(this.labelControl9);
            this.groupControl5.Controls.Add(this.checkEdit6);
            this.groupControl5.Controls.Add(this.checkEdit5);
            this.groupControl5.Controls.Add(this.checkEdit7);
            this.groupControl5.Controls.Add(this.checkEdit12);
            this.groupControl5.Controls.Add(this.checkEdit4);
            this.groupControl5.Controls.Add(this.checkEdit3);
            this.groupControl5.Location = new System.Drawing.Point(3, 3);
            this.groupControl5.Name = "groupControl5";
            this.groupControl5.Size = new System.Drawing.Size(200, 147);
            this.groupControl5.TabIndex = 13;
            this.groupControl5.Text = "Trading Settings";
            // 
            // chkEnable
            // 
            this.chkEnable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEnable.EditValue = true;
            this.chkEnable.Location = new System.Drawing.Point(5, 123);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Properties.Caption = "Enable";
            this.chkEnable.Size = new System.Drawing.Size(90, 19);
            this.chkEnable.TabIndex = 25;
            // 
            // chkFollowType
            // 
            this.chkFollowType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFollowType.EditValue = true;
            this.chkFollowType.Location = new System.Drawing.Point(5, 79);
            this.chkFollowType.Name = "chkFollowType";
            this.chkFollowType.Properties.Caption = "Follow / Unfollow";
            this.chkFollowType.Size = new System.Drawing.Size(190, 19);
            this.chkFollowType.TabIndex = 24;
            // 
            // txtLowestOddValue
            // 
            this.txtLowestOddValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLowestOddValue.EditValue = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.txtLowestOddValue.Location = new System.Drawing.Point(110, 103);
            this.txtLowestOddValue.Name = "txtLowestOddValue";
            this.txtLowestOddValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtLowestOddValue.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.txtLowestOddValue.Properties.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            131072});
            this.txtLowestOddValue.Properties.MinValue = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.txtLowestOddValue.Size = new System.Drawing.Size(85, 20);
            this.txtLowestOddValue.TabIndex = 12;
            // 
            // labelControl9
            // 
            this.labelControl9.Enabled = false;
            this.labelControl9.Location = new System.Drawing.Point(8, 105);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(90, 13);
            this.labelControl9.TabIndex = 11;
            this.labelControl9.Text = "Lowest Odd Value:";
            // 
            // checkEdit6
            // 
            this.checkEdit6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit6.EditValue = true;
            this.checkEdit6.Enabled = false;
            this.checkEdit6.Location = new System.Drawing.Point(5, 352);
            this.checkEdit6.Name = "checkEdit6";
            this.checkEdit6.Properties.Caption = "Non-Live";
            this.checkEdit6.Size = new System.Drawing.Size(190, 19);
            this.checkEdit6.TabIndex = 8;
            // 
            // checkEdit5
            // 
            this.checkEdit5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit5.EditValue = true;
            this.checkEdit5.Enabled = false;
            this.checkEdit5.Location = new System.Drawing.Point(5, 327);
            this.checkEdit5.Name = "checkEdit5";
            this.checkEdit5.Properties.Caption = "Live";
            this.checkEdit5.Size = new System.Drawing.Size(190, 19);
            this.checkEdit5.TabIndex = 7;
            // 
            // checkEdit7
            // 
            this.checkEdit7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit7.EditValue = true;
            this.checkEdit7.Enabled = false;
            this.checkEdit7.Location = new System.Drawing.Point(5, 52);
            this.checkEdit7.Name = "checkEdit7";
            this.checkEdit7.Properties.Caption = "Over/Under from min 30";
            this.checkEdit7.Size = new System.Drawing.Size(190, 19);
            this.checkEdit7.TabIndex = 9;
            // 
            // checkEdit12
            // 
            this.checkEdit12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit12.Location = new System.Drawing.Point(5, 302);
            this.checkEdit12.Name = "checkEdit12";
            this.checkEdit12.Properties.Caption = "IBET Under mode";
            this.checkEdit12.Size = new System.Drawing.Size(190, 19);
            this.checkEdit12.TabIndex = 23;
            // 
            // checkEdit4
            // 
            this.checkEdit4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit4.EditValue = true;
            this.checkEdit4.Enabled = false;
            this.checkEdit4.Location = new System.Drawing.Point(115, 26);
            this.checkEdit4.Name = "checkEdit4";
            this.checkEdit4.Properties.Caption = "Over/Under";
            this.checkEdit4.Size = new System.Drawing.Size(80, 19);
            this.checkEdit4.TabIndex = 6;
            // 
            // checkEdit3
            // 
            this.checkEdit3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit3.EditValue = true;
            this.checkEdit3.Enabled = false;
            this.checkEdit3.Location = new System.Drawing.Point(5, 26);
            this.checkEdit3.Name = "checkEdit3";
            this.checkEdit3.Properties.Caption = "Handicap";
            this.checkEdit3.Size = new System.Drawing.Size(90, 19);
            this.checkEdit3.TabIndex = 5;
            // 
            // groupControl4
            // 
            this.groupControl4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupControl4.Controls.Add(this.chkFollowPercent);
            this.groupControl4.Controls.Add(this.txtIBETFixedStake);
            this.groupControl4.Controls.Add(this.labelControl11);
            this.groupControl4.Controls.Add(this.labelControl6);
            this.groupControl4.Controls.Add(this.txtAllowTradeMinValue);
            this.groupControl4.Controls.Add(this.txtFollowPercent);
            this.groupControl4.Location = new System.Drawing.Point(621, 3);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(200, 147);
            this.groupControl4.TabIndex = 12;
            this.groupControl4.Text = "Stake Settings";
            // 
            // chkFollowPercent
            // 
            this.chkFollowPercent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFollowPercent.EditValue = true;
            this.chkFollowPercent.Location = new System.Drawing.Point(3, 52);
            this.chkFollowPercent.Name = "chkFollowPercent";
            this.chkFollowPercent.Properties.Caption = "Follow Percent (%)";
            this.chkFollowPercent.Size = new System.Drawing.Size(123, 19);
            this.chkFollowPercent.TabIndex = 34;
            // 
            // txtIBETFixedStake
            // 
            this.txtIBETFixedStake.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIBETFixedStake.EditValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.txtIBETFixedStake.Location = new System.Drawing.Point(132, 25);
            this.txtIBETFixedStake.Name = "txtIBETFixedStake";
            this.txtIBETFixedStake.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtIBETFixedStake.Properties.IsFloatValue = false;
            this.txtIBETFixedStake.Properties.Mask.EditMask = "N00";
            this.txtIBETFixedStake.Size = new System.Drawing.Size(63, 20);
            this.txtIBETFixedStake.TabIndex = 1;
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(7, 83);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(117, 13);
            this.labelControl11.TabIndex = 33;
            this.labelControl11.Text = "Minimum Stake Following";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(5, 28);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(85, 13);
            this.labelControl6.TabIndex = 0;
            this.labelControl6.Text = "IBET Fixed Stake:";
            // 
            // txtAllowTradeMinValue
            // 
            this.txtAllowTradeMinValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAllowTradeMinValue.EditValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txtAllowTradeMinValue.Location = new System.Drawing.Point(132, 80);
            this.txtAllowTradeMinValue.Name = "txtAllowTradeMinValue";
            this.txtAllowTradeMinValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtAllowTradeMinValue.Properties.MaxValue = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.txtAllowTradeMinValue.Properties.MinValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.txtAllowTradeMinValue.Size = new System.Drawing.Size(63, 20);
            this.txtAllowTradeMinValue.TabIndex = 32;
            // 
            // txtFollowPercent
            // 
            this.txtFollowPercent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFollowPercent.EditValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtFollowPercent.Location = new System.Drawing.Point(132, 51);
            this.txtFollowPercent.Name = "txtFollowPercent";
            this.txtFollowPercent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtFollowPercent.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.txtFollowPercent.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtFollowPercent.Size = new System.Drawing.Size(63, 20);
            this.txtFollowPercent.TabIndex = 10;
            // 
            // groupControl6
            // 
            this.groupControl6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupControl6.Controls.Add(this.txtListFollowAccounts);
            this.groupControl6.Location = new System.Drawing.Point(827, 3);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.Size = new System.Drawing.Size(293, 147);
            this.groupControl6.TabIndex = 19;
            this.groupControl6.Text = "Following Accounts";
            // 
            // txtListFollowAccounts
            // 
            this.txtListFollowAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtListFollowAccounts.EditValue = "FOLLOW HERE";
            this.txtListFollowAccounts.Location = new System.Drawing.Point(5, 25);
            this.txtListFollowAccounts.Name = "txtListFollowAccounts";
            this.txtListFollowAccounts.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txtListFollowAccounts.Properties.Appearance.Options.UseFont = true;
            this.txtListFollowAccounts.Size = new System.Drawing.Size(283, 117);
            this.txtListFollowAccounts.TabIndex = 20;
            // 
            // xtraTabPage5
            // 
            this.xtraTabPage5.Controls.Add(this.grdTransaction);
            this.xtraTabPage5.Name = "xtraTabPage5";
            this.xtraTabPage5.Size = new System.Drawing.Size(1124, 153);
            this.xtraTabPage5.Text = "Live Transaction";
            // 
            // grdTransaction
            // 
            this.grdTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTransaction.Location = new System.Drawing.Point(0, 0);
            this.grdTransaction.MainView = this.gridView2;
            this.grdTransaction.Name = "grdTransaction";
            this.grdTransaction.Size = new System.Drawing.Size(1124, 153);
            this.grdTransaction.TabIndex = 3;
            this.grdTransaction.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn6,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn10,
            this.gridColumn13,
            this.gridColumn39});
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
            this.gridView2.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn13, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "ID";
            this.gridColumn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn1.FieldName = "ID";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.FixedWidth = true;
            this.gridColumn1.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 50;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Home Team";
            this.gridColumn2.FieldName = "HomeTeamName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 187;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Away Team";
            this.gridColumn3.FieldName = "AwayTeamName";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 187;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Type";
            this.gridColumn6.FieldName = "OddType";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.FixedWidth = true;
            this.gridColumn6.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            this.gridColumn6.Width = 180;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Odd";
            this.gridColumn4.FieldName = "OddKindValue";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.FixedWidth = true;
            this.gridColumn4.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 5;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Value";
            this.gridColumn5.FieldName = "OddValue";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.FixedWidth = true;
            this.gridColumn5.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 6;
            this.gridColumn5.Width = 80;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Stake";
            this.gridColumn7.FieldName = "Stake";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.FixedWidth = true;
            this.gridColumn7.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 7;
            this.gridColumn7.Width = 100;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "I Allow";
            this.gridColumn8.FieldName = "IBETAllow";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.FixedWidth = true;
            this.gridColumn8.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 8;
            this.gridColumn8.Width = 50;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "I Trade";
            this.gridColumn10.FieldName = "IBETTrade";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.FixedWidth = true;
            this.gridColumn10.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 9;
            this.gridColumn10.Width = 50;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "DateTime";
            this.gridColumn13.DisplayFormat.FormatString = "hh:mm:ss";
            this.gridColumn13.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn13.FieldName = "DateTime";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.FixedWidth = true;
            this.gridColumn13.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.gridColumn13.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn13.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 10;
            this.gridColumn13.Width = 80;
            // 
            // gridColumn39
            // 
            this.gridColumn39.Caption = "Follow Ref";
            this.gridColumn39.FieldName = "FollowRef";
            this.gridColumn39.Name = "gridColumn39";
            this.gridColumn39.Visible = true;
            this.gridColumn39.VisibleIndex = 1;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(5, 9);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(0, 13);
            this.labelControl5.TabIndex = 6;
            this.labelControl5.Text = "Address:";
            // 
            // FollowSub
            // 
            this.ClientSize = new System.Drawing.Size(1130, 742);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.ribbonControl1);
            this.Icon = global::iBet.App.Properties.Resources._2;
            this.Name = "FollowSub";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IBET follow SUB";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TerminalFormIBETSBO_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbeSignatureTemplate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETAddress.Properties)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chooseSBOServer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBOBETAddress.Properties)).EndInit();
            this.xtraTabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSameMatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.xtraTabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridNonLiveMatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            this.tabBetList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdBetList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).EndInit();
            this.xtraTabControl2.ResumeLayout(false);
            this.xtraTabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransactionTimeSpan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxTimePerHalf.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAllowHalftime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkProxy.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBOBETUpdateInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETUpdateInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
            this.groupControl5.ResumeLayout(false);
            this.groupControl5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFollowType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLowestOddValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit6.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit5.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit7.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit12.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            this.groupControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkFollowPercent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETFixedStake.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAllowTradeMinValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFollowPercent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtListFollowAccounts.Properties)).EndInit();
            this.xtraTabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTransaction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }
        private void cbeSignatureTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtIBETAddress.Text = cbeSignatureTemplate.SelectedItem.ToString();
        }
        private void chooseSBOServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtSBOBETAddress.Text = chooseSBOServer.SelectedItem.ToString();
        }





    }
}
