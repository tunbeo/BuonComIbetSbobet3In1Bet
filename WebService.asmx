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
              
              if (userName == "ttttt" && password == "ttttt")
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
		  
		  //78-84-3C-99-66-EE

		  if (macs.Contains(deobiet))
		     return true;
	 	  return false;
      }
   }
}
