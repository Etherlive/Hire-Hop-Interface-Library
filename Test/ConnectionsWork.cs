using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hire_Hop_Interface.HireHop;

namespace Test
{
    [TestClass]
    public class ConnectionsWork
    {
        #region Methods

        [TestMethod]
        public void EnsureHomeReqWorks()
        {
            var cookie = new ConnecionCookie("odavies%40etherlive.co.uk", "7673062c13f3471556ca61c95e747123", "9Zzfi8vnNksC");

            var req = new Request("home.php", "get", cookie);
            var res = req.Execute();

            res.Wait();

            Assert.IsNotNull(res.Result);
        }

        #endregion Methods
    }
}