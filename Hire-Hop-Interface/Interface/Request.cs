﻿using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Hire_Hop_Interface.Interface
{
    public class Request
    {
        #region Fields

        public static readonly string hhMasterDomain = "myhirehop.com";
        public static readonly string hhMasterUrl = $"https://{hhMasterDomain}/";

        #endregion Fields

        #region Constructors

        public Request(string _url, string _method, Connections.CookieConnection _connecionCookie)
        {
            this.cookie = _connecionCookie;
            this.url = _url;
            this.method = _method;

            this.urlFormValues = new Dictionary<string, string>();
            this.urlQueryParams = new Dictionary<string, string>();
        }

        #endregion Constructors

        #region Properties

        public Connections.CookieConnection cookie { get; private set; }

        public string method { get; private set; }

        public string url { get; private set; }

        public Dictionary<string, string> urlFormValues { get; private set; }
        public Dictionary<string, string> urlQueryParams { get; private set; }

        public string urlWithParams
        {
            get
            {
                return urlQueryParams.Count > 0 ? QueryHelpers.AddQueryString(url, urlQueryParams) : url;
            }
        }

        #endregion Properties

        #region Methods

        public void AddOrSetForm(string key, string val)
        {
            if (!urlFormValues.TryAdd(key, val))
            {
                urlFormValues[key] = val;
            }
        }

        public void AddOrSetQuery(string key, string val)
        {
            if (!urlQueryParams.TryAdd(key, val))
            {
                urlQueryParams[key] = val;
            }
        }

        public virtual async Task<Response> Execute()
        {
            string urlWP = this.urlWithParams;

            using (var request = new HttpRequestMessage(new HttpMethod(this.method), $"{Request.hhMasterUrl}{urlWP}"))
            {
                if (this.urlFormValues.Count > 0)
                {
                    string form = string.Join("&", this.urlFormValues.Select(x => $"{HttpUtility.UrlEncode(x.Key)}={HttpUtility.UrlEncode(x.Value)}"));
                    request.Content = new StringContent(form);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
                }

                using (var response = await this.cookie.httpClient.SendAsync(request))
                {
                    string content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        Response r = new Response(this, content);

                        if (r.TryParseJson(out JsonElement? json))
                        {
                            if (json.Value.TryGetProperty("error", out JsonElement error))
                            {
                                throw new Exception(Interface.Errors.errorStrings[error.GetInt32()]);
                            }
                        }

                        return r;
                    }
                    else
                    {
                        throw new Exception("HH Error Occurred", new Exception(content));
                    }
                }
            }
        }

        public bool TryGetForm(string key, out string val)
        {
            return urlFormValues.TryGetValue(key, out val);
        }

        public bool TryGetQuery(string key, out string val)
        {
            return urlQueryParams.TryGetValue(key, out val);
        }

        #endregion Methods
    }
}