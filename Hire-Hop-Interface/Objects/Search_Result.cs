﻿using Hire_Hop_Interface.HireHop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class Search_Result : JsonObject
    {
        #region Constructors

        public Search_Result(JsonElement _json)
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

        public async Task<Job> GetJob(ConnectionCookie cookie)
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
    }
}