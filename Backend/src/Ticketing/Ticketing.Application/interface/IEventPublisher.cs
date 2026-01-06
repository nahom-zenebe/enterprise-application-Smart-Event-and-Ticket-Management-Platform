using System.Threading.Tasks;

namespace Ticketing.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(string eventType, string eventData);
    }
}