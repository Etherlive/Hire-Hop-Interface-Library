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

        public static async Task<JObject> GetJobItems(ClientConnection client, string jobId)
        {
            client = await RequestInterface.SendRequest(client, "frames/items_to_supply_list.php", contentList: new List<string>()
            {
                $"job={jobId}"
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

        #endregion Methods
    }
}