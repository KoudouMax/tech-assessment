using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeChooz.TechAssessment.Web.Api.Common;
using WeChooz.TechAssessment.Web.Services.Admin;

namespace WeChooz.TechAssessment.Web.Api.Admin;

[ApiController]
[Route("_api/admin/courses")]
[Authorize(Policy = "Formation")]
public sealed class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<CourseSummaryDto>>> ListAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        var (normalizedPage, normalizedSize) = Pagination.Normalize(page, pageSize);
        var courses = await _courseService.ListAsync(normalizedPage, normalizedSize, cancellationToken);
        return Ok(courses);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseSummaryDto>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var course = await _courseService.GetAsync(id, cancellationToken);

        if (course is null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpPost]
    public async Task<ActionResult<CourseSummaryDto>> CreateAsync([FromBody] CourseCreateRequest request, CancellationToken cancellationToken)
    {
        var course = await _courseService.CreateAsync(request, cancellationToken);
        return Ok(course);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CourseSummaryDto>> UpdateAsync(Guid id, [FromBody] CourseUpdateRequest request, CancellationToken cancellationToken)
    {
        var course = await _courseService.UpdateAsync(id, request, cancellationToken);
        if (course is null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _courseService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
