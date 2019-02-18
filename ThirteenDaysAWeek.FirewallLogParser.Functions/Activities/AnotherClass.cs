using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace ThirteenDaysAWeek.FirewallLogParser.Functions.Activities
{
    public static class AnotherClass
    {
        [FunctionName(nameof(DoGoodStuff))]
        public static async Task DoGoodStuff([ActivityTrigger] DurableActivityContext context, TraceWriter log)
        {
            log.Info("Child function called successfully");
        }
    }
}
