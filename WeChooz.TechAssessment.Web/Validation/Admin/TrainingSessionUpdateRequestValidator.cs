using FluentValidation;
using WeChooz.TechAssessment.Web.Api.Admin;

namespace WeChooz.TechAssessment.Web.Validation.Admin;

public sealed class TrainingSessionUpdateRequestValidator : AbstractValidator<TrainingSessionUpdateRequest>
{
    public TrainingSessionUpdateRequestValidator()
    {
        RuleFor(x => x.DeliveryMode).Must(m => m is "PRESENTIEL" or "DISTANCIEL");
        RuleFor(x => x.StartDate).NotEmpty();
    }
}
