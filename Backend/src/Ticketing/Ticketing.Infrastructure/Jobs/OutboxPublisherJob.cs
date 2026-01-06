using System.Threading.Tasks;
using Quartz;
using Ticketing.Application.Interfaces;

namespace Ticketing.Infrastructure.Jobs
{
    public class OutboxPublisherJob : IJob
    {
        private readonly IOutboxService _outboxService;
        private readonly IEventPublisher _eventPublisher;

        public OutboxPublisherJob(IOutboxService outboxService, IEventPublisher eventPublisher)
        {
            _outboxService = outboxService;
            _eventPublisher = eventPublisher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var unprocessedEvents = await _outboxService.GetUnprocessedEventsAsync();

            foreach (var outboxEvent in unprocessedEvents)
            {
                await _eventPublisher.PublishAsync(outboxEvent.EventType, outboxEvent.EventData);
                await _outboxService.MarkAsProcessedAsync(outboxEvent.Id);
            }
        }
    }
}