using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Interface.Caching;
using Hire_Hop_Interface.Interface.Connections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class PurchaseOrder : JsonObject
    {
        #region Properties

        public string CURRENCY
        {
            get { return json.Value.TryGetProperty("CURRENCY", out JsonElement e) ? e.GetString() : ""; }
        }

        public string desc
        {
            get { return json.Value.TryGetProperty("desc", out JsonElement e) ? e.GetString() : ""; }
        }

        public string ID
        {
            get { return json.Value.TryGetProperty("ID", out JsonElement e) ? e.GetString() : ""; }
        }

        public int JobId
        {
            get { return json.Value.GetProperty("data").TryGetProperty("MAIN_ID", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public int KIND
        {
            get { return json.Value.TryGetProperty("KIND", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public int level
        {
            get { return json.Value.TryGetProperty("level", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public int link
        {
            get { return json.Value.TryGetProperty("link", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public int NET
        {
            get { return json.Value.TryGetProperty("NET", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public string NUMBER
        {
            get { return json.Value.TryGetProperty("NUMBER", out JsonElement e) ? e.GetString() : ""; }
        }

        public int OWED
        {
            get { return json.Value.TryGetProperty("OWED", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public int STATUS
        {
            get { return json.Value.TryGetProperty("STATUS", out JsonElement e) ? e.GetInt32() : -1; }
        }

        public string sub
        {
            get { return json.Value.TryGetProperty("sub", out JsonElement e) ? e.GetString() : ""; }
        }

        public int TOTAL
        {
            get { return json.Value.TryGetProperty("TOTAL", out JsonElement e) ? e.GetInt32() : -1; }
        }

        #endregion Properties

        #region Methods

        public static async Task<SearchCollection<PurchaseOrder>> Search(Interface.Connections.CookieConnection cookie, int page = 1)
        {
            var req = new CacheableRequest("php_functions/subcontractors_list.php", "GET", cookie);

            req.AddOrSetQuery("main_id", "0");
            req.AddOrSetQuery("type", "11");
            req.AddOrSetQuery("fix", "0");
            req.AddOrSetQuery("_search", "false");
            req.AddOrSetQuery("rows", "10000");
            req.AddOrSetQuery("page", page.ToString());
            req.AddOrSetQuery("nd", "1646206198206");
            req.AddOrSetQuery("sidx", "ID");
            req.AddOrSetQuery("sord", "desc");

            var res = await req.ExecuteWithCache();

            if (res.TryParseJson(out JsonElement? json))
            {
                List<PurchaseOrder> results = new List<PurchaseOrder>();
                var rows = json.Value.GetProperty("rows").EnumerateArray();
                while (rows.MoveNext())
                {
                    results.Add(new PurchaseOrder() { json = rows.Current.GetProperty("cell") });
                }

                return new SearchCollection<PurchaseOrder>() { results = results.ToArray(), max_page = 1 };
            }
            return null;
        }

        public static async Task<SearchCollection<PurchaseOrder>> SearchForAll(Interface.Connections.CookieConnection cookie)
        {
            var search_1 = await Search(cookie);

            var search_actions = new Task<SearchCollection<PurchaseOrder>>[search_1.max_page - 2];

            for (int i = 0; i < search_actions.Length; i++)
            {
                search_actions[i] = Search(cookie, i + 2);
            }

            Task.WaitAll(search_actions);

            var all_results = search_1.results.Concat(search_actions.SelectMany(x => x.Result.results));

            return new SearchCollection<PurchaseOrder>() { results = all_results.ToArray(), max_page = search_1.max_page };
        }

        #endregion Methods
    }
}