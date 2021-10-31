namespace Hire_Hop_Interface.Management
{
    public class TrackedIdentity
    {
        #region Fields

        private string identity_str;
        public ClientConnection clientConnection = new ClientConnection();
        public User user;

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