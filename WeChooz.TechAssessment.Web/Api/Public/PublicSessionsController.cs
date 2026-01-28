using Microsoft.AspNetCore.Mvc;
using WeChooz.TechAssessment.Web.Api.Common;
using WeChooz.TechAssessment.Web.Services.Public;

namespace WeChooz.TechAssessment.Web.Api.Public;

[ApiController]
[Route("_api/public/sessions")]
public sealed class PublicSessionsController : ControllerBase
{
    private readonly IPublicSessionsService _service;

    public PublicSessionsController(IPublicSessionsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<PublicSessionSummaryDto>>> ListAsync(
        [FromQuery] string? targetAudience,
        [FromQuery] string? deliveryMode,
        [FromQuery] DateOnly? startDateFrom,
        [FromQuery] DateOnly? startDateTo,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        var (normalizedPage, normalizedSize) = Pagination.Normalize(page, pageSize);
        var sessions = await _service.ListAsync(
            targetAudience,
            deliveryMode,
            startDateFrom,
            startDateTo,
            normalizedPage,
            normalizedSize,
            cancellationToken);
        return Ok(sessions);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PublicSessionDetailDto>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var session = await _service.GetAsync(id, cancellationToken);
        return session is null ? NotFound() : Ok(session);
    }
}
