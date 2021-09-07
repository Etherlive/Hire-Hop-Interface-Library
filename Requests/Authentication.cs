using Hire_Hop_Interface.Management;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Requests
{
    public static class Authentication
    {
        #region Methods

        public static async Task<bool> Login(ClientConnection client, string username, string password, string companyId = "ELTH")
        {
            client = await RequestInterface.SendRequest(client, "login.php", contentList: new List<string>()
            {
                "loc=home.php", $"code={companyId}", "rem=1"
            });

            client = await RequestInterface.SendRequest(client, "login_msg.php", contentList: new List<string>()
            {
                "loc=home.php", $"code={companyId}", "rem=1", $"username={username}", $"password={password}"
            });

            var cookie = client.cookies;

            return cookie.Count > 0 && cookie.Any(x => x.Name == "id") && cookie["id"].Value != "deleted";
        }

        #endregion Methods
    }
}