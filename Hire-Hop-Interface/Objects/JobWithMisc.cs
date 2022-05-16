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

            public double supplyingPrice, serviceCost, equipmentCost, resourceCost, totalCost;

            #endregion Fields
        }
        public class Bill
        {
            #region Fields

            public double accrued, totalCredit, totalDebit;

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

            string d = log != null && log.results.Length > 0 ? log.results[0].GetProperty("cell").GetProperty("date_time").GetString() : null;

            return d != null ? DateTime.Parse(d) : DateTime.MinValue;
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
                            b.accrued += e.GetProperty("accrued").GetDouble();
                            break;
                        case 1:
                            b.totalDebit += e.GetProperty("debit").GetDouble();
                            break;
                        case 2:
                            b.totalCredit += e.GetProperty("credit").GetDouble();
                            break;
                        case 3:
                            b.totalCredit += e.GetProperty("credit").GetDouble();
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
            req.AddOrSetQuery("main_id", this.job.jobId);
            req.AddOrSetQuery("type", "1");
            req.AddOrSetQuery("_search", "false");
            req.AddOrSetQuery("nd", "1631267281622");
            req.AddOrSetQuery("rows", "100");
            req.AddOrSetQuery("page", "1");
            req.AddOrSetQuery("sidx", "date_time");
            req.AddOrSetQuery("sord", "desc");

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
            req.AddOrSetQuery("main_id", this.job.jobId);
            req.AddOrSetQuery("type", "1");
            req.AddOrSetQuery("local", date_str);
            req.AddOrSetQuery("tz", "Europe/London");
            req.AddOrSetQuery("fix", "0");
            req.AddOrSetQuery("_search", "false");
            req.AddOrSetQuery("rows", "10000");
            req.AddOrSetQuery("page", "1");
            req.AddOrSetQuery("sidx", "");
            req.AddOrSetQuery("sord", "asc");

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
