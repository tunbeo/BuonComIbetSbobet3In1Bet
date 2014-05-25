using DevExpress.XtraEditors;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using iBet.App.admin;
namespace iBet.App
{
	public class ChangePasswordForm : XtraForm
	{
        private admin.DataServiceSoapClient _dataService;
		private string _currentUserID;
		private System.ComponentModel.IContainer components = null;
		private LabelControl labelControl1;
		private TextEdit txtOldPassword;
		private TextEdit txtNewPassword;
		private LabelControl labelControl2;
		private TextEdit txtConfirmPassword;
		private LabelControl labelControl3;
		private SimpleButton btnChangePassword;
        public ChangePasswordForm(ref admin.DataServiceSoapClient dataService, ref string userID)
		{
			this.InitializeComponent();
			this._dataService = dataService;
			this._dataService.ChangePasswordCompleted += new EventHandler<admin.ChangePasswordCompletedEventArgs>(this._dataService_ChangePasswordCompleted);
			this._currentUserID = userID;
		}
		private void _dataService_ChangePasswordCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			MessageBox.Show("Change Password successful.");
		}
		private void btnChangePassword_Click(object sender, System.EventArgs e)
		{
			string text = this.txtOldPassword.Text.ToString().Trim();
			string text2 = this.txtNewPassword.Text.ToString().Trim();
			string b = this.txtConfirmPassword.Text.ToString().Trim();
			if (text == "")
			{
				MessageBox.Show("Please enter Old Password");
			}
			else
			{
				if (text2 == "")
				{
					MessageBox.Show("Please enter New Password");
				}
				else
				{
					if (text2 != b)
					{
						MessageBox.Show("New Password and Confirm Password must be same");
					}
					else
					{
						this._dataService.ChangePasswordAsync(this._currentUserID, text, text2);
					}
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
			this.labelControl1 = new LabelControl();
			this.txtOldPassword = new TextEdit();
			this.txtNewPassword = new TextEdit();
			this.labelControl2 = new LabelControl();
			this.txtConfirmPassword = new TextEdit();
			this.labelControl3 = new LabelControl();
			this.btnChangePassword = new SimpleButton();
			((System.ComponentModel.ISupportInitialize)this.txtOldPassword.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtNewPassword.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtConfirmPassword.Properties).BeginInit();
			base.SuspendLayout();
			this.labelControl1.Location = new System.Drawing.Point(12, 15);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(69, 13);
			this.labelControl1.TabIndex = 0;
			this.labelControl1.Text = "Old Password:";
			this.txtOldPassword.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtOldPassword.Location = new System.Drawing.Point(108, 12);
			this.txtOldPassword.Name = "txtOldPassword";
//			this.txtOldPassword.Properties.PasswordChar = '*';
			this.txtOldPassword.Size = new System.Drawing.Size(249, 20);
			this.txtOldPassword.TabIndex = 1;
			this.txtNewPassword.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtNewPassword.Location = new System.Drawing.Point(108, 38);
			this.txtNewPassword.Name = "txtNewPassword";
//			this.txtNewPassword.Properties.PasswordChar = '*';
			this.txtNewPassword.Size = new System.Drawing.Size(249, 20);
			this.txtNewPassword.TabIndex = 3;
			this.labelControl2.Location = new System.Drawing.Point(12, 41);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(74, 13);
			this.labelControl2.TabIndex = 2;
			this.labelControl2.Text = "New Password:";
			this.txtConfirmPassword.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtConfirmPassword.Location = new System.Drawing.Point(108, 64);
			this.txtConfirmPassword.Name = "txtConfirmPassword";
//			this.txtConfirmPassword.Properties.PasswordChar = '*';
			this.txtConfirmPassword.Size = new System.Drawing.Size(249, 20);
			this.txtConfirmPassword.TabIndex = 5;
			this.labelControl3.Location = new System.Drawing.Point(12, 67);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(90, 13);
			this.labelControl3.TabIndex = 4;
			this.labelControl3.Text = "Confirm Password:";
			this.btnChangePassword.Location = new System.Drawing.Point(108, 90);
			this.btnChangePassword.Name = "btnChangePassword";
			this.btnChangePassword.Size = new System.Drawing.Size(135, 23);
			this.btnChangePassword.TabIndex = 6;
			this.btnChangePassword.Text = "Change Password";
			this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
//			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(369, 132);
			base.Controls.Add(this.btnChangePassword);
			base.Controls.Add(this.txtConfirmPassword);
			base.Controls.Add(this.labelControl3);
			base.Controls.Add(this.txtNewPassword);
			base.Controls.Add(this.labelControl2);
			base.Controls.Add(this.txtOldPassword);
			base.Controls.Add(this.labelControl1);
//			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ChangePasswordForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Change Password";
			((System.ComponentModel.ISupportInitialize)this.txtOldPassword.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtNewPassword.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtConfirmPassword.Properties).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
