using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Interface.Caching;
using Hire_Hop_Interface.Interface.Connections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System;

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

        public string SUPPLIER_REF
        {
            get { return json.Value.TryGetProperty("SUPPLIER_REF", out JsonElement e) ? e.GetString() : ""; }
        }

        public int TOTAL
        {
            get { return json.Value.TryGetProperty("TOTAL", out JsonElement e) ? e.GetInt32() : -1; }
        }

        #endregion Properties

        #region Methods

        public static async Task<PurchaseOrder> CreateNew(CookieConnection cookie, string jobId, string description, string reference, DateTime start, DateTime finish, string memo = "", string deliveryAddress = "")
        {
            var req = new Request("php_functions/subcontractors_save.php", "POST", cookie);
            req.AddOrSetForm("ID", "0");
            req.AddOrSetForm("main_id", jobId);
            req.AddOrSetForm("type", "1");
            req.AddOrSetForm("pers", "2926");
            req.AddOrSetForm("internal", "0");
            req.AddOrSetForm("depot", "4");
            req.AddOrSetForm("desc", description);
            req.AddOrSetForm("kind", "0");
            req.AddOrSetForm("ref", reference);
            req.AddOrSetForm("tax_total", "0.00");
            req.AddOrSetForm("tax_rate", "0");
            req.AddOrSetForm("memo", memo);
            req.AddOrSetForm("addr", deliveryAddress);
            req.AddOrSetForm("checkin", "1");
            req.AddOrSetForm("checkout", "1");
            req.AddOrSetForm("sceduled", "1");
            req.AddOrSetForm("collect", "0");
            req.AddOrSetForm("return", "0");
            req.AddOrSetForm("fulfil", "1");
            req.AddOrSetForm("novat", "0");
            req.AddOrSetForm("res", "false");
            req.AddOrSetForm("start", start.ToString("yyyy-MM-dd HH:mm"));
            req.AddOrSetForm("finish", finish.ToString("yyyy-MM-dd HH:mm"));
            req.AddOrSetForm("currency[CODE]", "GBP");
            req.AddOrSetForm("currency[NAME]", "United Kingdom Pound");
            req.AddOrSetForm("currency[SYMBOL]", "£");
            req.AddOrSetForm("currency[DECIMALS]", "2");
            req.AddOrSetForm("currency[MULTIPLIER]", "1");
            req.AddOrSetForm("currency[NEGATIVE_FORMAT]", "1");
            req.AddOrSetForm("currency[SYMBOL_POSITION]", "0");
            req.AddOrSetForm("currency[DECIMAL_SEPARATOR]", ".");
            req.AddOrSetForm("currency[THOUSAND_SEPARATOR]", ",");

            var res = await req.Execute();
            if (res.TryParseJson(out JsonElement? json))
            {
                var j_enum = json.Value.GetProperty("rows").EnumerateArray();
                j_enum.MoveNext();
                return new PurchaseOrder() { json = j_enum.Current };
            }
            return null;
        }

        public static async Task<SearchCollection<PurchaseOrder>> SearchForAll(Interface.Connections.CookieConnection cookie)
        {
            var req = new CacheableRequest("php_functions/subcontractors_list.php", "GET", cookie);

            req.AddOrSetQuery("main_id", "0");
            req.AddOrSetQuery("type", "11");
            req.AddOrSetQuery("fix", "0");
            req.AddOrSetQuery("_search", "false");
            req.AddOrSetQuery("rows", "10000");
            req.AddOrSetQuery("page", "1");
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
                    results.Add(new PurchaseOrder() { json = rows.Current });
                }

                return new SearchCollection<PurchaseOrder>() { results = results.ToArray(), max_page = 1 };
            }
            return null;
        }

        #endregion Methods
    }
}