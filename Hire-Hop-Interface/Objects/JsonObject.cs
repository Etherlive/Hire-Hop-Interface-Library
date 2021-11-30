using Hire_Hop_Interface.HireHop;
using System;
using System.Collections.Generic;
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

        public virtual async Task<bool> LoadData(ConnectionCookie cookie)
        {
            throw new Exception("You are using a unoverloaded Data Load Function");
        }

        #endregion Methods
    }
}