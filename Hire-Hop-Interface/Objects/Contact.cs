using Hire_Hop_Interface.Interface.Caching;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;

namespace Hire_Hop_Interface.Objects
{
    public class Contact : JsonObject
    {
        #region Properties

        public string Company
        {
            get { return json.Value.GetProperty("COMPANY").GetString(); }
        }

        public string Enail
        {
            get { return json.Value.GetProperty("EMAIL").GetString(); }
        }

        public string Name
        {
            get { return json.Value.GetProperty("NAME").GetString(); }
        }

        #endregion Properties

        #region Methods

        public static async Task<SearchCollection<Contact>> Search(Interface.Connections.CookieConnection cookie, int page = 1)
        {
            var req = new CacheableRequest("modules/contacts/list.php", "GET", cookie);

            req.AddOrSetQuery("del", "false");
            req.AddOrSetQuery("_search", "false");
            req.AddOrSetQuery("rows", "80");
            req.AddOrSetQuery("page", page.ToString());
            req.AddOrSetQuery("nd", "1646206198206");
            req.AddOrSetQuery("sidx", "COMPANY");
            req.AddOrSetQuery("sord", "asc");

            var res = await req.ExecuteWithCache();

            if (res.TryParseJson(out JsonElement? json))
            {
                List<Contact> results = new List<Contact>();
                var rows = json.Value.GetProperty("rows").EnumerateArray();
                while (rows.MoveNext())
                {
                    results.Add(new Contact() { json = rows.Current.GetProperty("cell") });
                }

                return new SearchCollection<Contact>() { results = results.ToArray(), max_page = json.Value.GetProperty("total").GetInt32() };
            }
            return null;
        }

        public static async Task<SearchCollection<Contact>> SearchForAll(Interface.Connections.CookieConnection cookie)
        {
            var search_1 = await Search(cookie);

            var search_actions = new Task<SearchCollection<Contact>>[search_1.max_page - 2];

            for (int i = 0; i < search_actions.Length; i++)
            {
                search_actions[i] = Search(cookie, i + 2);
            }

            Task.WaitAll(search_actions);

            var all_results = search_1.results.Concat(search_actions.SelectMany(x => x.Result.results));

            return new SearchCollection<Contact>() { results = all_results.ToArray(), max_page = search_1.max_page };
        }

        #endregion Methods
    }
}