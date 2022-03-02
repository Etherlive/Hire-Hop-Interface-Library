using Hire_Hop_Interface.Interface.Connections;
using System.Linq;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Objects.JobProject
{
    public partial class SearchResult
    {
        #region Classes

        public class SearchResponse : SearchCollection<SearchResult>
        {
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