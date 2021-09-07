using System;
using System.Collections.Generic;
using System.Text;
using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;

namespace Hire_Hop_Interface.Objects
{
    public static class Default_Cost_Margins
    {
        public class CostMargin
        {
            public float cost, margin;
        }

        public static Dictionary<string, CostMargin> cost_margins = new Dictionary<string, CostMargin>();

        public static async void Load(ClientConnection client)
        {
            client = await RequestInterface.SendRequest(client, "modules/services/list.php", contentList: new List<string>()
            {
                "del=false",
                "rows=500",
                "page=1"
            });

            JObject data = client.__lastContentAsJson;

            foreach(JToken item in data["row"])
            {
                if (cost_margins.ContainsKey(item["ID"].ToString()))
                {
                }
            }
        }
    }
}
