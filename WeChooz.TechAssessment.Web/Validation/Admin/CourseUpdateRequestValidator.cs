using FluentValidation;
using WeChooz.TechAssessment.Web.Api.Admin;

namespace WeChooz.TechAssessment.Web.Validation.Admin;

public sealed class CourseUpdateRequestValidator : AbstractValidator<CourseUpdateRequest>
{
    public CourseUpdateRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.LongDescriptionMarkdown).NotEmpty();
        RuleFor(x => x.DurationDays).GreaterThan(0);
        RuleFor(x => x.TargetAudience).Must(a => a is "ELU" or "PRESIDENT");
        RuleFor(x => x.MaxCapacity).GreaterThan(0);
        RuleFor(x => x.TrainerFirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.TrainerLastName).NotEmpty().MaximumLength(100);
    }
}
