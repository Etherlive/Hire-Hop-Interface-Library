using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class JobCheck
    {
        #region Fields

        private Hire_Hop_Interface.Interface.Connections.CookieConnection cookie = new Hire_Hop_Interface.Interface.Connections.CookieConnection();
        private Job job;

        #endregion Fields

        #region Methods

        [TestMethod]
        public void EnsureCustomFieldUpdating()
        {
            job.customFields.Add(new Job.CustomField("0", "k0", "v0"));

            var admn_req = Authentication.ToggleAdmin(cookie);
            admn_req.Wait();

            Assert.IsTrue(admn_req.Result);

            var req = job.SaveCustomFields(cookie);
            req.Wait();

            Assert.IsTrue(req.Result);
            Assert.IsTrue(job.customFields[0].value == "v0");
        }

        [TestMethod]
        public void EnsureJobWorks()
        {
            Assert.IsTrue(job.json.HasValue);

            Assert.IsNotNull(job.id);
        }

        [TestInitialize]
        public void Setup()
        {
            var req = Authentication.Login(cookie, Details.hh_email, Details.hh_password);

            req.Wait();

            Assert.IsTrue(req.Result);

            job = new Job("1131");
            req = job.LoadData(cookie);

            req.Wait();
            Assert.IsTrue(req.Result);
        }

        #endregion Methods
    }
}