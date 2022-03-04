using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Objects;
using System;

namespace Test
{
    [TestClass]
    public class POCheck
    {
        #region Fields

        private Hire_Hop_Interface.Interface.Connections.CookieConnection cookie = new Hire_Hop_Interface.Interface.Connections.CookieConnection();

        #endregion Fields

        #region Methods

        [TestMethod]
        public void EnsureCreate()
        {
            var t = PurchaseOrder.CreateNew(cookie, "1131", "Test PO", "PO8888888", "2926", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(4));
            t.Wait();
            Assert.IsNotNull(t.Result);
        }

        [TestMethod]
        public void EnsureSearch()
        {
            var t = PurchaseOrder.SearchForAll(cookie);
            t.Wait();
            Assert.IsNotNull(t.Result);
        }

        [TestInitialize]
        public void Setup()
        {
            var req = Authentication.Login(cookie, Details.hh_email, Details.hh_password);

            req.Wait();
        }

        #endregion Methods
    }
}