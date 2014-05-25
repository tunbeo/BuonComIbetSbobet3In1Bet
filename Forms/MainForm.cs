#define full
//#define aThanh
using DevExpress.Data;
using DevExpress.LookAndFeel;
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
using DevExpress.XtraBars.Helpers;
using System.Linq;
//using System.Net;

using RulesEngine;
using System.Xml;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Threading;
#if aThanh
using DevExpress.UserSkins;
#endif

using iBet.App.admin ;
using iBet.DTO;
using iBet.Engine;

using BetBroker;

namespace iBet.App
{
    [CallbackBehavior(UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MainForm : RibbonForm, BetBroker.IBrokerServiceCallback
	{
        public static Size HoverSkinImageSize = new Size(116, 86);
        public static Size SkinImageSize = new Size(58, 43);

		private System.ComponentModel.IContainer components = null;
        
        BetBroker.IBrokerService betBrokerSvc = null;
        private SynchronizationContext _uiSyncContext = null;

		private RibbonControl ribbonControl1;
		private RibbonPage ribbonPage1;
		private RibbonPageGroup ribbonPageGroup1;
        private RibbonPageGroup ribbonPageGroup5;
        private SplitContainerControl splitContainerControl1;
        private XtraTabControl xtraTabControl1;
        private XtraTabControl xtraTabControl2;
        private XtraTabPage xtraTabPage1;
        private XtraTabPage xtraTabPage2;
        private XtraTabPage xtraTabPage3;
        private XtraTabPage xtraTabPage4;
        private XtraTabPage xtraTabPage5;

        private PanelControl panelControl1;
        private PanelControl panelControl2;

        private PanelControl panelControl3;
        private PanelControl panelControl4;

		//private DefaultLookAndFeel defaultLookAndFeel1;

		private BarButtonItem btnNewTerminal; // ibet-3in1
        private BarButtonItem btnNewTerminal2;// ibet-sbo
        private BarButtonItem btnNewTerminal3;// sbo-3in1
		private BarButtonItem btnStart;
		private BarButtonItem btnStop;
		private RibbonPageGroup ribbonPageGroup2;
		private BarButtonItem btnClear;
		private RibbonPageGroup ribbonPageGroup3;
		private BarEditItem txtUsername;
		private RepositoryItemTextEdit repositoryItemTextEdit1;
		private BarEditItem txtPassword;
		private RepositoryItemTextEdit repositoryItemTextEdit2;
		private BarButtonItem btnLogin;
		private BarButtonItem btnChangePassword;
		private BarButtonItem btnLogout;
		private RibbonStatusBar ribbonStatusBar1;
		private GridControl grdTransaction;
		private GridView gridView2;
		private GridColumn gridColumn1;
		private GridColumn gridColumn8;
		private GridColumn gridColumn2;
		private GridColumn gridColumn3;
		private GridColumn gridColumn6;
		private GridColumn gridColumn4;
		private GridColumn gridColumn5;
		private GridColumn gridColumn7;
		private GridColumn gridColumn10;
		private GridColumn gridColumn11;
		private GridColumn gridColumn12;
		private GridColumn gridColumn9;
		private GridColumn gridColumn13;
        private GridColumn gridColumn14;
        private GridColumn gridColumn15;
		private BarStaticItem lblSystemTime;
		private BarStaticItem lblTotalTransaction;
		private BarStaticItem lblTotalTerminal;
		private System.Collections.Generic.List<TransactionDTO> _listTransaction;
		private System.Collections.Generic.List<TerminalFormIBET3IN1> _listTerminal;
        private System.Collections.Generic.List<TerminalFormIBETSBO> _listTerminal2;
        private System.Collections.Generic.List<FollowSub> _listTerminal3;
		private admin.DataServiceSoapClient _dataService;
        private string _currentUserID = string.Empty;
        private string _lastOddID = string.Empty;
		private System.Windows.Forms.Timer _systemTimer;
        private System.Windows.Forms.Timer _refreshServerTimer;
        
        public int numTerminal = 0;
        public System.Collections.Generic.List<string> _list3in1Host;
        private WebBrowser webBrowser2;
        private Splitter splitter1;
        private WebBrowser webBrowser1;
        private PictureEdit pictureEdit1;
        private RibbonPageGroup ribbonPageGroup4;
        private BarEditItem barEditItem1;
        private RepositoryItemPopupContainerEdit repositoryItemPopupContainerEdit1;
        private RibbonGalleryBarItem ribbonGalleryBarItem1;
        private BarButtonGroup barButtonGroup1;
        private BarButtonGroup barButtonGroup2;
        private SimpleButton btnBetList;
        private SimpleButton btnStatement;
        private SimpleButton btnSignMeIn;
        private TextEdit txtServerIP;
        private TextEdit txtServerUserName;
        private TextEdit txtServerPassword;
        private Label label3;
        private Label label2;
        private Label label1;
        private CheckEdit chkRCloud;
        private Label label4;
        private SimpleButton simpleButton1;
        private TextEdit textEdit2;
        private TextEdit textEdit1;
        private TextEdit textEdit3;
        private SimpleButton simpleButton2;
        private MemoEdit textEdit4;
        public CheckEdit chkSCloud;
        private RibbonGalleryBarItem skinGalleryBarItem;
        private ComboBoxEdit cbeSignatureTemplate;     
        
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
            DevExpress.XtraBars.Ribbon.GalleryItemGroup galleryItemGroup1 = new DevExpress.XtraBars.Ribbon.GalleryItemGroup();
            DevExpress.XtraBars.Ribbon.GalleryItemGroup galleryItemGroup2 = new DevExpress.XtraBars.Ribbon.GalleryItemGroup();
            DevExpress.XtraBars.Ribbon.GalleryItemGroup galleryItemGroup3 = new DevExpress.XtraBars.Ribbon.GalleryItemGroup();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnNewTerminal = new DevExpress.XtraBars.BarButtonItem();
            this.btnNewTerminal2 = new DevExpress.XtraBars.BarButtonItem();
            this.btnNewTerminal3 = new DevExpress.XtraBars.BarButtonItem();
            this.btnStart = new DevExpress.XtraBars.BarButtonItem();
            this.btnStop = new DevExpress.XtraBars.BarButtonItem();
            this.btnClear = new DevExpress.XtraBars.BarButtonItem();
            this.txtUsername = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.txtPassword = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.btnLogin = new DevExpress.XtraBars.BarButtonItem();
            this.btnChangePassword = new DevExpress.XtraBars.BarButtonItem();
            this.btnLogout = new DevExpress.XtraBars.BarButtonItem();
            this.lblSystemTime = new DevExpress.XtraBars.BarStaticItem();
            this.lblTotalTransaction = new DevExpress.XtraBars.BarStaticItem();
            this.lblTotalTerminal = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemPopupContainerEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.ribbonGalleryBarItem1 = new DevExpress.XtraBars.RibbonGalleryBarItem();
            this.barButtonGroup1 = new DevExpress.XtraBars.BarButtonGroup();
            this.barButtonGroup2 = new DevExpress.XtraBars.BarButtonGroup();
            this.skinGalleryBarItem = new DevExpress.XtraBars.RibbonGalleryBarItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.grdTransaction = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.btnStatement = new DevExpress.XtraEditors.SimpleButton();
            this.btnBetList = new DevExpress.XtraEditors.SimpleButton();
            this.cbeSignatureTemplate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.webBrowser2 = new System.Windows.Forms.WebBrowser();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.xtraTabPage5 = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.textEdit4 = new DevExpress.XtraEditors.MemoEdit();
            this.chkSCloud = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit3 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkRCloud = new DevExpress.XtraEditors.CheckEdit();
            this.btnSignMeIn = new DevExpress.XtraEditors.SimpleButton();
            this.txtServerIP = new DevExpress.XtraEditors.TextEdit();
            this.txtServerUserName = new DevExpress.XtraEditors.TextEdit();
            this.txtServerPassword = new DevExpress.XtraEditors.TextEdit();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.xtraTabControl2 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTransaction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbeSignatureTemplate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.xtraTabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSCloud.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRCloud.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerUserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).BeginInit();
            this.xtraTabControl2.SuspendLayout();
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
            this.btnNewTerminal,
            this.btnNewTerminal2,
            this.btnNewTerminal3,
            this.btnStart,
            this.btnStop,
            this.btnClear,
            this.txtUsername,
            this.txtPassword,
            this.btnLogin,
            this.btnChangePassword,
            this.btnLogout,
            this.lblSystemTime,
            this.lblTotalTransaction,
            this.lblTotalTerminal,
            this.barEditItem1,
            this.ribbonGalleryBarItem1,
            this.barButtonGroup1,
            this.barButtonGroup2,
            this.skinGalleryBarItem});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 22;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemPopupContainerEdit1});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010;
            this.ribbonControl1.SelectedPage = this.ribbonPage1;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(1056, 125);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            // 
            // btnNewTerminal
            // 
            this.btnNewTerminal.Caption = "ibet-3in1";
            this.btnNewTerminal.Enabled = false;
            this.btnNewTerminal.Id = 1;
            this.btnNewTerminal.LargeGlyph = global::iBet.App.Properties.Resources.i4;
            this.btnNewTerminal.Name = "btnNewTerminal";
            this.btnNewTerminal.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNewTerminal_ItemClick);
            // 
            // btnNewTerminal2
            // 
            this.btnNewTerminal2.Caption = "ibet-sbo";
            this.btnNewTerminal2.Enabled = false;
            this.btnNewTerminal2.Id = 13;
            this.btnNewTerminal2.LargeGlyph = global::iBet.App.Properties.Resources.i9;
            this.btnNewTerminal2.Name = "btnNewTerminal2";
            this.btnNewTerminal2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNewTerminal2_ItemClick);
            // 
            // btnNewTerminal3
            // 
            this.btnNewTerminal3.Caption = "Follow Sub";
            this.btnNewTerminal3.Enabled = false;
            this.btnNewTerminal3.Id = 14;
            this.btnNewTerminal3.LargeGlyph = global::iBet.App.Properties.Resources.i10;
            this.btnNewTerminal3.Name = "btnNewTerminal3";
            this.btnNewTerminal3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNewTerminal3_ItemClick);
            // 
            // btnStart
            // 
            this.btnStart.Caption = "Start";
            this.btnStart.Enabled = false;
            this.btnStart.Id = 2;
            this.btnStart.LargeGlyph = global::iBet.App.Properties.Resources.i5;
            this.btnStart.Name = "btnStart";
            this.btnStart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStart_ItemClick);
            // 
            // btnStop
            // 
            this.btnStop.Caption = "Stop";
            this.btnStop.Enabled = false;
            this.btnStop.Id = 3;
            this.btnStop.LargeGlyph = global::iBet.App.Properties.Resources.i6;
            this.btnStop.Name = "btnStop";
            this.btnStop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStop_ItemClick);
            // 
            // btnClear
            // 
            this.btnClear.Caption = "Clear";
            this.btnClear.Enabled = false;
            this.btnClear.Glyph = global::iBet.App.Properties.Resources.i7;
            this.btnClear.Id = 4;
            this.btnClear.LargeGlyph = global::iBet.App.Properties.Resources.i7;
            this.btnClear.Name = "btnClear";
            this.btnClear.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClear_ItemClick);
            // 
            // txtUsername
            // 
            this.txtUsername.Caption = "Username";
            this.txtUsername.Edit = this.repositoryItemTextEdit1;
            this.txtUsername.Id = 5;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Width = 150;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // txtPassword
            // 
            this.txtPassword.Caption = "Password ";
            this.txtPassword.Edit = this.repositoryItemTextEdit2;
            this.txtPassword.Id = 6;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Width = 150;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            this.repositoryItemTextEdit2.PasswordChar = '*';
            // 
            // btnLogin
            // 
            this.btnLogin.Caption = "Login";
            this.btnLogin.Id = 7;
            this.btnLogin.LargeGlyph = global::iBet.App.Properties.Resources.i1;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLogin_ItemClick);
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.Caption = "Change Password";
            this.btnChangePassword.Enabled = false;
            this.btnChangePassword.Id = 8;
            this.btnChangePassword.LargeGlyph = global::iBet.App.Properties.Resources.i3;
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnChangePassword_ItemClick);
            // 
            // btnLogout
            // 
            this.btnLogout.Caption = "Logout";
            this.btnLogout.Enabled = false;
            this.btnLogout.Id = 9;
            this.btnLogout.LargeGlyph = global::iBet.App.Properties.Resources.i2;
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLogout_ItemClick);
            // 
            // lblSystemTime
            // 
            this.lblSystemTime.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.lblSystemTime.Id = 10;
            this.lblSystemTime.Name = "lblSystemTime";
            this.lblSystemTime.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // lblTotalTransaction
            // 
            this.lblTotalTransaction.Caption = "Total transaction(s): 0";
            this.lblTotalTransaction.Id = 11;
            this.lblTotalTransaction.Name = "lblTotalTransaction";
            this.lblTotalTransaction.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // lblTotalTerminal
            // 
            this.lblTotalTerminal.Caption = "Total Terminal(s): 0";
            this.lblTotalTerminal.Id = 12;
            this.lblTotalTerminal.Name = "lblTotalTerminal";
            this.lblTotalTerminal.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "barEditItem1";
            this.barEditItem1.Edit = this.repositoryItemPopupContainerEdit1;
            this.barEditItem1.Id = 15;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // repositoryItemPopupContainerEdit1
            // 
            this.repositoryItemPopupContainerEdit1.AutoHeight = false;
            this.repositoryItemPopupContainerEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemPopupContainerEdit1.Name = "repositoryItemPopupContainerEdit1";
            // 
            // ribbonGalleryBarItem1
            // 
            this.ribbonGalleryBarItem1.Caption = "ribbonGalleryBarItem1";
            this.ribbonGalleryBarItem1.Id = 17;
            this.ribbonGalleryBarItem1.Name = "ribbonGalleryBarItem1";
            // 
            // barButtonGroup1
            // 
            this.barButtonGroup1.Caption = "barButtonGroup1";
            this.barButtonGroup1.Id = 18;
            this.barButtonGroup1.Name = "barButtonGroup1";
            // 
            // barButtonGroup2
            // 
            this.barButtonGroup2.Caption = "barButtonGroup2";
            this.barButtonGroup2.Id = 19;
            this.barButtonGroup2.Name = "barButtonGroup2";
            // 
            // skinGalleryBarItem
            // 
            this.skinGalleryBarItem.Caption = "skinGalleryBarItem";
            // 
            // skinGalleryBarItem
            // 
            this.skinGalleryBarItem.Gallery.AllowHoverImages = true;
            this.skinGalleryBarItem.Gallery.FixedHoverImageSize = false;
            galleryItemGroup1.Caption = "Standard";
            galleryItemGroup2.Caption = "Bonus";
            galleryItemGroup2.Visible = false;
            galleryItemGroup3.Caption = "Office";
            galleryItemGroup3.Visible = false;
            this.skinGalleryBarItem.Gallery.Groups.AddRange(new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            galleryItemGroup1,
            galleryItemGroup2,
            galleryItemGroup3});
            this.skinGalleryBarItem.Gallery.ImageSize = new System.Drawing.Size(58, 43);
            this.skinGalleryBarItem.Gallery.ItemCheckMode = DevExpress.XtraBars.Ribbon.Gallery.ItemCheckMode.SingleRadio;
            this.skinGalleryBarItem.Id = 1;
            this.skinGalleryBarItem.Name = "skinGalleryBarItem";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup3,
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ribbonPageGroup5});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Terminal";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.txtUsername);
            this.ribbonPageGroup3.ItemLinks.Add(this.txtPassword);
            this.ribbonPageGroup3.ItemLinks.Add(this.btnLogin);
            this.ribbonPageGroup3.ItemLinks.Add(this.btnChangePassword);
            this.ribbonPageGroup3.ItemLinks.Add(this.btnLogout);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.ShowCaptionButton = false;
            this.ribbonPageGroup3.Text = "Authentication";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btnNewTerminal);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnNewTerminal2);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnNewTerminal3);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnStart);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnStop);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.ShowCaptionButton = false;
            this.ribbonPageGroup1.Text = "Terminal";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnClear);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.ShowCaptionButton = false;
            this.ribbonPageGroup2.Text = "Tracking";
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.ItemLinks.Add(this.skinGalleryBarItem);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            this.ribbonPageGroup5.Text = "Skins";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.ItemLinks.Add(this.lblSystemTime);
            this.ribbonStatusBar1.ItemLinks.Add(this.lblTotalTerminal);
            this.ribbonStatusBar1.ItemLinks.Add(this.lblTotalTransaction);
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 617);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1056, 31);
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
            this.splitContainerControl1.Size = new System.Drawing.Size(1056, 492);
            this.splitContainerControl1.SplitterPosition = 44;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1056, 443);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage3,
            this.xtraTabPage5});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.grdTransaction);
            this.xtraTabPage1.Controls.Add(this.panelControl3);
            this.xtraTabPage1.Controls.Add(this.panelControl4);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1050, 417);
            this.xtraTabPage1.Text = "Transactions";
            // 
            // grdTransaction
            // 
            this.grdTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTransaction.EmbeddedNavigator.ShowToolTips = false;
            this.grdTransaction.Location = new System.Drawing.Point(0, 0);
            this.grdTransaction.MainView = this.gridView2;
            this.grdTransaction.Name = "grdTransaction";
            this.grdTransaction.Size = new System.Drawing.Size(1050, 417);
            this.grdTransaction.TabIndex = 7;
            this.grdTransaction.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn8,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn6,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn7,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn9,
            this.gridColumn14,
            this.gridColumn15,
            this.gridColumn13});
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.GridControl = this.grdTransaction;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsCustomization.AllowGroup = false;
            this.gridView2.OptionsMenu.EnableColumnMenu = false;
            this.gridView2.OptionsMenu.EnableFooterMenu = false;
            this.gridView2.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView2.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
            this.gridView2.OptionsView.ShowAutoFilterRow = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsView.ShowPreview = true;
            this.gridView2.PreviewFieldName = "Note";
            this.gridView2.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gridView2.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn13, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "ID";
            this.gridColumn1.FieldName = "ID";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.OptionsColumn.AllowMove = false;
            this.gridColumn1.OptionsColumn.AllowSize = false;
            this.gridColumn1.OptionsColumn.FixedWidth = true;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 30;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Account Pair";
            this.gridColumn8.FieldName = "AccountPair";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowFocus = false;
            this.gridColumn8.OptionsColumn.FixedWidth = true;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 1;
            this.gridColumn8.Width = 120;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Home Team";
            this.gridColumn2.FieldName = "HomeTeamName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 107;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Away Team";
            this.gridColumn3.FieldName = "AwayTeamName";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 119;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Type";
            this.gridColumn6.FieldName = "OddType";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            this.gridColumn6.OptionsColumn.FixedWidth = true;
            this.gridColumn6.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            this.gridColumn6.Width = 150;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Odd";
            this.gridColumn4.FieldName = "OddKindValue";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.OptionsColumn.FixedWidth = true;
            this.gridColumn4.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 5;
            this.gridColumn4.Width = 60;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Value";
            this.gridColumn5.FieldName = "OddValue";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.OptionsColumn.FixedWidth = true;
            this.gridColumn5.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 6;
            this.gridColumn5.Width = 60;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Stake";
            this.gridColumn7.FieldName = "Stake";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowFocus = false;
            this.gridColumn7.OptionsColumn.FixedWidth = true;
            this.gridColumn7.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 7;
            this.gridColumn7.Width = 60;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "I";
            this.gridColumn10.FieldName = "IBETTrade";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowFocus = false;
            this.gridColumn10.OptionsColumn.FixedWidth = true;
            this.gridColumn10.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 8;
            this.gridColumn10.Width = 30;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "3";
            this.gridColumn11.FieldName = "THREEIN1Trade";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowFocus = false;
            this.gridColumn11.OptionsColumn.FixedWidth = true;
            this.gridColumn11.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 9;
            this.gridColumn11.Width = 30;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "3 Re";
            this.gridColumn12.FieldName = "THREEIN1ReTrade";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.OptionsColumn.AllowFocus = false;
            this.gridColumn12.OptionsColumn.FixedWidth = true;
            this.gridColumn12.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 11;
            this.gridColumn12.Width = 30;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "I Re";
            this.gridColumn9.FieldName = "IBETReTrade";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowFocus = false;
            this.gridColumn9.OptionsColumn.FixedWidth = true;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 12;
            this.gridColumn9.Width = 30;
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "B";
            this.gridColumn14.FieldName = "SBOBETTrade";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.OptionsColumn.AllowFocus = false;
            this.gridColumn14.OptionsColumn.FixedWidth = true;
            this.gridColumn14.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 10;
            this.gridColumn14.Width = 30;
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "B Re";
            this.gridColumn15.FieldName = "SBOBETReTrade";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.OptionsColumn.AllowFocus = false;
            this.gridColumn15.OptionsColumn.FixedWidth = true;
            this.gridColumn15.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 13;
            this.gridColumn15.Width = 30;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "DateTime";
            this.gridColumn13.DisplayFormat.FormatString = "dd/MM/yyyy hh:mm:ss";
            this.gridColumn13.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn13.FieldName = "DateTime";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.AllowFocus = false;
            this.gridColumn13.OptionsColumn.FixedWidth = true;
            this.gridColumn13.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.gridColumn13.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn13.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 14;
            this.gridColumn13.Width = 150;
            // 
            // panelControl3
            // 
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(200, 100);
            this.panelControl3.TabIndex = 8;
            // 
            // panelControl4
            // 
            this.panelControl4.Location = new System.Drawing.Point(0, 0);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(200, 100);
            this.panelControl4.TabIndex = 9;
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.btnStatement);
            this.xtraTabPage3.Controls.Add(this.btnBetList);
            this.xtraTabPage3.Controls.Add(this.cbeSignatureTemplate);
            this.xtraTabPage3.Controls.Add(this.panelControl1);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(1050, 417);
            this.xtraTabPage3.Text = "Bet List - Statement";
            // 
            // btnStatement
            // 
            this.btnStatement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStatement.Location = new System.Drawing.Point(295, 391);
            this.btnStatement.Name = "btnStatement";
            this.btnStatement.Size = new System.Drawing.Size(99, 23);
            this.btnStatement.TabIndex = 7;
            this.btnStatement.Text = "Statement";
            this.btnStatement.Click += new System.EventHandler(this.btnStatement_Click);
            // 
            // btnBetList
            // 
            this.btnBetList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBetList.Location = new System.Drawing.Point(198, 391);
            this.btnBetList.Name = "btnBetList";
            this.btnBetList.Size = new System.Drawing.Size(91, 23);
            this.btnBetList.TabIndex = 6;
            this.btnBetList.Text = "Refresh";
            this.btnBetList.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cbeSignatureTemplate
            // 
            this.cbeSignatureTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbeSignatureTemplate.EditValue = "Select account pair";
            this.cbeSignatureTemplate.Location = new System.Drawing.Point(3, 394);
            this.cbeSignatureTemplate.Name = "cbeSignatureTemplate";
            this.cbeSignatureTemplate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.cbeSignatureTemplate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbeSignatureTemplate.Properties.DropDownRows = 10;
            this.cbeSignatureTemplate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbeSignatureTemplate.Size = new System.Drawing.Size(189, 20);
            this.cbeSignatureTemplate.TabIndex = 5;
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.webBrowser2);
            this.panelControl1.Controls.Add(this.splitter1);
            this.panelControl1.Controls.Add(this.webBrowser1);
            this.panelControl1.Location = new System.Drawing.Point(2, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1044, 382);
            this.panelControl1.TabIndex = 3;
            // 
            // webBrowser2
            // 
            this.webBrowser2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser2.Location = new System.Drawing.Point(364, 2);
            this.webBrowser2.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser2.Name = "webBrowser2";
            this.webBrowser2.Size = new System.Drawing.Size(678, 378);
            this.webBrowser2.TabIndex = 2;
            this.webBrowser2.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(361, 2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 378);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Left;
            this.webBrowser1.Location = new System.Drawing.Point(2, 2);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(359, 378);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // xtraTabPage5
            // 
            this.xtraTabPage5.Controls.Add(this.panelControl2);
            this.xtraTabPage5.Name = "xtraTabPage5";
            this.xtraTabPage5.Size = new System.Drawing.Size(1050, 417);
            this.xtraTabPage5.Text = "Settings";
            // 
            // panelControl2
            // 
            this.panelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl2.Controls.Add(this.textEdit4);
            this.panelControl2.Controls.Add(this.chkSCloud);
            this.panelControl2.Controls.Add(this.simpleButton2);
            this.panelControl2.Controls.Add(this.textEdit3);
            this.panelControl2.Controls.Add(this.textEdit2);
            this.panelControl2.Controls.Add(this.textEdit1);
            this.panelControl2.Controls.Add(this.simpleButton1);
            this.panelControl2.Controls.Add(this.label4);
            this.panelControl2.Controls.Add(this.label3);
            this.panelControl2.Controls.Add(this.label2);
            this.panelControl2.Controls.Add(this.label1);
            this.panelControl2.Controls.Add(this.chkRCloud);
            this.panelControl2.Controls.Add(this.btnSignMeIn);
            this.panelControl2.Controls.Add(this.txtServerIP);
            this.panelControl2.Controls.Add(this.txtServerUserName);
            this.panelControl2.Controls.Add(this.txtServerPassword);
            this.panelControl2.Controls.Add(this.pictureEdit1);
            this.panelControl2.Location = new System.Drawing.Point(3, 3);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1044, 497);
            this.panelControl2.TabIndex = 0;
            // 
            // textEdit4
            // 
            this.textEdit4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textEdit4.Location = new System.Drawing.Point(244, 5);
            this.textEdit4.Name = "textEdit4";
            this.textEdit4.Size = new System.Drawing.Size(408, 406);
            this.textEdit4.TabIndex = 18;
            // 
            // chkSCloud
            // 
            this.chkSCloud.EditValue = true;
            this.chkSCloud.Location = new System.Drawing.Point(13, 27);
            this.chkSCloud.Name = "chkSCloud";
            this.chkSCloud.Properties.Caption = "S cloud";
            this.chkSCloud.Size = new System.Drawing.Size(86, 19);
            this.chkSCloud.TabIndex = 17;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(130, 307);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(97, 23);
            this.simpleButton2.TabIndex = 16;
            this.simpleButton2.Text = "Set";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // textEdit3
            // 
            this.textEdit3.EditValue = "0";
            this.textEdit3.Location = new System.Drawing.Point(94, 216);
            this.textEdit3.Name = "textEdit3";
            this.textEdit3.Size = new System.Drawing.Size(142, 20);
            this.textEdit3.TabIndex = 15;
            // 
            // textEdit2
            // 
            this.textEdit2.Location = new System.Drawing.Point(94, 268);
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Size = new System.Drawing.Size(142, 20);
            this.textEdit2.TabIndex = 14;
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(94, 241);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(142, 20);
            this.textEdit1.TabIndex = 13;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(18, 307);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(97, 23);
            this.simpleButton1.TabIndex = 12;
            this.simpleButton1.Text = "Get";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Username";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Server Address";
            // 
            // chkRCloud
            // 
            this.chkRCloud.EditValue = true;
            this.chkRCloud.Location = new System.Drawing.Point(12, 53);
            this.chkRCloud.Name = "chkRCloud";
            this.chkRCloud.Properties.Caption = "R cloud";
            this.chkRCloud.Size = new System.Drawing.Size(86, 19);
            this.chkRCloud.TabIndex = 7;
            this.chkRCloud.CheckedChanged += new System.EventHandler(this.chbCloud_CheckedChanged);
            // 
            // btnSignMeIn
            // 
            this.btnSignMeIn.Location = new System.Drawing.Point(154, 171);
            this.btnSignMeIn.Name = "btnSignMeIn";
            this.btnSignMeIn.Size = new System.Drawing.Size(75, 23);
            this.btnSignMeIn.TabIndex = 3;
            this.btnSignMeIn.Text = "Sign me in";
            this.btnSignMeIn.Click += new System.EventHandler(this.btnSignMeIn_Click);
            // 
            // txtServerIP
            // 
            this.txtServerIP.EditValue = "115.84.178.100";
            this.txtServerIP.Location = new System.Drawing.Point(95, 83);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(142, 20);
            this.txtServerIP.TabIndex = 4;
            // 
            // txtServerUserName
            // 
            this.txtServerUserName.EditValue = "tuns";
            this.txtServerUserName.Location = new System.Drawing.Point(95, 110);
            this.txtServerUserName.Name = "txtServerUserName";
            this.txtServerUserName.Size = new System.Drawing.Size(142, 20);
            this.txtServerUserName.TabIndex = 5;
            // 
            // txtServerPassword
            // 
            this.txtServerPassword.EditValue = "anhkomuonradi";
            this.txtServerPassword.Location = new System.Drawing.Point(95, 137);
            this.txtServerPassword.Name = "txtServerPassword";
            this.txtServerPassword.Properties.UseSystemPasswordChar = true;
            this.txtServerPassword.Size = new System.Drawing.Size(142, 20);
            this.txtServerPassword.TabIndex = 6;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(658, 5);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.AllowFocused = false;
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.PictureAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.pictureEdit1.Properties.ShowMenu = false;
            this.pictureEdit1.Size = new System.Drawing.Size(382, 448);
            this.pictureEdit1.TabIndex = 2;
            // 
            // xtraTabControl2
            // 
            this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl2.Name = "xtraTabControl2";
            this.xtraTabControl2.SelectedTabPage = this.xtraTabPage2;
            this.xtraTabControl2.Size = new System.Drawing.Size(1056, 44);
            this.xtraTabControl2.TabIndex = 1;
            this.xtraTabControl2.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage2,
            this.xtraTabPage4});
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1050, 18);
            this.xtraTabPage2.Text = "Waiting list";
            // 
            // xtraTabPage4
            // 
            this.xtraTabPage4.Name = "xtraTabPage4";
            this.xtraTabPage4.Size = new System.Drawing.Size(1050, 18);
            this.xtraTabPage4.Text = "Clones monitor";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.btnClear);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.ShowCaptionButton = false;
            this.ribbonPageGroup4.Text = "Tracking";
            // 
            // MainForm
            // 
            this.AutoHideRibbon = false;
            this.ClientSize = new System.Drawing.Size(1056, 648);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.Icon = global::iBet.App.Properties.Resources.BetBrokerLogo;
            this.Name = "MainForm";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "Bet Broker";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTransaction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.xtraTabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbeSignatureTemplate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.xtraTabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSCloud.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRCloud.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerUserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl2)).EndInit();
            this.xtraTabControl2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		public MainForm()
		{
            //DateTime betListDate = DateTime.UtcNow.AddHours(-4);
			this.InitializeComponent();
			this.InitializeObjects();
            InitSkins();
            btnSignMeIn.Enabled = false;
            //SbobetEngine sE = new SbobetEngine("as","test"
            //string aaa = "parent;p.RstrD();window.setTimeout(\"parent.ShowBetList('W','09/14/2013 13:21:32','l',Nl)\",10);";
            //string bbb = "window.setTimeout(\"parent.ShowBetList('";
            
            //int num = aaa.IndexOf(bbb);
            //if (num != -1)
            //{
            //    string ccc = aaa.Substring(num + bbb.Length + 4, 19);
            //}
            //string s = "1,892.87";
            //decimal d = decimal.Parse(s);
            //Console.Write(d);
            //var json = new System.Net.WebClient().DownloadString("https://spreadsheets.google.com/feeds/list/0ArjfL6TJnxG3dHZNNlNZbGU0a0JMT21oWno2WXR4bEE/od5/public/values?alt=json-in-script");
            //Test();

            //TestStrategy();

            //TestRuleEngine();
            TestCrawXMLfromGoogle("https://spreadsheets.google.com/feeds/list/0ArjfL6TJnxG3dEdEMGJNU2NtOU4tR2ROcGxwUkJlZEE/od5/public/values?alt=rss");
		}
        private void TestCrawXMLfromGoogle(string url)
        {
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    var text = webClient.DownloadString(url);
                    if (text.Contains("#N/A"))
                    {
                        //this.ShowErrorDialog("Error while loading strategy. \nDetails: " + ex.Message);
                        Console.WriteLine("Contain N/A");
                    }
                    else
                    {
                        XmlDocument content2 = new XmlDocument();

                        content2.LoadXml(text);
                        content2.Save("text.xml");
                    }
                }
            }
            catch (Exception ex)
            {
 
            }
        }


        private void TestRuleEngine()
        {
            RulesEngine.Engine engine = new RulesEngine.Engine();
            engine.For<Person>()                    
                    .Setup(p => p.Name)
                        .MustNotBeNull()
                        .MustMatchRegex("^[a-zA-z]+$")
                    .Setup(p => p.Phone)
                        .MustNotBeNull()
                        .MustMatchRegex("^[0-9]+$");

            engine.For<MatchDTO>()
                .Setup(m => m.HomeTeamName)
                    .MustEqual("Man U");

            MatchDTO match = new MatchDTO();
            match.HomeTeamName = "Man U";
            match.AwayTeamName = "Chelsea";
            match.AwayScore = "2";
            match.HomeScore = "1";

            bool isV = engine.Validate(match);
            iBet.Utilities.WriteLog.Write(isV.ToString());

            Person person = new Person();
            person.Name = "Bill";
            person.Phone = "1234214";
            //person.DateOfBirth = new DateTime(1999, 10, 2);

            bool isValid = engine.Validate(person);
            iBet.Utilities.WriteLog.Write(isValid.ToString());
        }
        private void TestStrategy()
        {
            System.Collections.Generic.List<Strategy> listStrategy = new List<Strategy>();
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString("https://spreadsheets.google.com/feeds/list/0ArjfL6TJnxG3dHZNNlNZbGU0a0JMT21oWno2WXR4bEE/od9/public/values?alt=json-in-script");
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
                                        Strategy strategy = new Strategy();

                                        Newtonsoft.Json.JavaScriptObject jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$name"];
                                        strategy.Name = jsObj2["$t"].ToString();

                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$stepid"];
                                        strategy.StepID = int.Parse(jsObj2["$t"].ToString());

                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$field"];
                                        strategy.Field = jsObj2["$t"].ToString();

                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$operator"];
                                        strategy.Operator = jsObj2["$t"].ToString();

                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$valuetocompare"];
                                        strategy.ValueToCompare = jsObj2["$t"].ToString();

                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$object"];
                                        strategy.Obj = jsObj2["$t"].ToString();

                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$expected"];
                                        strategy.Expected = bool.Parse(jsObj2["$t"].ToString());

                                        jsObj2 = (Newtonsoft.Json.JavaScriptObject)objCurrent["gsx$adminallowed"];
                                        strategy.AdminAllowed = bool.Parse(jsObj2["$t"].ToString());

                                        listStrategy.Add(strategy);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            MatchDTO match = new MatchDTO();
            match.HomeTeamName = "Man U";
            match.AwayTeamName = "Chelsea";
            match.AwayScore = "2";
            match.HomeScore = "1";
            match.Minute = 34;

            OddDTO odd = new OddDTO();            
            odd.Home = 0.95f;
            odd.Away = 0.95f;
            odd.Type = eOddType.FulltimeOverUnder;

            match.Odds = new List<OddDTO>();
            match.Odds.Add(odd);            
            
            foreach (Strategy st in listStrategy)
            {
                if (st.AdminAllowed == true && st.StepID != 100)
                {
                    if (st.Obj == "Odd")
                    {
                        foreach (OddDTO o in match.Odds)
                        {
                            if (st.ValueToCompare == "Odd.Away")
                                st.Result = Operator.Apply(o, st.Operator, st.Field, o.Away);
                            else
                                st.Result = Operator.Apply(o, st.Operator, st.Field, st.ValueToCompare);
                            if (st.Result == st.Expected)
                                break;
                        }
                    }
                    else if (st.Obj == "Match")
                    {
                        st.Result = Operator.Apply(match, st.Operator, st.Field, st.ValueToCompare);
                    }
                }
                if (st.Result == true)
                {
                    iBet.Utilities.WriteLog.Write(st.Name + " step " + st.StepID.ToString() + ": checking " + st.Obj + "." + st.Field
                        + " " + st.Operator + " " + st.ValueToCompare + " >>> RESULT: " +
                        st.Result.ToString());
                }
                else
                {
                    iBet.Utilities.WriteLog.Write(st.Name + " step " + st.StepID.ToString() + ": checking " + st.Obj + "." + st.Field
                        + " " + st.Operator + " " + st.ValueToCompare + " >>> RESULT: " +
                        st.Result.ToString());
                }
            }
        }
        private void Test()        
        {
            System.Collections.Generic.List<BetAnalyse> listBA = new List<BetAnalyse>();
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString("https://spreadsheets.google.com/feeds/list/0ArjfL6TJnxG3dHZNNlNZbGU0a0JMT21oWno2WXR4bEE/od5/public/values?alt=json-in-script");
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
                //this.ShowErrorDialog("Error while initialize SBOBET Engine. \nDetails: " + ex.Message);
            }
            if (listBA.Count > 0)
            {
                var max = listBA.Max(obj => obj.Diff);
                var min = listBA.Min(obj => obj.Diff);
                foreach (BetAnalyse ba in listBA)
                {
                    if (ba.Diff == max)
                        ba.isGoodOddToBet = true;
                    if (ba.Diff == min)
                        ba.isGoodOddToBet = true;
                }
                Console.Write(max);
            }
        }
        

        void InitSkins()
        {
            SkinHelper.InitSkinGallery(skinGalleryBarItem, true);
            UserLookAndFeel.Default.SetSkinStyle("DevExpress Dark Style");
        }
        

        internal void InitializeWCFService()
        {
            try
            {
                _uiSyncContext = SynchronizationContext.Current;

                EndpointAddress address = new EndpointAddress(new Uri("net.tcp://" + txtServerIP.Text + ":2202/BetBrokerService"));
                NetTcpBinding binding = new NetTcpBinding();
                DuplexChannelFactory<IBrokerService> factory = new DuplexChannelFactory<IBrokerService>(new InstanceContext(this), binding, address);
                factory.Credentials.Windows.ClientCredential = new System.Net.NetworkCredential(txtServerUserName.Text, txtServerPassword.Text, "WORKGROUP");
                if (betBrokerSvc == null)
                    betBrokerSvc = factory.CreateChannel();
                if (chkRCloud.Checked)
                    betBrokerSvc.Subscribe();//for callback
                else
                    betBrokerSvc.Unsubscribe();
            }
            catch (Exception ex)
            {
                ShowWaringDialog(ex.ToString());
            }
        }

		private void InitializeObjects()
		{
			this._systemTimer = new System.Windows.Forms.Timer();
			this._systemTimer.Interval = 1000;
			this._systemTimer.Tick += new System.EventHandler(this._systemTimer_Tick);
			this._systemTimer.Start();
                        
			this._listTerminal = new System.Collections.Generic.List<TerminalFormIBET3IN1>();
            this._listTerminal2 = new System.Collections.Generic.List<TerminalFormIBETSBO>();
            this._listTerminal3 = new System.Collections.Generic.List<FollowSub>();
			this._listTransaction = new System.Collections.Generic.List<TransactionDTO>();
            this._list3in1Host = new List<string>();            

			this.grdTransaction.DataSource = this._listTransaction;
            this._dataService = new DataServiceSoapClient();
            if (this._dataService.Endpoint.ListenUri.ToString().Contains("http://demo.tranbros.com") || this._dataService.Endpoint.ListenUri.ToString().Contains("http://apbenvironment.com"))
            {
                this._dataService.LoginCompleted += new EventHandler<admin.LoginCompletedEventArgs>(this._dataService_LoginCompleted);
            }
			
			this.Text = "Bet Broker - Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            //System.Collections.Generic.List<MatchDTO> testList = SbobetEngine.ConvertFullDataNew("[29887,0,1,[[[283,'Japan Emperor Cup','',''],[1948,'Spain Women Superliga','',''],[20831,'Indonesia Liga Divisi Utama Playoff','',''],[26674,'Korea K-League Classic Playoff','','']],[[1290020,1,283,'Tokyo Verdy','V-Varen Nagasaki','1.733',10,'09/08/2013 16:00',1,'',5],[1295232,1,283,'Tokyo Verdy (ET)','V-Varen Nagasaki (ET)','1.733A',10,'09/08/2013 16:00',0,'',5],[1295233,1,283,'Tokyo Verdy (PEN)','V-Varen Nagasaki (PEN)','1.733B',10,'09/08/2013 16:00',0,'',1],[1295234,1,283,'Tokyo Verdy (PEN)','V-Varen Nagasaki (PEN)','1.733C',10,'09/08/2013 16:00',0,'',1],[1291865,1,1948,'CD San Gabriel (w)','SC Huelva (w)','1.862',10,'09/08/2013 17:00',1,'',6],[1294201,1,20831,'Persikabo Bogor (n)','Persebaya Surabaya','1.891',10,'09/08/2013 16:30',0,'',3],[1293714,1,26674,'Jeonbuk Hyundai Motors','Pohang Steelers','1.734',6,'09/08/2013 16:00',1,'SG: CH 222',2]],[[352993,1290020,0,2,2,5],[361811,1295232,0,0,0,5],[361812,1295233,0,0,0,1],[361813,1295234,0,0,0,1],[356455,1291865,0,1,0,6],[360499,1294201,0,0,1,3],[359357,1293714,0,0,3,2]],[[352993,2,2,40,45,0,0,0],[361811,2,0,10,15,0,0,0],[361812,0,0,9,45,0,0,0],[361813,0,0,9,45,0,0,0],[356455,1,1,38,45,0,0,0],[360499,1,2,6,45,0,0,0],[359357,1,2,40,45,0,0,0]],,[[14311764,[352993,1,1,5000.00,0.00],[-0.98,0.9]],[14311765,[352993,1,1,3000.00,0.25],[-0.29,0.21]],[14311767,[352993,3,1,5000.00,4.50],[-0.36,0.28]],[14311768,[352993,3,1,3000.00,4.75],[-0.26,0.18]],[14392379,[361811,1,1,8000.00,0.00],[0.99,0.93]],[14392380,[361811,1,1,5000.00,0.25],[-0.55,0.47]],[14392382,[361811,3,1,8000.00,0.75],[-0.99,0.89]],[14392383,[361811,3,1,3000.00,0.50],[0.7,-0.8]],[14392400,[361812,1,1,8000.00,0.00],[0.88,-0.96]],[14392425,[361813,3,1,5000.00,7.00],[-0.85,0.75]],[14341997,[356455,1,1,1000.00,0.25],[0.99,0.85]],[14341999,[356455,3,1,1000.00,2.75],[0.92,0.9]],[14341998,[356455,7,1,500.00,0.00],[0.54,-0.7]],[14342000,[356455,9,1,500.00,1.50],[-0.43,0.25]],[14375895,[360499,1,1,500.00,-0.25],[0.81,-0.97]],[14375897,[360499,3,1,500.00,2.00],[0.84,0.98]],[14393227,[359357,1,1,3000.00,0.00],[0.75,-0.83]],[14393229,[359357,3,1,3000.00,3.50],[-0.39,0.31]]],,,'Fu\xDFball',0],[[],[],[1290020,1293714,1291865,1295232,1295233,1295234],0],,,0]");



		}
		private void _systemTimer_Tick(object sender, System.EventArgs e)
		{
			this.lblSystemTime.Caption = System.DateTime.Now.ToString();
			this.lblTotalTransaction.Caption = "Total Transaction(s): " + this._listTransaction.Count;
			this.lblTotalTerminal.Caption = "Total Terminal(s): " + (this._listTerminal.Count + this._listTerminal2.Count + this._listTerminal3.Count);
		}
        private void _refreshServer_Tick(object sender, System.EventArgs e)
        {
            Patient patient = new Patient();
            patient.FirstName = textEdit1.Text;
            patient.LastName = textEdit2.Text;

            betBrokerSvc.SetPatient(Convert.ToInt32(textEdit3.Text), patient);
        }
        private void _dataService_LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            this._currentUserID = e.userID;
            this.SetAuthorized(e.Result);
            if (!e.Result)
            {
                this.ShowWaringDialog("Your account is not authorized to run the application. \nPlease check your username, password");
            }
            else
            {
                //this.InitializeWCFService();
                //this._refreshServerTimer = new System.Windows.Forms.Timer();
                //this._refreshServerTimer.Interval = 300000;
                //this._refreshServerTimer.Tick += new System.EventHandler(this._refreshServer_Tick);
                //this._refreshServerTimer.Start();
                btnSignMeIn.Enabled = true;
            }

            base.UseWaitCursor = false;
        }        
        internal void AddLocalValidOdd(string fromIbetAccount, MatchDTO ibetmatch, MatchDTO sbobetmatch, eOddType oddtype, string ibetodd, string sbobetodd, string ibetoddType, string sbobetoddType, string ibetoddValue, string sbobetoddValue, bool homeFavor)
        {

            foreach (TerminalFormIBETSBO current in this._listTerminal2)
            {
                current.GetOddFromLocalCommunity(fromIbetAccount, ibetmatch, sbobetmatch, oddtype, ibetodd, sbobetodd, ibetoddType, sbobetoddType, ibetoddValue, sbobetoddValue, homeFavor);
            }            
        }
        internal void AddOddToClound(string fromIbetAccount, MatchDTO ibetmatch, MatchDTO sbobetmatch, eOddType oddtype, string ibetodd, string sbobetodd, string ibetoddType, string sbobetoddType, string ibetoddValue, string sbobetoddValue, bool homeFavor)
        {
            if (chkSCloud.Checked)
            {
                OddNews odd = new OddNews();
                odd.HomeTeamName = ibetmatch.HomeTeamName;
                odd.AwayTeamName = ibetmatch.AwayTeamName;
                odd.LeagueName = ibetmatch.LeagueName;
                odd.OddType = oddtype.ToString();
                odd.IbetOddType = ibetoddType;
                odd.SbobetOddType = sbobetoddType;
                odd.IbetOddValue = ibetoddValue;
                odd.SbobetOddValue = sbobetoddValue;
                odd.HomeFavor = homeFavor.ToString();
                odd.IbetOdd = ibetodd;
                odd.SbobetOdd = sbobetodd;
                try
                {
                    betBrokerSvc.GetOdd(odd, this._currentUserID);
                }
                catch (TimeoutException exception)
                {
                    //Console.WriteLine("Got {0}", exception.GetType());
                    //.Abort();
                }
                catch (CommunicationException exception)
                {
                    //InitializeWCFService();
                }
            }
        }
        public void GetOddFromGlobal(OddNews odd, DateTime timestamp, string fromUser)
        {
            if (fromUser != this._currentUserID)
            {
                try
                {
                    SendOrPostCallback callback = new SendOrPostCallback(delegate(object state)
                    {
                        MatchDTO matchDTO = new MatchDTO();
                        matchDTO.HomeTeamName = odd.HomeTeamName;
                        matchDTO.AwayTeamName = odd.AwayTeamName;
                        matchDTO.League = new LeagueDTO();
                        matchDTO.League.Name = odd.LeagueName;
                        eOddType EOddType = eOddType.Unknown;
                        if (odd.OddType == "FirstHalfHandicap")
                            EOddType = eOddType.FirstHalfHandicap;
                        else if (odd.OddType == "FirstHalfOverUnder")
                            EOddType = eOddType.FirstHalfOverUnder;
                        else if (odd.OddType == "FulltimeHandicap")
                            EOddType = eOddType.FulltimeHandicap;
                        else
                            EOddType = eOddType.FulltimeOverUnder;

                        this.AddLocalValidOdd(fromUser, matchDTO, matchDTO, EOddType, odd.IbetOdd, odd.SbobetOdd,odd.IbetOddType, odd.SbobetOddType, odd.IbetOddValue, odd.SbobetOddValue, Boolean.Parse(odd.HomeFavor));

                        textEdit4.Text += "\r\n-- ["  + timestamp.ToLongTimeString() + "] Get odd: " +
                        odd.HomeTeamName + " - " +
                        odd.AwayTeamName + " " +
                        odd.LeagueName + " ; " +
                        odd.OddType + " " +
                        odd.IbetOdd + " / " +
                        odd.SbobetOdd + " . " +
                        odd.IbetOddType + " / " +
                        odd.SbobetOddType + " / " +
                        odd.IbetOddValue + " / " +
                        odd.SbobetOddValue + " . " +
                        odd.HomeFavor +
                        ". From: " + fromUser;
                    });
                    _uiSyncContext.Post(callback, null);
                    //MessageBox.Show("call from Bet broker server", "Bet Broker", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Bet Broker", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            } 
        }
        public void PatientUpdated(Patient createdItem, DateTime time)
        {
            try
            {
                SendOrPostCallback callback = new SendOrPostCallback(delegate(object state)
                {
                    //MessageBox.Show("update completed", "call from Bet broker server", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    label4.Text = "Updated at " + time.ToLongTimeString();
                    textEdit4.Text +=  "\r\n" + this._currentUserID + " said \"Hello\" at:" + time.ToLongTimeString();
                });
                _uiSyncContext.Post(callback, null);
                //MessageBox.Show("call from Bet broker server", "Bet Broker", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Bet Broker", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        internal void AddToActiveListAccPair(string accpair)
        {
            if (!cbeSignatureTemplate.Properties.Items.Contains(accpair))
                this.cbeSignatureTemplate.Properties.Items.Add(accpair);
        }

        internal void WriteReport(string report)
        {
            textEdit4.Text += "\r\n" + report;
        }

		internal void AddTransaction(TransactionDTO transaction)
		{
            if (transaction.IBETTrade || transaction.SBOBETTrade || transaction.THREEIN1Trade)
			{
				TransactionDTO transactionDTO = BaseDTO.DeepClone<TransactionDTO>(transaction);
				transactionDTO.ID = (this._listTransaction.Count + 1).ToString();
				this._listTransaction.Add(transactionDTO);
				lock (this.grdTransaction)
				{
					this.grdTransaction.RefreshDataSource();
				}
				this._dataService.AddTransactionAsync(
                    this._currentUserID, 
                    transaction.AccountPair, 
                    transaction.HomeTeamName, 
                    transaction.AwayTeamName, 
                    transaction.OddType, 
                    transaction.Odd, 
                    transaction.OddValue, 
                    transaction.Stake, 
                    transaction.IBETTrade, 
                    transaction.THREEIN1Trade, 
                    transaction.SBOBETTrade,
                    transaction.THREEIN1ReTrade, 
                    transaction.IBETReTrade,
                    transaction.SBOBETReTrade,
                    transaction.Note, 
                    transaction.DateTime);
			}
		}
		private void btnNewTerminal_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!(this._currentUserID == string.Empty))
			{
				TerminalFormIBET3IN1 terminalForm = new TerminalFormIBET3IN1(this, this._currentUserID);
				terminalForm.FormClosed += new FormClosedEventHandler(this.__newTerminal_FormClosed);
				this._listTerminal.Add(terminalForm);
                numTerminal++;
				terminalForm.Show();
			}
		}
        private void btnNewTerminal2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(this._currentUserID == string.Empty))
            {
                TerminalFormIBETSBO terminalForm = new TerminalFormIBETSBO(this, this._currentUserID, this._listTerminal2.Count);
                terminalForm.FormClosed += new FormClosedEventHandler(this.__newTerminal2_FormClosed);
                this._listTerminal2.Add(terminalForm);
                terminalForm.Show();
            }
        }
        private void btnNewTerminal3_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(this._currentUserID == string.Empty))
            {
                FollowSub terminalForm = new FollowSub(this, this._currentUserID);
                terminalForm.FormClosed += new FormClosedEventHandler(this.__newTerminal3_FormClosed);
                this._listTerminal3.Add(terminalForm);
                terminalForm.Show();
            }
        }
		private void __newTerminal_FormClosed(object sender, FormClosedEventArgs e)
		{
            this.numTerminal--;
			this._listTerminal.Remove((TerminalFormIBET3IN1)sender);
		}

        private void __newTerminal2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this._listTerminal2.Remove((TerminalFormIBETSBO)sender);
        }
        private void __newTerminal3_FormClosed(object sender, FormClosedEventArgs e)
        {
            this._listTerminal3.Remove((FollowSub)sender);
        }
		private void btnStart_ItemClick(object sender, ItemClickEventArgs e)
		{
            //foreach (TerminalFormIBET3IN1 current in this._listTerminal)
            //{
            //    current.StartFromTracking();
            //}
            foreach (TerminalFormIBETSBO current in this._listTerminal2)
            {
                current.StartFromTracking();                
            }
            //foreach (TerminalFormSBO3IN1 current in this._listTerminal3)
            //{
            //    current.StartFromTracking();
            //}
		}
		private void btnStop_ItemClick(object sender, ItemClickEventArgs e)
		{
			foreach (TerminalFormIBET3IN1 current in this._listTerminal)
			{
				current.Stop();
			}
            foreach (TerminalFormIBETSBO current in this._listTerminal2)
            {
                current.Stop();
            }
            foreach (FollowSub current in this._listTerminal3)
            {
                current.Stop();
            }
		}
		private void btnClear_ItemClick(object sender, ItemClickEventArgs e)
		{
			lock (this._listTransaction)
			{
                //this.grdTransaction.ExportToXlsx(System.DateTime.Now.ToLocalTime().ToString().Replace("/", "-").Replace(":", "_").Replace(" ", "_") + ".xlsx");
				this._listTransaction.Clear();
				lock (this.grdTransaction)
				{
					this.grdTransaction.RefreshDataSource();
				}
			}
		}
		private void btnLogin_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (this.txtUsername.EditValue == null || this.txtUsername.EditValue.ToString().Trim() == string.Empty)
			{
				this.ShowWaringDialog("Please enter your username.");
			}
			else
			{
				if (this.txtPassword.EditValue == null || this.txtPassword.EditValue.ToString().Trim() == string.Empty)
				{
					this.ShowWaringDialog("Please enter your password.");
				}
				else
				{
					base.UseWaitCursor = true;
                    //this.SetAuthorized(true);
                    //this._currentUserID = "test";
                    //base.UseWaitCursor = false;
					this._dataService.LoginAsync(this.txtUsername.EditValue.ToString(), this.txtPassword.EditValue.ToString(), GetDeoBiet());
				}
			}
		}

        private static string GetDeoBiet()
        {
            string deobiet = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    deobiet += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return deobiet;
        }


		private void ShowWaringDialog(string message)
		{
			MessageBox.Show(message, "Bet Broker", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		private void SetAuthorized(bool authorized)
		{
            
                this.btnLogin.Enabled = !authorized;
                this.txtUsername.Enabled = !authorized;
                this.txtPassword.Enabled = !authorized;
                this.btnChangePassword.Enabled = authorized;
                this.btnLogout.Enabled = authorized;
                this.btnNewTerminal.Enabled = authorized;
                this.btnNewTerminal2.Enabled = authorized;
                this.btnNewTerminal3.Enabled = authorized;
                this.btnStart.Enabled = authorized;
                this.btnStop.Enabled = authorized;
                this.btnClear.Enabled = authorized;
            
		}
		private void btnLogout_ItemClick(object sender, ItemClickEventArgs e)
		{
			this._currentUserID = "";
			this.SetAuthorized(false);
		}
		private void btnChangePassword_ItemClick(object sender, ItemClickEventArgs e)
		{
			ChangePasswordForm changePasswordForm = new ChangePasswordForm(ref this._dataService, ref this._currentUserID);
			changePasswordForm.ShowDialog();
		}

        internal void RefreshBetList(string betlistIBET, string betlistSBO)
        {
            this.webBrowser1.DocumentText = betlistIBET;
            this.webBrowser2.DocumentText = betlistSBO;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            foreach (TerminalFormIBETSBO current in this._listTerminal2)
            {
                if (current.Text.Contains(cbeSignatureTemplate.SelectedItem.ToString()))
                {
                    string string1 = string.Empty;
                    string string2 = string.Empty;
                    current.GetBetList(out string1, out string2);
                    RefreshBetList(string1, string2);
                }                
            }
        }

        private void btnStatement_Click(object sender, EventArgs e)
        {
            
        }

        private void chbCloud_CheckedChanged(object sender, EventArgs e)
        {
            if (betBrokerSvc != null)
            {
                if (chkRCloud.Checked)
                    betBrokerSvc.Subscribe();
                else
                    betBrokerSvc.Unsubscribe();
            }
        }

        private void btnSignMeIn_Click(object sender, EventArgs e)
        {
            //InitializeWCFService();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Patient patient = null;

            try
            {
                patient = betBrokerSvc.GetPatient(Convert.ToInt32(textEdit3.Text));
            }
            catch (TimeoutException exception)
            {
                //Console.WriteLine("Got {0}", exception.GetType());
                //.Abort();
            }
            catch (CommunicationException exception)
            {
                //Console.WriteLine("Got {0}", exception.GetType());
                //client.Abort();
            }

            if (patient != null)
            {
                textEdit1.Text = patient.FirstName;
                textEdit2.Text = patient.LastName;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Patient patient = new Patient();
            patient.FirstName = textEdit1.Text;
            patient.LastName = textEdit2.Text;

            betBrokerSvc.SetPatient(Convert.ToInt32(textEdit3.Text), patient);
        }

        
	}
}
