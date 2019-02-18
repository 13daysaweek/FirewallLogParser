using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ThirteenDaysAWeek.FirewallLogParser.Functions.Orchestration;

namespace ThirteenDaysAWeek.FirewallLogParser.Functions
{
    public static class TimedStarter
    {
        [FunctionName(nameof(ScheduledStart))]
        public static async Task ScheduledStart([TimerTrigger("0 * * * * *")] TimerInfo timerInfo,
            [OrchestrationClient] DurableOrchestrationClient starter, ILogger log)
        {
            var instanceId = await starter.StartNewAsync(nameof(Orchestrator.StartOrchestrator), null);
            log.LogInformation("Timer elapsed, starting orchestration");
            log.LogInformation($"Instance Id {instanceId} started!");
        }
    }


}