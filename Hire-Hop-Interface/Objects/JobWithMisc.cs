using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hire_Hop_Interface.Objects;
using Hire_Hop_Interface.Interface.Connections;

namespace Hire_Hop_Interface.Objects
{
    public class JobWithMisc
    {
        public Job job;
        public Job.Bill bill;
        public Job.Costs costs;
        public DateTime lastModified;
        
        public JobWithMisc(Job job)
        {
            this.job = job;
        }

        public async Task LoadMisc(CookieConnection cookie)
        {
            bill = await job.CalculateBill(cookie);
        }
    }
}
