using Hire_Hop_Interface.Interface;
using System;
using System.Collections.Generic;
using System.Text;

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
            if (!memory_cookie_store.TryGetValue(key, out cookie))
            {
                return Miss(key, out cookie);
            }
            return true;
        }

        public bool Miss(string key, out CookieConnection cookie)
        {
            cookie = null;
            return false;
        }

        #endregion Methods
    }
}