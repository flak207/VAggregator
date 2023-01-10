using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KEA.VAggregator.StdLib
{
    public class WebClientEx : WebClient
    {
        private readonly CookieCollection _cookies = new CookieCollection();
        private bool _initialized = false;

        public event EventHandler ClientInitialized;

        public CookieCollection Cookies
        {
            get { return _cookies; }
            //set { container = value; }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest webRequest = base.GetWebRequest(address);
            var httpWebRequest = webRequest as HttpWebRequest;
            if (httpWebRequest != null)
            {
                httpWebRequest.CookieContainer = new CookieContainer();
                httpWebRequest.CookieContainer.Add(_cookies);
            }
            return webRequest;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse response = base.GetWebResponse(request, result);
            ReadCookies(response);
            return response;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            ReadCookies(response);
            return response;
        }

        private void ReadCookies(WebResponse r)
        {
            var response = r as HttpWebResponse;
            if (response != null && !_initialized)
            {
                _cookies.Add(response.Cookies);
                _initialized = true;
                ClientInitialized?.Invoke(this, new EventArgs());
                //Console.WriteLine(_cookies.Count);
            }
        }
    }
}
