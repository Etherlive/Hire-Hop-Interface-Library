using System;
using System.Collections.Generic;
using System.Text;

namespace Hire_Hop_Interface.Management
{
    public class TrackedIdentity
    {
        #region Fields

        public ClientConnection clientConnection = new ClientConnection();
        public User user;

        private string identity_str;

        #endregion Fields

        #region Constructors

        public TrackedIdentity(string identity)
        {
            identity_str = identity;
        }

        #endregion Constructors

        #region Methods

        public void Update()
        {
        }

        #endregion Methods
    }

    public class User
    {
        #region Properties

        public string username { get; set; }

        #endregion Properties
    }
}
