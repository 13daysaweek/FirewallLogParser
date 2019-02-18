using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace ThirteenDaysAWeek.FirewallLogParser.Functions.Activities
{
    public class LogRetriever
    {
        [FunctionName("GetLogs")]
        public static async Task<string> GetLogs([ActivityTrigger] DurableActivityContext context, ILogger log)
        {
            var hostKey = Convert.FromBase64String(Environment.GetEnvironmentVariable("router-key"));
            var loginName = Environment.GetEnvironmentVariable("router-username");
            var hostName = Environment.GetEnvironmentVariable("router-hostname");
            var portNumber = Convert.ToInt32(Environment.GetEnvironmentVariable("router-port-number"));
            var logContent = new StringBuilder();

            log.LogInformation("Got some variables");

            using (var keyStream = new MemoryStream(hostKey))
            using (var keyFile = new PrivateKeyFile(keyStream))
            using (var authMethod = new PrivateKeyAuthenticationMethod(loginName, keyFile))
            {
                var connectionInfo = new ConnectionInfo(hostName, portNumber, loginName, authMethod);

                using (var client = new SshClient(connectionInfo))
                {
                    client.Connect();

                    var directoryListing = client.CreateCommand("ls /tmp/syslog*");
                    var fileListing = directoryListing.Execute();
                    var files = fileListing.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

                    if (files != null && files.Any())
                    {
                        foreach (var file in files)
                        {
                            var catCommand = client.CreateCommand($"cat {file}");
                            var contents = catCommand.Execute();
                            logContent.Append(contents);
                        }
                    }
                    else
                    {
                        log.LogWarning("Did not find any matching log files");
                    }

                    client.Disconnect();
                }
            }

            return await Task.FromResult(logContent.ToString());
        }
    }
}
