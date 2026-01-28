namespace WeChooz.TechAssessment.Web.Data;

public sealed class TrainingSession
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public DateOnly StartDate { get; set; }
    public DeliveryMode DeliveryMode { get; set; }
    public string? Location { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public Course? Course { get; set; }
    public ICollection<SessionParticipant> SessionParticipants { get; set; } = new List<SessionParticipant>();
}
