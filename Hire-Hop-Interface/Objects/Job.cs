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

        public string company
        {
            get
            {
                return json.HasValue ? json.Value.GetProperty("COMPANY").GetString() : null;
            }
        }

        public string customer_email
        {
            get
            {
                return json.HasValue ? json.Value.GetProperty("EMAIL").GetString() : null;
            }
        }

        public string customer_landline
        {
            get
            {
                return json.HasValue ? json.Value.GetProperty("TELEPHONE").GetString() : null;
            }
        }

        public string customer_mobile
        {
            get
            {
                return json.HasValue ? json.Value.GetProperty("MOBILE").GetString() : null;
            }
        }

        public string customer_name
        {
            get
            {
                return json.HasValue ? json.Value.GetProperty("NAME").GetString() : null;
            }
        }

        public string customer_phone
        {
            get
            {
                string mobile = customer_mobile;
                return mobile != null ? mobile : customer_landline;
            }
        }

        public List<CustomField> customFields
        {
            get { return _customFields == null ? ExtractCustomFields() : _customFields; }
        }

        public string depot
        {
            get
            {
                return json.HasValue ? json.Value.GetProperty("DEPOT").GetString() : null;
            }
        }

        public DateTime end_date
        {
            get
            {
                return json.HasValue && json.Value.TryGetProperty("JOB_END", out var e) && e.ValueKind != JsonValueKind.Null ? DateTime.Parse(e.GetString()) : DateTime.MinValue;
            }
        }

        public string id
        {
            get { return this.json.Value.GetProperty("ID").GetString(); }
        }

        public string jobId { get; private set; }

        public string name
        { get { return json.HasValue ? json.Value.GetProperty("JOB_NAME").GetString() : null; } }

        public DateTime out_date
        {
            get
            {
                return json.HasValue ? DateTime.Parse(json.Value.GetProperty("OUT_DATE").GetString()) : DateTime.MinValue;
            }
        }

        public DateTime return_date
        {
            get
            {
                return json.HasValue && json.Value.TryGetProperty("RETURN_DATE", out var e) && e.ValueKind != JsonValueKind.Null ? DateTime.Parse(e.GetString()) : DateTime.MinValue;
            }
        }

        public DateTime start_date
        {
            get
            {
                return json.HasValue ? DateTime.Parse(json.Value.GetProperty("JOB_DATE").GetString()) : DateTime.MinValue;
            }
        }

        public int status
        {
            get { return this.json.Value.GetProperty("STATUS").GetInt32(); }
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