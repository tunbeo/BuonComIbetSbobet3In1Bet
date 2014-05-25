//#define aThanh
#define full
using System;
using System.Windows.Forms;
using DevExpress.Utils.Drawing.Helpers;
namespace iBet.App
{
	internal static class Program
	{
		[System.STAThread]
		private static void Main()
		{
            DevExpress.UserSkins.BonusSkins.Register();
#if aThanh
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
#endif
#if full

            
            if (!NativeVista.IsVista)
                DevExpress.Skins.SkinManager.EnableFormSkins();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);                        
            Application.Run(new MainForm());
#endif
		}
	}
}