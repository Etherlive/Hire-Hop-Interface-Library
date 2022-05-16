using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hire_Hop_Interface.Objects;
using Hire_Hop_Interface.Interface.Connections;
using System.Text.Json;
using Hire_Hop_Interface.Interface.Caching;

namespace Hire_Hop_Interface.Objects
{
    public class JobWithMisc
    {
        public Job job;
        public Bill bill;
        public Costs costs;
        public DateTime lastModified;
        public class Costs
        {
            #region Fields

            public float supplyingPrice, serviceCost, equipmentCost, resourceCost, totalCost;

            #endregion Fields
        }
        public class Bill
        {
            #region Fields

            public float accrued, totalCredit, totalDebit;

            #endregion Fields
        }

        public JobWithMisc(Job job)
        {
            this.job = job;
        }

        public async Task LoadMisc(CookieConnection cookie)
        {
            bill = await this.CalculateBill(cookie);
            lastModified = await GetLastModified(cookie);
        }

        public async Task<DateTime> GetLastModified(CookieConnection cookie)
        {
            var log = await GetJobHistory(cookie);

            return log != null && log.results.Length > 0 ? DateTime.Parse(log.results[0].GetProperty("cell").GetProperty("date_time").GetString()) : null;
        }

        public async Task<Bill> CalculateBill(CookieConnection cookie)
        {
            Bill b = new Bill();
            var bItems = await GetBill(cookie);

            foreach (var e in bItems.results)
            {
                if (e.TryGetProperty("kind", out var k))
                {
                    switch (k.GetInt32())
                    {
                        case 0:
                            b.accrued += k.GetProperty("accrued").GetInt32();
                            break;
                        case 1:
                            b.totalDebit += k.GetProperty("debit").GetInt32();
                            break;
                        case 2:
                            b.totalCredit += k.GetProperty("credit").GetInt32();
                            break;
                        case 3:
                            b.totalCredit += k.GetProperty("credit").GetInt32();
                            break;
                        default:
                            break;
                    }

                }
            }

            return b;
        }
        public async Task<SearchCollection<JsonElement>> GetJobHistory(CookieConnection cookie)
        {
            var req = new CacheableRequest("php_functions/log_list.php", "POST", cookie);
            req.AddOrSetForm("main_id", this.job.jobId);
            req.AddOrSetForm("type", "1");
            req.AddOrSetForm("_search", "false");
            req.AddOrSetForm("rows", "100");
            req.AddOrSetForm("page", "1");
            req.AddOrSetForm("sidx", "date_time");
            req.AddOrSetForm("sord", "desc");

            var d = await req.ExecuteWithCache();

            if (d.TryParseJson(out var json))
            {
                if (json.Value.TryGetProperty("rows", out var e))
                {
                    List<JsonElement> l = new List<JsonElement>();
                    var a = e.EnumerateArray();

                    while (a.MoveNext())
                    {
                        l.Add(a.Current);
                    }

                    return new SearchCollection<JsonElement>() { results = l.ToArray(), max_page = 1 };
                }
            }
            return null;
        }

        public async Task<SearchCollection<JsonElement>> GetBill(CookieConnection cookie)
        {
            string date_str = $"{DateTime.Now.ToString("yyyy-MM-dd+HH:mm:ss")}";

            var req = new CacheableRequest("php_functions/billing_list.php", "POST", cookie);
            req.AddOrSetForm("main_id", this.job.jobId);
            req.AddOrSetForm("type", "1");
            req.AddOrSetForm("local", date_str);
            req.AddOrSetForm("tz", "Europe/London");
            req.AddOrSetForm("fix", "0");
            req.AddOrSetForm("_search", "false");
            req.AddOrSetForm("rows", "10000");
            req.AddOrSetForm("page", "1");
            req.AddOrSetForm("sidx", "");
            req.AddOrSetForm("sord", "asc");

            var d = await req.ExecuteWithCache();

            if (d.TryParseJson(out var json))
            {
                if (json.Value.TryGetProperty("rows", out var e))
                {
                    List<JsonElement> l = new List<JsonElement>();
                    var a = e.EnumerateArray();

                    while (a.MoveNext())
                    {
                        l.Add(a.Current);
                    }

                    return new SearchCollection<JsonElement>() { results = l.ToArray(), max_page = 1 };
                }
            }
            return null;
        }
    }
}
