using System;
using System.ComponentModel.DataAnnotations;

namespace Ticketing.Domain.Entities
{
    public class OutboxEvent
    {
        [Key]
        public Guid Id { get; set; }
        public string EventType { get; set; }
        public string EventData { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public bool IsProcessed { get; set; }
        public int RetryCount { get; set; } = 0;
        public int MaxRetries { get; set; } = 3;
        public DateTime? NextRetryAt { get; set; }
        public string LastError { get; set; }
    }
}