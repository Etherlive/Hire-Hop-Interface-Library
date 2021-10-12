using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        #region Properties

        public DateTime dtstart
        {
            get { return DateTime.Parse(Data["dtstart"].ToString()); }
        }

        public DateTime due
        {
            get { return DateTime.Parse(Data["due"].ToString()); }
        }

        public int id
        {
            get { return int.Parse(Data["id"].ToString()); }
        }

        public int jobId
        {
            get { return int.Parse(Data["main_id"].ToString()); }
        }

        public int priority
        {
            get { return int.Parse(Data["priority"].ToString()); }
        }

        public int status
        {
            get { return int.Parse(Data["status"].ToString()); }
        }

        public string summary
        {
            get { return Data["description"].ToString(); }
        }

        public string username
        {
            get { return Data["username"].ToString(); }
        }

        #endregion Properties

        #region Methods

        public static async Task<Tasks[]> GetTasks(ClientConnection client, string jobid, int page = 1)
        {
            JObject data = await Requests.Tasks.GetJobTasks(client, jobid, page);
            return data["rows"].Select(x => new Tasks(x["cell"])).ToArray();
        }

        public async void Save(ClientConnection client)
        {
            Data = await Requests.Tasks.AddJobTask(client, this.jobId.ToString(), summary, dtstart, due, status, priority, this.id);
        }

        #endregion Methods
    }
}