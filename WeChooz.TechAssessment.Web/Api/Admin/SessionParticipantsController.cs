using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeChooz.TechAssessment.Web.Api.Common;
using WeChooz.TechAssessment.Web.Services.Admin;

namespace WeChooz.TechAssessment.Web.Api.Admin;

[ApiController]
[Route("_api/admin/sessions/{sessionId:guid}/participants")]
[Authorize(Roles = "sales,formation")]
public sealed class SessionParticipantsController : ControllerBase
{
    private readonly IParticipantService _participantService;

    public SessionParticipantsController(IParticipantService participantService)
    {
        _participantService = participantService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ParticipantSummaryDto>>> ListAsync(
        Guid sessionId,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        var (normalizedPage, normalizedSize) = Pagination.Normalize(page, pageSize);
        var participants = await _participantService.ListBySessionAsync(sessionId, normalizedPage, normalizedSize, cancellationToken);
        if (participants is null)
        {
            return NotFound();
        }

        return Ok(participants);
    }

    [HttpPost]
    public async Task<ActionResult<ParticipantSummaryDto>> AddAsync(Guid sessionId, [FromBody] AddParticipantRequest request, CancellationToken cancellationToken)
    {
        var participant = await _participantService.AddToSessionAsync(sessionId, request, cancellationToken);
        if (participant is null)
        {
            return BadRequest("Session not found or capacity reached.");
        }

        return Ok(participant);
    }

    [HttpDelete("{participantId:guid}")]
    public async Task<IActionResult> RemoveAsync(Guid sessionId, Guid participantId, CancellationToken cancellationToken)
    {
        var deleted = await _participantService.RemoveFromSessionAsync(sessionId, participantId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
