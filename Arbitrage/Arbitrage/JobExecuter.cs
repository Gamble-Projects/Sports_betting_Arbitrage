using System;
using System.Threading.Tasks;
using Quartz;

namespace Arbitrage
{
    public class JobExecuter : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var message = $"JobExecuter executed at ${DateTime.Now.ToString()}";
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
