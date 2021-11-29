namespace Hire_Hop_Interface.HireHop
{
    public class Response
    {
        #region Fields

        public readonly string body;

        public readonly Request request;

        #endregion Fields

        #region Constructors

        public Response(Request _request, string _body)
        {
            this.body = _body;
            this.request = _request;
        }

        #endregion Constructors
    }
}