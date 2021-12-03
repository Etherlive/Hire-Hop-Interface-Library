using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Interface.Caching
{
    public class CacheableRequest : Request
    {
        #region Constructors

        public CacheableRequest(string _url, string _method, Connections.CookieConnection _connectionCookie) : base(_url, _method, _connectionCookie)
        {
        }

        #endregion Constructors

        #region Methods

        public async Task<Response> Execute(ResponseCache cache)
        {
            if (cache.Find(this, out Response response))
            {
                return response;
            }
            else
            {
                var res = await base.Execute();
                cache.StoreOrUpdate(this, res);
                return res;
            }
        }

        #endregion Methods
    }
}