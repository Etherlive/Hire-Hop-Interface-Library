using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Hire_Hop_Interface.Management;

namespace Hire_Hop_Interface.Objects
{
    public class Note
    {
        #region Fields

        public JObject Data;

        #endregion Fields

        #region Constructors

        public Note(JToken _data)
        {
            Data = (JObject)_data;
        }

        public Note(ClientConnection client, string jobid, string noteBody)
        {
            var addTask = Requests.Note.AddJobNote(client, jobid, noteBody);
            addTask.Wait();
            Data = (JObject)addTask.Result["rows"][0]["cell"];
        }

        #endregion Constructors

        #region Properties

        public int id
        {
            get { return int.Parse(Data["id"].ToString()); }
        }

        public int jobId
        {
            get { return int.Parse(Data["main_id"].ToString()); }
        }

        public string note
        {
            get { return Data["note"].ToString(); }
        }

        public string username
        {
            get { return Data["username"].ToString(); }
        }

        #endregion Properties

        #region Methods

        public static async Task<Note[]> GetNotes(ClientConnection client, string jobid, int page = 1)
        {
            JObject data = await Requests.Note.GetJobNotes(client, jobid, page);
            return data["rows"].Select(x => new Note(x["cell"])).ToArray();
        }

        public async void Save(ClientConnection client)
        {
            Data = await Requests.Note.AddJobNote(client, this.jobId.ToString(), note, id);
        }

        #endregion Methods
    }
}