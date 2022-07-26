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

        public static async Task<Asset> FindByBarcode(Interface.Connections.CookieConnection cookie, string barcode)
        {
            var req = new CacheableRequest("php_functions/barcode_find.php", "GET", cookie);
            req.AddOrSetQuery("barcode", barcode);

            var res = await req.ExecuteWithCache();

            if (res.TryParseJson(out var json))
            {
                if (json.Value.TryGetProperty("stock", out var stock_e) && stock_e.TryGetInt32(out int stock) && json.Value.TryGetProperty("asset", out var asset_e) && asset_e.TryGetInt32(out int asset))
                {
                    return await Asset.GetAsset(cookie, barcode, stock.ToString());
                }
            }

            return null;
        }
        
        public static async Task<Asset> GetAsset(Interface.Connections.CookieConnection cookie, string barcode, string stock)
        {
            var req = new CacheableRequest("modules/stock/equipment_list.php", "GET", cookie);
            req.AddOrSetQuery("BARCODE", barcode);
            req.AddOrSetQuery("stock", stock);
            req.AddOrSetQuery("del", "false");
            req.AddOrSetQuery("_search", "true");
            req.AddOrSetQuery("rows", "50");
            req.AddOrSetQuery("nd", "1646206198206");
            req.AddOrSetQuery("sidx", "BARCODE");
            req.AddOrSetQuery("sord", "asc");

            var res = await req.ExecuteWithCache();

            if (res.TryParseJson(out var json))
            {
                if (json.Value.TryGetProperty("rows", out var rows_e))
                {
                    var row_enum = rows_e.EnumerateArray();
                    row_enum.MoveNext();
                    var asset = row_enum.Current.GetProperty("cell");

                    return new Asset() { json = asset };
                }
            }
            return null;
        }


        

        #endregion Methods
    }
}