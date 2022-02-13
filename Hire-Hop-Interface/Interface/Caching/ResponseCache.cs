using System;
using System.Collections.Generic;
using System.Linq;

namespace Hire_Hop_Interface.Interface.Caching
{
    public class ResponseCache
    {
        #region Fields

        private Dictionary<string, CachedResponse> responseStore = new Dictionary<string, CachedResponse>();

        #endregion Fields

        #region Methods

        private string GetKey(Request request)
        {
            string formKey = String.Join(",", request.urlFormValues.Select(x => $"{x.Key}-{x.Value}"));
            string key = $"{request.urlWithParams}-{request.method}-{formKey}";

            return key;
        }

        public bool Find(Request request, out Response response)
        {
            string key = GetKey(request);
            response = null;

            if (responseStore.TryGetValue(key, out CachedResponse c_response))
            {
                if (c_response.requestTime.AddMinutes(1) > DateTime.Now)
                {
                    response = c_response;
                    return true;
                }
            }
            return false;
        }

        public void StoreOrUpdate(Request request, Response response)
        {
            string key = GetKey(request);
            var c_response = new CachedResponse(response);

            if (!responseStore.TryAdd(key, c_response))
            {
                responseStore[key] = c_response;
            }
        }

        #endregion Methods
    }
}