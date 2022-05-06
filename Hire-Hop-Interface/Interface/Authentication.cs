using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Interface
{
    public static class Authentication
    {
        #region Methods

        public static async Task<bool> CanReachHome(Connections.CookieConnection connection)
        {
            var req = new Request("home.php", "get", connection);
            var res = await req.Execute();
            return res.body.Contains("<title>HireHop</title>");
        }

        public static async Task<bool> Login(Connections.CookieConnection connection, string username, string password, string company = "ELTH", int retryDepth = 0)
        {
            var req = new Request("login.php", "post", connection);
            req.AddOrSetForm("loc", "home.php");
            req.AddOrSetForm("code", company);
            req.AddOrSetForm("rem", "1");

            var res = await req.Execute();

            if (res != null)
            {
                req = new Request("login_msg.php", "post", connection);
                req.AddOrSetForm("loc", "home.php");
                req.AddOrSetForm("code", company);
                req.AddOrSetForm("username", username);
                req.AddOrSetForm("password", password);
                req.AddOrSetForm("rem", "1");

                res = await req.Execute();

                if (res != null)
                {
                    req = new Request("home.php", "GET", connection);
                    var res_home = await req.Execute();

                    //int user_data_start = res_home.body.IndexOf("var user=") + 9;
                    //int user_data_end = res_home.body.IndexOf(";var check_server=");

                    //string user_data = res_home.body.Substring(user_data_start, user_data_end - user_data_start);

                    try
                    {
                        string user_data = res_home.body.Split("var user=")[1].Split(";")[0];
                        connection.user_data = JsonSerializer.Deserialize<JsonElement>(user_data);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"An Error Occurred While Signing In");
                        if (retryDepth < 3) return await Login(connection, username, password, company, retryDepth+1);
                        else return false;
                    }

                    connection.extractHeadersFromHandler();

                    return connection.email != null && connection.password != null && connection.key != null;
                }
            }
            return false;
        }

        public static async Task<bool> ToggleAdmin(Connections.CookieConnection connection)
        {
            var req = new Request("php_functions/superuser.php", "POST", connection);
            var res = await req.Execute();
            if (res != null)
            {
                if (res.TryParseJson(out JsonElement? json))
                {
                    return json.Value.TryGetProperty("success", out JsonElement e) && e.GetBoolean();
                }
            }
            return false;
        }

        #endregion Methods
    }
}
