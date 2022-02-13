using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Interface.Caching;
using Hire_Hop_Interface.Objects;

namespace Test
{
    [TestClass]
    public class CachingCheck
    {
        #region Fields

        private ResponseCache cache = new ResponseCache();
        private Hire_Hop_Interface.Interface.Connections.CookieConnection cookie = new Hire_Hop_Interface.Interface.Connections.CookieConnection();

        #endregion Fields

        #region Methods

        [TestMethod]
        public void EnsureCanCacheAndRecall()
        {
            var req = new CacheableRequest("php_functions/job_refresh.php", "POST", cookie);
            req.AddOrSetForm("job", "1131");

            var res = req.ExecuteWithCache(cache);
            res.Wait();

            Assert.IsFalse(res.Result.fromCache);
            string lastBody = res.Result.body;

            res = req.ExecuteWithCache(cache);
            res.Wait();

            string newBody = res.Result.body;

            Assert.AreEqual(lastBody, newBody);
            Assert.IsTrue(res.Result.fromCache);
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