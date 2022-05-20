using Hire_Hop_Interface.Interface.Caching;
using Hire_Hop_Interface.Interface.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class JobItem : JsonObject
    {
        public string jobId { get; set; }
        public string id
        {
            get
            {
                return json.HasValue ? json.Value.GetProperty("ID").GetString() : null;
            }
        }
        public int flag
        {
            get
            {
                return json.HasValue ? int.Parse(json.Value.GetProperty("FLAG").GetString()) : -1;
            }
        }
        public string kind
        {
            get
            {
                return json.HasValue ? json.Value.GetProperty("kind").GetString() : null;
            }
        }
        public double qty
        {
            get
            {
                return json.HasValue ? double.Parse(json.Value.GetProperty("qty").GetString()) : -1;
            }
        }
        public double PRICE
        {
            get
            {
                return json.HasValue ? double.Parse(json.Value.GetProperty("PRICE").GetString()) : -1;
            }
        }
        public string title
        {
            get
            {
                return json.HasValue ? json.Value.GetProperty("title").GetString() : "";
            }
        }

        public async Task<bool> ProcessCost(CookieConnection cookie, DefaultCost[] labourCosts)
        {
            if (kind == "4")
            {
                if (DefaultCost.labour_cost_margins.ContainsKey(id)) return false;
                foreach (DefaultCost d in labourCosts)
                {
                    if (d.id == id)
                    {
                        var custom_fields = d.json.Value.GetProperty("cell").GetProperty("data").GetProperty("CUSTOM_FIELDS");
                        DefaultCost.CostMargin cm = new DefaultCost.CostMargin()
                        {
                            cost = custom_fields.TryGetProperty("ethl_inventory_default_cost", out var dc) ? dc.GetDouble() : 0,
                            margin = custom_fields.TryGetProperty("ethl_inventory_default_margin", out var dm) ? dm.GetDouble() : 0,
                        };
                        return DefaultCost.labour_cost_margins.TryAdd(id, cm);
                    }
                }
            }
            else
            {
                var req = new CacheableRequest("modules/stock/list.php", "POST", cookie);

                req.AddOrSetForm("unq", id);
                req.AddOrSetForm("rows", "1");

                var res = await req.ExecuteWithCache();

                if (res.TryParseJson(out JsonElement? json))
                {
                    List<DefaultCost> results = new List<DefaultCost>();
                    if (json.Value.TryGetProperty("items", out JsonElement e))
                    {
                        var custom_fields = e[0].GetProperty("CUSTOM_FIELDS");
                        DefaultCost.CostMargin cm = new DefaultCost.CostMargin()
                        {
                            cost = custom_fields.TryGetProperty("ethl_inventory_default_cost", out var dc) ? dc.GetDouble() : 0,
                            margin = custom_fields.TryGetProperty("ethl_inventory_default_margin", out var dm) ? dm.GetDouble() : 0,
                        };
                        return DefaultCost.inventory_cost_margins.TryAdd(id, cm);
                    }
                }
            }
            return false;
        }

        public static async Task<SearchCollection<JobItem>> GetJobItems(CookieConnection cookie, Job job)
        {
            var req = new CacheableRequest("frames/items_to_supply_list.php", "POST", cookie);
            req.AddOrSetForm("job", job.jobId);

            var d = await req.ExecuteWithCache();

            if (d.TryParseJson(out var json))
            {
                if (json.Value.TryGetProperty("items", out var e))
                {
                    List<JobItem> l = new List<JobItem>();
                    var a = e.EnumerateArray();

                    while (a.MoveNext())
                    {
                        l.Add(new JobItem() { json = a.Current });
                    }

                    return new SearchCollection<JobItem>() { results = l.ToArray(), max_page = 1 };
                }
            }
            return null;
        }
    }
}
