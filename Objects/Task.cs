using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Hire_Hop_Interface.Management;

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