using System.Threading.Tasks;
using Hire_Hop_Interface.Interface.Connections;
using System.Linq;

namespace Hire_Hop_Interface.Objects
{
    public partial class SearchResult
    {
        #region Classes

        public class SearchResponse
        {
            #region Fields

            public int max_page = -1;
            public SearchResult[] results;

            #endregion Fields

            #region Methods

            public async Task<Job[]> LoadAllJobs(CookieConnection cookie)
            {
                var job_tasks = results.Select(x => x.GetJob(cookie)).ToArray();

                Task.WaitAll(job_tasks);

                return job_tasks.Select(x => x.Result).ToArray();
            }

            #endregion Methods
        }

        #endregion Classes
    }
}