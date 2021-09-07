using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class Jobs
    {
        #region Fields

        public Costs costs;
        public JObject Data;

        #endregion Fields

        #region Constructors

        public Jobs(SearchResult searchResult)
        {
            Data = searchResult.Data;
        }

        #endregion Constructors

        #region Properties

        public string id
        {
            get { return Data["ID"].ToString().Replace("j", ""); }
        }

        #endregion Properties

        #region Methods

        public async Task<Costs> CalculateCosts(ClientConnection client)
        {
            JArray items = await Requests.Jobs.GetJobItems(client, id);
            Task.WaitAny(Default_Cost_Margins.Load(client, items));
            Costs _costs = new Costs();

            foreach (JToken item in items)
            {
                float qty = float.Parse(item["qty"].ToString());
                float price = float.Parse(item["PRICE"].ToString());

                _costs.supplyingPrice += price;

                if (item["FLAG"].ToString() != "0" && item["CUSTOM_FIELDS"] != null && item["CUSTOM_FIELDS"].HasValues)
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
                    float cost;
                    switch (item["kind"].ToString())
                    {
                        case "2":
                            if (Default_Cost_Margins.inventory_cost_margins.ContainsKey(item["ID"].ToString()))
                            {
                                cost = (Default_Cost_Margins.inventory_cost_margins[item["ID"].ToString()].cost * qty);
                                _costs.equipmentCost += cost;
                            }
                            break;

                        case "3":
                            Console.WriteLine("Unable to load default cost for service: " + item["title"]);
                            break;

                        case "4":
                            if (Default_Cost_Margins.labour_cost_margins.ContainsKey(item["ID"].ToString()))
                            {
                                cost = (Default_Cost_Margins.labour_cost_margins[item["ID"].ToString()].cost * qty);
                                _costs.resourceCost += cost;
                            }
                            break;
                    }
                }
            }
            _costs.totalCost = _costs.equipmentCost + _costs.resourceCost + _costs.serviceCost;
            return _costs;
        }

        #endregion Methods

        #region Classes

        public class Costs
        {
            #region Fields

            public float supplyingPrice, serviceCost, equipmentCost, resourceCost, totalCost;

            #endregion Fields
        }

        #endregion Classes
    }
}