using Hire_Hop_Interface.Interface.Caching;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class Asset : JsonObject
    {
        #region Properties

        public string barcode
        {
            get { return json.Value.GetProperty("BARCODE").GetString(); }
        }

        #endregion Properties

        #region Methods

        public static async Task<SearchCollection<Asset>> SearchForAll(Interface.Connections.CookieConnection cookie)
        {
            var req = new CacheableRequest("modules/stock/list.php", "GET", cookie);

            req.AddOrSetQuery("head", "0");
            req.AddOrSetQuery("del", "0");
            req.AddOrSetQuery("_search", "false");
            req.AddOrSetQuery("rows", "50");
            req.AddOrSetQuery("nd", "1646206198206");
            req.AddOrSetQuery("sidx", "TITLE");
            req.AddOrSetQuery("sord", "asc");
            req.AddOrSetQuery("local", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            int page = 1, lastCount = 0, lastPage = 100;
            List<Asset> results = new List<Asset>();

            while ((lastCount != results.Count && lastPage>=page) || page == 1)
            {
                lastCount = results.Count;
                req.AddOrSetQuery("page", page.ToString());
                var res = await req.ExecuteWithCache();

                if (res.TryParseJson(out JsonElement? json))
                {
                    lastPage = json.Value.GetProperty("total").GetInt32();
                    if (json.Value.TryGetProperty("rows", out var r))
                    {
                        var rows = r.EnumerateArray();
                        while (rows.MoveNext())
                        {
                            results.Add(new Asset() { json = rows.Current });
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                page++;
            }
            return new SearchCollection<Asset>() { results = results.ToArray(), max_page = page };
        }

        #endregion Methods
    }
}