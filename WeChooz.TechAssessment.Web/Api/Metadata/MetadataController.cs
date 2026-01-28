using Microsoft.AspNetCore.Mvc;
using WeChooz.TechAssessment.Web.Data;

namespace WeChooz.TechAssessment.Web.Api.Metadata;

[ApiController]
[Route("_api/metadata")]
public sealed class MetadataController : ControllerBase
{
    [HttpGet("audiences")]
    public ActionResult<IReadOnlyList<string>> GetAudiences()
        => Ok(Enum.GetNames<TargetAudience>());

    [HttpGet("delivery-modes")]
    public ActionResult<IReadOnlyList<string>> GetDeliveryModes()
        => Ok(Enum.GetNames<DeliveryMode>());
}
