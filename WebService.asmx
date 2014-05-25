<%@ WebService Language="C#"
Class="Tun.DataService" %>
using System.Web.Services;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Tun
{
  public class DataService: WebService 
  {   

      [WebMethod]
      public bool Login(string userName, string password, string deobiet, out string userID)
      {
          if (ValidDeoBiet(deobiet))
          {
              if (userName == "bb81" && password == "bb")
              {
                  object[] objArray = { true, "bb81" };
                  userID = (string)objArray[1];
                  //Write(userName + ", login,,,,,,,,,,,,,,,");
                  return (bool)objArray[0];
              }
              else if (userName == "admin" && password == "aaaaaa")
              {
                  object[] objArray = { true, "admin" };
                  userID = (string)objArray[1];
                  //Write(userName + ", login,,,,,,,,,,,,,,,");
                  return (bool)objArray[0];
              }
              
              else if (userName == "ttttt" && password == "ttttt")
              {
                  object[] objArray = { true, "ttttt" };
                  userID = (string)objArray[1];
                  //Write(userName + ", login,,,,,,,,,,,,,,,");
                  return (bool)objArray[0];
              }
              else if (userName == "ccccc" && password == "ccccc")
              {
                  object[] objArray = { true, "ccccc" };
                  userID = (string)objArray[1];
                  //Write(userName + ", login,,,,,,,,,,,,,,,");
                  return (bool)objArray[0];
              }
              
              
    
              else
              {
                  userID = "";
                  return false;
              }
          }
          else
          {
              userID = "";
              return false;
          }         
                    
      } 
      
	[WebMethod]
	public bool StartTerminal(string userID, string accountPair)
	{
        //Write(userID + ", start," + accountPair + ",,,,,,,,,,,,,,");
		return true;
	}
	
	[WebMethod]
	public bool StopTerminal(string userID, string accountPair)
	{
        //Write(userID + ", stop," + accountPair + ",,,,,,,,,,,,,,");
		return false;
	}
	
	
	[WebMethod]
	public bool AllowRun(string userID, string ibetAccount, string sbobetAccount)
	{
		return true;																								
	}
	[WebMethod]
	public bool ChangePassword(string userID, string oldPassword, string newPassword)
	{														
		return true;																								
	}																																				
	[WebMethod]
	public void AddTransaction(
	
		string a1, 
		string a2, 
		string a3, 
		string a4, 
		string a5, 
		string a6, 
		string a7, 
		string a8, 
		bool a9, 
		bool a10, 
		bool a11, 
		bool a12,
        bool a13,
        bool a14, 
		string a15, 
		DateTime a16)
	{
        Write(a1 + ",bet," + a2 + "," + a3 + "," + a4 + "," + a5 + "," + a6 + "," + a7 + "," + a8 + "," + a9 + "," + a10 + "," + a11 + ","
            + a12 + "," + a13 + "," + a14 + "," + a15 + "," + a16);
	
	}

      public static void Log(string logMessage, TextWriter w)
      {
          w.Write("\r\n");
          w.Write("{0} {1}", DateTime.Now.ToLongTimeString(),
              DateTime.Now.ToShortDateString() + ",");
          w.Write("{0}", logMessage);
          //w.WriteLine("-------------------------------");
          // Update the underlying file.
          w.Flush();
      }

      public static void Write(string text)
      {
          using (StreamWriter w = File.AppendText("F://Inetpub//vhosts//apbenvironment.com//httpdocs//log.csv"))
          {
              Log(text, w);
              w.Close();
          }          
      }      

      public static bool ValidDeoBiet(string deobiet)
      {
		  List<string> macs = new List<string>();
		  macs.Add("0003FF619043"); // virtual
		  macs.Add("109ADD639043"); // Tun
		  macs.Add("8C89A5319E75"); // Cuong
		  macs.Add("4061865B3290"); // bibop
		  macs.Add("E0CB4ED17A50"); // Thang to
		  macs.Add("00231443AA9C"); // Thang lap
		  macs.Add("54424904DF90"); // Thang lap
		  //macs.Add("1C6F65946A93"); // Thanh muoi
		  //macs.Add("78843C9966EE"); // Thanh muoi2		  
		  //macs.Add("040CCECE3614"); // Thanh muoi3		  
		  macs.Add("C83A35CAF7A1"); // Tun HN	
		  macs.Add("6C626D95DEC4"); // bibop
		  macs.Add("00304F845FCB"); // bibop00304f845fcb
		  macs.Add("0003FF669043"); // win7 ao
		  //macs.Add("001DBAF0FC96"); // be di nho xoa
		  //macs.Add("0022FB56C528"); // be di nho xoa
		  //78-84-3C-99-66-EE

		  if (macs.Contains(deobiet))
		     return true;
	 	  return false;
      }
   }
}