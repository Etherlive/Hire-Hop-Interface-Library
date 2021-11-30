using Hire_Hop_Interface.HireHop;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class Job : JsonObject
    {
        #region Fields

        private CustomField[] _customFields;

        private string jobId;

        #endregion Fields

        #region Constructors

        public Job(string _jobId)
        {
            if (_jobId.Contains("p")) throw new Exception("Id Is Of Project");

            this.jobId = _jobId.Replace("j", "");
        }

        #endregion Constructors

        #region Properties

        public CustomField[] customFields
        {
            get { return _customFields == null ? ExtractCustomFields() : _customFields; }
        }

        public string id
        {
            get { return this.json.Value.GetProperty("ID").GetString(); }
        }

        #endregion Properties

        #region Methods

        public override async Task<bool> LoadData(ConnectionCookie cookie)
        {
            var req = new Request("php_functions/job_refresh.php", "POST", cookie);
            req.AddOrSetForm("job", this.jobId);

            var res = await req.Execute();

            JsonElement? json;
            if (res.TryParseJson(out json))
            {
                this.json = json;
                return true;
            }
            return false;
        }

        public async Task<bool> SaveCustomFields(ConnectionCookie cookie)
        {
            var req = new Request("php_functions/job_save.php", "POST", cookie);

            req.AddOrSetForm("main_id", this.jobId);

            foreach (CustomField field in this._customFields)
            {
                string pre = $"fields[ethl_custom_fields][value][{field.id}]";
                req.AddOrSetForm($"{pre}[id]", field.id);
                req.AddOrSetForm($"{pre}[key]", field.key);
                req.AddOrSetForm($"{pre}[value]", field.value);
            }

            var res = await req.Execute();

            if (res != null)
            {
                this._customFields = null;

                JsonElement? json;
                if (res.TryParseJson(out json))
                {
                    this.json = json;
                    return true;
                }
            }
            return false;
        }

        private CustomField[] ExtractCustomFields()
        {
            string raw = this.json.Value.GetProperty("fields").GetProperty("ethl_custom_fields").GetProperty("value").GetRawText();
            _customFields = JsonSerializer.Deserialize<CustomField[]>(raw);
            return _customFields;
        }

        #endregion Methods

        #region Classes

        public class CustomField
        {
            #region Properties

            public string id { get; set; }
            public string key { get; set; }
            public string value { get; set; }

            #endregion Properties
        }

        #endregion Classes
    }
}