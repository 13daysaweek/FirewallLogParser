using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using ThirteenDaysAWeek.FirewallLogParser.Functions.Orchestration;

namespace ThirteenDaysAWeek.FirewallLogParser.Functions
{
    public static class TimedStarter
    {
        [FunctionName(nameof(ScheduledStart))]
        public static async Task ScheduledStart([TimerTrigger("0 * * * * *")] TimerInfo timerInfo,
            [OrchestrationClient] DurableOrchestrationClient starter, TraceWriter log)
        {
            var instanceId = await starter.StartNewAsync(nameof(Orchestrator.StartOrchestrator), null);
            log.Info("Timer elapsed, starting orchestration");
            log.Info($"Instance Id {instanceId} started!");
        }
    }


}