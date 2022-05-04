using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Interface.Caching;
using Hire_Hop_Interface.Interface.Connections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class Contact : JsonObject
    {
        #region Properties

        public string Company
        {
            get { return json.Value.TryGetProperty("COMPANY", out JsonElement e) ? e.GetString() : ""; }
        }

        public string Email
        {
            get { return json.Value.TryGetProperty("EMAIL", out JsonElement e) ? e.GetString() : ""; }
        }

        public string Id
        {
            get { return json.Value.TryGetProperty("id", out JsonElement e) ? e.GetInt32().ToString() : ""; }
        }

        public string Name
        {
            get { return json.Value.TryGetProperty("NAME", out JsonElement e) ? e.GetString() : ""; }
        }

        #endregion Properties

        #region Methods

        public static async Task<Contact> CreateNew(CookieConnection cookie, string company, string address, string telephone, string email)
        {
            var req = new Request("modules/contacts/save.php", "POST", cookie);
            req.AddOrSetForm("COMPANY", company);
            req.AddOrSetForm("FAX", "");
            req.AddOrSetForm("VAT_NUMBER", "");
            req.AddOrSetForm("SOURCE", "");
            req.AddOrSetForm("WEB", "");
            req.AddOrSetForm("NAME", "N/A");
            req.AddOrSetForm("IMAGE_ID", "");
            req.AddOrSetForm("JOB", "");
            req.AddOrSetForm("DD", "");
            req.AddOrSetForm("CELL", "");
            req.AddOrSetForm("MEMO", "");
            req.AddOrSetForm("ADDRESS", address);
            req.AddOrSetForm("TELEPHONE", telephone);
            req.AddOrSetForm("EMAIL", email);
            req.AddOrSetForm("MAIL", "0");
            req.AddOrSetForm("STATUS", "0");
            req.AddOrSetForm("CLIENT", "0");
            req.AddOrSetForm("VENUE", "0");
            req.AddOrSetForm("SUB", "1");
            req.AddOrSetForm("cID", "0");
            req.AddOrSetForm("ID", "0");
            req.AddOrSetForm("del", "false");
            req.AddOrSetForm("RATING", "0");
            //DEPOT_LIMITS: []

            var res = await req.Execute();
            if (res.TryParseJson(out JsonElement? json))
            {
                var j_enum = json.Value.GetProperty("rows").EnumerateArray();
                j_enum.MoveNext();
                return new Contact() { json = j_enum.Current.GetProperty("cell") };
            }
            return null;
        }

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
                if (json.Value.TryGetProperty("rows", out JsonElement e))
                {
                    var rows = e.EnumerateArray();
                    while (rows.MoveNext())
                    {
                        results.Add(new Contact() { json = rows.Current.GetProperty("cell") });
                    }

                    return new SearchCollection<Contact>() { results = results.ToArray(), max_page = json.Value.GetProperty("total").GetInt32() };
                }
                return null;
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