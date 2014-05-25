using BCCore;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace iBet.App
{
    public class frmIbet : Form
    {
        private IContainer components;
        private WebBrowser wb;
        
        private IBETAgent agent;
        private SBOAgent agent2;
        public string workingAgent = "IBET";
        private string url;
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
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmIbet));
            this.wb = new WebBrowser();
            base.SuspendLayout();
            this.wb.Dock = DockStyle.Fill;
            this.wb.Location = new Point(0, 0);
            this.wb.MinimumSize = new Size(20, 20);
            this.wb.Name = "wb";
            this.wb.ScriptErrorsSuppressed = true;
            this.wb.Size = new Size(1000, 600);
            this.wb.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(1000, 600);
            base.Controls.Add(this.wb);
            //base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            base.Name = "frmIbet";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = this.workingAgent;
            base.Load += new EventHandler(this.frmSbo_Load);
            base.ResumeLayout(false);
        }
        public frmIbet()
        {
            this.InitializeComponent();
        }
        public frmIbet(IBETAgent agent, string url) : this()
        {
            this.agent = agent;
            this.url = url;
        }
        public frmIbet(SBOAgent agent, string url)  : this()
        {
            this.agent2 = agent;
            this.url = url;
            this.workingAgent = "SBOBET";
        }
        public void SetCookieForBrowserControl(CookieContainer cc, Uri uri)
        {
            string uriString = uri.Scheme + Uri.SchemeDelimiter + uri.Host;
            Uri uri2 = new Uri(uriString);
            foreach (Cookie cookie in cc.GetCookies(uri2))
            {
                frmIbet.InternetSetCookieEx(uriString, cookie.Name, new StringBuilder(cookie.Value), 0, IntPtr.Zero);
            }
        }

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetSetCookieEx(string url, string cookieName, StringBuilder cookieData, int dwFlags, IntPtr lpReserved);

        private void frmSbo_Load(object sender, EventArgs e)
        {
            if (this.workingAgent == "SBOBET")
            {
                this.SetCookieForBrowserControl(this.agent2.cc, new Uri("http://" + this.agent2.Config.HostName));
                this.Text = this.workingAgent;
            }
            else
                this.SetCookieForBrowserControl(this.agent.cc, new Uri("http://" + this.agent.Config.HostName));
            this.wb.Navigate(this.url);
        }
    }
}
