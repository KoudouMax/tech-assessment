namespace WeChooz.TechAssessment.Web.Api.Admin;

public sealed record CourseSummaryDto(
    Guid Id,
    string Name,
    string ShortDescription,
    string LongDescriptionMarkdown,
    int DurationDays,
    string TargetAudience,
    int MaxCapacity,
    string TrainerFirstName,
    string TrainerLastName
);

public sealed record CourseCreateRequest(
    string Name,
    string ShortDescription,
    string LongDescriptionMarkdown,
    int DurationDays,
    string TargetAudience,
    int MaxCapacity,
    string TrainerFirstName,
    string TrainerLastName
);

public sealed record CourseUpdateRequest(
    string Name,
    string ShortDescription,
    string LongDescriptionMarkdown,
    int DurationDays,
    string TargetAudience,
    int MaxCapacity,
    string TrainerFirstName,
    string TrainerLastName
);

public sealed record TrainingSessionSummaryDto(
    Guid Id,
    Guid CourseId,
    string CourseName,
    DateOnly StartDate,
    string DeliveryMode,
    string? Location,
    int ParticipantsCount
);

public sealed record TrainingSessionCreateRequest(
    Guid CourseId,
    DateOnly StartDate,
    string DeliveryMode,
    string? Location
);

public sealed record TrainingSessionUpdateRequest(
    DateOnly StartDate,
    string DeliveryMode,
    string? Location
);

public sealed record ParticipantSummaryDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string CompanyName
);

public sealed record AddParticipantRequest(
    string FirstName,
    string LastName,
    string Email,
    string CompanyName
);
