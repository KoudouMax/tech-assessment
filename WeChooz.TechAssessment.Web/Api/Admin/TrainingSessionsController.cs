using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeChooz.TechAssessment.Web.Api.Common;
using WeChooz.TechAssessment.Web.Services.Admin;

namespace WeChooz.TechAssessment.Web.Api.Admin;

[ApiController]
[Route("_api/admin/sessions")]
[Authorize(Roles = "formation,sales")]
public sealed class TrainingSessionsController : ControllerBase
{
    private readonly ITrainingSessionService _sessionService;

    public TrainingSessionsController(ITrainingSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TrainingSessionSummaryDto>>> ListAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        var (normalizedPage, normalizedSize) = Pagination.Normalize(page, pageSize);
        var sessions = await _sessionService.ListAsync(normalizedPage, normalizedSize, cancellationToken);
        return Ok(sessions);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrainingSessionSummaryDto>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var session = await _sessionService.GetAsync(id, cancellationToken);

        if (session is null)
        {
            return NotFound();
        }

        return Ok(session);
    }

    [HttpPost]
    [Authorize(Policy = "Formation")]
    public async Task<ActionResult<TrainingSessionSummaryDto>> CreateAsync([FromBody] TrainingSessionCreateRequest request, CancellationToken cancellationToken)
    {
        var created = await _sessionService.CreateAsync(request, cancellationToken);
        if (created is null)
        {
            return BadRequest("Course not found.");
        }

        return Ok(created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Formation")]
    public async Task<ActionResult<TrainingSessionSummaryDto>> UpdateAsync(Guid id, [FromBody] TrainingSessionUpdateRequest request, CancellationToken cancellationToken)
    {
        var updated = await _sessionService.UpdateAsync(id, request, cancellationToken);
        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Formation")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _sessionService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
