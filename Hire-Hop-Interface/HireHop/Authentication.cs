﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.HireHop
{
    public static class Authentication
    {
        #region Methods

        public static async Task<bool> CanReachHome(ConnectionCookie connection)
        {
            var req = new Request("home.php", "get", connection);
            var res = await req.Execute();
            return res.body.Contains("<title>HireHop</title>");
        }

        public static async Task<bool> Login(ConnectionCookie connection, string username, string password, string company = "ELTH")
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
                    int user_data_start = res.body.IndexOf("var user=") + 9;
                    int user_data_end = res.body.IndexOf(";var check_server=");

                    string user_data = res.body.Substring(user_data_start, user_data_end - user_data_start);

                    try
                    {
                        connection.user_data = JsonSerializer.Deserialize<JsonElement>(user_data);
                    }
                    catch
                    {
                    }

                    connection.extractHeadersFromHandler();

                    return connection.email != null && connection.password != null && connection.key != null;
                }
            }
            return false;
        }

        #endregion Methods
    }
}