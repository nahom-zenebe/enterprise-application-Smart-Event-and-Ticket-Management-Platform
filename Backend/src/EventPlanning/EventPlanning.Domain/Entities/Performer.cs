namespace EventPlanning.Domain.Entities
{
    public class Performer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public string PerformanceType { get; set; } = string.Empty;
        public Dictionary<string, string> SocialLinks { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}