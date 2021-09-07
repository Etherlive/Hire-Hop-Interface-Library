using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hire_Hop_Interface.Management;
using Newtonsoft.Json.Linq;

namespace Hire_Hop_Interface.Requests
{
    public static class Jobs
    {
        public static async Task<JObject> GetJobData(ClientConnection client, string jobId)
        {
            client = await RequestInterface.SendRequest(client, "php_functions/job_refresh.php", contentList: new List<string>()
            {
                $"job={jobId}"
            });
            string job_data = await client.__lastResponse.Content.ReadAsStringAsync();
            try
            {
                return JObject.Parse(job_data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
