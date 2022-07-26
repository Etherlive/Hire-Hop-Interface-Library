using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class AssetChecks
    {
        private Hire_Hop_Interface.Interface.Connections.CookieConnection cookie = new Hire_Hop_Interface.Interface.Connections.CookieConnection();
        [TestMethod]
        public void EnsureSearchWorks()
        {
            var t_obj = Asset.SearchForAll(cookie);

            t_obj.Wait();

            var obj = t_obj.Result;

            Assert.IsTrue(obj.results.Length > 0);
        }
        [TestMethod]
        public void EnsureBarcodeWorks()
        {
            string barcode = "001122";
            var t_obj = Asset.FindByBarcode(cookie, barcode);

            t_obj.Wait();

            var obj = t_obj.Result;

            Assert.IsTrue(obj.barcode == barcode);
        }

        [TestInitialize]
        public void Setup()
        {
            var req = Authentication.Login(cookie, Details.hh_email, Details.hh_password);

            req.Wait();

            Assert.IsTrue(req.Result);
        }
    }
}
