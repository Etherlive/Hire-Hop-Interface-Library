using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Management
{
    public static class RequestInterface
    {
        #region Methods

        public static async Task<ClientConnection> SendRequest(ClientConnection client, string urlPath, string method = "POST", List<string> contentList = null, List<string> queryList = null)
        {
            var httpClient = client.httpClient;

            if (queryList != null)
            {
                urlPath = QueryHelpers.AddQueryString(urlPath, queryList.ToDictionary(x => x.Split("=")[0], x => x.Split("=")[1]));
            }

            // In production code, don't destroy the HttpClient through using, but better reuse an
            // existing instance https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            using (var request = new HttpRequestMessage(new HttpMethod(method), $"{ClientConnection.url}{urlPath}"))
            {
                if (contentList != null)
                {
                    request.Content = new StringContent(string.Join("&", contentList));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
                }

                try
                {
                    var response = await httpClient.SendAsync(request);

                    client.__lastResponse = response;
                    client.__lastContent = await response.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    if (!Directory.Exists("./logs")) Directory.CreateDirectory("./logs");
                    Console.WriteLine("A Request Error Occurred: " + e.Source);
                    string fname = $"{e.Source} {DateTime.Now.ToShortDateString()} {DateTime.Now.Ticks}".Replace("\\", "-").Replace("/", "-").Replace(".", "-");
                    File.WriteAllText($"./logs/{fname}.log", e.ToString());
                }

                return client;
            }
        }

        #endregion Methods
    }

    public class ClientConnection
    {
        #region Fields

        public static readonly string url = "https://myhirehop.com/";
        public string __id, __lastContent;
        public HttpResponseMessage __lastResponse;
        public JObject userData;
        private HttpClient __httpClient;
        private HttpClientHandler __httpClientHandler;

        #endregion Fields

        #region Properties

        public JObject __lastContentAsJson
        {
            get
            {
                try
                {
                    return JObject.Parse(__lastContent);
                }
                catch
                {
                    return new JObject();
                }
            }
        }

        public CookieCollection cookies
        {
            get
            {
                return __httpClientHandler.CookieContainer.GetCookies(new Uri(url));
            }
        }

        public HttpClient httpClient
        {
            get
            {
                __httpClient = __httpClient == null ? new HttpClient(httpClientHandler) : __httpClient;
                return __httpClient;
            }
        }

        public HttpClientHandler httpClientHandler
        {
            get
            {
                __httpClientHandler = __httpClientHandler == null ? constructHttpClient() : __httpClientHandler;
                return __httpClientHandler;
            }
        }

        public int uid
        {
            get { return int.Parse(userData["ID"].ToString()); }
        }

        #endregion Properties

        #region Methods

        private HttpClientHandler constructHttpClient()
        {
            HttpClientHandler http = new HttpClientHandler();
            http.UseCookies = true;
            return http;
        }

        #endregion Methods
    }
}