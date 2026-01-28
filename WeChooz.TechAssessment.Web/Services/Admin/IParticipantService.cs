using WeChooz.TechAssessment.Web.Api.Admin;
using WeChooz.TechAssessment.Web.Api.Common;

namespace WeChooz.TechAssessment.Web.Services.Admin;

public interface IParticipantService
{
    Task<PagedResult<ParticipantSummaryDto>?> ListBySessionAsync(Guid sessionId, int page, int pageSize, CancellationToken cancellationToken);
    Task<ParticipantSummaryDto?> AddToSessionAsync(Guid sessionId, AddParticipantRequest request, CancellationToken cancellationToken);
    Task<bool> RemoveFromSessionAsync(Guid sessionId, Guid participantId, CancellationToken cancellationToken);
}
