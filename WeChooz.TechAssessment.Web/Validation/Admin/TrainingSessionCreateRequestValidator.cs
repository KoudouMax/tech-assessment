using FluentValidation;
using WeChooz.TechAssessment.Web.Api.Admin;

namespace WeChooz.TechAssessment.Web.Validation.Admin;

public sealed class TrainingSessionCreateRequestValidator : AbstractValidator<TrainingSessionCreateRequest>
{
    public TrainingSessionCreateRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.DeliveryMode).Must(m => m is "PRESENTIEL" or "DISTANCIEL");
        RuleFor(x => x.StartDate).NotEmpty();
    }
}
