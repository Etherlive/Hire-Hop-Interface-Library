﻿using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Requests
{
    public static class Jobs
    {
        #region Methods

        public static async Task<JObject> GetJobHistory(ClientConnection client, string jobId)
        {
            client = await RequestInterface.SendRequest(client, "php_functions/log_list.php", queryList: new List<string>()
            {
                $"main_id={jobId}",
                "type=1",
                "_search=false",
                "nd=1631267281622",
                "rows=100",
                "page=1",
                "sidx=date_time",
                "sord=desc"
            });
            return client.__lastContentAsJson;
        }

        public static async Task<JObject> GetJobBill(ClientConnection client, string jobId)
        {
            string date_str = $"{ DateTime.Now.ToString("yyyy-MM-dd+HH:mm:ss")}";

            client = await RequestInterface.SendRequest(client, "php_functions/billing_list.php", queryList: new List<string>()
            {
                $"main_id={jobId}",
                "type=1",
                $"local={date_str}",
                "tz=Europe/London",
                "fix=0",
                "_search=false",
                "nd=1630575681100",
                "rows=10000",
                "page=1",
                "sidx=",
                "sord=asc"
            });
            return client.__lastContentAsJson;
        }

        public static async Task<JObject> GetJobData(ClientConnection client, string jobId)
        {
            client = await RequestInterface.SendRequest(client, "php_functions/job_refresh.php", contentList: new List<string>()
            {
                $"job={jobId}"
            });
            return client.__lastContentAsJson;
        }

        public static async Task<JArray> GetJobItems(ClientConnection client, string jobId)
        {
            client = await RequestInterface.SendRequest(client, "frames/items_to_supply_list.php", contentList: new List<string>()
            {
                $"job={jobId}"
            });
            return (JArray)client.__lastContentAsJson["items"];
        }

        #endregion Methods
    }
}