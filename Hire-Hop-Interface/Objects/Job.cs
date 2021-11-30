using Hire_Hop_Interface.HireHop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects
{
    public class Job : JsonObject
    {
        #region Fields

        private string jobId;

        #endregion Fields

        #region Constructors

        public Job(string _jobId)
        {
            this.jobId = _jobId;
        }

        #endregion Constructors

        #region Methods

        public override async Task<bool> LoadData(ConnectionCookie cookie)
        {
            var req = new Request("php_functions/job_refresh.php", "POST", cookie);
            req.AddOrSetForm("job", this.jobId);

            var res = await req.Execute();
            if (res != null)
            {
                this.json = res.json;
                return true;
            }
            return false;
        }

        #endregion Methods
    }
}