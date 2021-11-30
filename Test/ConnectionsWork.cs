using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hire_Hop_Interface.HireHop;

namespace Test
{
    [TestClass]
    public class ConnectionsWork
    {
        #region Fields

        private ConnectionCookie cookie = new ConnectionCookie();

        #endregion Fields

        #region Methods

        [TestMethod]
        public void EnsureHomeReqWorks()
        {
            var req = Authentication.CanReachHome(cookie);

            req.Wait();

            Assert.IsTrue(req.Result);
        }

        [TestMethod]
        public void EnsureJSONWorks()
        {
            var req = new Request("php_functions/job_refresh.php", "get", cookie);

            req.AddOrSetQuery("job", "1131");

            var res = req.Execute();

            res.Wait();

            Assert.IsNotNull(res.Result.json);
        }

        [TestInitialize]
        public void EnsureLoginWorks()
        {
            var req = Authentication.Login(cookie, Details.hh_email, Details.hh_password);

            req.Wait();

            Assert.IsTrue(req.Result);
        }

        #endregion Methods
    }
}