using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using Ticketing.Application.Interfaces;

namespace Ticketing.Infrastructure.Jobs
{
    public class OutboxPublisherJob : IJob
    {
        private readonly IOutboxService _outboxService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<OutboxPublisherJob> _logger;

        public OutboxPublisherJob(IOutboxService outboxService, IEventPublisher eventPublisher, ILogger<OutboxPublisherJob> logger)
        {
            _outboxService = outboxService;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Starting outbox publisher job");
            
            var retryableEvents = await _outboxService.GetRetryableEventsAsync();
            var processedCount = 0;
            var failedCount = 0;

            foreach (var outboxEvent in retryableEvents)
            {
                try
                {
                    await _eventPublisher.PublishAsync(outboxEvent.EventType, outboxEvent.EventData);
                    await _outboxService.MarkAsProcessedAsync(outboxEvent.Id);
                    processedCount++;
                    _logger.LogInformation($"Successfully published event {outboxEvent.Id} of type {outboxEvent.EventType}");
                }
                catch (Exception ex)
                {
                    failedCount++;
                    _logger.LogError(ex, $"Failed to publish event {outboxEvent.Id} (attempt {outboxEvent.RetryCount + 1})");
                    await _outboxService.MarkAsFailedAsync(outboxEvent.Id, ex.Message);
                }
            }
            
            _logger.LogInformation($"Outbox publisher job completed. Processed: {processedCount}, Failed: {failedCount}");
        }
    }
}