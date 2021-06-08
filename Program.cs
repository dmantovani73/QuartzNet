using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuartzNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddQuartz(q =>
                    {
                        // Abilita dependency injection.
                        q.UseMicrosoftDependencyInjectionScopedJobFactory();

                        // Si attribuisce un nome al job.
                        var jobKey = new JobKey(nameof(TimeJob));
                        q.AddJob<TimeJob>(options => options.WithIdentity(jobKey));

                        // Si definisce quali sono i trigger che avviano il job.
                        q.AddTrigger(options => options
                            .ForJob(jobKey)
                            .WithIdentity($"{jobKey.Name} Trigger")
                            .StartNow()
                            
                            // https://crontab.guru/
                            //.WithCronSchedule("* * * * *")
                            
                            .WithSimpleSchedule(schedule => schedule
                                .WithInterval(TimeSpan.FromSeconds(1))
                                .RepeatForever()
                                ));
                    });

                    // Si attende che i job completino la propria esecuzione quando viene terminato il worker.
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                });
    }
}
