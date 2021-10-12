using Hire_Hop_Interface.Management;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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

            int startOfUData = client.__lastContent.IndexOf("var user=") + 9;

            int endOfUData = client.__lastContent.IndexOf(";var check_server=");

            string uData = client.__lastContent.Substring(startOfUData, endOfUData - startOfUData);

            try
            {
                client.userData = JObject.Parse(uData);
            }
            catch
            {
            }

            var cookie = client.cookies;

            return cookie.Count > 0 && cookie.Any(x => x.Name == "id") && cookie["id"].Value != "deleted";
        }

        #endregion Methods
    }
}