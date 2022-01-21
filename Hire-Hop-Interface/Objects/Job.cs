using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Interface.Caching;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class Job : JsonObject
    {
        #region Fields

        private List<CustomField> _customFields;

        private string jobId;

        #endregion Fields

        #region Methods

        private List<CustomField> ExtractCustomFields()
        {
            List<CustomField> fields = new List<CustomField>();
            try
            {
                var values = this.json.Value.GetProperty("fields").GetProperty("ethl_custom_fields").GetProperty("value").EnumerateArray();
                while (values.MoveNext())
                {
                    fields.Add(JsonSerializer.Deserialize<CustomField>(values.Current.GetRawText()));
                }
            }
            catch (Exception e)
            {
            }
            this._customFields = fields;
            return this._customFields;
        }

        #endregion Methods

        #region Constructors

        public Job(string _jobId)
        {
            if (_jobId.Contains("p")) throw new Exception("Id Is Of Project");

            this.jobId = _jobId.Replace("j", "");
        }

        #endregion Constructors

        #region Properties

        public List<CustomField> customFields
        {
            get { return _customFields == null ? ExtractCustomFields() : _customFields; }
        }

        public string id
        {
            get { return this.json.Value.GetProperty("ID").GetString(); }
        }

        #endregion Properties

        public override async Task<bool> LoadData(Interface.Connections.CookieConnection cookie)
        {
            var req = new CacheableRequest("php_functions/job_refresh.php", "POST", cookie);
            req.AddOrSetForm("job", this.jobId);

            return await LoadData(req);
        }

        public async Task<bool> SaveCustomFields(Interface.Connections.CookieConnection cookie)
        {
            var req = new Request("php_functions/job_save.php", "POST", cookie);

            req.AddOrSetForm("id_main", this.id);

            req.AddOrSetForm("fields[ethl_custom_fields][type]", "array");

            foreach (CustomField field in this.customFields)
            {
                string pre = $"fields[ethl_custom_fields][value][{field.id}]";
                req.AddOrSetForm($"{pre}[id]", field.id);
                req.AddOrSetForm($"{pre}[key]", field.key);
                req.AddOrSetForm($"{pre}[value]", field.value);
            }

            var res = await req.Execute();

            this._customFields = null;

            JsonElement? json;
            if (res.TryParseJson(out json))
            {
                this.json = json;
                return true;
            }
            return false;
        }

        public class CustomField
        {
            #region Constructors

            public CustomField()
            { }

            public CustomField(string id, string key, string value)
            {
                this.id = id;
                this.key = key;
                this.value = value;
            }

            #endregion Constructors

            #region Properties

            public string id { get; set; }
            public string key { get; set; }
            public string value { get; set; }

            #endregion Properties
        }
    }
}