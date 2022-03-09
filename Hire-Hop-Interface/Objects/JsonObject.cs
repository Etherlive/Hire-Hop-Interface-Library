using Hire_Hop_Interface.Interface;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class JsonObject
    {
        #region Constructors

        public JsonObject()
        {
        }

        public JsonObject(JsonElement _json)
        {
            this.json = _json;
        }

        #endregion Constructors

        #region Properties

        public JsonElement? json { get; set; }

        #endregion Properties

        #region Methods

        public virtual async Task<bool> LoadData(Interface.Connections.CookieConnection cookie)
        {
            throw new Exception("You are using a unoverloaded Data Load Function");
        }

        public async Task<bool> LoadData(Request req)
        {
            var res = await req.ExecuteWithCache();

            JsonElement? json;
            if (res.TryParseJson(out json))
            {
                this.json = json;
                if (this.json.Value.TryGetProperty("error", out JsonElement e))
                {
                    Console.WriteLine($"An Error Occurred: {e.ToString()}");
                    return false;
                }
                return true;
            }
            return false;
        }

        #endregion Methods
    }
}