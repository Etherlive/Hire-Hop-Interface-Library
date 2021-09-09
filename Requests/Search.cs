using Hire_Hop_Interface.Management;
using Hire_Hop_Interface.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Requests
{
    public static class Search
    {
        #region Methods

        public static async Task<SearchResult[]> GetAllResults(ClientConnection client, SearchParams @params)
        {
            JObject jobs = await Search.LookFor(client, @params);
            int.TryParse(jobs["total"].ToString(), out int _max_page);

            Console.WriteLine($"Loading Pages 1 - {_max_page}");

            Task<JObject>[] job_page_tasks = new Task<JObject>[_max_page];
            for (int i = 1; i <= _max_page; i++)
            {
                @params._page = i;
                job_page_tasks[i - 1] = Search.LookFor(client, @params);
            }
            Task.WaitAll(job_page_tasks);

            JToken[] job_data = job_page_tasks.SelectMany(x => x.Result["rows"].Children()).ToArray();

            Console.WriteLine($"Pages Returned {job_data.Length} Jobs");

            List<SearchResult> list_results = new List<SearchResult>();
            foreach (JToken x in job_data)
            {
                SearchResult res = new SearchResult(x["cell"]);
                if (!list_results.Any(z => z.id == res.id))
                    list_results.Add(res);
                else
                    Console.WriteLine($"Skipped Duplicate Key {res.id}");
            }

            var results = list_results.ToArray();

            Console.WriteLine($"Loaded {results.Length} Jobs");

            return results;
        }

        public static async Task<JObject> LookFor(ClientConnection client, SearchParams @params)
        {
            string date_str = $"{ DateTime.Now.ToString("yyyy-MM-dd+HH:mm:ss")}";

            var query = new List<string>()
            {
                $"local={date_str}",
                "jobs="+(@params._jobs ? "1" : "0"),
                "projects="+(@params._projects ? "1" : "0"),
                "open="+(@params._open ? "1" : "0"),
                "closed="+(@params._closed ? "1" : "0"),
                "money_owed="+(@params._money_owed ? "1" : "0"),
                "is_late="+(@params._is_late ? "1" : "0"),
                "mine="+(@params._mine ? "1" : "0"),
                "no_user="+(@params._no_user ? "1" : "0"),
                "needs_bill="+(@params._needs_bill ? "1" : "0"),
                $"status={@params._status}",
                "last_search_idx=-1",
                $"_search="+(@params._search ? "true" : "false"),
                "nd=1630572957447",
                $"rows={@params._rows}",
                $"page={@params._page}",
                $"sidx=OUT_DATE",
                $"sord=asc"
            };

            if (@params._depot != -1) query.Add($"DEPOT={@params._depot}");

            client = await RequestInterface.SendRequest(client, "frames/search_field_results.php", queryList: query);
            return client.__lastContentAsJson;
        }

        #endregion Methods

        #region Classes

        public class SearchParams
        {
            #region Fields

            public int _depot = 1,
               _rows = 40, _page = 1;

            public bool _jobs = true, _projects = false,
                           _open = true, _closed = false, _search = true,
               _money_owed = false, _is_late = false, _mine = false, _no_user = false, _needs_bill = false;

            public string _status = "0,1,2,3,4,5,6,7,8";

            #endregion Fields
        }

        #endregion Classes
    }
}