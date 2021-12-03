using System;
using System.Collections.Generic;
using System.Text;

namespace Hire_Hop_Interface.Interface.Caching
{
    public class CachedResponse : Response
    {
        #region Fields

        public DateTime requestTime;

        #endregion Fields

        #region Constructors

        public CachedResponse(Response response) : base(response.request, response.body)
        {
            fromCache = true;
            requestTime = DateTime.Now;
        }

        public CachedResponse(Request _request, string _body) : base(_request, _body)
        {
            fromCache = true;
            requestTime = DateTime.Now;
        }

        #endregion Constructors
    }
}