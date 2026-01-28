namespace WeChooz.TechAssessment.Web.Data;

public sealed class SessionParticipant
{
    public Guid TrainingSessionId { get; set; }
    public Guid ParticipantId { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public TrainingSession? TrainingSession { get; set; }
    public Participant? Participant { get; set; }
}
