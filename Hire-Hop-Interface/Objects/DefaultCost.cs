using Hire_Hop_Interface.Interface.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class DefaultCost : JsonObject
    {
        public static async Task<SearchCollection<DefaultCost>> Search(Interface.Connections.CookieConnection cookie, int page = 1)
        {
            var req = new CacheableRequest("modules/services/list.php", "GET", cookie);

            req.AddOrSetQuery("del", "false");
            req.AddOrSetQuery("rows", "500");
            req.AddOrSetQuery("page", page.ToString());

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
