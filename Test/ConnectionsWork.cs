using Hire_Hop_Interface.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class ConnectionsWork
    {
        #region Fields

        private Hire_Hop_Interface.Interface.Connections.CookieConnection cookie = new Hire_Hop_Interface.Interface.Connections.CookieConnection();

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

            Assert.IsNotNull(res.Result.TryParseJson(out _));
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