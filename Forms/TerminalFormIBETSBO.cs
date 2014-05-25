//#define DEBUG
#define TESTMODE
using BCCore;
using BCCore.Utis;

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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using iBet.App.admin;
using iBet.DTO;
using System.Collections;
using iBet.Engine;
using iBet.Utilities;


namespace iBet.App
{
    public class TerminalFormIBETSBO : RibbonForm
    {
        //Code moi hoan toan
        
        private BetEngineManager engineManager;
        private ScanningType scantype;
        BetAgentConfig config;
        ////////////////////////////////////////////////////////////////
        
        private const int INTERNET_COOKIE_HTTPONLY = 8192;
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
        private IBetEngine _ibetEngine;
        public bool isScanner = false;
        private SbobetEngine _sbobetEngine;
        private System.Collections.Generic.List<MatchDTO> _listIBETMatch;
        private Dictionary<string, IbetMatch> _ibetMatchs;
        private Dictionary<string, IbetMatch> _ibetMatchsSnapShot;
        private Dictionary<string, SboMatch> _sboMatchs;
        List<Bet> bl;
        private System.Collections.Generic.List<MatchDTO> _listSbobetMatch;
        private System.Collections.Generic.List<MatchDTO> _listSameMatch;
        private System.Collections.Generic.List<TransactionDTO> _listTransaction;
        private System.Collections.Generic.Dictionary<string, string> _oddTransactionHistory;
        System.Collections.Generic.List<BetAnalyse> listBA;
        private bool _running = false;
        private bool _comparing = false;
        private bool _compareAgain = false;
        private bool _betting = false;
        private System.Windows.Forms.Timer _forceRefreshTimer;
        private System.Windows.Forms.Timer _matchesRefreshTimer;
        private System.Windows.Forms.Timer _creditRefreshTimer;
        private string _ibetAccount = "";
        private string _sbobetAccount = "";
        private admin.DataServiceSoapClient _dataService;
        private string _currentUserID;
        private System.DateTime _lastTransactionTime = System.DateTime.Now;
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
        private PopupMenu pmNew;
        private PopupMenu pmNewR;
        private BarButtonItem iNew;
        private BarButtonItem iNew2;
        private BarButtonItem iNewR;
        private BarButtonItem iNewR2IB;
        private BarButtonItem iLoadStrategy;
        private BarButtonItem iLoadAllStrategy;

        private XtraTabPage xtraTabPage1;
        private XtraTabPage xtraTabPageMatches;
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
        private RibbonPageGroup ribbonPageGroup5;
        private BarButtonItem btnClear;
        private BarButtonItem btnSnapShot;
        private XtraTabPage xtraTabPage2;
        private PanelControl panelControl3;
        
        private PanelControl panelControl2;        
        private LabelControl labelControl1;
        private PanelControl panelControl5;
        private PanelControl panelControl4;

        private PanelControl panelControl9;
        private PanelControl panelControl10;

        
        private SimpleButton btnIBETGO2;
        
        private TextEdit txtIBETAddress2;
        private LabelControl labelControl5;
        private GridControl grdSameMatch;
        private GridView gridView1;
        private GridColumn gridColumn15;
        private GridColumn gridColumn16;
        private GridColumn gridColumn17;
        private GridColumn gridColumn18;
        private GridColumn gridColumn19;
        private GroupControl groupControl5;

        //private CheckEdit checkEdit13; // 
        private CheckEdit checkEdit15; // messenger
        private CheckEdit checkEdit14; // local odd receive
        private CheckEdit checkEdit13; // local odd add
        private CheckEdit checkEdit12; // IBET over only
        private CheckEdit checkEdit11; // secured bet
        private CheckEdit checkEdit10; // check tran
        private CheckEdit checkEdit9; // sound
        private CheckEdit checkEdit8; // credit
        private CheckEdit checkEdit7; // rung
        private CheckEdit checkEdit6; // non-live
        private CheckEdit checkEdit5; // live
        private CheckEdit checkEdit4; // over/under
        private CheckEdit checkEdit3; // handicap
        private GroupControl groupControl4;
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
        private GridColumn gridColumn21;
        private GridColumn gridColumn22;
        private GridColumn gridColumn23;
        private SpinEdit txtSBOBETFixedStake;
        private LabelControl labelControl2;
        
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
        private SpinEdit txtOddValueDifferenet;
        private LabelControl labelControl8;
        private LabelControl labelControl18;
        private SimpleButton btnSetUpdateInterval;
        private SpinEdit txtLowestOddValue;
        private SpinEdit txtAddValue;
        private LabelControl labelControl9;
        private SpinEdit txtTransactionTimeSpan;
        private LabelControl labelControl10;
        private CheckEdit chbHighRevenueBoost;

        private GroupControl groupControl6;
        private Label label1;
        private CheckEdit checkEdit1;
        private SpinEdit spinEdit3;
        private LabelControl labelControl13;
        private SpinEdit spinEdit2;
        private SpinEdit spinEdit1;
        private SpinEdit spinEdit4;
        private XtraTabPage xtraTabPageBetList1;
        private XtraTabPage xtraTabPageBetList2;
        private GridControl girdBetList1;
        private GridView gridView3;
        private GridColumn gridColumn24;
        private GridColumn gridColumn25;
        private GridColumn gridColumn28;
        private GridColumn gridColumn29;
        private GridColumn gridColumn30;
        private GridColumn gridColumn31;
        private GridColumn gridColumn32;
        private GridColumn gridColumn33;
        private LabelControl labelControl12;
        private LabelControl labelControl11;
        private CheckEdit checkEdit18;
        private CheckEdit checkEdit17;
        private CheckEdit checkEdit2;
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
            if (!TerminalFormIBETSBO.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
            {
                if (num < 0u)
                {
                    result = null;
                    return result;
                }
                num = 1024u;
                stringBuilder = new System.Text.StringBuilder(1024);
                if (!TerminalFormIBETSBO.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
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
            if (!TerminalFormIBETSBO.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
            {
                if (num < 0u)
                {
                    result = null;
                    return result;
                }
                num = 1024u;
                stringBuilder = new System.Text.StringBuilder(1024);
                if (!TerminalFormIBETSBO.InternetGetCookieEx(uri, "ASP.NET_SessionId", stringBuilder, ref num, 8192, System.IntPtr.Zero))
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
            if (!TerminalFormIBETSBO.InternetGetCookieEx(uri, null, stringBuilder, ref num, 8192, System.IntPtr.Zero))
            {
                if (num < 0u)
                {
                    result = null;
                    return result;
                }
                num = 1024u;
                stringBuilder = new System.Text.StringBuilder(1024);
                if (!TerminalFormIBETSBO.InternetGetCookieEx(uri, null, stringBuilder, ref num, 8192, System.IntPtr.Zero))
                {
                    result = null;
                    return result;
                }
            }
            result = stringBuilder.ToString();
            return result;
        }
        public TerminalFormIBETSBO(MainForm mainForm, string currentUserID, int formCount)
        {
            

            this.InitializeComponent();
            List<BetAgentConfig> agents = ConfigurationInstance<BetEngineConfiguration>.GetConfig().Agents;
            this.config = agents[formCount];
            
            
            //list.Add("55640842");
            //Thread thread = new Thread((ThreadStart)(() =>
            //{
            //    this.engineManager = new BetEngineManager();
            //    List<string> list = new List<string>();
            //    this.engineManager.Start(list);
            //}));
            //thread.Start();
            this.scantype = new ScanningType();
            this._ibetMatchs = new Dictionary<string, IbetMatch>();
            this._ibetMatchsSnapShot = new Dictionary<string, IbetMatch>();
            this._sboMatchs = new Dictionary<string, SboMatch>(); 

            this._mainForm = mainForm;
            this._dataService = new App.admin.DataServiceSoapClient();
            this._dataService.AllowRunCompleted += new System.EventHandler<iBet.App.admin.AllowRunCompletedEventArgs>(this._dataService_AllowRunCompleted);
            this._currentUserID = currentUserID;
            this._forceRefreshTimer = new System.Windows.Forms.Timer();
            this._forceRefreshTimer.Interval = 180000;
            this._forceRefreshTimer.Tick += new System.EventHandler(this._forceRefreshTimer_Tick);

            this._creditRefreshTimer = new System.Windows.Forms.Timer();
            this._creditRefreshTimer.Interval = 180000;
            this._creditRefreshTimer.Tick += new System.EventHandler(this._creditRefreshTimer_Tick);

            this._matchesRefreshTimer = new System.Windows.Forms.Timer();
            this._matchesRefreshTimer.Interval = 60000;
            this._matchesRefreshTimer.Tick += new System.EventHandler(this._matchesRefreshTimer_Tick);

            this._oddTransactionHistory = new System.Collections.Generic.Dictionary<string, string>();
            this._listTransaction = new System.Collections.Generic.List<TransactionDTO>();
            this.grdTransaction.DataSource = this._listTransaction;
            this._listSameMatch = new System.Collections.Generic.List<MatchDTO>();
            this.grdSameMatch.DataSource = this._listSameMatch;
            this.listBA = new List<BetAnalyse>();
            this.bl = new List<Bet>();
            this.girdBetList1.DataSource = this.bl;





            //MatchDTO match = new MatchDTO();
            //match.HomeTeamName = "Test Home Team";
            //match.AwayTeamName = "Another Away Team";
            //match.HomeScore = "0";
            //match.AwayScore = "0";
            //match.Minute = 28;
            //match.Half = 1;
            //match.KickOffTime = "20/10 13:41 Live!";
            //match.League = new LeagueDTO();
            //match.League.Name = "Ein Element mit dem gleichen Schlüssel".ToUpper();

            //MatchDTO match2 = new MatchDTO();
            //match2.HomeTeamName = "Test Home Team";
            //match2.AwayTeamName = "Another Away Team";
            //match2.HomeScore = "0";
            //match2.AwayScore = "0";
            //match2.Minute = 28;
            //match2.Half = 1;
            //match2.KickOffTime = "20/10 13:43";
            //match2.League = new LeagueDTO();
            //match2.League.Name = "Ein Element mit dem gleichen Schlüssel".ToUpper();

            //this._listSameMatch.Add(match); this._listSameMatch.Add(match);
            //this._listSameMatch.Add(match); this._listSameMatch.Add(match);
            //this._listSameMatch.Add(match); this._listSameMatch.Add(match);
            //this._listSameMatch.Add(match); this._listSameMatch.Add(match);
            //this._listSameMatch.Add(match2); this._listSameMatch.Add(match2); this._listSameMatch.Add(match2);
            //this._listSameMatch.Add(match2); this._listSameMatch.Add(match2); this._listSameMatch.Add(match2);
            //this._listSameMatch.Add(match2); this._listSameMatch.Add(match2);
            //this.gridView1.RefreshData();
        }
        private void _dataService_AllowRunCompleted(object sender, AllowRunCompletedEventArgs e)
        {
            if (e.Result)
            {
                this.Start();
                this._dataService.StartTerminalAsync(this._currentUserID, this.Text);
                //this._ibetEngine.UpdateDataInterval = (int)this.txtIBETUpdateInterval.Value * 1000;
                //this._sbobetEngine.UpdateDataInterval = (int)this.txtSBOBETUpdateInterval.Value * 1000;
            }
            else
            {
                this.ShowWarningDialog("Your logged in account is not permitted to start the current betting pair account. \nPlease contact with Administrators.");
            }
        }
        private void _creditRefreshTimer_Tick(object sender, System.EventArgs e)
        {
            BarItem arg_1A_0 = this.lblIbetCurrentCredit;
            if (this._ibetEngine != null && this._ibetEngine.ibetAgent.State == AgentState.Running)
            {
                this._ibetEngine.ibetAgent.RefreshCredit();
                arg_1A_0.Caption = this._ibetEngine.ibetAgent.Config.Balance.ToString() + " (" + this._ibetEngine.ibetAgent.Config.Cash.ToString() + ")";
                //this.LoadAnalyse();
            }
            //BarItem arg_39_0 = this.lblSbobetCurrentCredit;
            //float currentCredit = this._sbobetEngine.GetCurrentCredit();
            //arg_39_0.Caption = currentCredit.ToString();
            //this._mainForm.InitializeWCFService();
        }
        private void _forceRefreshTimer_Tick(object sender, System.EventArgs e)
        {
#if DEBUG
            Utilities.WriteLog.Write("System is forced to restart");
#endif

            if (this._sbobetEngine != null && this._sbobetEngine.sboAgent.State == AgentState.Running)
            {
                this._sbobetEngine.Stop();
                this._sbobetEngine.Start();
            }
            if (this._ibetEngine != null && this._ibetEngine.ibetAgent.State == AgentState.Running)
            {
                this._ibetEngine.Stop();
                this._ibetEngine.Start();
                this._ibetEngine.ibetAgent.RefreshBetList();                
            }
        }
        private void _matchesRefreshTimer_Tick(object sender, System.EventArgs e)
        {
            RefreshAllowedListMatches();
        }
        internal void Start()
        {
            if (this._ibetEngine != null && this._ibetEngine.ibetAgent.State == AgentState.Running)
            {
                this._ibetEngine.Start();
                if (this._sbobetEngine == null || this._sbobetEngine.sboAgent.State != AgentState.Running)
                {
                    if (this._ibetEngine.ibetAgent2.State == AgentState.Running)
                    {
                        scantype = ScanningType.ibetVSibet;
                        this.lblStatus.Caption = "RUNNING IB vs IB";
                    }
                    else
                    {
                        scantype = ScanningType.ibet;
                        this.lblStatus.Caption = "RUNNING IB STRG";
                    }
                }
                else
                {
                    scantype = ScanningType.both;
                    this.lblStatus.Caption = "RUNNING BOTH";
                }
            }
            if (this._sbobetEngine != null && this._sbobetEngine.sboAgent.State == AgentState.Running)
            {
                this._sbobetEngine.Start();
                if (this._ibetEngine == null || this._ibetEngine.ibetAgent.State != AgentState.Running)
                {
                    scantype = ScanningType.sbo;
                    this.lblStatus.Caption = "RUNNING SB STRG";
                }
                else
                {
                    scantype = ScanningType.both;
                    this.lblStatus.Caption = "RUNNING BOTH";
                }
            }
            this._running = true;
            this._forceRefreshTimer.Start();
            this._creditRefreshTimer.Start();
            if (checkEdit10.Checked)
            {
                //this._matchesRefreshTimer.Start();
            }
            
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            this.btnClear.Enabled = true;
            this.btnSnapShot.Enabled = true;
            this.btnIbetGetInfo.Enabled = false;
            this.btnSbobetGetInfo.Enabled = false;
        }
        internal void StartFromTracking()
        {
            this._dataService.AllowRunAsync(this._currentUserID, this._ibetAccount.ToUpper(), this._sbobetAccount.ToUpper());
        }
        internal void Stop()
        {
            if (this._ibetEngine != null)
            {
                this._ibetEngine.Stop();
            }
            if (this._sbobetEngine != null)
            {
                this._sbobetEngine.Stop();
            }
            this._running = false;
            this._forceRefreshTimer.Stop();
            this._creditRefreshTimer.Stop();
            if (checkEdit10.Checked)
            {
                this._matchesRefreshTimer.Stop();
            }
            this.lblStatus.Caption = "STOPPED";
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;
            this.btnClear.Enabled = false;
            this.btnSnapShot.Enabled = false;
            this.btnIbetGetInfo.Enabled = true;
            this.btnSbobetGetInfo.Enabled = true;
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
        private void btnLoginIbet2(object sender, ItemClickEventArgs e)
        {
            this.InitCoreIbetEngine2();
        }
        private void InitCoreIbetEngine2()
        {
            if (_ibetEngine != null && _ibetEngine.ibetAgent2.State == AgentState.Running)
            {
                this._ibetEngine.ibetAgent2.Logout();
                this.iNewR2IB.Caption = "Login to 2nd IBET";
                this.iNew2.Enabled = false;
                //this.lblIbetCurrentCredit.Caption = "-";
                //this.iNew.Enabled = false;
                //this.iLoadStrategy.Enabled = false;
            }
            else
            {
                BCCore.BetAccount ibetAccount = new BCCore.BetAccount();                
                
                ibetAccount.Account = config.Ibet2.Account;
                ibetAccount.Password = config.Ibet2.Password;
                ibetAccount.Website = config.Ibet2.Website;                
                
                BCCore.LoginStatus loginStatus = this._ibetEngine.ibetAgent2.Login();
                

                if (loginStatus == BCCore.LoginStatus.OK)
                {
                    this.rpgIbet.Text = "IBET: " + this._ibetEngine.ibetAgent.Config.Account + " vs " + ibetAccount.Account;                    
                    this._ibetEngine.ibetAgent2.RefreshCredit();
                    this._ibetEngine.ibetAgent2.RefreshBetList();
                    this.lblIbetCurrentCredit.Caption = this._ibetEngine.ibetAgent.Config.Balance.ToString() + " (" + this._ibetEngine.ibetAgent.Config.Cash.ToString() + ")"
                        + " / " + this._ibetEngine.ibetAgent2.Config.Balance.ToString() + " (" + this._ibetEngine.ibetAgent2.Config.Cash.ToString() + ")";
                    this.iNewR2IB.Caption = "Logout 2nd IBET";
                    this.iNew2.Enabled = true;
                }
            }
        }

        private void InitCoreIbetEngine()
        {
            if (_ibetEngine != null && _ibetEngine.ibetAgent.State == AgentState.Running)
            {
                this._ibetEngine.ibetAgent.Logout();
                this.lblIbetCurrentCredit.Caption = "-";
                this.iNew.Enabled = false;
                this.iLoadStrategy.Enabled = false;
            }
            else
            {
                BCCore.BetAccount ibetAccount = new BCCore.BetAccount();
                BCCore.BetAccount ibetAccount2 = new BCCore.BetAccount();
                
                ibetAccount.Account = config.Ibet.Account;
                ibetAccount.Password = config.Ibet.Password;
                ibetAccount.Website = config.Ibet.Website;                

                BCCore.Utis.EngineLogger logger = new BCCore.Utis.EngineLogger("log");
                if (config.Ibet2 != null)
                {
                    ibetAccount2.Account = config.Ibet2.Account;
                    ibetAccount2.Password = config.Ibet2.Password;
                    ibetAccount2.Website = config.Ibet2.Website;
                    this._ibetEngine = new IBetEngine(ibetAccount, logger, "Engine1", 
                                                      ibetAccount2, logger, "Engine2");
                }
                else
                    this._ibetEngine = new IBetEngine(ibetAccount, logger, "Engine1");
                BCCore.LoginStatus loginStatus = this._ibetEngine.ibetAgent.Login();
                //while (loginStatus == BCCore.LoginStatus.InvalidCaptcha)
                //{
                //    this._ibetEngine.ibetAgent.Login();
                //}
                
                if (loginStatus == BCCore.LoginStatus.OK)
                {
                    this.rpgIbet.Text = "IBET - " + ibetAccount.Account;
                    this._ibetEngine.UpdateCompleted += new EngineDelegate(this._ibetEngine_UpdateCompleted);
                    this._ibetEngine.ibetAgent.RefreshCredit();
                    this._ibetEngine.ibetAgent.RefreshBetList();
                    this.lblIbetCurrentCredit.Caption = this._ibetEngine.ibetAgent.Config.Balance.ToString() + " (" + this._ibetEngine.ibetAgent.Config.Cash.ToString() + ")";
                    this.btnStart.Enabled = true;
                    this.iNew.Enabled = true;
                    this.iLoadStrategy.Enabled = true;
                    this.btnIbetGetInfo.Caption = "Log Out";
                    if (config.Ibet2 != null)
                    {
                        this.iNewR2IB.Enabled = true;
                    }
                }
            }
        }

        private void InitCoreSbobetEngine()
        {
            if (_sbobetEngine != null && _sbobetEngine.sboAgent.State == AgentState.Running)
            {
                this._sbobetEngine.sboAgent.Logout();
                this.lblSbobetCurrentCredit.Caption = "-";
                this.iNewR.Enabled = false;
            }
            else
            {
                BCCore.BetAccount sboAccount = new BCCore.BetAccount();
                sboAccount.Account = this.config.Sbo.Account;
                sboAccount.Password = this.config.Sbo.Password;
                sboAccount.Website = this.config.Sbo.Website;

                BCCore.Utis.EngineLogger logger = new BCCore.Utis.EngineLogger("log");
                this._sbobetEngine = new SbobetEngine(sboAccount, logger, "Engine2");

                BCCore.LoginStatus loginStatus = this._sbobetEngine.sboAgent.Login();
                if (loginStatus == BCCore.LoginStatus.OK)
                {
                    this.rpgSbobet.Text = "SBO - " + sboAccount.Account;                                        
                    this._sbobetEngine.FullDataCompleted += new EngineDelegate(this._sbobetEngine_FullDataCompleted);
                    this._sbobetEngine.sboAgent.RefreshCredit();
                    this.lblSbobetCurrentCredit.Caption = this._sbobetEngine.sboAgent.Config.Balance.ToString();
                    this.btnStart.Enabled = true;
                    this.iNewR.Enabled = true;
                    this.btnSbobetGetInfo.Caption = "Log out";
                }
            }
        }        
        
        private void _sbobetEngine_FullDataCompleted(BaseEngine sender, EngineEventArgs eventArgs)
        {
            switch (eventArgs.Type)
            {
                case eEngineEventType.Success:
                    {                        
                        this.StartCompareSameMatch();
                        this.lblSbobetLastUpdate.Caption = System.DateTime.Now.ToString();
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
                        
                        this.StartCompareSameMatch();
                        //this._listIBETMatch = BaseDTO.DeepClone<System.Collections.Generic.List<MatchDTO>>((System.Collections.Generic.List<MatchDTO>)eventArgs.Data);
                        this.lblIbetLastUpdate.Caption = System.DateTime.Now.ToString();                        
                        //this.lblSbobetLastUpdate.Caption = System.DateTime.Now.ToString();
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
                        this.lblIbetLastUpdate.Caption = System.DateTime.Now.ToString();
                        this.StartCompareSameMatch();
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
        private void AddOddToLocalCommunity(MatchDTO ibetmatch, MatchDTO sbobetmatch, eOddType oddtype, string ibetodd, string sbobetodd, string ibetoddType, string sbobetoddType, string ibetoddValue, string sbobetoddValue, bool homeFavor)
        {
            this._mainForm.AddLocalValidOdd(this._ibetAccount, ibetmatch, sbobetmatch, oddtype, ibetodd, sbobetodd, ibetoddType, sbobetoddType, ibetoddValue, sbobetoddValue, homeFavor);
            if (this._mainForm.chkSCloud.Checked)
                this._mainForm.AddOddToClound(this._ibetAccount, ibetmatch, sbobetmatch, oddtype, ibetodd, sbobetodd, ibetoddType, sbobetoddType, ibetoddValue, sbobetoddValue, homeFavor);
        }
        internal void GetBetList(out string string1,out string string2)
        {
            string1 = string.Empty;
            string2 = string.Empty;
            try
            {
                string1 = this._ibetEngine.GetBetListMini();
                string2 = this._sbobetEngine.GetBetList();
            }
            catch (Exception ex)
            {
                ShowWarningDialog("Error: " + ex);
            }
        }
        private void SendReportToMainForm(string report)
        {
            this._mainForm.WriteReport(report);
        }
        internal void GetOddFromLocalCommunity(string ibetAccount, MatchDTO ibetmatch, MatchDTO sbobetmatch, eOddType oddtype, string ibetOdd, string sbobetOdd, string ibetoddType, string sbobetoddType, string ibetoddValue, string sbobetoddValue, bool homeFavor)
        {
            if (!checkEdit15.Checked && (checkEdit14.Checked && ibetAccount != this._ibetAccount && this._running && !this._betting && this._listSameMatch != null))
            {
                try
                {
                    TransactionDTO transactionDTO;
                    //phai tim betID trong list vi khong giong nhau cho tung user
                    string sbobetoddID = string.Empty;
                    string ibetoddID = string.Empty;
                    System.Collections.Generic.List<MatchDTO> listSbobetMatch = this._listSbobetMatch;
                    System.Collections.Generic.List<MatchDTO> listIbetMatch = this._listIBETMatch;
                    MatchDTO matchDTO = MatchDTO.SearchMatch(ibetmatch, listSbobetMatch);
                    if (matchDTO != null && matchDTO.Odds.Count > 0)
                    {
                        OddDTO oddDTO = OddDTO.SearchOdd(oddtype, sbobetOdd, matchDTO.Odds);
                        if (oddDTO != null)
                        {
                            sbobetoddID = oddDTO.ID;
                            MatchDTO matchDTO2 = MatchDTO.SearchMatch(ibetmatch, listIbetMatch);
                            if (matchDTO2 != null && matchDTO.Odds.Count > 0)
                            {
                                OddDTO oddDTO2 = OddDTO.SearchOdd(oddtype, ibetOdd, matchDTO2.Odds);
                                if (oddDTO2 != null)
                                {
                                    ibetoddID = oddDTO2.ID;
                                    if (this.AllowOddBet(ibetoddID,""))
                                    {
                                        if (this.chbRandomStake.Checked)
                                        {
                                            int num = 0;
                                            while (num == 0)
                                            {
                                                string strNum = this.txtStake.Lines[new System.Random().Next(this.txtStake.Lines.Length)];
                                                int.TryParse(strNum, out num);
                                            }
                                            int ibetStake = num;
                                            int sbobetStake = num;

                                            transactionDTO = this.PlaceBetAllowMaxBet(false, ibetmatch, matchDTO, oddtype, ibetoddID, sbobetoddID, ibetoddType, sbobetoddType, ibetoddValue, sbobetoddValue.ToString(), ibetStake, sbobetStake, homeFavor, this._ibetEngine, this._sbobetEngine, sbobetOdd);
                                            transactionDTO.OddType = oddtype + " - " + ibetoddType + " / " + sbobetoddType;
                                            this.AddTransaction(transactionDTO);
                                        }
                                        else
                                        {
                                            int num = 0;
                                            while (num == 0)
                                            {
                                                string strNum = this.txtStake.Lines[new System.Random().Next(this.txtStake.Lines.Length)];
                                                int.TryParse(strNum, out num);
                                            }

                                            int ibetStake = num;
                                            int sbobetStake = (int)Math.Round(num * ((float)this.txtSBOBETFixedStake.Value / (float)this.txtIBETFixedStake.Value));

                                            transactionDTO = this.PlaceBet(false, ibetmatch, matchDTO, oddtype, ibetoddID, sbobetoddID, ibetoddType, sbobetoddType, ibetoddValue, sbobetoddValue.ToString(), ibetStake, sbobetStake, this._ibetEngine, this._sbobetEngine, sbobetOdd, homeFavor);
                                            transactionDTO.OddType = oddtype + " - " + ibetoddType + " / " + sbobetoddType;
                                            this.AddTransaction(transactionDTO);
                                        }
                                        if (transactionDTO != null && transactionDTO.IBETTrade)
                                        {
                                            this.UpdateOddBetHistory(ibetoddID,"");
                                        }
                                    }
                                }
                                else
                                    SendReportToMainForm(this._ibetAccount + "-" + this._sbobetAccount + " >> " + ibetmatch.HomeTeamName + " - " + ibetmatch.AwayTeamName + " : can not find odd " + sbobetOdd + "in IBET list");
                            }
                            else
                                SendReportToMainForm(this._ibetAccount + " match not found in ibet list");                            
                        }
                        else
                            SendReportToMainForm(this._ibetAccount + "-" + this._sbobetAccount + " >> " + ibetmatch.HomeTeamName + " - " + ibetmatch.AwayTeamName + " : can not find odd " + sbobetOdd + "in SBOBET list");
                    }
                    else
                        SendReportToMainForm(this._ibetAccount + "-" + this._sbobetAccount + " >> " + ibetmatch.HomeTeamName + " - " + ibetmatch.AwayTeamName + ": match not found in sbobet list");
                    
                }
                catch (Exception ex)
                {
                    //ShowWarningDialog("Error: " + ex);
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
            lock (this._listSameMatch)
            {
                System.Collections.Generic.List<MatchDTO> listIBETMatch = this._listIBETMatch;
                System.Collections.Generic.List<MatchDTO> listSbobetMatch = this._listSbobetMatch;

                if (this._listSameMatch != null)
                {
                    this._listSameMatch.Clear();
                }
                else
                {
                    this._listSameMatch = new System.Collections.Generic.List<MatchDTO>();
                }

                if (scantype == ScanningType.both)
                {
                    #region BuonComIBETSBO
                    Dictionary<string, IbetMatch> iMatchs = this._ibetEngine.ibetAgent.parser.LdicMatches[0];
                    Dictionary<string, SboMatch> sMatchs = this._sbobetEngine.sboAgent.parserLive.LdicMatches[0];
                    //var i = iMatchs.Values.
                    foreach (KeyValuePair<string, IbetMatch> iM in iMatchs)
                    {
                        foreach (KeyValuePair<string, SboMatch> sM in sMatchs)
                        {
                            if ((iM.Value.Home == sM.Value.home) || (iM.Value.Away == sM.Value.away))
                            {
                                MatchDTO matchDTO = new MatchDTO();
                                matchDTO.ID = iM.Value.MatchId;
                                matchDTO.AwayTeamName = iM.Value.Away + " / " + sM.Value.away;
                                matchDTO.HomeTeamName = iM.Value.Home + " / " + sM.Value.home;
                                matchDTO.Minute = iM.Value.Minute;
                                matchDTO.HomeScore = iM.Value.ScoreH.ToString();
                                matchDTO.AwayScore = iM.Value.ScoreA.ToString();
                                if (iM.Value.Period == 0)
                                    matchDTO.IsHalfTime = true;
                                else if (iM.Value.Period == 1)
                                    matchDTO.Half = 1;
                                else if (iM.Value.Period == 2)
                                    matchDTO.Half = 2;

                                LeagueDTO leagueDTO = new LeagueDTO();
                                leagueDTO.Name = iM.Value.LeagueName + " / " + sM.Value.leagueName;
                                leagueDTO.ID = iM.Value.LeagueId;
                                matchDTO.League = leagueDTO;

                                foreach (KeyValuePair<string, SboOdd> sO in sM.Value.dicOdds)
                                {
                                    foreach (KeyValuePair<string, IbetOdd> iO in iM.Value.dicOdds)
                                    {
                                        if (sO.Value.oddType == iO.Value.oddType)
                                        {
                                            if (sO.Value.home + iO.Value.away == -0.01m || sO.Value.home + iO.Value.away == 0)
                                            {
                                                //iBet.Utilities.WriteLog.Write("Odd Found:iH sA: " + iM.Value.Home + "/" + sM.Value.home + "-" + iM.Value.Away + "/" + sM.Value.away +
                                                //    " >> ibet Odd: " + iO.Value.home + "/" + iO.Value.away + " >> sbo Odd:" + sO.Value.home + "/" + sO.Value.away +
                                                //    " >> " + iO.Value.oddType + ":" + sO.Value.oddType);
                                                //BetObject betObject = new BetObject();
                                                //betObject.ibet = new Bet();

                                                //this._ibetEngine.ibetAgent.CheckOdds

                                                TransactionDTO transactionDTO = new TransactionDTO();
                                            }
                                            if (sO.Value.away + iO.Value.home == -0.01m || sO.Value.away + iO.Value.home == 0)
                                            {
                                                //iBet.Utilities.WriteLog.Write("Odd Found:iA sH: " + iM.Value.Home + "/" + sM.Value.home + "-" + iM.Value.Away + "/" + sM.Value.away +
                                                //    " >> ibet Odd: " + iO.Value.home + "/" + iO.Value.away + " >> sbo Odd:" + sO.Value.home + "/" + sO.Value.away +
                                                //    " >> " + iO.Value.oddType + ":" + sO.Value.oddType);
                                            }
                                        }
                                    }
                                }
                                this._listSameMatch.Add(matchDTO);
                            }
                        }
                    }
                    System.DateTime now = System.DateTime.Now;
                    System.TimeSpan timeSpan;

                    System.DateTime now2 = System.DateTime.Now;
                    timeSpan = now2 - now;
                    double totalMilliseconds = timeSpan.TotalMilliseconds;
                    BarItem arg_648_0 = this.lblSbobetTotalMatch;
                    int count = sMatchs.Count;
                    arg_648_0.Caption = count.ToString();
                    BarItem arg_663_0 = this.lblIbetTotalMatch;
                    count = iMatchs.Count;
                    arg_663_0.Caption = count.ToString();
                    this.lblSameMatch.Caption = "Total Same Match: " + this._listSameMatch.Count;
                    this.lblLastUpdate.Caption = System.DateTime.Now.ToString();
                    #endregion
                }
                else if (scantype == ScanningType.ibet || scantype == ScanningType.ibetVSibet)
                {                    
                    Dictionary<string, IbetMatch> iMatchs1 = new Dictionary<string, IbetMatch>();
                    Dictionary<string, IbetMatch> iMatchs0 = new Dictionary<string, IbetMatch>();
                    if (checkEdit5.Checked)//live
                        iMatchs0 = this._ibetEngine.ibetAgent.parser.LdicMatches[0];
                    if (checkEdit6.Checked)//non live
                        iMatchs1 = this._ibetEngine.ibetAgent.parser.LdicMatches[1];

                    this._ibetMatchs = iMatchs1.Concat(iMatchs0).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);
                    lock (this._ibetMatchs)
                    {
                        foreach (KeyValuePair<string, IbetMatch> iM in _ibetMatchs)
                        {
                            #region SAMEWORD_PREPARE_MATCHLIST
                            MatchDTO matchDTO = new MatchDTO();
                            matchDTO.ID = iM.Value.MatchId;
                            matchDTO.AwayTeamName = iM.Value.Away;
                            matchDTO.HomeTeamName = iM.Value.Home;
                            //matchDTO.KickOffTime = iM.Value.KickOffTime;
                            string formatString = "yyyyMMddHHmm";
                            //string sample = "201006112219";
                            DateTime dt = DateTime.ParseExact(iM.Value.KickOffTime, formatString, null);
                            matchDTO.KickOffTime = dt.ToString("dd/MM HH:mm");
                            DateTime ct = DateTime.Parse(this._ibetEngine.ibetAgent.CT);
                            TimeSpan ts = dt.Subtract(ct);//kick off time - current time
                            double tic = ts.TotalSeconds;
                            if (tic <= 300 && tic > 0)
                            {
                                matchDTO.KickOffTime += " - " + ts.Minutes.ToString() + " mins to start";
                            }
                            else if (tic < 0)
                            {
                                matchDTO.KickOffTime += " !Live";
                            }

                            matchDTO.Minute = iM.Value.Minute;
                            matchDTO.HomeScore = iM.Value.ScoreH.ToString();
                            matchDTO.AwayScore = iM.Value.ScoreA.ToString();

                            if (iM.Value.Period == 0)
                                matchDTO.IsHalfTime = true;
                            else if (iM.Value.Period == 1)
                                matchDTO.Half = 1;
                            else if (iM.Value.Period == 2)
                                matchDTO.Half = 2;

                            LeagueDTO leagueDTO = new LeagueDTO();
                            leagueDTO.Name = iM.Value.LeagueName;
                            leagueDTO.ID = iM.Value.LeagueId;
                            matchDTO.League = leagueDTO;

                            this._listSameMatch.Add(matchDTO);
                            int num = 0;
                            if (this.chbRandomStake.Checked)
                            {
                                while (num == 0)
                                {
                                    string strNum = this.txtStake.Lines[new System.Random().Next(this.txtStake.Lines.Length)];
                                    int.TryParse(strNum, out num);
                                }
                            }

                            #endregion

                            if (scantype == ScanningType.ibetVSibet)
                            {
                                #region IBET_vs_IBET
                                if (leagueDTO.Name.Contains("SPECIFIC 15 MINS OVER/UNDER"))
                                {
                                    if (((System.DateTime.Now - this._lastTransactionTime).Seconds > 8) && !this._betting)
                                    {
                                        if (matchDTO.HomeTeamName.Contains("30:01-45:00") || matchDTO.HomeTeamName.Contains("75:01-90:00"))
                                        {
                                            if (!matchDTO.KickOffTime.Contains("Live"))
                                            {
                                                foreach (KeyValuePair<string, IbetOdd> iO in iM.Value.dicOdds)
                                                {
                                                    if (iO.Value.oddType == 3)//keo OU
                                                    {
                                                        TransactionDTO transactionDTO;
                                                        string strOU = "a";
                                                        if (checkEdit2.Checked)
                                                        {
                                                            strOU = "h";
                                                        }
                                                        if (matchDTO.HomeTeamName.Contains("30:01-45:00"))
                                                        {
                                                            if (checkEdit17.Checked)//allow Half 1
                                                            {
                                                               
                                                                transactionDTO = PlaceSingleIBET("Over15_Fang", iO.Value.home.ToString(),
                                                                    matchDTO, iO.Value.oddsId, iO.Value.hdp.ToString(), eOddType.FulltimeOverUnder, strOU,
                                                                    num.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent.Config.Account);
                                                                this.AddTransaction(transactionDTO);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (checkEdit18.Checked) //allow half 2
                                                            {
                                                                transactionDTO = PlaceSingleIBET("Over15_Fang", iO.Value.home.ToString(),
                                                                matchDTO, iO.Value.oddsId, iO.Value.hdp.ToString(), eOddType.FulltimeOverUnder, strOU,
                                                                num.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent.Config.Account);
                                                                this.AddTransaction(transactionDTO);
                                                            }
                                                        }
                                                        
                                                    }
                                                }
                                            }
 
                                        }
                                    } 
                                }
                                #endregion
                            }
                            else if (scantype == ScanningType.ibet)
                            {
                                #region IBET_Strategies
                                foreach (KeyValuePair<string, IbetOdd> iO in iM.Value.dicOdds)
                                {
                                    if (((System.DateTime.Now - this._lastTransactionTime).Seconds > 8) && !this._betting)
                                    {
                                        if (checkEdit12.Checked) // under strategy
                                        {
                                            if (iO.Value.oddType == 3 && iO.Value.home == iO.Value.away)
                                            {
                                                if (checkEdit9.Checked)
                                                {
                                                    if (!matchDTO.KickOffTime.Contains("Live"))
                                                    {
                                                        TransactionDTO transactionDTO = PlaceSingleIBET("Under", iO.Value.home.ToString(),
                                                        matchDTO, iO.Value.oddsId, iO.Value.hdp.ToString(), eOddType.FulltimeOverUnder, "a",
                                                        num.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent.Config.Account);
                                                        this.AddTransaction(transactionDTO);
                                                    }
                                                }
                                                else
                                                {
                                                    TransactionDTO transactionDTO = PlaceSingleIBET("Under", iO.Value.home.ToString(),
                                                        matchDTO, iO.Value.oddsId, iO.Value.hdp.ToString(), eOddType.FulltimeOverUnder, "a",
                                                        num.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent.Config.Account);
                                                    this.AddTransaction(transactionDTO);
                                                }
                                            }
                                        }
                                        if (checkEdit8.Checked) //over strategy
                                        {
                                            if (iO.Value.oddType == 3 && iO.Value.home == iO.Value.away)
                                            {
                                                TransactionDTO transactionDTO = PlaceSingleIBET("Over", iO.Value.home.ToString(),
                                                    matchDTO, iO.Value.oddsId, iO.Value.hdp.ToString(), eOddType.FulltimeOverUnder, "h",
                                                    num.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent.Config.Account);
                                                this.AddTransaction(transactionDTO);
                                            }
                                        }
                                        if (checkEdit7.Checked) // Over 92/90
                                        {
                                            if (matchDTO.KickOffTime.Contains("Live"))
                                            {
                                                if (iO.Value.oddType == 3 || iO.Value.oddType == 8)
                                                {
                                                    if (iO.Value.hdp - ((decimal)iM.Value.ScoreH + (decimal)iM.Value.ScoreA) == (decimal)0.5)
                                                    {
                                                        if (iM.Value.Home.Contains("No. of Corners") && iO.Value.home == (decimal)0.90 && iO.Value.away == (decimal)0.92)
                                                        {
                                                            TransactionDTO transactionDTO = PlaceSingleIBET("Over9290", iO.Value.home.ToString(),
                                                            matchDTO, iO.Value.oddsId, iO.Value.hdp.ToString(), eOddType.FulltimeOverUnder, "h",
                                                            num.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent.Config.Account);
                                                            this.AddTransaction(transactionDTO);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (checkEdit9.Checked) // Odd down: Sap keo
                                        {
                                            if (matchDTO.KickOffTime.Contains("mins to start"))
                                            {
                                                if (iO.Value.oddType == 3)
                                                {
                                                    List<Bet> bl = this._ibetEngine.ibetAgent.betList;
                                                    lock (bl)
                                                    {
                                                        foreach (Bet bet in bl)
                                                        {
                                                            if (bet.Home == matchDTO.HomeTeamName && bet.Away == matchDTO.AwayTeamName)
                                                            {
                                                                iBet.Utilities.WriteLog.Write("Sap Keo:: Tim thay keo trong bet list trong tran : " + bet.Home + " - " + bet.Away);
                                                                if (bet.Handicap == iO.Value.hdp)
                                                                {
                                                                    if (iO.Value.away < 0 || iO.Value.away - bet.OddsValue >= (decimal)0.05)
                                                                    {
                                                                        iBet.Utilities.WriteLog.Write("Sap Keo:: Keo bi sap " + (iO.Value.away - bet.OddsValue).ToString() + " gia. Go to bet over");
                                                                        //keo under sap xuo^'ng an cao hon
                                                                        TransactionDTO transactionDTO = PlaceSingleIBET("SapKeo", iO.Value.home.ToString(),
                                                                            matchDTO, iO.Value.oddsId, iO.Value.hdp.ToString(), eOddType.FulltimeOverUnder, "h",
                                                                            num.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent.Config.Account);
                                                                        this.AddTransaction(transactionDTO);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (chbHighRevenueBoost.Checked) // Best analysed strategy
                                        {
                                            if (!leagueDTO.Name.Contains("Cup") && !leagueDTO.Name.Contains("CUP"))
                                            {
                                                if (iO.Value.oddType == 3 && iO.Value.home == iO.Value.away)
                                                {
                                                    OddDTO oddDTO = new OddDTO();
                                                    oddDTO.Odd = iO.Value.hdp.ToString();
                                                    string s = CheckBestStrategyValidation(matchDTO, oddDTO);
                                                    if (s == "nguoc")
                                                    {
                                                        TransactionDTO transactionDTO = PlaceSingleIBET("nguoc", iO.Value.home.ToString(),
                                                        matchDTO, iO.Value.oddsId, iO.Value.home.ToString(), eOddType.FulltimeOverUnder, "h",
                                                        num.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent.Config.Account);
                                                        this.AddTransaction(transactionDTO);
                                                        //Console.Write("nguoc");
                                                    }
                                                    else if (s == "xuoi")
                                                    {
                                                        TransactionDTO transactionDTO = PlaceSingleIBET("xuoi", iO.Value.home.ToString(),
                                                        matchDTO, iO.Value.oddsId, iO.Value.home.ToString(), eOddType.FulltimeOverUnder, "h",
                                                        num.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent.Config.Account);
                                                        this.AddTransaction(transactionDTO);
                                                        //Console.Write("xuoi");
                                                    }
                                                }
                                            }
                                        }
                                        if (checkEdit11.Checked)//fair odd
                                        {
                                            if (!leagueDTO.Name.Contains("Cup") && !leagueDTO.Name.Contains("CUP"))
                                            {
                                                if (iM.Value.Minute >= 35 && iM.Value.Period == 1 && iM.Value.ScoreA == iM.Value.ScoreH)
                                                {
                                                    iBet.Utilities.WriteLog.Write("01:Found fair odd: " + iM.Value.Home + "-" + iM.Value.Away + ":" + iM.Value.Minute + "m");
                                                    if ((iO.Value.home > (decimal)0.9)
                                                        && iO.Value.oddType == 3
                                                        && (iO.Value.hdp - (decimal)(iM.Value.ScoreH + iM.Value.ScoreA) == (decimal)1.75))
                                                    {
                                                        iBet.Utilities.WriteLog.Write("02:Found fair odd correct");
                                                        foreach (KeyValuePair<string, IbetMatch> snapshotMacht in _ibetMatchsSnapShot)
                                                        {
                                                            if (snapshotMacht.Value.Home == iM.Value.Home && snapshotMacht.Value.Away == iM.Value.Away)
                                                            {
                                                                iBet.Utilities.WriteLog.Write("03:Found match in Snapshot");
                                                                foreach (KeyValuePair<string, IbetOdd> snapshotOdd in snapshotMacht.Value.dicOdds)
                                                                {
                                                                    if (snapshotOdd.Value.oddType == 3
                                                                        && snapshotOdd.Value.hdp >= (decimal)2.5
                                                                        && (snapshotOdd.Value.home >= (decimal)0.92 || snapshotOdd.Value.home < (decimal)0))
                                                                    {
                                                                        iBet.Utilities.WriteLog.Write("04:Found odd over 2.5 correct in Snapshot");
                                                                        if (snapshotOdd.Value.oddType == 1 && snapshotOdd.Value.hdp <= (decimal)0.75)
                                                                        {
                                                                            iBet.Utilities.WriteLog.Write("05:Found handicap correct in Snapshot, go to Bet");
                                                                            TransactionDTO transactionDTO = PlaceSingleIBET("Over1.75", iO.Value.home.ToString(),
                                                                            matchDTO, iO.Value.oddsId, iO.Value.home.ToString(), eOddType.FulltimeOverUnder, "h",
                                                                            num.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent.Config.Account);
                                                                            this.AddTransaction(transactionDTO);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }                            
                        }
                    }
                    if (scantype == ScanningType.ibetVSibet)
                    {
                        #region IBET_vs_IBET_continue
                        
                        Dictionary<string, IbetMatch> iMatchs2 = new Dictionary<string, IbetMatch>();         
                        iMatchs2 = this._ibetEngine.ibetAgent.parser.LdicMatches[0];
                        //this._ibetEngine.ibetAgent.RefreshBetList();
                        lock (iMatchs2)
                        {
                            foreach (KeyValuePair<string, IbetMatch> iM2 in _ibetMatchs)
                            {                                
                                if ((iM2.Value.Period == 1 && iM2.Value.Minute >= (int)spinEdit1.Value) || (iM2.Value.Period == 2 && iM2.Value.Minute >= (int)spinEdit2.Value)) // chi chon nhung tran p31 cua hiep 1
                                {
                                    foreach (KeyValuePair<string, IbetOdd> iO2 in iM2.Value.dicOdds)
                                    {
                                        if (((System.DateTime.Now - this._lastTransactionTime).Seconds > 8) && !this._betting)
                                        {
                                            if (iO2.Value.oddType == 8 || iO2.Value.oddType == 3)
                                            {
                                                if (iO2.Value.hdp - ((decimal)iM2.Value.ScoreH + (decimal)iM2.Value.ScoreA) == (decimal)0.5)
                                                {
                                                    bl = this._ibetEngine.ibetAgent.betList;
                                                    foreach (Bet bet in bl)
                                                    {
                                                        if (bet.Home.Contains("30:01-45:00") || bet.Home.Contains("75:01-90:00"))
                                                        {
                                                            string HomeTeam = "";
                                                            if (iO2.Value.oddType == 8)
                                                            {
                                                                HomeTeam = bet.Home.Replace(" 30:01-45:00", "");

                                                            }
                                                            else
                                                            {
                                                                HomeTeam = bet.Home.Replace(" 75:01-90:00", "");
                                                            }
                                                            

                                                            if (HomeTeam == iM2.Value.Home.Replace("(N)", "").TrimEnd())
                                                            {                                                                    
                                                                MatchDTO matchDTO = new MatchDTO();
                                                                matchDTO.HomeTeamName = iM2.Value.Home.Replace("(N)", "").TrimEnd();
                                                                matchDTO.AwayTeamName = iM2.Value.Away.Replace("(N)","").TrimEnd();
                                                                matchDTO.Minute = iM2.Value.Minute;
                                                                matchDTO.HomeScore = iM2.Value.ScoreH.ToString();
                                                                matchDTO.AwayScore = iM2.Value.ScoreA.ToString();
                                                                matchDTO.Half = iM2.Value.Period;

                                                                int num = (int)bet.Stake;
                                                                int ExRate1 = (int)config.Ibet.ExchangeRate;
                                                                int ExRate2 = (int)config.Ibet2.ExchangeRate;
                                                                float stake2 = num * ExRate1 / ExRate2;

                                                                decimal giatri1;
                                                                string OU = "h";

                                                                if (bet.Choice == Choice.H)
                                                                {
                                                                    giatri1 = iO2.Value.away;
                                                                    OU = "a";
                                                                }
                                                                else
                                                                {
                                                                    giatri1 = iO2.Value.home;
                                                                }

                                                                //iBet.Utilities.WriteLog.Write("Tim thay tran :" + bet.Home + " -vs- " + bet.Away +
                                                                //    ", o phut thu:" + matchDTO.Minute.ToString() + ", hiep " + matchDTO.Half.ToString() +
                                                                //    "chuan bi xa..");                                                                
                                                                eOddType oddtype;

                                                                if ((giatri1 > 0 && bet.OddsValue + giatri1 >= spinEdit4.Value) || giatri1 < 0)
                                                                {
                                                                    if (checkEdit1.Checked)
                                                                    {
                                                                        
                                                                        string textx = "";
                                                                        if (bet.Home.Contains("30:01-45:00"))
                                                                        {
                                                                            textx = "Over15_XA > 30-45 > Xa loi gia > Min:" + matchDTO.Minute + "half " + matchDTO.Half.ToString() + " > " + bet.OddsValue.ToString() + "/" + giatri1.ToString() + " > Ref ID:" + bet.Id;
                                                                            oddtype = eOddType.FirstHalfOverUnder;
                                                                        }
                                                                        else
                                                                        {
                                                                            textx = "Over15_XA > 75-90 > Xa loi gia > Min:" + matchDTO.Minute + "half " + matchDTO.Half.ToString() + " > " + bet.OddsValue.ToString() + "/" + giatri1.ToString() + " > Ref ID:" + bet.Id;
                                                                            oddtype = eOddType.FulltimeOverUnder;
                                                                        }
                                                                        TransactionDTO transactionDTO = PlaceSingleIBET(textx, giatri1.ToString(),
                                                                            matchDTO, iO2.Value.oddsId, iO2.Value.hdp.ToString(), oddtype, OU,
                                                                            stake2.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent2.Config.Account);
                                                                        this.AddTransaction(transactionDTO);
                                                                        iBet.Utilities.WriteLog.Write(textx);
                                                                    }
                                                                }
                                                                else 
                                                                {
                                                                    string textx = "";
                                                                    if (bet.Choice == Choice.H && matchDTO.Minute >= 30)
                                                                    {
                                                                        
                                                                        if (bet.Home.Contains("30:01-45:00"))
                                                                        {
                                                                            textx = "Over15_XA > 30-45 > Xa thuong > Min:" + matchDTO.Minute + " > " + bet.OddsValue.ToString() + "/" + iO2.Value.away.ToString() + " > Ref ID:" + bet.Id;
                                                                            oddtype = eOddType.FirstHalfOverUnder;
                                                                        }
                                                                        else
                                                                        {
                                                                            textx = "Over15_XA > 75-90 > Xa thuong > Min:" + matchDTO.Minute + " > " + bet.OddsValue.ToString() + "/" + iO2.Value.away.ToString() + " > Ref ID:" + bet.Id;
                                                                            oddtype = eOddType.FulltimeOverUnder;
                                                                        }
                                                                        TransactionDTO transactionDTO = PlaceSingleIBET(textx, iO2.Value.away.ToString(),
                                                                            matchDTO, iO2.Value.oddsId, iO2.Value.hdp.ToString(), oddtype, OU,
                                                                            stake2.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent2.Config.Account);
                                                                        this.AddTransaction(transactionDTO);
                                                                        //iBet.Utilities.WriteLog.Write(textx);
                                                                    }
                                                                    else if (bet.Choice == Choice.A && matchDTO.Minute >= 34)
                                                                    {
                                                                        if (bet.Home.Contains("30:01-45:00"))
                                                                        {
                                                                            textx = "Over15_XA > 30-45 > Xa thuong > Min:" + matchDTO.Minute + " > " + bet.OddsValue.ToString() + "/" + iO2.Value.away.ToString() + " > Ref ID:" + bet.Id;
                                                                            oddtype = eOddType.FirstHalfOverUnder;
                                                                        }
                                                                        else
                                                                        {
                                                                            textx = "Over15_XA > 75-90 > Xa thuong > Min:" + matchDTO.Minute + " > " + bet.OddsValue.ToString() + "/" + iO2.Value.away.ToString() + " > Ref ID:" + bet.Id;
                                                                            oddtype = eOddType.FulltimeOverUnder;
                                                                        }
                                                                        
                                                                        TransactionDTO transactionDTO = PlaceSingleIBET(textx, iO2.Value.away.ToString(),
                                                                            matchDTO, iO2.Value.oddsId, iO2.Value.hdp.ToString(), oddtype, OU,
                                                                            stake2.ToString(), matchDTO.HomeScore, matchDTO.AwayScore, this._ibetEngine, true, "", this._ibetEngine.ibetAgent2.Config.Account);
                                                                        this.AddTransaction(transactionDTO); 
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }                                                    
                                                }
                                            }
                                        }
                                    }                                     
                                }
                            }
                        }
                        #endregion
                    }
                    BarItem arg_663_0 = this.lblIbetTotalMatch;
                    int count = _ibetMatchs.Count;
                    arg_663_0.Caption = count.ToString() + " (" + iMatchs0.Count.ToString() + " live)";
                    this.lblLastUpdate.Caption = System.DateTime.Now.ToString();
                    this.rpgIbet.Text = "IBET - " + this._ibetEngine.ibetAgent.Config.Account + " " + this._ibetEngine.ibetAgent.CT;
                    
                }
                else if (scantype == ScanningType.sbo)
                {
                    #region SBO_Strategies
                    Dictionary<string, SboMatch> sMatchs0 = this._sbobetEngine.sboAgent.parserLive.dicMatches;
                    Dictionary<string, SboMatch> sMatchs1 = this._sbobetEngine.sboAgent.parserNonlive.dicMatches;
                    Dictionary<string, SboMatch> sMatchs = sMatchs1.Concat(sMatchs0).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);
                    foreach (KeyValuePair<string, SboMatch> sM in sMatchs)
                    {
                        MatchDTO matchDTO = new MatchDTO();
                        matchDTO.ID = sM.Value.matchId;
                        matchDTO.AwayTeamName = sM.Value.away;
                        matchDTO.HomeTeamName = sM.Value.home;

                        matchDTO.AwayScore = sM.Value.awayscore.ToString();
                        matchDTO.HomeScore = sM.Value.homescore.ToString();
                        matchDTO.Minute = sM.Value.minute;
                        matchDTO.Half = sM.Value.half;
                        if (matchDTO.Half == 0)
                            matchDTO.IsHalfTime = true;

                        LeagueDTO leagueDTO = new LeagueDTO();
                        leagueDTO.Name = sM.Value.leagueName;
                        leagueDTO.ID = sM.Value.leagueId;
                        matchDTO.League = leagueDTO;

                        this._listSameMatch.Add(matchDTO);
                    }
                    BarItem arg_648_0 = this.lblSbobetTotalMatch;
                    int count = sMatchs.Count;
                    arg_648_0.Caption = count.ToString() + " (" + sMatchs0.Count.ToString() + " live)";
                    #endregion
                }
                else if (scantype == ScanningType.ibetVSibet)
                {
                    
                }
                lock (this.grdSameMatch)
                {
                    this.grdSameMatch.RefreshDataSource();
                }
                lock (girdBetList1)
                {
                    this.girdBetList1.RefreshDataSource();
                }
            }
            
            this._comparing = false;
            if (this._compareAgain && !this._comparing)
            {
                this._compareAgain = false;
                this.CompareSameMatch();
            }
        }
        private void LoadAnalyse()
        {
            this.listBA.Clear();
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString("");
                    string s = json.ToString().Replace("gdata.io.handleScriptLoaded(", "").Replace(");", "");
                    Newtonsoft.Json.JavaScriptObject jSObj = (Newtonsoft.Json.JavaScriptObject)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(s);
                    if (jSObj != null)
                    {
                        Newtonsoft.Json.JavaScriptObject jSObjFeed = (Newtonsoft.Json.JavaScriptObject)jSObj["feed"];
                        if (jSObjFeed != null)
                        {
                            Newtonsoft.Json.JavaScriptArray jSArrEntry = (Newtonsoft.Json.JavaScriptArray)jSObjFeed["entry"];
                            if (jSArrEntry != null)
                            {
                                using (System.Collections.Generic.List<object>.Enumerator enumerator = jSArrEntry.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        Newtonsoft.Json.JavaScriptObject objCurrent = (Newtonsoft.Json.JavaScriptObject)enumerator.Current;
                                        BetAnalyse betAnalyse = new BetAnalyse();
                                        Newtonsoft.Json.JavaScriptObject jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$odd"];
                                        betAnalyse.OddType = jsObj2["$t"].ToString();
                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$count"];
                                        betAnalyse.Count = int.Parse(jsObj2["$t"].ToString());
                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$won"];
                                        betAnalyse.Won = int.Parse(jsObj2["$t"].ToString());
                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$lose"];
                                        betAnalyse.Lose = int.Parse(jsObj2["$t"].ToString());
                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$draw"];
                                        betAnalyse.Draw = int.Parse(jsObj2["$t"].ToString());
                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$allow"];
                                        if (jsObj2["$t"].ToString() == "1")
                                            betAnalyse.Allow = true;
                                        else
                                            betAnalyse.Allow = false;
                                        listBA.Add(betAnalyse);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.ShowErrorDialog("Error while loading strategy. \nDetails: " + ex.Message);
            }
            if (this.listBA.Count > 0)
            {
                var max = listBA.Max(obj => obj.Diff);
                var min = listBA.Min(obj => obj.Diff);
                string s = "";
                foreach (BetAnalyse ba in listBA)
                {
                    if (ba.Diff >= (int)txtAddValue.Value && (ba.WinPercent >= 0.68 || ba.WinPercent <= 0.30 ))
                    {
                        ba.isGoodOddToBet = true;
                        s += ba.OddType + " with Under;\n";
                    }
                    if (ba.Diff <= -(int)txtAddValue.Value && (ba.WinPercent >= 0.68 || ba.WinPercent <= 0.30))
                    {
                        ba.isGoodOddToBet = true;
                        s += ba.OddType + " with Over;\n";
                    }
                }
                this.ShowWarningDialog("Found odd:\n" + s);
                iBet.Utilities.WriteLog.Write("Load Odd Analyse: Rank " + this.txtAddValue.Value.ToString() + "\n" + s);
            }            
        }
        private string CheckBestStrategyValidation(MatchDTO match, OddDTO odd)
        {
            string result = "";

            var listOddGoodToBet = from p in listBA
                        where p.isGoodOddToBet == true
                        select p;
            foreach (BetAnalyse ba in listOddGoodToBet)
            {
                string intHomeScore = "0";
                string intAwayScore = "0";
                string[] array4 = ba.OddType.Split(new string[] { " " }, System.StringSplitOptions.None);
                string OddValue = array4[1];

                if (ba.OddType.Contains("["))
                {
                    string[] array1 = ba.OddType.Split(new string[] { " [" }, System.StringSplitOptions.None);
                    string[] array2 = array1[1].Split(new string[] { "]" }, System.StringSplitOptions.None);
                    string[] array3 = array2[0].Split(new string[] { "-" }, System.StringSplitOptions.None);
                    intHomeScore = array3[0];
                    intAwayScore = array3[1];
                    if (!match.IsHalfTime && match.HomeScore == intHomeScore && match.AwayScore == intAwayScore)
                    {
                        if (odd.Odd == OddValue)
                        {
                            if (ba.Diff < 0)
                            {
                                iBet.Utilities.WriteLog.Write("Live:: Checking valid strategy: nguoc :: " + match.HomeTeamName + ":" + match.HomeScore
                                    + "-" + match.AwayScore + ":" + match.AwayTeamName + " ::: Odd of match: " + odd.Odd + " ::: Odd of strategy: " + ba.OddType);
                                return "nguoc";
                            }
                            else
                            {
                                iBet.Utilities.WriteLog.Write(" Checking valid strategy: xuoi :: " + match.HomeTeamName + ":" + match.HomeScore
                                    + "-" + match.AwayScore + ":" + match.AwayTeamName + " ::: Odd of match: " + odd.Odd + " ::: Odd of strategy: " + ba.OddType);
                                return "xuoi";
                            }
                        }
                    }
                }
                else
                {
                    if (match.IsHalfTime && match.HomeScore == "0" && match.AwayScore == "0" && odd.Odd == OddValue)
                    {
                        if (ba.Diff < 0)
                        {
                            iBet.Utilities.WriteLog.Write("NonLive:: Checking valid strategy: nguoc :: " + match.HomeTeamName + ":" + match.HomeScore
                                    + "-" + match.AwayScore + ":" + match.AwayTeamName + " ::: Odd of match: " + odd.Odd + " ::: Odd of strategy: " + ba.OddType);
                            return "nguoc";
                        }
                        else
                        {
                            iBet.Utilities.WriteLog.Write("NonLive:: Checking valid strategy: xuoi :: " + match.HomeTeamName + ":" + match.HomeScore
                                    + "-" + match.AwayScore + ":" + match.AwayTeamName + " ::: Odd of match: " + odd.Odd + " ::: Odd of strategy: " + ba.OddType);
                            return "xuoi";
                        }
                    }
                }
            }

            return result;
        }
        private void CompareSameMatchOld()
        {
            this._comparing = true;
            lock (this._listSameMatch)
            {
                System.Collections.Generic.List<MatchDTO> listIBETMatch = this._listIBETMatch;
                System.Collections.Generic.List<MatchDTO> listSbobetMatch = this._listSbobetMatch;
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
                foreach (MatchDTO current in listIBETMatch) // current = IBET
                {
                    MatchDTO matchDTO = MatchDTO.SearchMatch(current, listSbobetMatch);//matchDTO = SBOBET
                    if (matchDTO != null)
                    {
                        this._listSameMatch.Add(current);
                        bool NOTchkAllowMatches = true;
                        if (checkEdit10.Checked)
                        {
                            int numFound = chkListAllowedMatch.FindString(current.HomeTeamName + " - " + current.AwayTeamName);
                            NOTchkAllowMatches = numFound != -1 && chkListAllowedMatch.Items[numFound].CheckState.ToString() == "Checked";

                        }

                        if (NOTchkAllowMatches)
                        {

                            if (this.chbAllowHalftime.Checked || !current.IsHalfTime)
                            {
                                if (current.Minute <= (int)this.txtMaxTimePerHalf.Value)
                                {
                                    foreach (OddDTO current2 in current.Odds) //current2 = odd ibet
                                    {
                                        if (this.AllowOddBet(current2.ID,""))
                                        {
                                            OddDTO oddDTO = OddDTO.SearchOdd(current2, matchDTO.Odds);
                                            if (oddDTO != null)
                                            {
#if DEBUG
                                                Utilities.WriteLog.Write("Odd found: " + oddDTO.Odd.ToString());
#endif
                                                if ((checkEdit4.Checked && ((oddDTO.Type == eOddType.FirstHalfOverUnder || oddDTO.Type == eOddType.FulltimeOverUnder) && (current.Minute < 30 || (checkEdit7.Checked && current.Minute >= 30)))) || (checkEdit3.Checked && (oddDTO.Type == eOddType.FirstHalfHandicap || oddDTO.Type == eOddType.FulltimeHandicap)))
                                                {
                                                    if (current2.Home >= (float)this.txtLowestOddValue.Value || 0f - current2.Home >= (float)this.txtLowestOddValue.Value)
                                                    {
                                                        if (OddDTO.IsValidOddPair(current2.Home, oddDTO.Away, (double)this.txtOddValueDifferenet.Value, this.chbHighRevenueBoost.Checked))
                                                        {//IBET HOME - SBO AWAY
                                                            if (checkEdit15.Checked)//gui lenh pure odd
                                                            {
                                                                var taskA = new Task(() => this.AddOddToLocalCommunity(current, matchDTO, current2.Type, current2.Odd, oddDTO.Odd, "h", "a", current2.Home.ToString(), oddDTO.Away.ToString(), current2.HomeFavor));
                                                                taskA.Start();
                                                            }
                                                            else
                                                            {
                                                                TransactionDTO transactionDTO = new TransactionDTO();
                                                                if (this.chbRandomStake.Checked)
                                                                {
                                                                    int num = 0;
                                                                    while (num == 0)
                                                                    {
                                                                        string strNum = this.txtStake.Lines[new System.Random().Next(this.txtStake.Lines.Length)];
                                                                        int.TryParse(strNum, out num);
                                                                    }
                                                                    int ibetStake = num;
                                                                    int sbobetStake = num;
                                                                    MatchDTO ibetmatch = current;
                                                                    MatchDTO sbobetmatch = matchDTO;
                                                                    eOddType oddtype = current2.Type;
                                                                    string ibetoddID = current2.ID;
                                                                    string sbobetoddID = oddDTO.ID;
                                                                    string ibetoddType = "h";
                                                                    string sbobetoddType = "a";
                                                                    //float num2 = current2.Home;
                                                                    string ibetOddValuee = current2.Home.ToString();
                                                                    float sbobetOddValue = oddDTO.Away;
                                                                    //transactionDTO.OddType = current2.Type.ToString() + " - Home / Away";
                                                                    var taskA = new Task(() =>
                                                                        TransactionProcessMaxBet(ibetmatch, sbobetmatch, oddtype, ibetoddID, sbobetoddID, ibetoddType, sbobetoddType, ibetOddValuee, sbobetOddValue.ToString(), ibetStake, sbobetStake, current2.HomeFavor, this._ibetEngine, this._sbobetEngine, oddDTO.Odd)
                                                                        );
                                                                    taskA.Start();

                                                                }
                                                                else
                                                                {
                                                                    int num = 0;
                                                                    while (num == 0)
                                                                    {
                                                                        string strNum = this.txtStake.Lines[new System.Random().Next(this.txtStake.Lines.Length)];
                                                                        int.TryParse(strNum, out num);
                                                                    }

                                                                    int ibetStake = num;
                                                                    int sbobetStake = (int)Math.Round(num * ((float)this.txtSBOBETFixedStake.Value / (float)this.txtIBETFixedStake.Value));

                                                                    MatchDTO ibetmatch = current;
                                                                    MatchDTO sbobetmatch = matchDTO;
                                                                    eOddType oddtype = current2.Type;
                                                                    string ibetoddID = current2.ID;
                                                                    string sbobetoddID = oddDTO.ID;
                                                                    string ibetoddType = "h";
                                                                    string sbobetoddType = "a";
                                                                    string ibetOddValuee = current2.Home.ToString();
                                                                    float sbobetOddValue = oddDTO.Away;
                                                                    //transactionDTO = this.PlaceBet(ibetmatch, sbobetmatch, oddtype, ibetoddID, sbobetoddID, ibetoddType, sbobetoddType, ibetOddValuee, sbobetOddValue.ToString(), ibetStake, sbobetStake, this._ibetEngine, this._sbobetEngine);
                                                                    //transactionDTO.OddType = current2.Type.ToString() + " - Home / Away";
                                                                    //this.AddTransaction(transactionDTO);
                                                                    var taskA = new Task(() =>
                                                                        TransactionProcess(ibetmatch, sbobetmatch, oddtype, ibetoddID, sbobetoddID, ibetoddType, sbobetoddType, ibetOddValuee, sbobetOddValue.ToString(), ibetStake, sbobetStake, this._ibetEngine, this._sbobetEngine, oddDTO.Odd, current2.IsHomeGive)
                                                                        );
                                                                    taskA.Start();
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (current2.Away >= (float)this.txtLowestOddValue.Value || 0f - current2.Away >= (float)this.txtLowestOddValue.Value)
                                                    {
                                                        if (OddDTO.IsValidOddPair(current2.Away, oddDTO.Home, (double)this.txtOddValueDifferenet.Value, this.chbHighRevenueBoost.Checked))
                                                        {
                                                            if (checkEdit15.Checked)//gui lenh pure odd
                                                            {
                                                                var taskA = new Task(() => this.AddOddToLocalCommunity(current, matchDTO, current2.Type, current2.Odd, oddDTO.Odd, "h", "a", current2.Home.ToString(), oddDTO.Away.ToString(), current2.HomeFavor));
                                                                taskA.Start();
                                                            }
                                                            else
                                                            {
                                                                if (this.chbRandomStake.Checked)
                                                                {
                                                                    int num = 0;
                                                                    while (num == 0)
                                                                    {
                                                                        string strNum = this.txtStake.Lines[new System.Random().Next(this.txtStake.Lines.Length)];
                                                                        int.TryParse(strNum, out num);
                                                                    }
                                                                    int ibetStake = num;
                                                                    int sbobetStake = num;
                                                                    MatchDTO ibetmatch = current;
                                                                    MatchDTO sbobetmatch = matchDTO;
                                                                    eOddType oddtype = current2.Type;
                                                                    string ibetoddID = current2.ID;
                                                                    string sbobetoddID = oddDTO.ID;
                                                                    string ibetoddType = "a";
                                                                    string sbobetoddType = "h";
                                                                    float num2 = current2.Away;
                                                                    string ibetOddValuee = num2.ToString();
                                                                    string sbobetOddValue = oddDTO.Home.ToString();//sbobetodd value
                                                                    //transactionDTO = this.PlaceBetAllowMaxBet(ibetmatch, sbobetmatch, oddtype, ibetoddID, sbobetoddID, ibetoddType, sbobetoddType, ibetOddValuee, num2.ToString(), ibetStake, sbobetStake, current2.HomeFavor, this._ibetEngine, this._sbobetEngine, current2.Odd, oddDTO.Odd);
                                                                    //transactionDTO.OddType = current2.Type.ToString() + " - Away / Home";
                                                                    //this.AddTransaction(transactionDTO);
                                                                    var taskA = new Task(() =>
                                                                        TransactionProcessMaxBet(ibetmatch, sbobetmatch, oddtype, ibetoddID, sbobetoddID, ibetoddType, sbobetoddType, ibetOddValuee, sbobetOddValue, ibetStake, sbobetStake, current2.HomeFavor, this._ibetEngine, this._sbobetEngine, oddDTO.Odd)
                                                                        );
                                                                    taskA.Start();
                                                                }
                                                                else
                                                                {
                                                                    int num = 0;
                                                                    while (num == 0)
                                                                    {
                                                                        string strNum = this.txtStake.Lines[new System.Random().Next(this.txtStake.Lines.Length)];
                                                                        int.TryParse(strNum, out num);
                                                                    }
                                                                    //int ibetStake = (int)this.txtIBETFixedStake.Value;
                                                                    int ibetStake = num;
                                                                    int sbobetStake = (int)Math.Round(num * ((float)this.txtSBOBETFixedStake.Value / (float)this.txtIBETFixedStake.Value));

                                                                    MatchDTO ibetmatch = current;
                                                                    MatchDTO sbobetmatch = matchDTO;
                                                                    eOddType oddtype = current2.Type;
                                                                    string ibetoddID = current2.ID;
                                                                    string sbobetoddID = oddDTO.ID;
                                                                    string ibetoddType = "a";
                                                                    string sbobetoddType = "h";
                                                                    float num2 = current2.Away;
                                                                    string ibetOddValuee = num2.ToString();
                                                                    string sbobetOddValue = oddDTO.Home.ToString();//sbobet odd value
                                                                    //transactionDTO = this.PlaceBet(ibetmatch, sbobetmatch, oddtype, ibetoddID, sbobetoddID, ibetoddType, sbobetoddType, ibetOddValuee, num2.ToString(), ibetStake, sbobetStake, this._ibetEngine, this._sbobetEngine);
                                                                    //transactionDTO.OddType = current2.Type.ToString() + " - Away / Home";
                                                                    //this.AddTransaction(transactionDTO);
                                                                    var taskA = new Task(() =>
                                                                        TransactionProcess(ibetmatch, sbobetmatch, oddtype, ibetoddID, sbobetoddID, ibetoddType, sbobetoddType, ibetOddValuee, sbobetOddValue.ToString(), ibetStake, sbobetStake, this._ibetEngine, this._sbobetEngine, oddDTO.Odd, current2.IsHomeGive)
                                                                        );
                                                                    taskA.Start();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                System.DateTime now2 = System.DateTime.Now;
                timeSpan = now2 - now;
                double totalMilliseconds = timeSpan.TotalMilliseconds;
                BarItem arg_648_0 = this.lblSbobetTotalMatch;
                int count = listSbobetMatch.Count;
                arg_648_0.Caption = count.ToString();
                BarItem arg_663_0 = this.lblIbetTotalMatch;
                count = listIBETMatch.Count;
                arg_663_0.Caption = count.ToString();
                this.lblSameMatch.Caption = "Total Same Match: " + this._listSameMatch.Count;
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
        private void TerminalFormIBETSBO_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close?", "Bet Broker", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                if (this._ibetEngine != null && this._ibetEngine.ibetAgent.State == AgentState.Running)
                {
                    this._ibetEngine.Stop();
                    this._ibetEngine.ibetAgent.Logout();
                    if (this._ibetEngine.ibetAgent2.State == AgentState.Running)
                        System.GC.SuppressFinalize(this._ibetEngine.ibetAgent2);
                    System.GC.SuppressFinalize(this._ibetEngine.ibetAgent);
                    System.GC.SuppressFinalize(this._ibetEngine);
                }
                if (this._sbobetEngine != null && this._sbobetEngine.sboAgent.State == AgentState.Running)
                {
                    this._sbobetEngine.Stop();
                    this._sbobetEngine.sboAgent.Logout();
                    System.GC.SuppressFinalize(this._sbobetEngine.sboAgent);
                    System.GC.SuppressFinalize(this._sbobetEngine);
                }
            }
        }
        
        private void TransactionProcessMaxBet(
            MatchDTO ibetMatch, MatchDTO sbobetMatch, eOddType oddType, string ibetOddID, string sbobetOddID, string ibetOddType, 
            string sbobetOddType, string ibetOddValue, string sbobetOddValue, int ibetStake, int sbobetStake, bool ibetHomeGive, 
            IBetEngine ibetEngine, SbobetEngine sbobetEngine, string sbobetOdd)
        {
            if (!this._betting)
            {
                TransactionDTO transactionDTO = new TransactionDTO();
                transactionDTO = this.PlaceBetAllowMaxBet(true, ibetMatch, sbobetMatch, oddType, ibetOddID, sbobetOddID, ibetOddType, sbobetOddType, ibetOddValue, sbobetOddValue.ToString(), ibetStake, sbobetStake, ibetHomeGive, this._ibetEngine, this._sbobetEngine, sbobetOdd);
                this.AddTransaction(transactionDTO);
                if (transactionDTO != null && transactionDTO.IBETTrade)
                {
                    this.UpdateOddBetHistory(ibetOddID,"");
                }
            }
        }
        private void TransactionProcess(MatchDTO ibetMatch, MatchDTO sbobetMatch, eOddType oddType, string ibetOddID, string sbobetOddID, string ibetOddType, string sbobetOddType, string ibetOddValue, string sbobetOddValue, int ibetStake, int sbobetStake, IBetEngine ibetEngine, SbobetEngine sbobetEngine, string sbobetOdd, bool homeFavor)
        {
            if (!this._betting)
            {
                TransactionDTO transactionDTO = new TransactionDTO();
                transactionDTO = this.PlaceBet(true, ibetMatch, sbobetMatch, oddType, ibetOddID, sbobetOddID, ibetOddType, sbobetOddType, ibetOddValue, sbobetOddValue.ToString(), ibetStake, sbobetStake, this._ibetEngine, this._sbobetEngine, sbobetOdd, homeFavor);
                this.AddTransaction(transactionDTO);
                if (transactionDTO != null && transactionDTO.IBETTrade)
                {
                    this.UpdateOddBetHistory(ibetOddID,"");
                }
            }
        }
        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns["KickOffTime"]);
                if (category.Contains("Live"))
                {
                    e.Appearance.Font = new System.Drawing.Font("Tahoma", 9.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    //e.Appearance.ForeColor = Color.SlateGray;
                    //e.Appearance.BackColor2 = Color.SeaShell;
                }

            }
        }
        private TransactionDTO PlaceSingleIBET(string BetStrategy, string oddPrice, MatchDTO ibetMatch, string oddID, string oddValue, eOddType oddType, string ibetOddType, string stake, string homeScore, string awayScore, IBetEngine ibetEngine, bool followtype, string followref, string accountName)
        {
            this._betting = true;
            TransactionDTO transactionDTO = new TransactionDTO();
            string acctoScan = "";
            List<Bet> bl;
            string RefID = "";
            if (BetStrategy.Contains("Over15_XA"))
            {
                bl = this._ibetEngine.ibetAgent2.betList;
                acctoScan = this._ibetEngine.ibetAgent2.Config.Account;
                string[] array = BetStrategy.Split(new string[] { "Ref ID:" }, System.StringSplitOptions.None);
                RefID = array[1];
                if (!AllowOddBet(RefID + ibetOddType, ibetOddType))
                {
                    transactionDTO.HomeTeamName = ibetMatch.HomeTeamName;
                    transactionDTO.AwayTeamName = ibetMatch.AwayTeamName;
                    transactionDTO.Note = "Under account: " + acctoScan + " just bet on this odd";
                    transactionDTO.DateTime = DateTime.Now;
                    this._betting = false;
                    return transactionDTO;
                }
            }            
            else
            {
                bl = this._ibetEngine.ibetAgent.betList;
                acctoScan = this._ibetEngine.ibetAgent.Config.Account;
            }
            if (BetStrategy.Contains("Warning"))
            {
                transactionDTO.HomeTeamName = ibetMatch.HomeTeamName;
                transactionDTO.AwayTeamName = ibetMatch.AwayTeamName;
                transactionDTO.Note = BetStrategy;
                transactionDTO.DateTime = DateTime.Now;
                this._betting = false;
                return transactionDTO;
            }
            foreach (Bet bet in bl)
            {
                if (bet.Home == ibetMatch.HomeTeamName)
                {                    
                    if (bet.Handicap == decimal.Parse(oddValue))
                    {
                        if (bet.Choice.ToString().ToLower() == ibetOddType)
                        {
                            transactionDTO.HomeTeamName = ibetMatch.HomeTeamName;
                            transactionDTO.AwayTeamName = ibetMatch.AwayTeamName;
                            transactionDTO.Note = acctoScan + " : Bet list contains this odd already";
                            transactionDTO.DateTime = DateTime.Now;
                            this._betting = false;
                            return transactionDTO;
                        }
                    }                    
                }
            }
            if (!AllowOddBet(oddID + ibetOddType,ibetOddType))
            {
                transactionDTO.HomeTeamName = ibetMatch.HomeTeamName;
                transactionDTO.AwayTeamName = ibetMatch.AwayTeamName;
                transactionDTO.Note = acctoScan + " just bet on this odd";
                transactionDTO.DateTime = DateTime.Now;
                this._betting = false;
                return transactionDTO;
            }
            if (this._ibetEngine.ibetAgent.Config.Balance < int.Parse(stake))
            {
                transactionDTO.HomeTeamName = ibetMatch.HomeTeamName;
                transactionDTO.AwayTeamName = ibetMatch.AwayTeamName;
                transactionDTO.Note = "Not enough money";
                transactionDTO.DateTime = DateTime.Now;
                this._betting = false;
                return transactionDTO;
            }

            
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

            //System.Collections.Generic.List<MatchDTO> listIbetMatch = this._listIBETMatch;
            Dictionary<string, IbetMatch> listIbetMatch = this._ibetMatchs;
            //MatchDTO matchDTO = MatchDTO.SearchMatchFull(ibetMatch, listIbetMatch);
            if (ibetMatch != null && ibetMatch.HomeTeamName != string.Empty)
            {
                //OddDTO oddDTO = OddDTO.SearchOdd(oddType, odd, true, ibetMatch.Odds);
                //if (oddDTO != null)
                //{
                    string ibetOddNow = string.Empty;
                    //if (ibetOddType == "a")
                    //    ibetOddNow = oddDTO.Away.ToString();
                    //else
                    //    ibetOddNow = oddDTO.Home.ToString();
                    if (followtype || (!followtype && ((float.Parse(ibetOddNow) >= (float)txtLowestOddValue.Value && float.Parse(ibetOddNow) > 0)) || float.Parse(ibetOddNow) < 0))
                    {
                        int maxBet = 0;
                        int minBet = 0;
                        string ibetOddID = oddID;
                        try
                        {
                            string valueAway = "";
                            if (BetStrategy == "Under" || BetStrategy == "Over")
                            {
                                #region UNDER_OVER
                                if (BetStrategy == "Under")
                                {
                                    ibetEngine.PrepareBet2(ibetOddID, "h", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                    valueAway = newOddValue;
                                    Thread.Sleep(2000);
                                    ibetEngine.PrepareBet2(ibetOddID, "a", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                }
                                else if (BetStrategy == "Over")
                                {
                                    ibetEngine.PrepareBet2(ibetOddID, "a", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                    valueAway = newOddValue;
                                    Thread.Sleep(2000);
                                    ibetEngine.PrepareBet2(ibetOddID, "h", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                }
                                if (homeTeamName == ibetMatch.HomeTeamName)
                                {
                                    if (valueAway == newOddValue)
                                    {
                                        ibetEngine.ConfirmBet(oddType, newOddValue, stake, minBet.ToString(), maxBet.ToString(), out flag2);
                                        if (!flag2)
                                        {
                                            if (BetStrategy == "Under")
                                                text = "Under:: Confirm bet failed";
                                            else
                                                text = "Over:: Confirm bet failed";
                                        }
                                        else
                                        {
                                            this._lastTransactionTime = DateTime.Now;
                                            if (BetStrategy == "Under")
                                            {
                                                text = "Under:: - Success ";
                                                UpdateOddBetHistory(oddID + "a", "a");
                                            }
                                            else
                                            {
                                                text = "Over:: - Success ";
                                                UpdateOddBetHistory(oddID + "h", "h");
                                            }
                                            this._ibetEngine.ibetAgent.RefreshBetList();
                                        }
                                    }
                                    else
                                    {
                                        if (BetStrategy == "Under")
                                            text = "Under:: Odd requested: " + oddPrice.ToString();
                                        else
                                            text = "Over:: Odd requested: " + oddPrice.ToString();
                                    }
                                }
                                else
                                {
                                    if (BetStrategy == "Under")
                                        text = "Under:: Not same match. Comparing: " + ibetMatch.HomeTeamName + " - " + ibetMatch.AwayTeamName;
                                    else
                                        text = "Over:: Not same match. Comparing: " + ibetMatch.HomeTeamName + " - " + ibetMatch.AwayTeamName;
                                }
                                #endregion
                            }
                            else if (BetStrategy == "Over9290")
                            {
                                #region Over9290
                                ibetEngine.PrepareBet2(ibetOddID, "h", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                if (flag && homeTeamName == ibetMatch.HomeTeamName)
                                {
                                    ibetEngine.ConfirmBet(oddType, newOddValue, stake, minBet.ToString(), maxBet.ToString(), out flag2);
                                    if (!flag2)
                                    {
                                        text = "Over 92/90:: Confirm bet failed";
                                    }
                                    else
                                    {
                                        this._lastTransactionTime = DateTime.Now;
                                        text = "Over 92/90:: - Success ";
                                        //this._ibetEngine.ibetAgent.RefreshBetList();
                                        UpdateOddBetHistory(oddID + "h", "h");
                                    }
                                }
                                else
                                {
                                    text = "Over1.75:: Not same match. Comparing: " + ibetMatch.HomeTeamName + " - " + ibetMatch.AwayTeamName;
                                }
                                #endregion 
                            }
                            else if (BetStrategy == "SapKeo")
                            {
                                #region SAP_KEO
                                stake = ((int)(int.Parse(stake) * 1.5)).ToString();
                                ibetEngine.PrepareBet2(ibetOddID, "h", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                if (newOddValue == oddPrice.ToString())
                                {
                                    if (homeTeamName == ibetMatch.HomeTeamName)
                                    {
                                        ibetEngine.ConfirmBet(oddType, newOddValue, stake, minBet.ToString(), maxBet.ToString(), out flag2);
                                        if (!flag2)
                                        {
                                            text = "Sap Keo:: Confirm bet failed";
                                        }
                                        else
                                        {
                                            this._lastTransactionTime = DateTime.Now;
                                            text = "Odd Down:: - Success ";
                                            //this._ibetEngine.ibetAgent.RefreshBetList();
                                            UpdateOddBetHistory(oddID + "h", "h");
                                        }
                                    }
                                    else
                                    {
                                        text = "Sap Keo:: Not same match. Comparing: " + ibetMatch.HomeTeamName + " - " + ibetMatch.AwayTeamName;
                                    }
                                }
                                else
                                {
                                    text = "Sap Keo:: Odd change. Requested: " + oddPrice.ToString();
                                }
                                #endregion
                            }
                            else if (BetStrategy == "Over1.75")
                            {
                                #region Over1.75
                                ibetEngine.PrepareBet2(ibetOddID, "h", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                if (homeTeamName == ibetMatch.HomeTeamName)
                                {
                                    ibetEngine.ConfirmBet(oddType, newOddValue, stake, minBet.ToString(), maxBet.ToString(), out flag2);
                                    if (!flag2)
                                    {
                                        text = "Over1.75:: Confirm bet failed";
                                    }
                                    else
                                    {
                                        this._lastTransactionTime = DateTime.Now;
                                        text = "Over1.75:: - Success ";
                                        //this._ibetEngine.ibetAgent.RefreshBetList();
                                        UpdateOddBetHistory(oddID + "h", "h");
                                    }
                                }
                                else
                                {
                                    text = "Over1.75:: Not same match. Comparing: " + ibetMatch.HomeTeamName + " - " + ibetMatch.AwayTeamName;
                                }
                                #endregion
                            }
                            else if (BetStrategy == "nguoc" || BetStrategy == "xuoi")
                            {
                                #region BEST_STRATEGIES
                                if (BetStrategy == "nguoc")
                                {
                                    ibetEngine.PrepareBet2(ibetOddID, "a", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                    valueAway = newOddValue;
                                    Thread.Sleep(2000);
                                    ibetEngine.PrepareBet2(ibetOddID, "h", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                }
                                else if (BetStrategy == "xuoi")
                                {
                                    ibetEngine.PrepareBet2(ibetOddID, "h", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                    valueAway = newOddValue;
                                    Thread.Sleep(2000);
                                    ibetEngine.PrepareBet2(ibetOddID, "a", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                }

                                if (homeTeamName == ibetMatch.HomeTeamName)
                                {
                                    if (valueAway == newOddValue)
                                    {
                                        ibetEngine.ConfirmBet(oddType, newOddValue, stake, minBet.ToString(), maxBet.ToString(), out flag2);
                                        if (!flag2)
                                        {
                                            text = "Best Strategy:: Confirm bet failed";
                                        }
                                        else
                                        {
                                            this._lastTransactionTime = DateTime.Now;
                                            text = "Best Strategy:" + this.txtAddValue.Value.ToString() + " : - Success. " + BetStrategy.Replace("nguoc", "Over").Replace("xuoi", "Under");
                                            this._ibetEngine.ibetAgent.RefreshBetList();
                                            if (BetStrategy == "nguoc")
                                            {
                                                UpdateOddBetHistory(oddID + "h", "h");
                                            }
                                            else if (BetStrategy == "xuoi")
                                            {
                                                UpdateOddBetHistory(oddID + "a", "a");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        text = "Best Strategy:: Odd requested: " + oddPrice.ToString();
                                    }
                                }
                                else
                                {
                                    text = "Best Strategy:: Not same match. Comparing: " + ibetMatch.HomeTeamName + " - " + ibetMatch.AwayTeamName;
                                }
                                #endregion
                            }
                            #region IBET_vs_IBET_Over
                            else if (BetStrategy.Contains("Over15_Fang"))
                            {

                                ibetEngine.PrepareBet2(ibetOddID, "h", oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                if (homeTeamName == ibetMatch.HomeTeamName
                                    && (homeTeamName.Contains("30:01-45:00") || homeTeamName.Contains("75:01-90:00"))
                                    )
                                {
                                    ibetEngine.ConfirmBet(oddType, newOddValue, stake, minBet.ToString(), maxBet.ToString(), out flag2);
                                    if (!flag2)
                                    {
                                        text = "Over_15:: Confirm bet failed";
                                    }
                                    else
                                    {
                                        this._lastTransactionTime = DateTime.Now;
                                        text = "Over_15:: - Success ";
                                        UpdateOddBetHistory(oddID + "h", "h");

                                        //this._ibetEngine.ibetAgent.RefreshBetList();
                                    }
                                }
                            }

                            #endregion
                            else if (BetStrategy.Contains("Over15_XA"))
                            {
                                #region IBET_vs_IBET_Over30-45_XA

                                ibetEngine.PrepareBetFor2ndIBET(ibetOddID, ibetOddType, oddPrice.ToString(), stake, out flag, out minBet, out maxBet, out betKindValue, out homeTeamName, out awayTeamName, out newOddValue, out newHomeScore, out newAwayScore);
                                if (homeTeamName.TrimEnd() == ibetMatch.HomeTeamName && awayTeamName.TrimEnd() == ibetMatch.AwayTeamName)
                                {
                                    
                                    if (float.Parse(newOddValue) >= 0.35f || float.Parse(newOddValue) < 0f)//tranh suu ban
                                    {
                                        if (!betKindValue.EndsWith("75"))
                                        {
                                            ibetEngine.ConfirmBetFor2ndIBET(oddType, newOddValue, stake, minBet.ToString(), maxBet.ToString(), out flag2);
                                            if (!flag2)
                                            {
                                                text = BetStrategy + ":: Confirm bet failed";
                                            }
                                            else
                                            {
                                                this._lastTransactionTime = DateTime.Now;
                                                text = BetStrategy + ":: - Success ";
                                                UpdateOddBetHistory(oddID + ibetOddType, ibetOddType);
                                                UpdateOddBetHistory(RefID + ibetOddType, ibetOddType);
                                                this._ibetEngine.ibetAgent2.RefreshBetList();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        text = BetStrategy + ":: Keo nho hon 0.35 >> Hien tai: " + newOddValue + ". Xem xet xa tay >>>>>>>>>>";
                                    }
                                }
                                else
                                {
                                    iBet.Utilities.WriteLog.Write("Not same match: Tim: " + ibetMatch.HomeTeamName + "-" 
                                    + ibetMatch.AwayTeamName + "Tra ve: " + homeTeamName + "-" + awayTeamName);
                                }

                                #endregion


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
                //}
                //else
                //{
                //    string text2 = string.Empty;                    
                //    text = "Odd not found.";
                //}
            }
            else
                text = "Match not found ";


            transactionDTO.HomeTeamName = homeTeamName + ":" + homeScore;
            transactionDTO.AwayTeamName = awayTeamName + ":" + awayScore;
            transactionDTO.HomeScore = homeScore;
            transactionDTO.AwayScore = awayScore;
            transactionDTO.Score = homeScore + "-" + awayScore;
            transactionDTO.AccountPair = this._ibetAccount;// +" - " + this._ibetSubAccount;
            transactionDTO.OddType = oddType.ToString();
            transactionDTO.Odd = betKindValue;
            transactionDTO.OddKindValue = betKindValue;
            transactionDTO.OddValue = newOddValue;
            transactionDTO.Stake = stake;            
            transactionDTO.Note = text + " >>> " + acctoScan;
            transactionDTO.IBETTrade = flag2;
            transactionDTO.IBETAllow = flag;
            transactionDTO.IsFollowTypeTrans = true;
            transactionDTO.DateTime = DateTime.Now;
            transactionDTO.FollowRef = followref;

            this._betting = false;
            return transactionDTO;
        }
        private TransactionDTO PlaceBet(bool sendOrNOT, MatchDTO ibetMatch, MatchDTO sbobetMatch, eOddType oddType, string ibetOddID, string sbobetOddID, string ibetOddType, string sbobetOddType, string ibetOddValue, string sbobetOddValue, int ibetStake, int sbobetStake, IBetEngine ibetEngine, SbobetEngine sbobetEngine, string sbobetOdd, bool ibetHomeGive)
        {
            this._betting = true;
            TransactionDTO transactionDTO = new TransactionDTO();
            //transactionDTO.OddType = oddType.ToString() + " - " + ibetOddType.Replace("h", "Home").Replace("a", "Away") + " / " + sbobetOddType.Replace("h", "Home").Replace("a", "Away");
            string text = "";
            string text2 = "";
            string text3 = "";
            bool flag = false;
            bool flag2 = false;
            int num = 0;
            int num2 = 0;
            string text4 = "";
            string text5 = "";
            string text6 = "";
            bool flag3 = false;
            bool flag4 = false;
            bool sBOBETReTrade = false;
            int num3 = 0;
            string text7 = "";
            string betCount = "";
            string text8 = "";
            bool flag5 = false;

            float ibetOddValue_ = float.Parse(ibetOddValue);
            float sboOddValue_ = float.Parse(sbobetOddValue);

            if (ibetOddValue_ > 0.84 || (ibetOddValue_ > 0f && txtAddValue.Value.ToString() == "0.01"))
            {
                float valueAdd = ibetStake * (float)txtAddValue.Value;
                ibetStake = ibetStake + (int)valueAdd;
            }
            if (sboOddValue_ > 0.84 || (sboOddValue_ > 0f && txtAddValue.Value.ToString() == "0.01"))
            {
                float valueAdd = sbobetStake * (float)txtAddValue.Value;
                sbobetStake = sbobetStake + (int)valueAdd;
            }

            if (MatchDTO.IsSameMatch(ibetMatch.HomeTeamName.ToLower(), sbobetMatch.HomeTeamName.ToLower(), ibetMatch.AwayTeamName.ToLower(), sbobetMatch.AwayTeamName.ToLower()))
            {
                try
                {
                    ibetEngine.PrepareBet(ibetOddID, ibetOddType, ibetOddValue, ibetStake.ToString(), out flag, out num, out num2, out text4, out text2, out text3);
                    sbobetEngine.PrepareBet(sbobetOddID, sbobetOddValue, sbobetOddType, out betCount, out num3, out flag3, out text7, out text5, out text6);
                    if (MatchDTO.IsSameMatch(text2, text5, text3, text6))
                    {
                        if (flag && flag3)
                        {
                            if ((checkEdit13.Checked || checkEdit15.Checked) && sendOrNOT)//gui lenh
                            {
                                var taskA = new Task(() => this.AddOddToLocalCommunity(ibetMatch, sbobetMatch, oddType, sbobetOdd, sbobetOdd, ibetOddType, sbobetOddType, ibetOddValue, sbobetOddValue, ibetHomeGive));
                                taskA.Start();
                            }
                            float num4 = float.Parse(text4);
                            float num5 = float.Parse(text7);
                            if (num4 + num5 == 0f || num4 == num5)
                            {
                                if (ibetStake <= num2 && sbobetStake <= num3)
                                {
                                    try
                                    {                                        
                                        float currentCredit = 0;
                                        float currentCredit2 = 0;
                                        if (checkEdit8.Checked)
                                        {
                                            currentCredit = ibetEngine.GetCurrentCredit();
                                            currentCredit2 = sbobetEngine.GetCurrentCredit();
                                        }
                                        else
                                        {
                                            currentCredit = ibetEngine._currentCredit;
                                            currentCredit2 = sbobetEngine._currentCredit;
                                        }

                                        if ((float)ibetStake <= currentCredit && (float)sbobetStake <= currentCredit2)
                                        {
                                            try
                                            {
                                                
                                                ibetEngine.ConfirmBet(oddType, ibetOddValue, ibetStake.ToString(), num.ToString(), num2.ToString(), out flag2);
                                                if (flag2)
                                                {
                                                    sbobetEngine.ConfirmBet(sbobetOddID, sbobetOddValue, sbobetOddType, sbobetStake.ToString(), betCount, out text8, out flag4);
                                                    if (!flag4)
                                                    {
                                                        try
                                                        {
                                                            sbobetEngine.PrepareBet(sbobetOddID, text8, sbobetOddType, out betCount, out num3, out flag5, out text7, out text5, out text6);
                                                            if (MatchDTO.IsSameMatch(text2, text5, text3, text6))
                                                            {
                                                                num5 = float.Parse(text7);
                                                                if (num4 + num5 == 0f || num4 == num5)
                                                                {
                                                                    text = "Retrade. IBET: " + ibetOddValue + " -  SBOBET: " + text8;
                                                                    sbobetEngine.ConfirmBet(sbobetOddID, text8, sbobetOddType, sbobetStake.ToString(), betCount, out text8, out sBOBETReTrade);
                                                                    this._lastTransactionTime = System.DateTime.Now;
                                                                }
                                                                else
                                                                {
                                                                    text = "Invalid Odd while Retrade. IBET Odd: " + text4 + " - SBOBET Odd:" + text7;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                text = string.Concat(new string[]
																{
																	"Not Same Match - Retrade. IBET: ", 
																	text2, 
																	" / ", 
																	text3, 
																	" - SBOBET: ", 
																	text5, 
																	" / ", 
																	text6
																});
                                                            }
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
												" -  SBOBET Credit: ", 
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
										" - SBOBET: ", 
										num3
									});
                                }
                            }
                            else
                            {
                                text = "Invalid Odd while Preparing Ticket. IBET Odd: " + text4 + " - SBOBET Odd:" + text7;
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
							" - SBOBET: ", 
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
					" - SBOBET: ", 
					sbobetMatch.HomeTeamName, 
					" / ", 
					sbobetMatch.AwayTeamName
				});
            }
            transactionDTO.HomeTeamName = ibetMatch.HomeTeamName + " / " + sbobetMatch.HomeTeamName;
            transactionDTO.AwayTeamName = ibetMatch.AwayTeamName + " / " + sbobetMatch.AwayTeamName;
            transactionDTO.Odd = text4 + " / " + text7;
            transactionDTO.OddKindValue = text4 + " / " + text7;
            transactionDTO.OddValue = ibetOddValue + " / " + sbobetOddValue;
            transactionDTO.Stake = ibetStake + " / " + sbobetStake;
            transactionDTO.IBETAllow = flag;
            transactionDTO.IBETTrade = flag2;
            transactionDTO.IBETReTrade = false;
            transactionDTO.SBOBETAllow = flag3;
            transactionDTO.SBOBETTrade = flag4;
            transactionDTO.SBOBETReTrade = sBOBETReTrade;
            transactionDTO.OddType = oddType.ToString();
            transactionDTO.Note = text;
            transactionDTO.DateTime = System.DateTime.Now;
            transactionDTO.OddType = oddType.ToString() + " - " + ibetOddType.Replace("h", "Home").Replace("a", "Away") + " / " + sbobetOddType.Replace("h", "Home").Replace("a", "Away");
            this._betting = false;
            return transactionDTO;
            
        }
        private TransactionDTO PlaceBetAllowMaxBet(bool sendOrNOT, MatchDTO ibetMatch, MatchDTO sbobetMatch, eOddType oddType, string ibetOddID, string sbobetOddID, string ibetOddType, string sbobetOddType, string ibetOddValue, string sbobetOddValue, int ibetStake, int sbobetStake, bool ibetHomeGive, IBetEngine ibetEngine, SbobetEngine sbobetEngine, string sbobetOdd)
        {
            this._betting = true;
            TransactionDTO transactionDTO = new TransactionDTO();
            
            //transactionDTO.OddType = current2.Type.ToString() + " - Home / Away";
            string text = "";
            string text2 = "";
            string text3 = "";
            bool flag = false;
            bool flag2 = false;
            int num = 0;
            int num2 = 0;
            string text4 = "";
            string text5 = "";
            string text6 = "";
            bool flag3 = false;
            bool flag4 = false;
            bool sBOBETReTrade = false;
            int num3 = 0;
            string text7 = "";
            string betCount = "";
            string text8 = "";
            bool flag5 = false;

            float ibetOddValue_ = float.Parse(ibetOddValue);
            float sboOddValue_ = float.Parse(sbobetOddValue);

            if (ibetOddValue_ > 0.84 || (ibetOddValue_ > 0f && txtAddValue.Value.ToString() == "0.01"))
            {
                float valueAdd = ibetStake * (float)txtAddValue.Value;
                ibetStake = ibetStake + (int)valueAdd;
            }
            if (sboOddValue_ > 0.84 || (sboOddValue_ > 0f && txtAddValue.Value.ToString() == "0.01"))
            {
                float valueAdd = sbobetStake * (float)txtAddValue.Value;
                sbobetStake = sbobetStake + (int)valueAdd;
            }

            bool ibetGoFirst = true;// who is Away or Under

            if (checkEdit11.Checked) //safe bet
            {
                if (sbobetOddType == "a") //  ibet  vs sbo
                {
                    if (oddType == eOddType.FirstHalfOverUnder || oddType == eOddType.FulltimeOverUnder)//neu bong over under
                    {
                        ibetGoFirst = false;
                    }
                    else // bong handicap
                    {
                        if (ibetHomeGive) // neu away la doi cua duoi
                        {
                            ibetGoFirst = false;
                        }
                    }
                }
                else //  sbo vs ibet
                {
                    if (oddType == eOddType.FirstHalfOverUnder || oddType == eOddType.FulltimeOverUnder)//neu bong over under
                    {
                        ibetGoFirst = true;
                    }
                    else // bong handicap
                    {
                        if (ibetHomeGive) // neu away la doi cua duoi
                        {
                            ibetGoFirst = true;
                        }
                    }
                }
            }

            if (MatchDTO.IsSameMatch(ibetMatch.HomeTeamName.ToLower(), sbobetMatch.HomeTeamName.ToLower(), ibetMatch.AwayTeamName.ToLower(), sbobetMatch.AwayTeamName.ToLower()))
            {
                try
                {
                    ibetEngine.PrepareBet(ibetOddID, ibetOddType, ibetOddValue, ibetStake.ToString(), out flag, out num, out num2, out text4, out text2, out text3);
                    sbobetEngine.PrepareBet(sbobetOddID, sbobetOddValue, sbobetOddType, out betCount, out num3, out flag3, out text7, out text5, out text6);
                    //object result = this.CallJavascriptFunction("bet", new object[] { 0, ibetMatch.ID.Remove(0, 2), ibetOddID, ibetOddType, ibetOddValue });
                    
                    if (MatchDTO.IsSameMatch(text2, text5, text3, text6)) // ten hom - away giong nhau
                    {                        
                        float num4 = 0f;
                        float num5 = 0f;
                        if (flag && flag3 && float.TryParse(text4, out num4) && float.TryParse(text7, out num5))
                        {
                            if ((checkEdit13.Checked || checkEdit15.Checked) && sendOrNOT)//gui lenh
                            {
                                var taskA = new Task(() => this.AddOddToLocalCommunity(ibetMatch, sbobetMatch, oddType, sbobetOdd, sbobetOdd, ibetOddType, sbobetOddType, ibetOddValue, sbobetOddValue, ibetHomeGive));
                                taskA.Start();
                            }

                            //try parse de tranh hien tuong mat Odd 1 ben
                            //float num4 = float.Parse(text4); 
                            //float num5 = float.Parse(text7);
                            if (num4 + num5 == 0f || num4 == num5)
                            {
                                int num6 = 0;
                                if (num2 >= num3) // num2 maxbet ibet
                                {
                                    num6 = num3; // num3 maxbet sbo
                                }
                                else
                                {
                                    if (num2 <= num3)
                                    {
                                        num6 = num2;
                                    }
                                }
                                if (ibetStake >= num6)
                                {
                                    ibetStake = num6;
                                }
                                if (sbobetStake >= num6)
                                {
                                    sbobetStake = num6;
                                }
                                try
                                {
                                    float currentCredit = 0;
                                    float currentCredit2 = 0;
                                    if (checkEdit8.Checked)
                                    {
                                        currentCredit = ibetEngine.GetCurrentCredit();
                                        currentCredit2 = sbobetEngine.GetCurrentCredit();
                                    }
                                    else
                                    {
                                        currentCredit = ibetEngine._currentCredit;
                                        currentCredit2 = sbobetEngine._currentCredit;
                                    }

                                    if ((float)ibetStake <= currentCredit && (float)sbobetStake <= currentCredit2)
                                    {
                                        try
                                        {
                                            if (ibetGoFirst)
                                            {
                                                
#if TESTMODE
                                                #region IBET_FIRTST_THEN_SBO
                                                if (!checkEdit12.Checked || (checkEdit12.Checked && ibetOddType == "a" && (oddType == eOddType.FirstHalfOverUnder || oddType == eOddType.FulltimeOverUnder)))
                                                    ibetEngine.ConfirmBet(oddType, ibetOddValue, ibetStake.ToString(), num.ToString(), num2.ToString(), out flag2);
                                                if (flag2 && !checkEdit12.Checked)
                                                {
                                                    sbobetEngine.ConfirmBet(sbobetOddID, sbobetOddValue, sbobetOddType, sbobetStake.ToString(), betCount, out text8, out flag4);
                                                    if (!flag4)
                                                    {
                                                        try
                                                        {
                                                            sbobetEngine.PrepareBet(sbobetOddID, text8, sbobetOddType, out betCount, out num3, out flag5, out text7, out text5, out text6);
                                                            if (MatchDTO.IsSameMatch(text2, text5, text3, text6))
                                                            {
                                                                if (!float.TryParse(text7, out num5)) //text 7 = BetKindValue
                                                                {
                                                                    Thread.Sleep(3000);
                                                                    sbobetEngine.PrepareBet(sbobetOddID, text8, sbobetOddType, out betCount, out num3, out flag5, out text7, out text5, out text6);
                                                                    float.TryParse(text7, out num5);
                                                                }

                                                                if (num4 + num5 == 0f || num4 == num5)
                                                                {
                                                                    text = "Retrade. IBET: " + ibetOddValue + " -  SBOBET: " + text8;
                                                                    //text = "No retrade in this version please trade manually.";
                                                                    sbobetEngine.ConfirmBet(sbobetOddID, text8, sbobetOddType, sbobetStake.ToString(), betCount, out text8, out sBOBETReTrade);
                                                                    this._lastTransactionTime = System.DateTime.Now;
                                                                    playSound(false);
                                                                    
                                                                }
                                                                else
                                                                {
                                                                    text = "Invalid Odd in SBOBET. IBET Odd: " + text4 + " - SBOBET Odd:" + text7;
                                                                    playSound(false);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                text = string.Concat(new string[]
															    {
																    "Not Same Match - Retrade. IBET: ", 
																    text2, 
																    " / ", 
																    text3, 
																    " - SBOBET: ", 
																    text5, 
																    " / ", 
																    text6
															    });
                                                            }
                                                        }
                                                        catch (System.Exception ex)
                                                        {
                                                            text = "Error while Retrade. Details: " + ex.Message;
                                                            playSound(false);
                                                        }
                                                    }
                                                    else
                                                    {
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
                                                    ibetEngine.GetCurrentCredit();
                                                    sbobetEngine.GetCurrentCredit();

                                                }
                                                
                                                #endregion
#endif
                                            }

                                            else
                                            {
                                                text = "No more SBOBET comes first.";
                                                //#region SBO_BET_FIRST_THEN_IBET

                                                //sbobetEngine.ConfirmBet(sbobetOddID, sbobetOddValue, sbobetOddType, sbobetStake.ToString(), betCount, out text8, out flag4);
                                                //if (flag4)
                                                //{ 
                                                //    ibetEngine.ConfirmBet(oddType, ibetOddValue, ibetStake.ToString(), num.ToString(), num2.ToString(), out flag2);
                                                //    if (!flag2)
                                                //    {
                                                //        text = "Invalid Odd while Retrade.";// IBET Odd: " + text4 + " - SBOBET Odd:" + text7;
                                                //        sbobetEngine.GetCurrentCredit();
                                                //        playSound(false);
                                                //    }
                                                //    else
                                                //    {
                                                //        this._lastTransactionTime = System.DateTime.Now;
                                                //        object obj = text;
                                                //        text = string.Concat(new object[]
                                                //        {
                                                //            obj, 
                                                //            " - Success Transaction. Half: ", 
                                                //            ibetMatch.Half, 
                                                //            " - Minute: ", 
                                                //            ibetMatch.Minute, 
                                                //            " - Halftime: ", 
                                                //            ibetMatch.IsHalfTime
                                                //        });
                                                //        ibetEngine.GetCurrentCredit();
                                                //        sbobetEngine.GetCurrentCredit();
                                                //        playSound(true);
                                                //    }
                                                //}

                                                //#endregion
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
											" -  SBOBET Credit: ", 
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
                                text = "Invalid Odd while Preparing Ticket. IBET Odd: " + text4 + " - SBOBET Odd:" + text7;
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
							" - SBOBET: ", 
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
					" - SBOBET: ", 
					sbobetMatch.HomeTeamName, 
					" / ", 
					sbobetMatch.AwayTeamName
				});
            }
            transactionDTO.HomeTeamName = ibetMatch.HomeTeamName + ":" + ibetMatch.HomeScore + " / " + sbobetMatch.HomeTeamName;
            transactionDTO.AwayTeamName = ibetMatch.AwayTeamName + ":" + ibetMatch.AwayScore + " / " + sbobetMatch.AwayTeamName;
            transactionDTO.Odd = text4 + " / " + text7;
            transactionDTO.OddKindValue = text4 + " / " + text7;
            transactionDTO.OddValue = ibetOddValue + " / " + sbobetOddValue;
            transactionDTO.Stake = ibetStake + " / " + sbobetStake;
            transactionDTO.IBETAllow = flag;
            transactionDTO.IBETTrade = flag2;
            transactionDTO.IBETReTrade = false;
            transactionDTO.SBOBETAllow = flag3;
            transactionDTO.SBOBETTrade = flag4;
            transactionDTO.SBOBETReTrade = sBOBETReTrade;
            //transactionDTO.OddType = oddType.ToString() + ;
            transactionDTO.OddType = oddType.ToString() + " - " + ibetOddType.Replace("h", "Home").Replace("a", "Away") + " / " + sbobetOddType.Replace("h", "Home").Replace("a", "Away");
            transactionDTO.Note = text;
            transactionDTO.DateTime = System.DateTime.Now.ToLocalTime();

            transactionDTO.HomeTeamSBOBET = sbobetMatch.HomeTeamName;
            transactionDTO.AwayTeamSBOBET = sbobetMatch.AwayTeamName;
            
            this._betting = false;
            return transactionDTO;            
        }        
        private bool AllowOddBet(string oddID, string type)
        {
            //return !this._oddTransactionHistory.ContainsKey(oddID) || (System.DateTime.Now - this._oddTransactionHistory[oddID]).Seconds > (int)this.txtTransactionTimeSpan.Value;
            //return !this._oddTransactionHistory.ContainsKey(oddID);
            if (this._oddTransactionHistory.ContainsKey(oddID))
            {
                if (this._oddTransactionHistory[oddID] == type)
                {
                    return false;
                }
            }
            return true;
        }
        private void UpdateOddBetHistory(string oddID, string type)
        {
            //this._oddTransactionHistory.Remove(oddID);
            this._oddTransactionHistory.Add(oddID, type);
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

        private void btnViewIbetWeb(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmIbet frmIBet = new frmIbet(this._ibetEngine.ibetAgent, "http://" + this._ibetEngine.ibetAgent.Config.HostName + "/main.aspx");
                frmIBet.Show();
            }
            catch (System.Exception ex)
            {
                this.ShowErrorDialog("Error. \nDetails: " + ex.Message);
            }
        }
        private void btnViewIbet2Web(object sender, ItemClickEventArgs e)
        {
            try
            {
                frmIbet frmIBet = new frmIbet(this._ibetEngine.ibetAgent2, "http://" + this._ibetEngine.ibetAgent2.Config.HostName + "/main.aspx");
                frmIBet.Show();
            }
            catch (System.Exception ex)
            {
                this.ShowErrorDialog("Error. \nDetails: " + ex.Message);
            }
        }
        private void btnViewSboWeb(object sender, ItemClickEventArgs e)
        {
            try
            {
                //http://cqqzmx3ycf0z.asia.sbobet.com/web-root/restricted/default.aspx?loginname=3ba581087ef24933183c2e518d35fcd1&redirect=true
                frmIbet frmIBet = new frmIbet(this._sbobetEngine.sboAgent, "http://" + this._sbobetEngine.sboAgent.Config.HostName
                    + "/web-root/restricted/default.aspx?loginname="
                    + this._sbobetEngine.sboAgent.loginName
                    + "&redirect=true");
                frmIBet.Show();
            }
            catch (System.Exception ex)
            {
                this.ShowErrorDialog("Error. \nDetails: " + ex.Message);
            }
        }
        private void btnLoadStrategy(object sender, ItemClickEventArgs e)
        {
            this.LoadAnalyse();
        }
        
        private void btnSbobetGetInfo_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.InitCoreSbobetEngine();
            }
            catch (System.Exception ex)
            {
                this.ShowErrorDialog("Error while initialize SBOBET Engine. \nDetails: " + ex.Message);
            }
        }
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.InitCoreIbetEngine();                
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
            //this._dataService.AllowRunAsync(this._currentUserID, this._ibetAccount.ToUpper(), this._sbobetAccount.ToUpper());
            this.Start();
            //this._dataService.StartTerminalAsync(this._currentUserID, this.Text);
            //this._ibetEngine.UpdateDataInterval = (int)this.txtIBETUpdateInterval.Value * 1000;
            //this._sbobetEngine.UpdateDataInterval = (int)this.txtSBOBETUpdateInterval.Value * 1000;
        }
        private void btnStop_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Stop();
            this._dataService.StopTerminalAsync(this._currentUserID, this.Text);
        }
        private void btnSnapShot_ItemClick(object sender, ItemClickEventArgs e)
        {
            this._ibetMatchsSnapShot = new Dictionary<string,IbetMatch>(this._ibetEngine.ibetAgent.parser.LdicMatches[1]);
            this.btnSnapShot.Caption = "Snap Shot on " + DateTime.Now.ToShortTimeString();
        }
        private void btnClear_ItemClick(object sender, ItemClickEventArgs e)
        {
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
        private void txtSBOBETUpdateInterval_EditValueChanged(object sender, System.EventArgs e)
        {
        }
        private void btnSetUpdateInterval_Click(object sender, System.EventArgs e)
        {
            SetInterval();
        }
        internal void SetInterval()
        {
            if (this._sbobetEngine != null)
            {
                this._sbobetEngine.Stop();
                this._sbobetEngine.UpdateDataInterval = (int)this.txtSBOBETUpdateInterval.Value * 1000;
                this._sbobetEngine.Start();
            }

            if (this._ibetEngine != null)
            {
                this._ibetEngine.Stop();
                this._ibetEngine.UpdateDataInterval = (int)this.txtIBETUpdateInterval.Value * 1000;
                this._ibetEngine.Start();
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
            this.components = new System.ComponentModel.Container();
            this.gridColumn23 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.iNew = new DevExpress.XtraBars.BarButtonItem();
            this.iNew2 = new DevExpress.XtraBars.BarButtonItem();
            this.iNewR = new DevExpress.XtraBars.BarButtonItem();
            this.iNewR2IB = new DevExpress.XtraBars.BarButtonItem();
            this.iLoadStrategy = new DevExpress.XtraBars.BarButtonItem();
            this.pmNew = new DevExpress.XtraBars.PopupMenu(this.components);
            this.pmNewR = new DevExpress.XtraBars.PopupMenu(this.components);
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
            this.btnSnapShot = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rpgIbet = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpgSbobet = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageMatches = new DevExpress.XtraTab.XtraTabPage();
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
            this.xtraTabPageBetList1 = new DevExpress.XtraTab.XtraTabPage();
            this.girdBetList1 = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn33 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn24 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn25 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn28 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn29 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn30 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn31 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn32 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabPageBetList2 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.checkEdit18 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit17 = new DevExpress.XtraEditors.CheckEdit();
            this.spinEdit4 = new DevExpress.XtraEditors.SpinEdit();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.spinEdit3 = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.spinEdit2 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.txtTransactionTimeSpan = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.txtMaxTimePerHalf = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.chbAllowHalftime = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.checkEdit15 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit14 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit13 = new DevExpress.XtraEditors.CheckEdit();
            this.btnSetUpdateInterval = new DevExpress.XtraEditors.SimpleButton();
            this.txtSBOBETUpdateInterval = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtIBETUpdateInterval = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
            this.chbHighRevenueBoost = new DevExpress.XtraEditors.CheckEdit();
            this.txtAddValue = new DevExpress.XtraEditors.SpinEdit();
            this.txtLowestOddValue = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.txtOddValueDifferenet = new DevExpress.XtraEditors.SpinEdit();
            this.checkEdit6 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit5 = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl18 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.checkEdit7 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit12 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit11 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit10 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit9 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit8 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit4 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit3 = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.txtSBOBETFixedStake = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtStake = new DevExpress.XtraEditors.MemoEdit();
            this.chbRandomStake = new DevExpress.XtraEditors.CheckEdit();
            this.txtIBETFixedStake = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.chkListAllowedMatch = new DevExpress.XtraEditors.CheckedListBoxControl();
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
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl9 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl10 = new DevExpress.XtraEditors.PanelControl();
            this.btnIBETGO2 = new DevExpress.XtraEditors.SimpleButton();
            this.txtIBETAddress2 = new DevExpress.XtraEditors.TextEdit();
            this.xtraTabPage9 = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.pmNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmNewR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPageMatches.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSameMatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.xtraTabPageBetList1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.girdBetList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).BeginInit();
            this.xtraTabControl2.SuspendLayout();
            this.xtraTabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit18.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit17.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransactionTimeSpan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxTimePerHalf.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAllowHalftime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit15.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit14.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit13.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBOBETUpdateInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETUpdateInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
            this.groupControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chbHighRevenueBoost.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLowestOddValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOddValueDifferenet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit6.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit5.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit7.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit12.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit11.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit10.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit9.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit8.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBOBETFixedStake.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStake.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbRandomStake.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETFixedStake.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkListAllowedMatch)).BeginInit();
            this.xtraTabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTransaction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl10)).BeginInit();
            this.panelControl10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETAddress2.Properties)).BeginInit();
            this.xtraTabPage9.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridColumn23
            // 
            this.gridColumn23.Caption = "Kick Off";
            this.gridColumn23.FieldName = "KickOffTime";
            this.gridColumn23.Name = "gridColumn23";
            this.gridColumn23.Visible = true;
            this.gridColumn23.VisibleIndex = 8;
            this.gridColumn23.Width = 150;
            // 
            // iNew
            // 
            this.iNew.Caption = "Open Website";
            this.iNew.CategoryGuid = new System.Guid("4b511317-d784-42ba-b4ed-0d2a746d6c1f");
            this.iNew.Description = "Open Website";
            this.iNew.Enabled = false;
            this.iNew.Hint = "Open main web";
            this.iNew.Id = 0;
            this.iNew.ImageIndex = 6;
            this.iNew.LargeImageIndex = 0;
            this.iNew.Name = "iNew";
            this.iNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnViewIbetWeb);
            // 
            // iNew2
            // 
            this.iNew2.Caption = "Open 2nd Website";
            this.iNew2.CategoryGuid = new System.Guid("1b511317-d784-42ba-b4ed-0d2a746d6c1f");
            this.iNew2.Description = "Open 2nd Website";
            this.iNew2.Enabled = false;
            this.iNew2.Hint = "Open main web";
            this.iNew2.Id = 0;
            this.iNew2.ImageIndex = 6;
            this.iNew2.LargeImageIndex = 0;
            this.iNew2.Name = "iNew2";
            this.iNew2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnViewIbet2Web);
            // 
            // iNewR
            // 
            this.iNewR.Caption = "Open Website";
            this.iNewR.CategoryGuid = new System.Guid("ab511317-d784-42ba-b4ed-0d2a746d6c1f");
            this.iNewR.Description = "Open Website";
            this.iNewR.Enabled = false;
            this.iNewR.Hint = "Open main web";
            this.iNewR.Id = 0;
            this.iNewR.ImageIndex = 6;
            this.iNewR.LargeImageIndex = 0;
            this.iNewR.Name = "iNewR";
            this.iNewR.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnViewSboWeb);
            // 
            // iNewR2IB
            // 
            this.iNewR2IB.Caption = "Login to 2nd IBET";
            this.iNewR2IB.CategoryGuid = new System.Guid("ab511317-d784-42ba-b4ed-0d2a746d6c1f");
            this.iNewR2IB.Description = "2nd IB";
            this.iNewR2IB.Enabled = false;
            this.iNewR2IB.Hint = "2nd IB";
            this.iNewR2IB.Id = 0;
            this.iNewR2IB.ImageIndex = 6;
            this.iNewR2IB.LargeImageIndex = 0;
            this.iNewR2IB.Name = "iNewR2IB";
            this.iNewR2IB.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLoginIbet2);
            // 
            // iLoadStrategy
            // 
            this.iLoadStrategy.Caption = "Load Strategy";
            this.iLoadStrategy.Description = "Load Strategy";
            this.iLoadStrategy.Enabled = false;
            this.iLoadStrategy.Hint = "Best Strategy";
            this.iLoadStrategy.Id = 0;
            this.iLoadStrategy.ImageIndex = 6;
            this.iLoadStrategy.LargeImageIndex = 0;
            this.iLoadStrategy.Name = "iLoadStrategy";
            this.iLoadStrategy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLoadStrategy);
            // 
            // pmNew
            // 
            this.pmNew.ItemLinks.Add(this.iNew);
            this.pmNew.ItemLinks.Add(this.iLoadStrategy);
            this.pmNew.ItemLinks.Add(this.iNewR2IB);
            this.pmNew.ItemLinks.Add(this.iNew2);
            this.pmNew.Name = "pmNew";
            // 
            // pmNewR
            // 
            this.pmNewR.ItemLinks.Add(this.iNewR);

            this.pmNewR.Name = "pmNewR";
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
            this.iNew,
            this.iNew2,
            this.iNewR,
            this.iNewR2IB,
            this.iLoadStrategy,
            this.lblStatus,
            this.lblSameMatch,
            this.lblLastUpdate,
            this.btnStart,
            this.btnStop,
            this.btnClear,
            this.btnSnapShot});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 23;
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
            this.btnSbobetGetInfo.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.btnSbobetGetInfo.Caption = "Log In";
            this.btnSbobetGetInfo.DropDownControl = this.pmNewR;
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
            this.barStaticItem8.Caption = "Total Match:";
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
            this.btnIbetGetInfo.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.btnIbetGetInfo.Caption = "Log In";
            this.btnIbetGetInfo.DropDownControl = this.pmNew;
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
            this.lblSameMatch.Caption = "Total Same Match: -";
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
            // btnSnapShot
            // 
            this.btnSnapShot.Caption = "Snap Shot";
            this.btnSnapShot.Enabled = false;
            this.btnSnapShot.Id = 21;
            this.btnSnapShot.LargeGlyph = global::iBet.App.Properties.Resources.i11;
            this.btnSnapShot.Name = "btnSnapShot";
            this.btnSnapShot.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSnapShot_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rpgIbet,
            this.rpgSbobet,
            this.ribbonPageGroup3,
            this.ribbonPageGroup4,
            this.ribbonPageGroup5});
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
            this.rpgSbobet.Text = "SBOBET";
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
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.ItemLinks.Add(this.btnSnapShot);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            this.ribbonPageGroup5.ShowCaptionButton = false;
            this.ribbonPageGroup5.Text = "Match Data";
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
            this.splitContainerControl1.Size = new System.Drawing.Size(1130, 553);
            this.splitContainerControl1.SplitterPosition = 275;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPageMatches;
            this.xtraTabControl1.Size = new System.Drawing.Size(1130, 273);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageMatches,
            this.xtraTabPageBetList1,
            this.xtraTabPageBetList2});
            // 
            // xtraTabPageMatches
            // 
            this.xtraTabPageMatches.Controls.Add(this.grdSameMatch);
            this.xtraTabPageMatches.Name = "xtraTabPageMatches";
            this.xtraTabPageMatches.Size = new System.Drawing.Size(1124, 247);
            this.xtraTabPageMatches.Text = "Live Match";
            // 
            // grdSameMatch
            // 
            this.grdSameMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSameMatch.Location = new System.Drawing.Point(0, 0);
            this.grdSameMatch.MainView = this.gridView1;
            this.grdSameMatch.Name = "grdSameMatch";
            this.grdSameMatch.Size = new System.Drawing.Size(1124, 247);
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
            this.gridColumn19,
            this.gridColumn23});
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
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn23, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView1_RowCellStyle);
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
            this.gridColumn16.VisibleIndex = 1;
            this.gridColumn16.Width = 368;
            // 
            // gridColumn21
            // 
            this.gridColumn21.Caption = "HScore";
            this.gridColumn21.FieldName = "HomeScore";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 2;
            // 
            // gridColumn22
            // 
            this.gridColumn22.Caption = "AScore";
            this.gridColumn22.FieldName = "AwayScore";
            this.gridColumn22.Name = "gridColumn22";
            this.gridColumn22.Visible = true;
            this.gridColumn22.VisibleIndex = 3;
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
            this.gridColumn17.Width = 40;
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "Min";
            this.gridColumn18.FieldName = "Minute";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.OptionsColumn.FixedWidth = true;
            this.gridColumn18.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 6;
            this.gridColumn18.Width = 40;
            // 
            // gridColumn19
            // 
            this.gridColumn19.Caption = "HT";
            this.gridColumn19.FieldName = "IsHalfTime";
            this.gridColumn19.Name = "gridColumn19";
            this.gridColumn19.OptionsColumn.FixedWidth = true;
            this.gridColumn19.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn19.Visible = true;
            this.gridColumn19.VisibleIndex = 7;
            this.gridColumn19.Width = 40;
            // 
            // xtraTabPageBetList1
            // 
            this.xtraTabPageBetList1.Controls.Add(this.girdBetList1);
            this.xtraTabPageBetList1.Name = "xtraTabPageBetList1";
            this.xtraTabPageBetList1.Size = new System.Drawing.Size(1124, 247);
            this.xtraTabPageBetList1.Text = "Bet List 1";
            // 
            // girdBetList1
            // 
            this.girdBetList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.girdBetList1.Location = new System.Drawing.Point(0, 0);
            this.girdBetList1.MainView = this.gridView3;
            this.girdBetList1.Name = "girdBetList1";
            this.girdBetList1.Size = new System.Drawing.Size(1124, 247);
            this.girdBetList1.TabIndex = 4;
            this.girdBetList1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn33,
            this.gridColumn24,
            this.gridColumn25,
            this.gridColumn28,
            this.gridColumn29,
            this.gridColumn30,
            this.gridColumn31,
            this.gridColumn32});
            this.gridView3.GridControl = this.girdBetList1;
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
            this.gridView3.PreviewFieldName = "LeagueName";
            this.gridView3.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn32, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // gridColumn33
            // 
            this.gridColumn33.Caption = "RefID";
            this.gridColumn33.FieldName = "ID";
            this.gridColumn33.Name = "gridColumn33";
            this.gridColumn33.Visible = true;
            this.gridColumn33.VisibleIndex = 0;
            this.gridColumn33.Width = 69;
            // 
            // gridColumn24
            // 
            this.gridColumn24.Caption = "Home Team";
            this.gridColumn24.FieldName = "Home";
            this.gridColumn24.Name = "gridColumn24";
            this.gridColumn24.Visible = true;
            this.gridColumn24.VisibleIndex = 1;
            this.gridColumn24.Width = 300;
            // 
            // gridColumn25
            // 
            this.gridColumn25.Caption = "Away Team";
            this.gridColumn25.FieldName = "Away";
            this.gridColumn25.Name = "gridColumn25";
            this.gridColumn25.Visible = true;
            this.gridColumn25.VisibleIndex = 2;
            this.gridColumn25.Width = 300;
            // 
            // gridColumn28
            // 
            this.gridColumn28.Caption = "Choose";
            this.gridColumn28.FieldName = "Choice";
            this.gridColumn28.Name = "gridColumn28";
            this.gridColumn28.OptionsColumn.FixedWidth = true;
            this.gridColumn28.Visible = true;
            this.gridColumn28.VisibleIndex = 3;
            this.gridColumn28.Width = 70;
            // 
            // gridColumn29
            // 
            this.gridColumn29.Caption = "Odd";
            this.gridColumn29.FieldName = "Handicap";
            this.gridColumn29.Name = "gridColumn29";
            this.gridColumn29.OptionsColumn.FixedWidth = true;
            this.gridColumn29.Visible = true;
            this.gridColumn29.VisibleIndex = 4;
            this.gridColumn29.Width = 40;
            // 
            // gridColumn30
            // 
            this.gridColumn30.Caption = "Odd Value";
            this.gridColumn30.FieldName = "OddsValue";
            this.gridColumn30.Name = "gridColumn30";
            this.gridColumn30.OptionsColumn.FixedWidth = true;
            this.gridColumn30.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.gridColumn30.Visible = true;
            this.gridColumn30.VisibleIndex = 5;
            this.gridColumn30.Width = 60;
            // 
            // gridColumn31
            // 
            this.gridColumn31.Caption = "Stake";
            this.gridColumn31.FieldName = "Stake";
            this.gridColumn31.Name = "gridColumn31";
            this.gridColumn31.OptionsColumn.FixedWidth = true;
            this.gridColumn31.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn31.Visible = true;
            this.gridColumn31.VisibleIndex = 6;
            this.gridColumn31.Width = 40;
            // 
            // gridColumn32
            // 
            this.gridColumn32.Caption = "Bet Time";
            this.gridColumn32.FieldName = "BetTime";
            this.gridColumn32.Name = "gridColumn32";
            this.gridColumn32.Visible = true;
            this.gridColumn32.VisibleIndex = 7;
            this.gridColumn32.Width = 227;
            // 
            // xtraTabPageBetList2
            // 
            this.xtraTabPageBetList2.Name = "xtraTabPageBetList2";
            this.xtraTabPageBetList2.Size = new System.Drawing.Size(1124, 247);
            this.xtraTabPageBetList2.Text = "Bet List 2";
            // 
            // xtraTabControl2
            // 
            this.xtraTabControl2.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.xtraTabControl2.Appearance.Options.UseBackColor = true;
            this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl2.Name = "xtraTabControl2";
            this.xtraTabControl2.SelectedTabPage = this.xtraTabPage4;
            this.xtraTabControl2.Size = new System.Drawing.Size(1130, 275);
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
            this.xtraTabPage4.Size = new System.Drawing.Size(1124, 249);
            this.xtraTabPage4.Text = "Settings";
            // 
            // groupControl2
            // 
            this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupControl2.Controls.Add(this.checkEdit2);
            this.groupControl2.Controls.Add(this.labelControl12);
            this.groupControl2.Controls.Add(this.labelControl11);
            this.groupControl2.Controls.Add(this.checkEdit18);
            this.groupControl2.Controls.Add(this.checkEdit17);
            this.groupControl2.Controls.Add(this.spinEdit4);
            this.groupControl2.Controls.Add(this.checkEdit1);
            this.groupControl2.Controls.Add(this.spinEdit3);
            this.groupControl2.Controls.Add(this.labelControl13);
            this.groupControl2.Controls.Add(this.spinEdit2);
            this.groupControl2.Controls.Add(this.spinEdit1);
            this.groupControl2.Controls.Add(this.txtTransactionTimeSpan);
            this.groupControl2.Controls.Add(this.labelControl10);
            this.groupControl2.Controls.Add(this.txtMaxTimePerHalf);
            this.groupControl2.Controls.Add(this.labelControl7);
            this.groupControl2.Controls.Add(this.chbAllowHalftime);
            this.groupControl2.Location = new System.Drawing.Point(209, 3);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(200, 243);
            this.groupControl2.TabIndex = 14;
            this.groupControl2.Text = "Time Settings";
            // 
            // checkEdit2
            // 
            this.checkEdit2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit2.Location = new System.Drawing.Point(135, 191);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Properties.Caption = "O/U";
            this.checkEdit2.Size = new System.Drawing.Size(60, 19);
            this.checkEdit2.TabIndex = 24;
            this.checkEdit2.CheckedChanged += new System.EventHandler(this.checkEdit2_CheckedChanged);
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(7, 140);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(56, 13);
            this.labelControl12.TabIndex = 23;
            this.labelControl12.Text = "Flush Half 2";
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(7, 112);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(56, 13);
            this.labelControl11.TabIndex = 22;
            this.labelControl11.Text = "Flush Half 1";
            // 
            // checkEdit18
            // 
            this.checkEdit18.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit18.EditValue = true;
            this.checkEdit18.Location = new System.Drawing.Point(69, 191);
            this.checkEdit18.Name = "checkEdit18";
            this.checkEdit18.Properties.Caption = "Half 2";
            this.checkEdit18.Size = new System.Drawing.Size(60, 19);
            this.checkEdit18.TabIndex = 21;
            // 
            // checkEdit17
            // 
            this.checkEdit17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit17.EditValue = true;
            this.checkEdit17.Location = new System.Drawing.Point(5, 191);
            this.checkEdit17.Name = "checkEdit17";
            this.checkEdit17.Properties.Caption = "Half 1";
            this.checkEdit17.Size = new System.Drawing.Size(58, 19);
            this.checkEdit17.TabIndex = 20;
            // 
            // spinEdit4
            // 
            this.spinEdit4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spinEdit4.EditValue = new decimal(new int[] {
            2,
            0,
            0,
            131072});
            this.spinEdit4.Location = new System.Drawing.Point(85, 166);
            this.spinEdit4.Name = "spinEdit4";
            this.spinEdit4.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEdit4.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.spinEdit4.Size = new System.Drawing.Size(52, 20);
            this.spinEdit4.TabIndex = 17;
            // 
            // checkEdit1
            // 
            this.checkEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit1.EditValue = true;
            this.checkEdit1.Location = new System.Drawing.Point(5, 166);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "Take Profit";
            this.checkEdit1.Size = new System.Drawing.Size(77, 19);
            this.checkEdit1.TabIndex = 16;
            // 
            // spinEdit3
            // 
            this.spinEdit3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spinEdit3.EditValue = new decimal(new int[] {
            15,
            0,
            0,
            131072});
            this.spinEdit3.Location = new System.Drawing.Point(143, 166);
            this.spinEdit3.Name = "spinEdit3";
            this.spinEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEdit3.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            -2147352576});
            this.spinEdit3.Properties.MinValue = new decimal(new int[] {
            2,
            0,
            0,
            -2147352576});
            this.spinEdit3.Size = new System.Drawing.Size(52, 20);
            this.spinEdit3.TabIndex = 15;
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(5, 169);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(0, 13);
            this.labelControl13.TabIndex = 14;
            // 
            // spinEdit2
            // 
            this.spinEdit2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spinEdit2.EditValue = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.spinEdit2.Location = new System.Drawing.Point(123, 137);
            this.spinEdit2.Name = "spinEdit2";
            this.spinEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEdit2.Properties.IsFloatValue = false;
            this.spinEdit2.Properties.Mask.EditMask = "N00";
            this.spinEdit2.Size = new System.Drawing.Size(72, 20);
            this.spinEdit2.TabIndex = 13;
            // 
            // spinEdit1
            // 
            this.spinEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spinEdit1.EditValue = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(123, 109);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEdit1.Properties.IsFloatValue = false;
            this.spinEdit1.Properties.Mask.EditMask = "N00";
            this.spinEdit1.Size = new System.Drawing.Size(72, 20);
            this.spinEdit1.TabIndex = 11;
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
            40,
            0,
            0,
            0});
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
            this.chbAllowHalftime.Location = new System.Drawing.Point(5, 26);
            this.chbAllowHalftime.Name = "chbAllowHalftime";
            this.chbAllowHalftime.Properties.Caption = "Allow Halftime";
            this.chbAllowHalftime.Size = new System.Drawing.Size(102, 19);
            this.chbAllowHalftime.TabIndex = 5;
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupControl1.Controls.Add(this.checkEdit15);
            this.groupControl1.Controls.Add(this.checkEdit14);
            this.groupControl1.Controls.Add(this.checkEdit13);
            this.groupControl1.Controls.Add(this.btnSetUpdateInterval);
            this.groupControl1.Controls.Add(this.txtSBOBETUpdateInterval);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.txtIBETUpdateInterval);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Location = new System.Drawing.Point(415, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(200, 243);
            this.groupControl1.TabIndex = 13;
            this.groupControl1.Text = "Data Settings";
            // 
            // checkEdit15
            // 
            this.checkEdit15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit15.Location = new System.Drawing.Point(5, 151);
            this.checkEdit15.Name = "checkEdit15";
            this.checkEdit15.Properties.Caption = "Scanner mode: Pure odd sms";
            this.checkEdit15.Size = new System.Drawing.Size(190, 19);
            this.checkEdit15.TabIndex = 30;
            // 
            // checkEdit14
            // 
            this.checkEdit14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit14.EditValue = true;
            this.checkEdit14.Location = new System.Drawing.Point(5, 128);
            this.checkEdit14.Name = "checkEdit14";
            this.checkEdit14.Properties.Caption = "Receive odd from community";
            this.checkEdit14.Size = new System.Drawing.Size(190, 19);
            this.checkEdit14.TabIndex = 29;
            // 
            // checkEdit13
            // 
            this.checkEdit13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit13.EditValue = true;
            this.checkEdit13.Location = new System.Drawing.Point(5, 104);
            this.checkEdit13.Name = "checkEdit13";
            this.checkEdit13.Properties.Caption = "Submit odd to community";
            this.checkEdit13.Size = new System.Drawing.Size(190, 19);
            this.checkEdit13.TabIndex = 28;
            // 
            // btnSetUpdateInterval
            // 
            this.btnSetUpdateInterval.Location = new System.Drawing.Point(59, 77);
            this.btnSetUpdateInterval.Name = "btnSetUpdateInterval";
            this.btnSetUpdateInterval.Size = new System.Drawing.Size(136, 23);
            this.btnSetUpdateInterval.TabIndex = 6;
            this.btnSetUpdateInterval.Text = "Set Update Interval";
            this.btnSetUpdateInterval.Click += new System.EventHandler(this.btnSetUpdateInterval_Click);
            // 
            // txtSBOBETUpdateInterval
            // 
            this.txtSBOBETUpdateInterval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSBOBETUpdateInterval.EditValue = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.txtSBOBETUpdateInterval.Location = new System.Drawing.Point(132, 51);
            this.txtSBOBETUpdateInterval.Name = "txtSBOBETUpdateInterval";
            this.txtSBOBETUpdateInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSBOBETUpdateInterval.Properties.IsFloatValue = false;
            this.txtSBOBETUpdateInterval.Properties.Mask.EditMask = "N00";
            this.txtSBOBETUpdateInterval.Size = new System.Drawing.Size(63, 20);
            this.txtSBOBETUpdateInterval.TabIndex = 5;
            this.txtSBOBETUpdateInterval.EditValueChanged += new System.EventHandler(this.txtSBOBETUpdateInterval_EditValueChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(5, 54);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(121, 13);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "SBOBET Update Interval:";
            // 
            // txtIBETUpdateInterval
            // 
            this.txtIBETUpdateInterval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIBETUpdateInterval.EditValue = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.txtIBETUpdateInterval.Location = new System.Drawing.Point(132, 25);
            this.txtIBETUpdateInterval.Name = "txtIBETUpdateInterval";
            this.txtIBETUpdateInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtIBETUpdateInterval.Properties.IsFloatValue = false;
            this.txtIBETUpdateInterval.Properties.Mask.EditMask = "N00";
            this.txtIBETUpdateInterval.Size = new System.Drawing.Size(63, 20);
            this.txtIBETUpdateInterval.TabIndex = 1;
            this.txtIBETUpdateInterval.EditValueChanged += new System.EventHandler(this.txtIBETUpdateInterval_EditValueChanged);
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
            this.groupControl5.Controls.Add(this.chbHighRevenueBoost);
            this.groupControl5.Controls.Add(this.txtAddValue);
            this.groupControl5.Controls.Add(this.txtLowestOddValue);
            this.groupControl5.Controls.Add(this.labelControl9);
            this.groupControl5.Controls.Add(this.txtOddValueDifferenet);
            this.groupControl5.Controls.Add(this.checkEdit6);
            this.groupControl5.Controls.Add(this.checkEdit5);
            this.groupControl5.Controls.Add(this.labelControl18);
            this.groupControl5.Controls.Add(this.labelControl8);
            this.groupControl5.Controls.Add(this.checkEdit7);
            this.groupControl5.Controls.Add(this.checkEdit12);
            this.groupControl5.Controls.Add(this.checkEdit11);
            this.groupControl5.Controls.Add(this.checkEdit10);
            this.groupControl5.Controls.Add(this.checkEdit9);
            this.groupControl5.Controls.Add(this.checkEdit8);
            this.groupControl5.Controls.Add(this.checkEdit4);
            this.groupControl5.Controls.Add(this.checkEdit3);
            this.groupControl5.Location = new System.Drawing.Point(3, 3);
            this.groupControl5.Name = "groupControl5";
            this.groupControl5.Size = new System.Drawing.Size(200, 243);
            this.groupControl5.TabIndex = 13;
            this.groupControl5.Text = "Trading Settings";
            // 
            // chbHighRevenueBoost
            // 
            this.chbHighRevenueBoost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chbHighRevenueBoost.Location = new System.Drawing.Point(5, 172);
            this.chbHighRevenueBoost.Name = "chbHighRevenueBoost";
            this.chbHighRevenueBoost.Properties.Caption = "Top Class";
            this.chbHighRevenueBoost.Size = new System.Drawing.Size(90, 19);
            this.chbHighRevenueBoost.TabIndex = 13;
            // 
            // txtAddValue
            // 
            this.txtAddValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddValue.EditValue = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.txtAddValue.Location = new System.Drawing.Point(110, 148);
            this.txtAddValue.Name = "txtAddValue";
            this.txtAddValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtAddValue.Size = new System.Drawing.Size(85, 20);
            this.txtAddValue.TabIndex = 22;
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
            this.txtLowestOddValue.Location = new System.Drawing.Point(110, 121);
            this.txtLowestOddValue.Name = "txtLowestOddValue";
            this.txtLowestOddValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtLowestOddValue.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.txtLowestOddValue.Properties.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.txtLowestOddValue.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.txtLowestOddValue.Size = new System.Drawing.Size(85, 20);
            this.txtLowestOddValue.TabIndex = 12;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(5, 123);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(90, 13);
            this.labelControl9.TabIndex = 11;
            this.labelControl9.Text = "Lowest Odd Value:";
            // 
            // txtOddValueDifferenet
            // 
            this.txtOddValueDifferenet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOddValueDifferenet.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            -2147352576});
            this.txtOddValueDifferenet.Location = new System.Drawing.Point(110, 94);
            this.txtOddValueDifferenet.Name = "txtOddValueDifferenet";
            this.txtOddValueDifferenet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtOddValueDifferenet.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            -2147352576});
            this.txtOddValueDifferenet.Properties.MinValue = new decimal(new int[] {
            2,
            0,
            0,
            -2147352576});
            this.txtOddValueDifferenet.Size = new System.Drawing.Size(85, 20);
            this.txtOddValueDifferenet.TabIndex = 10;
            // 
            // checkEdit6
            // 
            this.checkEdit6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit6.EditValue = true;
            this.checkEdit6.Location = new System.Drawing.Point(108, 51);
            this.checkEdit6.Name = "checkEdit6";
            this.checkEdit6.Properties.Caption = "Non-Live";
            this.checkEdit6.Size = new System.Drawing.Size(77, 19);
            this.checkEdit6.TabIndex = 8;
            // 
            // checkEdit5
            // 
            this.checkEdit5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit5.EditValue = true;
            this.checkEdit5.Location = new System.Drawing.Point(5, 72);
            this.checkEdit5.Name = "checkEdit5";
            this.checkEdit5.Properties.Caption = "Live";
            this.checkEdit5.Size = new System.Drawing.Size(52, 19);
            this.checkEdit5.TabIndex = 7;
            // 
            // labelControl18
            // 
            this.labelControl18.Location = new System.Drawing.Point(5, 150);
            this.labelControl18.Name = "labelControl18";
            this.labelControl18.Size = new System.Drawing.Size(95, 13);
            this.labelControl18.TabIndex = 29;
            this.labelControl18.Text = "Min of |Won-Lose|: ";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(5, 97);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(99, 13);
            this.labelControl8.TabIndex = 9;
            this.labelControl8.Text = "Odd Value Different:";
            // 
            // checkEdit7
            // 
            this.checkEdit7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit7.Location = new System.Drawing.Point(108, 221);
            this.checkEdit7.Name = "checkEdit7";
            this.checkEdit7.Properties.Caption = "Over 92/90";
            this.checkEdit7.Size = new System.Drawing.Size(83, 19);
            this.checkEdit7.TabIndex = 9;
            // 
            // checkEdit12
            // 
            this.checkEdit12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit12.Location = new System.Drawing.Point(108, 196);
            this.checkEdit12.Name = "checkEdit12";
            this.checkEdit12.Properties.Caption = "Under Equal";
            this.checkEdit12.Size = new System.Drawing.Size(83, 19);
            this.checkEdit12.TabIndex = 23;
            // 
            // checkEdit11
            // 
            this.checkEdit11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit11.Location = new System.Drawing.Point(5, 221);
            this.checkEdit11.Name = "checkEdit11";
            this.checkEdit11.Properties.Caption = "Over 1.75";
            this.checkEdit11.Size = new System.Drawing.Size(95, 19);
            this.checkEdit11.TabIndex = 19;
            // 
            // checkEdit10
            // 
            this.checkEdit10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit10.Location = new System.Drawing.Point(5, 47);
            this.checkEdit10.Name = "checkEdit10";
            this.checkEdit10.Properties.Caption = "Matches List";
            this.checkEdit10.Size = new System.Drawing.Size(90, 19);
            this.checkEdit10.TabIndex = 19;
            // 
            // checkEdit9
            // 
            this.checkEdit9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit9.Location = new System.Drawing.Point(108, 172);
            this.checkEdit9.Name = "checkEdit9";
            this.checkEdit9.Properties.Caption = "Odd Down";
            this.checkEdit9.Size = new System.Drawing.Size(76, 19);
            this.checkEdit9.TabIndex = 19;
            // 
            // checkEdit8
            // 
            this.checkEdit8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit8.Location = new System.Drawing.Point(5, 196);
            this.checkEdit8.Name = "checkEdit8";
            this.checkEdit8.Properties.Caption = "Over Equal";
            this.checkEdit8.Size = new System.Drawing.Size(95, 19);
            this.checkEdit8.TabIndex = 14;
            // 
            // checkEdit4
            // 
            this.checkEdit4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit4.EditValue = true;
            this.checkEdit4.Location = new System.Drawing.Point(108, 25);
            this.checkEdit4.Name = "checkEdit4";
            this.checkEdit4.Properties.Caption = "Over/Under";
            this.checkEdit4.Size = new System.Drawing.Size(87, 19);
            this.checkEdit4.TabIndex = 6;
            // 
            // checkEdit3
            // 
            this.checkEdit3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEdit3.EditValue = true;
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
            this.groupControl4.Controls.Add(this.txtSBOBETFixedStake);
            this.groupControl4.Controls.Add(this.labelControl2);
            this.groupControl4.Controls.Add(this.txtStake);
            this.groupControl4.Controls.Add(this.chbRandomStake);
            this.groupControl4.Controls.Add(this.txtIBETFixedStake);
            this.groupControl4.Controls.Add(this.labelControl6);
            this.groupControl4.Location = new System.Drawing.Point(621, 3);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(200, 243);
            this.groupControl4.TabIndex = 12;
            this.groupControl4.Text = "Stake Settings";
            // 
            // txtSBOBETFixedStake
            // 
            this.txtSBOBETFixedStake.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSBOBETFixedStake.EditValue = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.txtSBOBETFixedStake.Location = new System.Drawing.Point(112, 51);
            this.txtSBOBETFixedStake.Name = "txtSBOBETFixedStake";
            this.txtSBOBETFixedStake.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSBOBETFixedStake.Properties.IsFloatValue = false;
            this.txtSBOBETFixedStake.Properties.Mask.EditMask = "N00";
            this.txtSBOBETFixedStake.Size = new System.Drawing.Size(83, 20);
            this.txtSBOBETFixedStake.TabIndex = 5;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(5, 54);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(101, 13);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "SBOBET Fixed Stake:";
            // 
            // txtStake
            // 
            this.txtStake.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStake.EditValue = "10\r\n20\r\n15\r\n5";
            this.txtStake.Location = new System.Drawing.Point(5, 103);
            this.txtStake.Name = "txtStake";
            this.txtStake.Size = new System.Drawing.Size(190, 135);
            this.txtStake.TabIndex = 3;
            // 
            // chbRandomStake
            // 
            this.chbRandomStake.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chbRandomStake.EditValue = true;
            this.chbRandomStake.Location = new System.Drawing.Point(3, 78);
            this.chbRandomStake.Name = "chbRandomStake";
            this.chbRandomStake.Properties.Caption = "Random Stake";
            this.chbRandomStake.Size = new System.Drawing.Size(192, 19);
            this.chbRandomStake.TabIndex = 2;
            this.chbRandomStake.CheckedChanged += new System.EventHandler(this.chbRandomStake_CheckedChanged);
            // 
            // txtIBETFixedStake
            // 
            this.txtIBETFixedStake.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIBETFixedStake.EditValue = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.txtIBETFixedStake.Location = new System.Drawing.Point(112, 25);
            this.txtIBETFixedStake.Name = "txtIBETFixedStake";
            this.txtIBETFixedStake.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtIBETFixedStake.Properties.IsFloatValue = false;
            this.txtIBETFixedStake.Properties.Mask.EditMask = "N00";
            this.txtIBETFixedStake.Size = new System.Drawing.Size(83, 20);
            this.txtIBETFixedStake.TabIndex = 1;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(5, 28);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(85, 13);
            this.labelControl6.TabIndex = 0;
            this.labelControl6.Text = "IBET Fixed Stake:";
            // 
            // groupControl6
            // 
            this.groupControl6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupControl6.Controls.Add(this.chkListAllowedMatch);
            this.groupControl6.Location = new System.Drawing.Point(827, 3);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.Size = new System.Drawing.Size(293, 243);
            this.groupControl6.TabIndex = 19;
            this.groupControl6.Text = "Allowed Betting Matches";
            // 
            // chkListAllowedMatch
            // 
            this.chkListAllowedMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkListAllowedMatch.CheckOnClick = true;
            this.chkListAllowedMatch.HighlightedItemStyle = DevExpress.XtraEditors.HighlightStyle.Skinned;
            this.chkListAllowedMatch.ItemHeight = 16;
            this.chkListAllowedMatch.Location = new System.Drawing.Point(5, 25);
            this.chkListAllowedMatch.Name = "chkListAllowedMatch";
            this.chkListAllowedMatch.Size = new System.Drawing.Size(283, 213);
            this.chkListAllowedMatch.TabIndex = 20;
            // 
            // xtraTabPage5
            // 
            this.xtraTabPage5.Controls.Add(this.grdTransaction);
            this.xtraTabPage5.Name = "xtraTabPage5";
            this.xtraTabPage5.Size = new System.Drawing.Size(1124, 249);
            this.xtraTabPage5.Text = "Live Transaction";
            // 
            // grdTransaction
            // 
            this.grdTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTransaction.Location = new System.Drawing.Point(0, 0);
            this.grdTransaction.MainView = this.gridView2;
            this.grdTransaction.Name = "grdTransaction";
            this.grdTransaction.Size = new System.Drawing.Size(1124, 249);
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
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn20,
            this.gridColumn13});
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
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 20;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Away Team";
            this.gridColumn3.FieldName = "AwayTeamName";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 20;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Type";
            this.gridColumn6.FieldName = "OddType";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.FixedWidth = true;
            this.gridColumn6.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
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
            this.gridColumn4.VisibleIndex = 4;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Value";
            this.gridColumn5.FieldName = "OddValue";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.FixedWidth = true;
            this.gridColumn5.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 5;
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
            this.gridColumn7.VisibleIndex = 6;
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
            this.gridColumn8.VisibleIndex = 7;
            this.gridColumn8.Width = 50;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "B Allow";
            this.gridColumn9.FieldName = "SBOBETAllow";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.FixedWidth = true;
            this.gridColumn9.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 8;
            this.gridColumn9.Width = 50;
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
            // gridColumn11
            // 
            this.gridColumn11.Caption = "B Trade";
            this.gridColumn11.FieldName = "SBOBETTrade";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.FixedWidth = true;
            this.gridColumn11.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 10;
            this.gridColumn11.Width = 50;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "B Retrade";
            this.gridColumn12.FieldName = "SBOBETReTrade";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.OptionsColumn.FixedWidth = true;
            this.gridColumn12.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 11;
            this.gridColumn12.Width = 50;
            // 
            // gridColumn20
            // 
            this.gridColumn20.Caption = "I Retrade";
            this.gridColumn20.FieldName = "IBETReTrade";
            this.gridColumn20.Name = "gridColumn20";
            this.gridColumn20.OptionsColumn.FixedWidth = true;
            this.gridColumn20.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn20.Visible = true;
            this.gridColumn20.VisibleIndex = 12;
            this.gridColumn20.Width = 50;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "DateTime";
            this.gridColumn13.DisplayFormat.FormatString = "dd/MM/yyyy hh:mm:ss";
            this.gridColumn13.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn13.FieldName = "DateTime";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.FixedWidth = true;
            this.gridColumn13.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.gridColumn13.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn13.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 13;
            this.gridColumn13.Width = 130;
            // 
            // panelControl5
            // 
            this.panelControl5.Location = new System.Drawing.Point(0, 0);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(200, 100);
            this.panelControl5.TabIndex = 0;
            // 
            // panelControl4
            // 
            this.panelControl4.Location = new System.Drawing.Point(0, 0);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(200, 100);
            this.panelControl4.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Address:";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(0, 0);
            // 
            // panelControl3
            // 
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(200, 100);
            this.panelControl3.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(200, 100);
            this.panelControl2.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(0, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 14);
            this.labelControl1.TabIndex = 0;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(5, 9);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(43, 13);
            this.labelControl5.TabIndex = 6;
            this.labelControl5.Text = "Address:";
            // 
            // panelControl9
            // 
            this.panelControl9.Location = new System.Drawing.Point(0, 0);
            this.panelControl9.Name = "panelControl9";
            this.panelControl9.Size = new System.Drawing.Size(200, 100);
            this.panelControl9.TabIndex = 0;
            // 
            // panelControl10
            // 
            this.panelControl10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl10.Controls.Add(this.btnIBETGO2);
            this.panelControl10.Controls.Add(this.txtIBETAddress2);
            this.panelControl10.Controls.Add(this.labelControl5);
            this.panelControl10.Location = new System.Drawing.Point(3, 3);
            this.panelControl10.Name = "panelControl10";
            this.panelControl10.Size = new System.Drawing.Size(987, 29);
            this.panelControl10.TabIndex = 2;
            // 
            // btnIBETGO2
            // 
            this.btnIBETGO2.Location = new System.Drawing.Point(0, 0);
            this.btnIBETGO2.Name = "btnIBETGO2";
            this.btnIBETGO2.Size = new System.Drawing.Size(75, 23);
            this.btnIBETGO2.TabIndex = 0;
            // 
            // txtIBETAddress2
            // 
            this.txtIBETAddress2.Location = new System.Drawing.Point(0, 0);
            this.txtIBETAddress2.Name = "txtIBETAddress2";
            this.txtIBETAddress2.Size = new System.Drawing.Size(100, 20);
            this.txtIBETAddress2.TabIndex = 1;
            // 
            // xtraTabPage9
            // 
            this.xtraTabPage9.Controls.Add(this.panelControl9);
            this.xtraTabPage9.Controls.Add(this.panelControl10);
            this.xtraTabPage9.Name = "xtraTabPage9";
            this.xtraTabPage9.Size = new System.Drawing.Size(993, 276);
            this.xtraTabPage9.Text = "ibet Test";
            // 
            // TerminalFormIBETSBO
            // 
            this.ClientSize = new System.Drawing.Size(1130, 678);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.ribbonControl1);
            this.Icon = global::iBet.App.Properties.Resources._2;
            this.Name = "TerminalFormIBETSBO";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IBET vs SBOBET";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TerminalFormIBETSBO_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pmNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmNewR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPageMatches.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSameMatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.xtraTabPageBetList1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.girdBetList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).EndInit();
            this.xtraTabControl2.ResumeLayout(false);
            this.xtraTabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit18.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit17.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransactionTimeSpan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxTimePerHalf.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAllowHalftime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit15.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit14.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit13.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBOBETUpdateInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETUpdateInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
            this.groupControl5.ResumeLayout(false);
            this.groupControl5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chbHighRevenueBoost.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLowestOddValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOddValueDifferenet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit6.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit5.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit7.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit12.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit11.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit10.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit9.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit8.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            this.groupControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSBOBETFixedStake.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStake.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbRandomStake.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETFixedStake.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkListAllowedMatch)).EndInit();
            this.xtraTabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTransaction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl10)).EndInit();
            this.panelControl10.ResumeLayout(false);
            this.panelControl10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIBETAddress2.Properties)).EndInit();
            this.xtraTabPage9.ResumeLayout(false);
            this.ResumeLayout(false);

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

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit2.Checked)
            {
                spinEdit1.Text = "28";
                spinEdit2.Text = "28";
            }
            else
            {
                spinEdit1.Text = "32";
                spinEdit2.Text = "32";
            }
        }

        

        

        

        
        
    }
}
