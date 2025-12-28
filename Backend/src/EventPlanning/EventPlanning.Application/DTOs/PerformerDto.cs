namespace EventPlanning.Application.DTOs
{
    public class PerformerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public string PerformanceType { get; set; } = string.Empty;
        public Dictionary<string, string> SocialLinks { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreatePerformerDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public string PerformanceType { get; set; } = string.Empty;
        public Dictionary<string, string> SocialLinks { get; set; } = new();
    }

    public class UpdatePerformerDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public string PerformanceType { get; set; } = string.Empty;
        public Dictionary<string, string> SocialLinks { get; set; } = new();
    }
}