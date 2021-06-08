using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace QuartzNet
{
    class TimeJob : IJob
    {
        readonly ILogger<TimeJob> _log;

        public TimeJob(ILogger<TimeJob> log)
        {
            _log = log;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _log.LogInformation("Time is: {time}", DateTime.UtcNow);

            return Task.CompletedTask;
        }
    }
}
