using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Interface.Connections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class Note : JsonObject
    {
        #region Constructors

        public Note()
        { }

        public Note(CookieConnection cookie, string jobId, string body)
        {
            var save = Save(cookie, jobId, body);
            save.Wait();
        }

        #endregion Constructors

        #region Properties

        public string created_by
        {
            get { return json.Value.GetProperty("username").GetString(); }
        }

        public string id
        {
            get { return json.Value.GetProperty("id").GetString(); }
        }

        public string job_id
        {
            get; private set;
        }

        public string note
        {
            get { return json.Value.GetProperty("note").GetString(); }
        }

        #endregion Properties

        #region Methods

        public async Task Delete(CookieConnection cookie)
        {
            var req = new Request("php_functions/notes_delete.php", "POST", cookie);
            req.AddOrSetForm("id", this.id);
            req.AddOrSetForm("main_id", this.job_id);
            req.AddOrSetForm("type", "1");

            var res = await req.Execute();
            this.json = null;
        }

        public async Task Save(CookieConnection cookie)
        {
            await Save(cookie, this.job_id, this.note, this.id);
        }

        public async Task Save(CookieConnection cookie, string jobId, string note, string id = "0")
        {
            var req = new Request("php_functions/notes_save.php", "POST", cookie);
            req.AddOrSetForm("main_id", jobId);
            req.AddOrSetForm("note", note);
            req.AddOrSetForm("type", "1");
            req.AddOrSetForm("id", id);

            var res = await req.Execute();
            if (res.TryParseJson(out JsonElement? json))
            {
                var j_enum = json.Value.GetProperty("rows").EnumerateArray();
                j_enum.MoveNext();
                this.json = j_enum.Current.GetProperty("cell");
                this.job_id = jobId;
            }
        }

        #endregion Methods
    }
}