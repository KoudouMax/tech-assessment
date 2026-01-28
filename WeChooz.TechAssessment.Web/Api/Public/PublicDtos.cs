namespace WeChooz.TechAssessment.Web.Api.Public;

public sealed record PublicSessionSummaryDto(
    Guid Id,
    string CourseName,
    string ShortDescription,
    string TargetAudience,
    DateOnly StartDate,
    int DurationDays,
    string DeliveryMode,
    int RemainingSeats,
    string TrainerFirstName,
    string TrainerLastName
);

public sealed record PublicSessionDetailDto(
    Guid Id,
    string CourseName,
    string ShortDescription,
    string TargetAudience,
    DateOnly StartDate,
    int DurationDays,
    string DeliveryMode,
    int RemainingSeats,
    string TrainerFirstName,
    string TrainerLastName,
    string LongDescriptionMarkdown,
    string LongDescriptionHtml
);
