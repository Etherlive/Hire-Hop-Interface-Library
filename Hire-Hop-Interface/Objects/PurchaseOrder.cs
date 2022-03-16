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

        public string CREATE_USER
        {
            get { return json.Value.GetProperty("data").TryGetProperty("CREATE_USER", out JsonElement e) ? e.GetString() : ""; }
        }

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

        public PurchaseOrderLine[] items
        {
            get { return json.Value.GetProperty("data").TryGetProperty("ITEMS", out JsonElement e) ? e.EnumerateArray().Select(x => new PurchaseOrderLine() { json = x }).ToArray() : null; }
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
            get { return json.Value.GetProperty("data").TryGetProperty("SUPPLIER_REF", out JsonElement e) ? e.GetString() : ""; }
        }

        public int TOTAL
        {
            get { return json.Value.TryGetProperty("TOTAL", out JsonElement e) ? e.GetInt32() : -1; }
        }

        #endregion Properties

        #region Methods

        public static async Task<PurchaseOrder> CreateNew(CookieConnection cookie, string jobId, string description, string reference, string supplierId, DateTime start, DateTime finish, string memo = "", string deliveryAddress = "")
        {
            var req = new Request("php_functions/subcontractors_save.php", "POST", cookie);
            req.AddOrSetForm("ID", "0");
            req.AddOrSetForm("main_id", jobId);
            req.AddOrSetForm("type", "1");
            req.AddOrSetForm("pers", supplierId);
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
                if (json.Value.TryGetProperty("rows", out JsonElement e))
                {
                    var j_enum = e.EnumerateArray();
                    j_enum.MoveNext();
                    return new PurchaseOrder() { json = j_enum.Current };
                }
                else
                {
                    if (json.Value.GetProperty("error").ToString().Contains("too many transactions"))
                    {
                        System.Threading.Thread.Sleep(60000);
                        return await CreateNew(cookie, jobId, description, reference, supplierId, start, finish, memo, deliveryAddress);
                    }
                    Console.WriteLine($"PO Create Failed On Job {jobId}: {json.Value.ToString()}");
                }
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
            req.AddOrSetQuery("rows", "200");
            req.AddOrSetQuery("nd", "1646206198206");
            req.AddOrSetQuery("sidx", "ID");
            req.AddOrSetQuery("sord", "desc");

            int page = 1;
            List<PurchaseOrder> results = new List<PurchaseOrder>();

            while (results.Count % 200 == 0)
            {
                req.AddOrSetQuery("page", page.ToString());
                var res = await req.ExecuteWithCache();

                if (res.TryParseJson(out JsonElement? json))
                {
                    var rows = json.Value.GetProperty("rows").EnumerateArray();
                    while (rows.MoveNext())
                    {
                        results.Add(new PurchaseOrder() { json = rows.Current });
                    }
                }
                page++;
            }
            return new SearchCollection<PurchaseOrder>() { results = results.ToArray(), max_page = page };
        }

        public async Task AddLineItem(CookieConnection cookie, double qty, double unit, double total, double vat_rate, int vat_id, string desc)
        {
            var req = new Request("php_functions/subcontractors_save_item.php", "POST", cookie);
            req.AddOrSetForm("id", "0");
            req.AddOrSetForm("kind", "3");
            req.AddOrSetForm("sub", ID.Replace("sub", ""));
            req.AddOrSetForm("qty", qty.ToString());
            req.AddOrSetForm("unit", unit.ToString());
            req.AddOrSetForm("total", total.ToString());
            req.AddOrSetForm("vat_rate", vat_rate.ToString());
            req.AddOrSetForm("vat_id", vat_id.ToString());
            req.AddOrSetForm("nominal", "38");
            req.AddOrSetForm("selected", "");
            req.AddOrSetForm("tz", " Europe/London");
            req.AddOrSetForm("desc", desc);
            req.AddOrSetForm("memo", "Imported From IC");

            var res = await req.Execute();
        }

        public async Task Delete(CookieConnection cookie)
        {
            var req = new Request("php_functions/subcontractors_delete.php", "POST", cookie);
            req.AddOrSetForm("id", ID.Replace("sub", ""));
            req.AddOrSetForm("main_id", "0");
            req.AddOrSetForm("type", "11");

            var res = await req.Execute();
        }

        public async Task UpdateStatus(CookieConnection cookie, int status)
        {
            var req = new Request("php_functions/subcontractors_save_status.php", "POST", cookie);
            req.AddOrSetForm("id", ID.Replace("sub", ""));
            req.AddOrSetForm("status", status.ToString());
            req.AddOrSetForm("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            var res = await req.Execute();
        }

        #endregion Methods
    }
}