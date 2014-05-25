using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace iBet.App.Engine.Core
{
    public enum HttpMethod
	{
		Post,
		Get
	}
    public sealed class HttpHelper
    {
        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:16.0) Gecko/20100101 Firefox/16.0";
        private const string DF_ACCEPT = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        private const string DF_CONTENT_TYPE = "application/x-www-form-urlencoded";
        private CookieCollection cookies;
        private string accept;
        private bool keepAlive;
        private string contentType;
        private Encoding responseEncoding;
        private Encoding postDataEncoding;
        private readonly object _cookielocker = new object();
        public CookieContainer cookieContainer
        {
            get;
            set;
        }
        public string Accept
        {
            get
            {
                return this.accept;
            }
            set
            {
                this.accept = value;
            }
        }
        public bool KeepAlive
        {
            get
            {
                return this.keepAlive;
            }
            set
            {
                this.keepAlive = value;
            }
        }
        public string ContentType
        {
            get
            {
                return this.contentType;
            }
            set
            {
                this.contentType = value;
            }
        }
        public Encoding ResponseEncoding
        {
            get
            {
                return this.responseEncoding;
            }
            set
            {
                this.responseEncoding = value;
            }
        }
        public Encoding PostDataEncoding
        {
            get
            {
                return this.postDataEncoding;
            }
            set
            {
                this.postDataEncoding = value;
            }
        }
        public HttpHelper()
        {
            ServicePointManager.DefaultConnectionLimit = 24;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            object cookielocker;
            Monitor.Enter(cookielocker = this._cookielocker);
            try
            {
                this.cookies = new CookieCollection();
                this.cookieContainer = new CookieContainer();
            }
            finally
            {
                Monitor.Exit(cookielocker);
            }
            this.accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            this.contentType = "application/x-www-form-urlencoded";
            this.keepAlive = true;
            this.responseEncoding = Encoding.UTF8;
            this.postDataEncoding = Encoding.GetEncoding(1252);
        }
        public string Fetch(string url, HttpMethod method, string referer, string postData)
        {
            string result = string.Empty;
            try
            {
                using (HttpWebResponse httpWebResponse = this.FetchResponse(url, method, referer, postData))
                {
                    if (httpWebResponse != null)
                    {
                        using (Stream responseStream = httpWebResponse.GetResponseStream())
                        {
                            using (StreamReader streamReader = new StreamReader(responseStream, this.responseEncoding, true))
                            {
                                result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLogger.Log(ex, LogMode.Normal, StaticLog.Main);
            }
            return result;
        }
        private void CloseResponse(HttpWebResponse res)
        {
            if (res != null)
            {
                res.Close();
            }
        }
        private HttpWebResponse FetchResponse(string url, HttpMethod method, string referer, string postData)
        {
            HttpWebResponse httpWebResponse = null;
            try
            {
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    //WebRequest.DefaultWebProxy = null;
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0";
                    httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                    httpWebRequest.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);
                    httpWebRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
                    httpWebRequest.Headers.Add(HttpRequestHeader.KeepAlive, "true");
                    if (!string.IsNullOrEmpty(referer))
                    {
                        httpWebRequest.Referer = referer;
                    }
                    object cookielocker;
                    Monitor.Enter(cookielocker = this._cookielocker);
                    try
                    {
                        httpWebRequest.CookieContainer = this.cookieContainer;
                    }
                    finally
                    {
                        Monitor.Exit(cookielocker);
                    }
                    httpWebRequest.Method = method.ToString().ToUpper();
                    if (method == HttpMethod.Post && !string.IsNullOrEmpty(postData))
                    {
                        byte[] bytes = this.postDataEncoding.GetBytes(postData);
                        httpWebRequest.ContentLength = (long)bytes.Length;
                        Stream requestStream = httpWebRequest.GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    httpWebResponse.Cookies = httpWebRequest.CookieContainer.GetCookies(httpWebRequest.RequestUri);
                    object cookielocker2;
                    Monitor.Enter(cookielocker2 = this._cookielocker);
                    try
                    {
                        this.cookies.Add(httpWebResponse.Cookies);
                        this.cookieContainer.Add(this.cookies);
                    }
                    finally
                    {
                        Monitor.Exit(cookielocker2);
                    }
                }
            }
            catch (Exception ex)
            {
                this.CloseResponse(httpWebResponse);
                httpWebResponse = null;
                EngineLogger.Log(ex, LogMode.Normal, StaticLog.Main);
            }
            return httpWebResponse;
        }
        public Image FetchImage(string url, HttpMethod method, string referer, string postData)
        {
            Image result = null;
            try
            {
                using (HttpWebResponse httpWebResponse = this.FetchResponse(url, method, referer, postData))
                {
                    if (httpWebResponse != null)
                    {
                        using (Stream responseStream = httpWebResponse.GetResponseStream())
                        {
                            using (StreamReader streamReader = new StreamReader(responseStream, this.responseEncoding, true))
                            {
                                result = Image.FromStream(streamReader.BaseStream);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLogger.Log(ex, LogMode.Normal, StaticLog.Main);
                string message = this.Fetch(url, method, referer, postData);
                EngineLogger.Log("FK==>>>", message, LogMode.Normal, StaticLog.Main);
            }
            return result;
        }
        public string FetchResponseUri(string url, HttpMethod method, string referer, string postData)
        {
            string result = string.Empty;
            try
            {
                using (HttpWebResponse httpWebResponse = this.FetchResponse(url, method, referer, postData))
                {
                    if (httpWebResponse != null)
                    {
                        result = httpWebResponse.ResponseUri.AbsoluteUri;
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLogger.Log(ex, LogMode.Normal, StaticLog.Main);
            }
            return result;
        }
        public void ClearCookies()
        {
            object cookielocker;
            Monitor.Enter(cookielocker = this._cookielocker);
            try
            {
                this.cookies = new CookieCollection();
                this.cookieContainer = new CookieContainer();
            }
            finally
            {
                Monitor.Exit(cookielocker);
            }
        }
        public string FetchWithProxy(string url, HttpMethod method, string referer, string postData)
        {
            string result = string.Empty;
            try
            {
                using (HttpWebResponse httpWebResponse = this.FetchResponseWithProxy(url, method, referer, postData))
                {
                    if (httpWebResponse != null)
                    {
                        using (Stream responseStream = httpWebResponse.GetResponseStream())
                        {
                            using (StreamReader streamReader = new StreamReader(responseStream, this.responseEncoding, true))
                            {
                                result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLogger.Log(ex, LogMode.Normal, StaticLog.Main);
            }
            return result;
        }
        public Image FetchImageWithProxy(string url, HttpMethod method, string referer, string postData)
        {
            Image result = null;
            try
            {
                using (HttpWebResponse httpWebResponse = this.FetchResponseWithProxy(url, method, referer, postData))
                {
                    if (httpWebResponse != null)
                    {
                        using (Stream responseStream = httpWebResponse.GetResponseStream())
                        {
                            using (StreamReader streamReader = new StreamReader(responseStream, this.responseEncoding, true))
                            {
                                result = Image.FromStream(streamReader.BaseStream);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLogger.Log(ex, LogMode.Normal, StaticLog.Main);
                string message = this.Fetch(url, method, referer, postData);
                EngineLogger.Log("FK==>>>", message, LogMode.Normal, StaticLog.Main);
            }
            return result;
        }
        public string FetchResponseUriWithProxy(string url, HttpMethod method, string referer, string postData)
        {
            string result = string.Empty;
            try
            {
                using (HttpWebResponse httpWebResponse = this.FetchResponseWithProxy(url, method, referer, postData))
                {
                    if (httpWebResponse != null)
                    {
                        result = httpWebResponse.ResponseUri.AbsoluteUri;
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLogger.Log(ex, LogMode.Normal, StaticLog.Main);
            }
            return result;
        }
        private HttpWebResponse FetchResponseWithProxy(string url, HttpMethod method, string referer, string postData)
        {
            HttpWebResponse httpWebResponse = null;
            try
            {
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.Proxy = new WebProxy("Insert Proxy IP Here", 1111);
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:16.0) Gecko/20100101 Firefox/16.0";
                    httpWebRequest.Accept = this.accept;
                    httpWebRequest.KeepAlive = this.keepAlive;
                    httpWebRequest.ContentType = this.contentType;
                    if (!string.IsNullOrEmpty(referer))
                    {
                        httpWebRequest.Referer = referer;
                    }
                    object cookielocker;
                    Monitor.Enter(cookielocker = this._cookielocker);
                    try
                    {
                        httpWebRequest.CookieContainer = this.cookieContainer;
                    }
                    finally
                    {
                        Monitor.Exit(cookielocker);
                    }
                    httpWebRequest.Method = method.ToString().ToUpper();
                    if (method == HttpMethod.Post && !string.IsNullOrEmpty(postData))
                    {
                        byte[] bytes = this.postDataEncoding.GetBytes(postData);
                        httpWebRequest.ContentLength = (long)bytes.Length;
                        Stream requestStream = httpWebRequest.GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    httpWebResponse.Cookies = httpWebRequest.CookieContainer.GetCookies(httpWebRequest.RequestUri);
                    object cookielocker2;
                    Monitor.Enter(cookielocker2 = this._cookielocker);
                    try
                    {
                        this.cookies.Add(httpWebResponse.Cookies);
                        this.cookieContainer.Add(this.cookies);
                    }
                    finally
                    {
                        Monitor.Exit(cookielocker2);
                    }
                }
            }
            catch (Exception ex)
            {
                this.CloseResponse(httpWebResponse);
                httpWebResponse = null;
                EngineLogger.Log(ex, LogMode.Normal, StaticLog.Main);
            }
            return httpWebResponse;
        }
    }
}
