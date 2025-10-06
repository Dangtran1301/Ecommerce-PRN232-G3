namespace CatalogService.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public Guid? AggregateId { get; set; }
        public string? AggregateType { get; set; }
        public string EventType { get; set; } = null!;
        public string Payload { get; set; } = null!;
        public string? Metadata { get; set; }
        public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedOn { get; set; }
        public string Status { get; set; } = "Pending";
    }
}