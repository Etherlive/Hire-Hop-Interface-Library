using System;
using System.Net.Http;
using System.Net;

namespace Hire_Hop_Interface.HireHop
{
    public class ConnecionCookie
    {
        #region Fields

        private HttpClient _httpClient = null;
        private HttpClientHandler _httpHandler = null;

        #endregion Fields

        #region Methods

        private HttpClient constructClient()
        {
            this._httpClient = new HttpClient(this.httpHandler);
            return this._httpClient;
        }

        private HttpClientHandler constructHandler()
        {
            this._httpHandler = new HttpClientHandler();
            this._httpHandler.UseCookies = true;

            this._httpHandler.CookieContainer.Add(new Cookie("email", this.email, "/", Request.hhMasterDomain));
            this._httpHandler.CookieContainer.Add(new Cookie("password", this.password, "/", Request.hhMasterDomain));
            this._httpHandler.CookieContainer.Add(new Cookie("key", this.key, "/", Request.hhMasterDomain));

            return this._httpHandler;
        }

        #endregion Methods

        public readonly string email, password, key;

        public ConnecionCookie(string _email, string _password, string _key)
        {
            this.email = _email;
            this.password = _password;
            this.key = _key;
        }

        public CookieCollection cookies
        {
            get
            {
                return this._httpHandler.CookieContainer.GetCookies(new Uri(Request.hhMasterUrl));
            }
        }

        public HttpClient httpClient
        {
            get { return this._httpClient == null ? constructClient() : this._httpClient; }
            private set { this._httpClient = value; }
        }

        public HttpClientHandler httpHandler
        {
            get { return this._httpHandler == null ? this.constructHandler() : this._httpHandler; }
            private set { this._httpHandler = value; }
        }
    }
}