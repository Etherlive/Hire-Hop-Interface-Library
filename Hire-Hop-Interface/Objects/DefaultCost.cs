﻿using Hire_Hop_Interface.Interface.Caching;
using Hire_Hop_Interface.Interface.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Hire_Hop_Interface.Objects
{
    public class DefaultCost : JsonObject
    {
        public class CostMargin
        {
            #region Fields

            public double cost, margin;

            #endregion Fields
        }

        public static ConcurrentDictionary<string, CostMargin> inventory_cost_margins = new ConcurrentDictionary<string, CostMargin>(), 
            labour_cost_margins = new ConcurrentDictionary<string, CostMargin>();

        public int kind
        {
            get
            {
                return json.HasValue ? json.Value.GetProperty("kind").GetInt32() : -1;
            }
        }
        public string id
        {
            get
            {
                return json.HasValue ? json.Value.GetProperty("id").GetString() : null;
            }
        }

        public static Task LoadCosts(CookieConnection cookie, JobItem[] jobItems, DefaultCost[] labourCosts)
        {
            return Task.WhenAll(jobItems.Select(x => x.ProcessCost(cookie, labourCosts)).ToArray());
        }

        public static async Task<SearchCollection<DefaultCost>> GetAllLabourCosts(Interface.Connections.CookieConnection cookie)
        {
            var req = new CacheableRequest("modules/services/list.php", "GET", cookie);

            req.AddOrSetQuery("del", "false");
            req.AddOrSetQuery("rows", "10000");
            req.AddOrSetQuery("page", "1");

            var res = await req.ExecuteWithCache();

            if (res.TryParseJson(out JsonElement? json))
            {
                List<DefaultCost> results = new List<DefaultCost>();
                if (json.Value.TryGetProperty("rows", out JsonElement e))
                {
                    var rows = e.EnumerateArray();
                    while (rows.MoveNext())
                    {
                        results.Add(new DefaultCost() { json = rows.Current.GetProperty("cell") });
                    }

                    return new SearchCollection<DefaultCost>() { results = results.ToArray(), max_page = json.Value.GetProperty("total").GetInt32() };
                }
                return null;
            }
            return null;
        }
    }
}
