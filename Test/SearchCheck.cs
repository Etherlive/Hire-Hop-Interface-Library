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
        private SearchResult.SearchResponse searchResponse;

        #endregion Fields

        #region Methods

        [TestMethod]
        public void EnsureLoadData()
        {
            var results = SearchResult.Search(new SearchResult.SearchOptions(), cookie);
            results.Wait();

            var search = results.Result.results[0];
            var job = search.GetJob(cookie);
            job.Wait();

            Assert.IsTrue(job.Result.id == search.trimmedId);
        }

        [TestMethod]
        public void EnsureSearch()
        {
            var results = SearchResult.Search(new SearchResult.SearchOptions(), cookie);
            results.Wait();

            Assert.IsNotNull(results.Result);

            Assert.IsTrue(results.Result.results[0].is_job || results.Result.results[0].is_project);

            Assert.IsTrue(results.Result.max_page > -1);
        }

        [TestMethod]
        public void EnsureSearchAll()
        {
            var results = SearchResult.SearchForAll(new SearchResult.SearchOptions(), cookie);
            results.Wait();

            Assert.IsNotNull(results.Result);
            searchResponse = results.Result;
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