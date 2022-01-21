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

        public JsonElement? json { get; protected set; }

        #endregion Properties

        #region Methods

        public virtual async Task<bool> LoadData(Interface.Connections.CookieConnection cookie)
        {
            throw new Exception("You are using a unoverloaded Data Load Function");
        }

        public async Task<bool> LoadData(Request req)
        {
            var res = await req.Execute();

            JsonElement? json;
            if (res.TryParseJson(out json))
            {
                this.json = json;
                return true;
            }
            return false;
        }

        #endregion Methods
    }
}