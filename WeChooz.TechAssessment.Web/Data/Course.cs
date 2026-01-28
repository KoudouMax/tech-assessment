namespace WeChooz.TechAssessment.Web.Data;

public sealed class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public string LongDescriptionMarkdown { get; set; } = "";
    public int DurationDays { get; set; }
    public TargetAudience TargetAudience { get; set; }
    public int MaxCapacity { get; set; }
    public string TrainerFirstName { get; set; } = "";
    public string TrainerLastName { get; set; } = "";
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public ICollection<TrainingSession> Sessions { get; set; } = new List<TrainingSession>();
}
