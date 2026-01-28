using WeChooz.TechAssessment.Web.Api.Public;
using WeChooz.TechAssessment.Web.Api.Common;

namespace WeChooz.TechAssessment.Web.Services.Public;

public interface IPublicSessionsService
{
    Task<PagedResult<PublicSessionSummaryDto>> ListAsync(
        string? targetAudience,
        string? deliveryMode,
        DateOnly? startDateFrom,
        DateOnly? startDateTo,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<PublicSessionDetailDto?> GetAsync(Guid id, CancellationToken cancellationToken);
}
