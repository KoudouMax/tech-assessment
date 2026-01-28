namespace WeChooz.TechAssessment.Web.Data;

public sealed class Participant
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public ICollection<SessionParticipant> SessionParticipants { get; set; } = new List<SessionParticipant>();
}
