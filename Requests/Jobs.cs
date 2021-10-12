using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Requests
{
    public static class Jobs
    {
        #region Methods

        public static async Task<JObject> GetJobBill(ClientConnection client, string jobId, int page = 1)
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
                $"page={page}",
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

        public static async Task<JObject> GetJobHistory(ClientConnection client, string jobId, int page = 1)
        {
            client = await RequestInterface.SendRequest(client, "php_functions/log_list.php", queryList: new List<string>()
            {
                $"main_id={jobId}",
                "type=1",
                "_search=false",
                "nd=1631267281622",
                "rows=100",
                $"page={page}",
                "sidx=date_time",
                "sord=desc"
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

        public static async Task<JObject> SaveCustomFields(ClientConnection client, string jobId, List<Objects.JobCustomField> customFields)
        {
            var content = new List<string>()
            {
                $"main_id={jobId}"
            };

            customFields.ForEach(x =>
            {
                string pre = $"fields[ethl_custom_fields][value][{x.id}]";
                content.Add($"{pre}[id]={x.id}");
                content.Add($"{pre}[key]={x.key}");
                content.Add($"{pre}[value]={x.value}");
            });

            client = await RequestInterface.SendRequest(client, "php_functions/job_save.php", contentList: content);
            return client.__lastContentAsJson;
        }

        #endregion Methods
    }
}