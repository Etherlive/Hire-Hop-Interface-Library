using Hire_Hop_Interface.Interface;
using Hire_Hop_Interface.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        public void EnsureGetNotes()
        {
            var notes = Note.GetNotes(cookie, "1131");
            notes.Wait();

            Assert.IsNotNull(notes.Result);

            if (notes.Result.Length > 0)
            {
                Assert.IsNotNull(notes.Result[0].job_id);
                Assert.IsNotNull(notes.Result[0].json);
            }
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