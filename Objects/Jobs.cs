using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Hire_Hop_Interface.Management;
using Hire_Hop_Interface.Requests;

namespace Hire_Hop_Interface.Objects
{
    public class Jobs
    {
        public JObject data;
        public JObject Data
        {
            get { return data; }
        }

        public string id
        {
            get { return data["ID"].ToString().Replace("j",""); }
        }

        public Jobs(SearchResult searchResult)
        {
            data = searchResult.Data;
        }

        public class Costs
        {
            public float supplyingPrice, serviceCost, equipmentCost, resourceCost, totalCost;
        }

        public Costs costs;

        public async void CalculateCosts(ClientConnection client)
        {
            JObject items = await Requests.Jobs.GetJobItems(client, id);
            Costs _costs = new Costs();

            foreach (JToken item in items)
            {
                float qty = int.Parse(item["qty"].ToString());
                float price = int.Parse(item["PRICE"].ToString());

                _costs.supplyingPrice += price;

                if (item["FLAG"].ToString() != "0" && item["CUSTOM_FIELDS"] != null)
                {
                    if (item["CUSTOM_FIELDS"]["ethl_inventory_cost"] != null)
                    {
                        float cost = (float.Parse(item["CUSTOM_FIELDS"]["ethl_inventory_cost"]["value"].ToString()) * qty);
                        switch (item["kind"].ToString())
                        {
                            case "2": // Equipment cost
                                _costs.equipmentCost += cost;
                                break;

                            case "3": // Service cost
                                _costs.serviceCost += cost;
                                break;

                            case "4": // Labour cost
                                _costs.resourceCost += cost;
                                break;
                        }
                    }
                }
                else
                {
                    switch (item["kind"].ToString())
                    {
                        //case "2":
                        //    if (that.ethl_default_costs_and_margins.inventory[rowData.LIST_ID] !== undefined)
                        //    {
                        //        cost = (parseFloat(that.ethl_default_costs_and_margins.inventory[rowData.LIST_ID].cost) * quantity);
                        //        if (cost)
                        //        {
                        //            equipmentCost += cost;
                        //        }
                        //    }
                        //    break;

                        //case "3":
                        //    console.log("Unable to load default cost for service: " + rowData.title);
                        //    break;

                        //case "4":
                        //    if (that.ethl_default_costs_and_margins.labour[rowData.LIST_ID] !== undefined)
                        //    {
                        //        cost = (parseFloat(that.ethl_default_costs_and_margins.labour[rowData.LIST_ID].cost) * quantity);
                        //        if (cost)
                        //        {
                        //            resourceCost += cost;
                        //        }
                        //    }
                        //    break;
                    }
                }
            }
        }
    }
}
