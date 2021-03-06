using System.Collections.Generic;

namespace Hire_Hop_Interface.Interface.Connections
{
    public class Manager
    {
        #region Fields

        private Dictionary<string, CookieConnection> memory_cookie_store;

        #endregion Fields

        #region Constructors

        public Manager()
        {
            memory_cookie_store = new Dictionary<string, CookieConnection>();
        }

        #endregion Constructors

        #region Methods

        public void AddOrSetCookie(string key, CookieConnection cookie)
        {
            if (!memory_cookie_store.TryAdd(key, cookie))
            {
                memory_cookie_store[key] = cookie;
            }
        }

        public void DropCookie(string key)
        {
            if (memory_cookie_store.ContainsKey(key))
            {
                memory_cookie_store.Remove(key);
            }
        }

        public bool FindCookie(string key, out CookieConnection cookie)
        {
            return memory_cookie_store.TryGetValue(key, out cookie);
        }

        #endregion Methods
    }
}