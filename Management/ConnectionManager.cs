using System;
using System.Collections.Generic;

namespace Hire_Hop_Interface.Management
{
    public static class ConnectionManager
    {
        #region Fields

        private static Random rnd = new Random();
        private static Dictionary<string, TrackedIdentity> tracked_connections = new Dictionary<string, TrackedIdentity>();

        #endregion Fields

        #region Methods

        public static string CreateClient()
        {
            string identifier = rndString();
            tracked_connections.Add(identifier, new TrackedIdentity(identifier));
            return identifier;
        }

        public static bool IsIdentity(string identity, out TrackedIdentity client)
        {
            if (tracked_connections.ContainsKey(identity))
            {
                client = tracked_connections[identity];
                return true;
            }
            else
            {
                client = null;
                return false;
            }
        }

        private static string rndString(uint length = 32)
        {
            string s = "";
            for (uint i = 0; i < length; i++) s += (char)rnd.Next(65, 122);
            return s.Replace("\\", "/");
        }

        #endregion Methods
    }
}
