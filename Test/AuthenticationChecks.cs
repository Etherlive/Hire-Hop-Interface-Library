using Hire_Hop_Interface.HireHop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    [TestClass]
    public class AuthenticationChecks
    {
        #region Fields

        private ConnectionCookie cookie = new ConnectionCookie();

        #endregion Fields

        #region Methods

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