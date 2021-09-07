using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public static class LabourData
    {
        #region Fields

        public static JArray labourItems;

        #endregion Fields

        #region Methods

        public static async Task Load(ClientConnection client)
        {
            if (labourItems == null)
            {
                client = await RequestInterface.SendRequest(client, "modules/services/list.php", contentList: new List<string>()
                {
                    "del=false",
                    "rows=500",
                    "page=1"
                });

                JObject data = client.__lastContentAsJson;

                labourItems = (JArray)data["rows"];
            }
            return;
        }

        #endregion Methods
    }
}