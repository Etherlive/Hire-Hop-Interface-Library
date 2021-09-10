using Hire_Hop_Interface.Management;
using Hire_Hop_Interface.Objects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hire_Hop_Interface.Requests
{
    public static class BulkAdditionalData
    {
        #region Methods
        public static void SetLastModified(ref Objects.Job[] jobs, ClientConnection client)
        {
            Console.WriteLine("Getting Last Modified ...");

            var loadTasks = jobs.Select(x => x.GetLastModified(client)).ToArray();

            Task.WaitAll(loadTasks);

            int idx = 0;
            foreach (Hire_Hop_Interface.Objects.Job j in jobs)
            {
                j.lastModified = loadTasks[idx].Result;
                idx++;
            }

            Console.WriteLine("Set Last Modified");
        }

        public static void CalculateBilling(ref Objects.Job[] jobs, ClientConnection client)
        {
            Console.WriteLine("Calculating Bills ...");

            var loadTasks = jobs.Select(x => x.CalculateBilling(client)).ToArray();

            Task.WaitAll(loadTasks);

            int idx = 0;
            foreach (Hire_Hop_Interface.Objects.Job j in jobs)
            {
                j.bill = loadTasks[idx].Result;
                idx++;
            }

            Console.WriteLine("Calculated Bills");
        }

        public static void CalculateCosts(ref Objects.Job[] jobs, ClientConnection client)
        {
            Console.WriteLine("Calculating Costs ...");

            var loadTasks = jobs.Select(x => x.CalculateCosts(client)).ToArray();

            Task.WaitAll(loadTasks);

            int idx = 0;
            foreach (Hire_Hop_Interface.Objects.Job j in jobs)
            {
                j.costs = loadTasks[idx].Result;
                idx++;
            }

            Console.WriteLine("Calculated Costs");
        }

        public static void LoadExtraDetail(ref SearchResult[] results, ClientConnection client)
        {
            Console.WriteLine("Loading Extra Detail ...");

            var tasks = results.Select(x => x.LoadDetail(client)).ToArray();
            Task.WaitAll(tasks);

            int idx = 0;
            foreach (SearchResult result in results)
            {
                result.data = tasks[idx].Result;
                idx++;
            }

            Console.WriteLine("Loaded Detail");
        }

        public static Hire_Hop_Interface.Objects.Job[] SearchToJob(SearchResult[] results)
        {
            return results.Select(x => new Hire_Hop_Interface.Objects.Job(x)).ToArray();
        }

        #endregion Methods
    }
}