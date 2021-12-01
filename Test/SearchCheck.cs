using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Objects;

namespace Test
{
    [TestClass]
    public class SearchCheck
    {
        #region Fields

        private Hire_Hop_Interface.Interface.Connections.CookieConnection cookie = new Hire_Hop_Interface.Interface.Connections.CookieConnection();

        #endregion Fields

        #region Methods

        [TestMethod]
        public void EnsureSearch()
        {
            var results = SearchResult.Search(new SearchResult.SearchOptions(), cookie);
            results.Wait();

            Assert.IsNotNull(results.Result);

            Assert.IsTrue(results.Result[0].is_job || results.Result[0].is_project);
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