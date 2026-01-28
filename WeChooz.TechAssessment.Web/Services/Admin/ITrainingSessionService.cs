using WeChooz.TechAssessment.Web.Api.Admin;
using WeChooz.TechAssessment.Web.Api.Common;

namespace WeChooz.TechAssessment.Web.Services.Admin;

public interface ITrainingSessionService
{
    Task<PagedResult<TrainingSessionSummaryDto>> ListAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<TrainingSessionSummaryDto?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<TrainingSessionSummaryDto?> CreateAsync(TrainingSessionCreateRequest request, CancellationToken cancellationToken);
    Task<TrainingSessionSummaryDto?> UpdateAsync(Guid id, TrainingSessionUpdateRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
