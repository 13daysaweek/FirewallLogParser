using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace ThirteenDaysAWeek.FirewallLogParser.Functions
{
    public static class Orchestrator
    {
        [FunctionName(nameof(ScheduledStart))]
        public static async Task ScheduledStart([TimerTrigger("0 * * * * *")] TimerInfo timerInfo,
            [OrchestrationClient] DurableOrchestrationClient starter, TraceWriter log)
        {
            var instanceId = await starter.StartNewAsync(nameof(StartOrchestrator), null);
            log.Info("Timer elapsed, starting orchestration");
            log.Info($"Instance Id {instanceId} started!");
        }

        [FunctionName(nameof(StartOrchestrator))]
        public static async Task StartOrchestrator([OrchestrationTrigger] DurableOrchestrationContext context,
            TraceWriter log)
        {
            await context.CallActivityAsync(nameof(AnotherClass.DoGoodStuff), null);
        }
    }

    public static class AnotherClass
    {
        [FunctionName(nameof(DoGoodStuff))]
        public static async Task DoGoodStuff([ActivityTrigger] DurableActivityContext context, TraceWriter log)
        {
            log.Info("Child function called successfully");
        }
    }
}