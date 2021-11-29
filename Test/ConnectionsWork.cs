using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hire_Hop_Interface.HireHop;

namespace Test
{
    [TestClass]
    public class ConnectionsWork
    {
        #region Fields

        private ConnecionCookie cookie = new ConnecionCookie("odavies%40etherlive.co.uk", "7673062c13f3471556ca61c95e747123", "9Zzfi8vnNksC");

        #endregion Fields

        #region Methods

        [TestMethod]
        public void EnsureHomeReqWorks()
        {
            var req = new Request("home.php", "get", cookie);
            var res = req.Execute();

            res.Wait();

            Assert.IsNotNull(res.Result);
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

        #endregion Methods
    }
}