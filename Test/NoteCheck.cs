using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Objects;

namespace Test
{
    [TestClass]
    public class NoteCheck
    {
        #region Fields

        private Hire_Hop_Interface.Interface.Connections.CookieConnection cookie = new Hire_Hop_Interface.Interface.Connections.CookieConnection();

        #endregion Fields

        #region Methods

        [TestMethod]
        public void EnsureCreateAndDeleteNote()
        {
            var note = new Note(cookie, "1131", "Various Text");

            Assert.IsNotNull(note.id);

            var delete = note.Delete(cookie);
            delete.Wait();

            Assert.IsNull(note.json);
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