using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Hire_Hop_Interface.Management;
using System;

namespace Hire_Hop_Interface.Objects
{
    public class Tasks
    {
        #region Fields

        public JObject Data;

        #endregion Fields

        #region Constructors

        public Tasks(JToken _data)
        {
            Data = (JObject)_data;
        }

        public Tasks(ClientConnection client, string jobid, string summary, DateTime start, DateTime due, int status, int priority)
        {
            var addTask = Requests.Tasks.AddJobTask(client, jobid, summary, start, due, status, priority);
            addTask.Wait();
            Data = (JObject)addTask.Result["rows"]["0"]["cell"];
        }

        #endregion Constructors

        #region Methods

        public static async Task<Tasks[]> GetTasks(ClientConnection client, string jobid, int page = 1)
        {
            JObject data = await Requests.Tasks.GetJobTasks(client, jobid, page);
            return data["rows"].Select(x => new Tasks(x["cell"])).ToArray();
        }

        #endregion Methods
    }
}