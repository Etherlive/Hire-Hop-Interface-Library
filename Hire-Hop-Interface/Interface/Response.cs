using System.Text.Json;

namespace Hire_Hop_Interface.Interface
{
    public class Response
    {
        #region Fields

        private JsonElement? _json;
        public readonly string body;
        public readonly Request request;
        public bool fromCache = false;

        #endregion Fields

        #region Constructors

        public Response(Request _request, string _body)
        {
            this.body = _body;
            this.request = _request;
        }

        #endregion Constructors

        #region Methods

        public bool TryParseJson(out JsonElement? json)
        {
            try
            {
                json = JsonSerializer.Deserialize<JsonElement>(this.body);
                return true;
            }
            catch
            {
                json = null;
                return false;
            }
        }

        #endregion Methods
    }
}