using WeChooz.TechAssessment.Web.Api.Admin;
using WeChooz.TechAssessment.Web.Api.Common;

namespace WeChooz.TechAssessment.Web.Services.Admin;

public interface ICourseService
{
    Task<PagedResult<CourseSummaryDto>> ListAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<CourseSummaryDto?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<CourseSummaryDto> CreateAsync(CourseCreateRequest request, CancellationToken cancellationToken);
    Task<CourseSummaryDto?> UpdateAsync(Guid id, CourseUpdateRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
