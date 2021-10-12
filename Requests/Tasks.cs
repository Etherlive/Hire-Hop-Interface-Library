using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Requests
{
    public static class Tasks
    {
        #region Methods

        public static async Task<JObject> AddJobTask(ClientConnection client, string jobId, string summary, DateTime start, DateTime due, int status, int priority)
        {
            client = await RequestInterface.SendRequest(client, "php_functions/todo_save.php", contentList: new List<string>()
            {
                "id=0",
                $"main_id={jobId}",
                "type=1",
                $"summary={summary}",
                $"dtstart={start.ToString("yyyy-MM-dd")}",
                $"due={due.ToString("yyyy-MM-dd")}",
                $"status={status}",
                $"priority={priority}",
                $"user={client.uid}",
                "tz=Europe/London"
            });
            return client.__lastContentAsJson;
        }

        public static async Task<JObject> GetJobTasks(ClientConnection client, string jobId, int page = 1)
        {
            client = await RequestInterface.SendRequest(client, "php_functions/todo_list.php", queryList: new List<string>()
            {
                $"main_id={jobId}",
                "type=1",
                "_search=false",
                "nd=1634027079497",
                "rows=50",
                $"page={page}",
                "sidx=DTSTART",
                "sord=desc"
            });
            return client.__lastContentAsJson;
        }

        #endregion Methods
    }
}