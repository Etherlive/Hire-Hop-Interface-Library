﻿using Hire_Hop_Interface.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class SearchResult : JsonObject
    {
        #region Constructors

        public SearchResult()
        { }

        public SearchResult(JsonElement _json)
        {
            this.json = _json;
        }

        #endregion Constructors

        #region Properties

        public string id
        {
            get { return this.json.Value.GetProperty("ID").GetString(); }
        }

        public bool is_job
        {
            get { return this.id.StartsWith("j"); }
        }

        public bool is_project
        {
            get { return this.id.StartsWith("p"); }
        }

        #endregion Properties

        #region Methods

        public static async Task<SearchResult[]> Search(SearchOptions options, Interface.Connections.CookieConnection cookie)
        {
            var req = new Request("frames/search_field_results.php", "POST", cookie);

            req.AddOrSetQuery("local", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            req.AddOrSetQuery("jobs", options.jobs ? "1" : "0");
            req.AddOrSetQuery("projects", options.projects ? "1" : "0");
            req.AddOrSetQuery("open", options.open ? "1" : "0");
            req.AddOrSetQuery("closed", options.closed ? "1" : "0");
            req.AddOrSetQuery("money_owed", options.money_owed ? "1" : "0");
            req.AddOrSetQuery("is_late", options.is_late ? "1" : "0");
            req.AddOrSetQuery("mine", options.mine ? "1" : "0");
            req.AddOrSetQuery("no_user", options.no_user ? "1" : "0");
            req.AddOrSetQuery("needs_bill", options.needs_bill ? "1" : "0");

            req.AddOrSetQuery("status", options.status);
            req.AddOrSetQuery("_search", options.search ? "true" : "false");
            req.AddOrSetQuery("rows", options.rows.ToString());
            req.AddOrSetQuery("page", options.page.ToString());

            req.AddOrSetQuery("last_search_idx", "-1");
            req.AddOrSetQuery("nd", "1630572957447");
            req.AddOrSetQuery("sidx", "OUT_DATE");
            req.AddOrSetQuery("sord", "asc");

            var res = await req.Execute();

            if (res.TryParseJson(out JsonElement? json))
            {
                List<SearchResult> results = new List<SearchResult>();
                var rows = json.Value.GetProperty("rows").EnumerateArray();
                while (rows.MoveNext())
                {
                    results.Add(new SearchResult(rows.Current.GetProperty("cell")));
                }

                return results.ToArray();
            }
            return null;
        }

        public async Task<Job> GetJob(Interface.Connections.CookieConnection cookie)
        {
            var job = new Job(this.id);
            if (await job.LoadData(cookie))
            {
                return job;
            }
            else
            {
                throw new Exception("Failed to load Job Data");
            }
        }

        #endregion Methods

        #region Classes

        public class SearchOptions
        {
            #region Fields

            public int depot = -1, rows = 40, page = 1;

            public string from = "", to = "", status = "0,1,2,3,4,5,6,7,8", job_name;

            public bool jobs = true, projects = false, open = false, closed = false, search = false, money_owed = false, is_late = false, mine = false, no_user = false, needs_bill = false;

            #endregion Fields
        }

        #endregion Classes
    }
}