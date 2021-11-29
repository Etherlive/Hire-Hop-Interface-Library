using System.Text.Json;

namespace Hire_Hop_Interface.HireHop
{
    public class Response
    {
        #region Fields

        private JsonElement? _json;

        #endregion Fields

        #region Methods

        private JsonElement parseBody()
        {
            this._json = JsonSerializer.Deserialize<JsonElement>(this.body);
            return this._json.Value;
        }

        #endregion Methods

        public readonly string body;

        public readonly Request request;

        public Response(Request _request, string _body)
        {
            this.body = _body;
            this.request = _request;
        }

        public JsonElement json
        {
            get { return this._json.HasValue ? this._json.Value : this.parseBody(); }
        }
    }
}