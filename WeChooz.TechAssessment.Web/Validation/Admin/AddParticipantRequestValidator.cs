using FluentValidation;
using WeChooz.TechAssessment.Web.Api.Admin;

namespace WeChooz.TechAssessment.Web.Validation.Admin;

public sealed class AddParticipantRequestValidator : AbstractValidator<AddParticipantRequest>
{
    public AddParticipantRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(200);
    }
}
