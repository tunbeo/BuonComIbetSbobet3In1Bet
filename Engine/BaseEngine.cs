using System;
using System.Net;
namespace iBet.Engine
{
    public class BaseEngine
    {
        public static string CreateWebRequest(string requestType, string url, string referer, CookieContainer cookies, string host, bool useproxy, string text)
        {
            string returntext = string.Empty;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);            
            webRequest.Method = requestType;
            webRequest.CookieContainer = cookies;
            webRequest.Headers.Add("Accept-Language", "en-US");            
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0";
            webRequest.Host = host;
            webRequest.Referer = referer;
            webRequest.Timeout = 15000;
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.KeepAlive = true;
            webRequest.AutomaticDecompression = (System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate);

            if (webRequest.Method == "POST")
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = (long)text.Length;
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
                webRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
            }

            System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
            System.IO.StreamReader streamReader = new System.IO.StreamReader(httpWebResponse.GetResponseStream());
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
            {
                streamReader.Close();
                httpWebResponse.Close();
                return returntext;
            }
            else
            {
                returntext = streamReader.ReadToEnd();
                streamReader.Close();
                httpWebResponse.Close();
                return returntext;
            }
        }
    }
}
