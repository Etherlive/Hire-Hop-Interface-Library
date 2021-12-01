using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace Hire_Hop_Interface.Interface.Connections
{
    public class CookieConnection
    {
        #region Fields

        private HttpClient _httpClient = null;
        private HttpClientHandler _httpHandler = null;

        #endregion Fields

        #region Constructors

        public CookieConnection()
        {
            this.email = null;
            this.password = null;
            this.key = null;
        }

        public CookieConnection(string _email, string _password, string _key)
        {
            this.email = _email;
            this.password = _password;
            this.key = _key;
        }

        #endregion Constructors

        #region Properties

        public CookieCollection cookies
        {
            get
            {
                return this._httpHandler.CookieContainer.GetCookies(new Uri(Request.hhMasterUrl));
            }
        }

        public string email { get; private set; }

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

        public string key { get; private set; }
        public string password { get; private set; }
        public JsonElement user_data { get; set; }

        #endregion Properties

        #region Methods

        public void extractHeadersFromHandler()
        {
            var cookies = this._httpHandler.CookieContainer.GetCookies(new Uri(Request.hhMasterUrl));
            this.email = cookies.FirstOrDefault(x => x.Name == "email").Value;
            this.password = cookies.FirstOrDefault(x => x.Name == "password").Value;
            this.key = cookies.FirstOrDefault(x => x.Name == "key").Value;
        }

        private HttpClient constructClient()
        {
            this._httpClient = new HttpClient(this.httpHandler);
            return this._httpClient;
        }

        private HttpClientHandler constructHandler()
        {
            this._httpHandler = new HttpClientHandler();
            this._httpHandler.UseCookies = true;

            if (this.email != null)
            {
                this._httpHandler.CookieContainer.Add(new System.Net.Cookie("email", this.email, "/", Request.hhMasterDomain));
                this._httpHandler.CookieContainer.Add(new System.Net.Cookie("password", this.password, "/", Request.hhMasterDomain));
                this._httpHandler.CookieContainer.Add(new System.Net.Cookie("key", this.key, "/", Request.hhMasterDomain));
            }

            return this._httpHandler;
        }

        #endregion Methods
    }
}