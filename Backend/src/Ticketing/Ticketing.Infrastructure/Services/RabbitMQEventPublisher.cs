using System.Threading.Tasks;
using Ticketing.Application.Interfaces;

namespace Ticketing.Infrastructure.Services
{
    public class RabbitMQEventPublisher : IEventPublisher
    {
        public async Task PublishAsync(string eventType, string eventData)
        {
            // TODO: Implement RabbitMQ publishing when RabbitMQ.Client package is added
            // For now, just log the event
            System.Console.WriteLine($"Publishing event: {eventType} - {eventData}");
            await Task.CompletedTask;
        }
    }
}