using Hire_Hop_Interface.Interface;
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

        private List<CustomField> _customFields;

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

        public List<CustomField> customFields
        {
            get { return _customFields == null ? ExtractCustomFields() : _customFields; }
        }

        public string id
        {
            get { return this.json.Value.GetProperty("ID").GetString(); }
        }

        #endregion Properties

        #region Methods

        public override async Task<bool> LoadData(Interface.Cookies.Connection cookie)
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

        public async Task<bool> SaveCustomFields(Interface.Cookies.Connection cookie)
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

        private List<CustomField> ExtractCustomFields()
        {
            try
            {
                var values = this.json.Value.GetProperty("fields").GetProperty("ethl_custom_fields").GetProperty("value").EnumerateArray();
                _customFields = values.Select(x => JsonSerializer.Deserialize<CustomField>(x.GetRawText())).ToList();
            }
            catch (Exception e)
            {
                _customFields = new List<CustomField>();
            }
            return _customFields;
        }

        #endregion Methods

        #region Classes

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

        #endregion Classes
    }
}