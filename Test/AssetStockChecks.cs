using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class AssetStockChecks
    {
        #region Fields

        private Hire_Hop_Interface.Interface.Connections.CookieConnection cookie = new Hire_Hop_Interface.Interface.Connections.CookieConnection();

        #endregion Fields

        #region Methods

        [TestMethod]
        public void EnsureBarcodeWorks()
        {
            string barcode = "001122";
            var t_obj = Asset.FindByBarcode(cookie, barcode);

            t_obj.Wait();

            var obj = t_obj.Result;

            Assert.IsTrue(obj.barcode == barcode);
        }

        [TestMethod]
        public void EnsureGetAssetWorks()
        {
            var t_obj = Asset.GetAssets(cookie, 25);

            t_obj.Wait();

            var obj = t_obj.Result;

            Assert.IsTrue(obj.results.Length > 0);
        }

        [TestMethod]
        public void EnsureIDLookupWorks()
        {
            var t_obj_1 = Stock.FindStock(cookie, 402);
            var t_obj_2 = Stock.FindStock(cookie, 156);

            t_obj_1.Wait();
            t_obj_2.Wait();

            var obj_1 = t_obj_1.Result;
            var obj_2 = t_obj_2.Result;

            Assert.IsNotNull(obj_1);
            Assert.IsNotNull(obj_2);
        }

        [TestMethod]
        public void EnsureSearchWorks()
        {
            var t_obj = Stock.SearchForAll(cookie);

            t_obj.Wait();

            var obj = t_obj.Result;

            Assert.IsTrue(obj.results.Length > 0);
        }

        [TestInitialize]
        public void Setup()
        {
            var req = Authentication.Login(cookie, Details.hh_email, Details.hh_password);

            req.Wait();

            Assert.IsTrue(req.Result);
        }

        #endregion Methods
    }
}