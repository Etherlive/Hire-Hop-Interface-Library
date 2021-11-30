using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hire_Hop_Interface.HireHop;
using Hire_Hop_Interface.Objects;

namespace Test
{
    [TestClass]
    public class JobCheck
    {
        #region Fields

        private ConnectionCookie cookie = new ConnectionCookie();

        #endregion Fields

        #region Methods

        [TestMethod]
        public void EnsureJobWorks()
        {
            var job = new Job("1131");
            var req = job.LoadData(cookie);

            req.Wait();

            Assert.IsTrue(req.Result);

            Assert.IsTrue(job.json.HasValue);
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