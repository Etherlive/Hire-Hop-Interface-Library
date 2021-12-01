using Hire_Hop_Interface.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Hire_Hop_Interface.Interface.Connections;

namespace Test
{
    [TestClass]
    public class AuthenticationChecks
    {
        #region Fields

        private CookieConnection cookie = new CookieConnection();
        private Manager manager = new Manager();

        #endregion Fields

        #region Methods

        [TestMethod]
        public void EnsureCookieManagerWorks()
        {
            manager.AddOrSetCookie("local", cookie);

            Assert.IsTrue(manager.FindCookie("local", out _));
        }

        [TestMethod]
        public void EnsureLoginWorks()
        {
            var req = Authentication.Login(cookie, Details.hh_email, Details.hh_password);

            req.Wait();

            Assert.IsTrue(req.Result);
        }

        #endregion Methods
    }
}