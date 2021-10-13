using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Requests
{
    public static class Note
    {
        #region Methods

        public static async Task<JObject> AddJobNote(ClientConnection client, string jobId, string note, int id = 0)
        {
            client = await RequestInterface.SendRequest(client, "php_functions/notes_save.php", contentList: new List<string>()
            {
                $"id={id}",
                $"main_id={jobId}",
                "type=1",
                $"note={note}",
            });
            return client.__lastContentAsJson;
        }

        public static async Task DeleteJobNote(ClientConnection client, string jobId, int id = 0)
        {
            client = await RequestInterface.SendRequest(client, "php_functions/notes_delete.php", contentList: new List<string>()
            {
                $"id={id}",
                $"main_id={jobId}",
                "type=1"
            });
        }

        public static async Task<JObject> GetJobNotes(ClientConnection client, string jobId, int page = 1)
        {
            client = await RequestInterface.SendRequest(client, "php_functions/notes_list.php", queryList: new List<string>()
            {
                $"main_id={jobId}",
                "type=1",
                "_search=false",
                "nd=1634027079497",
                "rows=50",
                $"page={page}",
                "sidx=CREATE_DATE",
                "sord=desc"
            });
            return client.__lastContentAsJson;
        }

        #endregion Methods
    }
}