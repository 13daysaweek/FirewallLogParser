using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ThirteenDaysAWeek.FirewallLogParser.Functions.Activities
{
    public class LogRetriever
    {
        [FunctionName("GetLogs")]
        public static async Task GetLogs([ActivityTrigger] DurableActivityContext context, ILogger log)
        {
            var hostKey = Convert.FromBase64String(Environment.GetEnvironmentVariable("router-key"));
            var loginName = Environment.GetEnvironmentVariable("router-username");
            var hostName = Environment.GetEnvironmentVariable("router-hostname");

            log.LogInformation("Got some variables");
        }
    }
}
