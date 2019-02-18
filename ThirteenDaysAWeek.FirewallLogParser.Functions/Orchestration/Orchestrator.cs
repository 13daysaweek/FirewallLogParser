using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ThirteenDaysAWeek.FirewallLogParser.Functions.Activities;

namespace ThirteenDaysAWeek.FirewallLogParser.Functions.Orchestration
{
    public static class Orchestrator
    {
        [FunctionName(nameof(StartOrchestrator))]
        public static async Task StartOrchestrator([OrchestrationTrigger] DurableOrchestrationContext context,
            ILogger log)
        {
            var user = Environment.GetEnvironmentVariable("router-username");
            await context.CallActivityAsync(nameof(LogRetriever.GetLogs), null);
        }
    }
}
