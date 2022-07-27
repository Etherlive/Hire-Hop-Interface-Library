using Hire_Hop_Interface.Interface.Caching;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class Asset : JsonObject
    {
        #region Fields

        public int stockId = -1;

        #endregion Fields

        #region Properties

        public string barcode
        {
            get { return json.Value.GetProperty("BARCODE").GetString(); }
        }

        #endregion Properties

        #region Methods

        public static async Task<Asset> FindByBarcode(Interface.Connections.CookieConnection cookie, string barcode)
        {
            var req = new CacheableRequest("php_functions/barcode_find.php", "GET", cookie);
            req.AddOrSetQuery("barcode", barcode);

            var res = await req.ExecuteWithCache();

            if (res.TryParseJson(out var json))
            {
                if (json.Value.TryGetProperty("stock", out var stock_e) && stock_e.TryGetInt32(out int stock) && json.Value.TryGetProperty("asset", out var asset_e) && asset_e.TryGetInt32(out int asset))
                {
                    var ass = await Asset.GetAssets(cookie, stock, barcode);

                    if (ass.results.Length > 0)
                    {
                        return ass.results[0];
                    }
                }
            }

            return null;
        }

        public static async Task<SearchCollection<Asset>> GetAssets(Interface.Connections.CookieConnection cookie, int stock, string barcode = "")
        {
            var req = new CacheableRequest("modules/stock/equipment_list.php", "GET", cookie);
            if (barcode.Length > 0) req.AddOrSetQuery("BARCODE", barcode);
            req.AddOrSetQuery("stock", stock.ToString());
            req.AddOrSetQuery("del", "false");
            req.AddOrSetQuery("_search", "true");
            req.AddOrSetQuery("rows", "50");
            req.AddOrSetQuery("nd", "1646206198206");
            req.AddOrSetQuery("sidx", "BARCODE");
            req.AddOrSetQuery("sord", "asc");

            int page = 1, lastCount = 0, lastPage = 100;
            List<Asset> results = new List<Asset>();

            while ((lastCount != results.Count && lastPage >= page) || page == 1)
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
                            results.Add(new Asset() { json = rows.Current.GetProperty("cell") });
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