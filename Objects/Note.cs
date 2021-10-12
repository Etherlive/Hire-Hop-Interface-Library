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

        #endregion Constructors

        #region Methods

        public static async Task<Note[]> GetNotes(ClientConnection client, string jobid, int page = 1)
        {
            JObject data = await Requests.Note.GetJobNotes(client, jobid, page);
            return data["rows"].Select(x => new Note(x["cell"])).ToArray();
        }

        #endregion Methods
    }
}