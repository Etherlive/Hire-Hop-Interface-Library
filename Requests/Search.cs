using Hire_Hop_Interface.Management;
using Hire_Hop_Interface.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Requests
{
    public static class Search
    {
        #region Methods

        private static async Task<Dictionary<string,SearchResult>> LoadPage(ClientConnection client, SearchParams @params, int page)
        {
            @params._page = page;
            JObject jobs = await Search.LookFor(client, @params);

            Dictionary<string, SearchResult> results = new Dictionary<string, SearchResult>();

            foreach (JObject resultRow in jobs["rows"])
            {
                string rId = resultRow["id"].ToString();
                SearchResult result = new SearchResult(resultRow["cell"]);
                if (!results.ContainsKey(rId))
                {
                    results.Add(rId, result);
                }
            }

            return results;
        }

        public static async Task<Dictionary<string, SearchResult>> GetAllResults(ClientConnection client, SearchParams @params, bool LoadInDetail = false)
        {
            JObject jobs = await Search.LookFor(client, @params);
            int.TryParse(jobs["total"].ToString(), out int _max_page);

            Console.WriteLine($"Loading Pages 1 - {_max_page}");

            Dictionary<string, SearchResult> results = new Dictionary<string, SearchResult>();

            Task<Dictionary<string, SearchResult>>[] pageTasks = new Task<Dictionary<string, SearchResult>>[_max_page];
            for (int i = 1; i <= _max_page; i++) pageTasks[i-1] = LoadPage(client, @params, i);

            Task.WaitAll(pageTasks);

            var page_results = pageTasks.SelectMany(x => x.Result).ToArray();

            Console.WriteLine($"Pages Returned {page_results.Length} Jobs");

            foreach (KeyValuePair<string,SearchResult> pair in page_results)
            {
                if (!results.ContainsKey(pair.Key))
                    results.Add(pair.Key, pair.Value);
            }

            Console.WriteLine($"Loaded {results.Count} Jobs");

            if (LoadInDetail)
            {
                Console.WriteLine("Loading Extra Detail");

                var tasks = results.Select(x => x.Value.LoadDetail(client)).ToArray();
                Task.WaitAll(tasks);

                int idx = 0;
                foreach (KeyValuePair<string, SearchResult> result in results)
                {
                    result.Value.data = tasks[idx].Result;
                    idx++;
                }

                Console.WriteLine("Loaded Detail");
            }

            return results;
        }

        public static async Task<JObject> LookFor(ClientConnection client, SearchParams @params)
        {
            string date_str = $"{ DateTime.Now.ToString("yyyy-MM-dd+HH:mm:ss")}";
            client = await RequestInterface.SendRequest(client, "frames/search_field_results.php", queryList: new List<string>()
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
                $"DEPOT={@params._depot}",
                $"_search=true",
                "nd=1630572957447",
                $"rows={@params._rows}",
                $"page={@params._page}",
                $"sidx=OUT_DATE",
                $"sord=asc"
            });
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
                           _open = true, _closed = false,
               _money_owed = true, _is_late = false, _mine = false, _no_user = false, _needs_bill = false;

            public string _status = "0,1,2,3,4,5,6,7,8";

            #endregion Fields
        }

        #endregion Classes
    }
}