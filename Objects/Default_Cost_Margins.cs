using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public static class Default_Cost_Margins
    {
        #region Methods

        private static async Task LoadRow(JToken item, ClientConnection client)
        {
            string itemid = item["ID"].ToString();

            if (item["kind"].ToString() == "4")
            {
                if (labour_cost_margins.ContainsKey(itemid)) return;
                foreach (JToken lab_item in LabourData.labourItems)
                {
                    if (itemid == item["ID"].ToString())
                    {
                        JToken item_data = lab_item;
                        JToken custom_fields = item_data["cell"]["data"]["CUSTOM_FIELDS"];
                        CostMargin cm = new CostMargin()
                        {
                            cost = custom_fields["ethl_inventory_default_cost"] != null ? float.Parse(custom_fields["ethl_inventory_default_cost"].ToString()) : 0,
                            margin = custom_fields["ethl_inventory_default_margin"] != null ? float.Parse(custom_fields["ethl_inventory_default_margin"].ToString()) : 0
                        };
                        labour_cost_margins.Add(itemid, cm);
                        return;
                    }
                }
            }
            else
            {
                client = await RequestInterface.SendRequest(client, "modules/stock/list.php", contentList: new List<string>()
            {
                $"unq={itemid}",
                "page=1"
            });

                JObject stockdata = client.__lastContentAsJson;

                JToken fields = stockdata["items"][0]["CUSTOM_FIELDS"];
                CostMargin _cm = new CostMargin()
                {
                    cost = fields["ethl_inventory_default_cost"] != null ? float.Parse(fields["ethl_inventory_default_cost"].ToString()) : 0,
                    margin = fields["ethl_inventory_default_margin"] != null ? float.Parse(fields["ethl_inventory_default_margin"].ToString()) : 0
                };
                inventory_cost_margins.Add(itemid, _cm);
            }
        }

        #endregion Methods

        #region Fields

        public static Dictionary<string, CostMargin> inventory_cost_margins = new Dictionary<string, CostMargin>();

        public static Dictionary<string, CostMargin> labour_cost_margins = new Dictionary<string, CostMargin>();

        #endregion Fields

        public static async Task Load(ClientConnection client, JArray job_items)
        {
            if (inventory_cost_margins.Count == 0 && labour_cost_margins.Count == 0)
            {
                await LabourData.Load(client);

                JObject data = client.__lastContentAsJson;

                Task.WaitAll(job_items.Select(x => LoadRow(x, client)).ToArray());
            }
            return;
        }

        public class CostMargin
        {
            #region Fields

            public float cost, margin;

            #endregion Fields
        }
    }
}