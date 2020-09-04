using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage
{
    public class JobExecuter : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var message = $"JobExecuter executed at ${DateTime.Now.ToString()}";
            System.Diagnostics.Debug.WriteLine(message);
            Console.WriteLine("here");
        }
    }
}
