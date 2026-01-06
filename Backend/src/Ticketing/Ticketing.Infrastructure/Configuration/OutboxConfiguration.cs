using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Ticketing.Infrastructure.Jobs;

namespace Ticketing.Infrastructure.Configuration
{
    public static class OutboxConfiguration
    {
        public static IServiceCollection AddOutboxPattern(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("OutboxPublisher");
                q.AddJob<OutboxPublisherJob>(opts => opts.WithIdentity(jobKey));
                
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("OutboxPublisher-trigger")
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(30)
                        .RepeatForever()));
            });
            
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            
            return services;
        }
    }
}