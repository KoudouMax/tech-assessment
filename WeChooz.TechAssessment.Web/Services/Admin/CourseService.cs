using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WeChooz.TechAssessment.Web.Api.Admin;
using WeChooz.TechAssessment.Web.Api.Common;
using WeChooz.TechAssessment.Web.Data;

namespace WeChooz.TechAssessment.Web.Services.Admin;

public sealed class CourseService : ICourseService
{
    private readonly FormationDbContext _dbContext;

    public CourseService(FormationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<CourseSummaryDto>> ListAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _dbContext.Courses.AsNoTracking();
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(MapCourseExpr)
            .ToListAsync(cancellationToken);

        return new PagedResult<CourseSummaryDto>(items, totalCount, page, pageSize);
    }

    public async Task<CourseSummaryDto?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Courses
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(MapCourseExpr)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<CourseSummaryDto> CreateAsync(CourseCreateRequest request, CancellationToken cancellationToken)
    {
        var course = new Course
        {
            Name = request.Name,
            ShortDescription = request.ShortDescription,
            LongDescriptionMarkdown = request.LongDescriptionMarkdown,
            DurationDays = request.DurationDays,
            TargetAudience = Enum.Parse<TargetAudience>(request.TargetAudience, true),
            MaxCapacity = request.MaxCapacity,
            TrainerFirstName = request.TrainerFirstName,
            TrainerLastName = request.TrainerLastName,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapCourse(course);
    }

    public async Task<CourseSummaryDto?> UpdateAsync(Guid id, CourseUpdateRequest request, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (course is null)
        {
            return null;
        }

        course.Name = request.Name;
        course.ShortDescription = request.ShortDescription;
        course.LongDescriptionMarkdown = request.LongDescriptionMarkdown;
        course.DurationDays = request.DurationDays;
        course.TargetAudience = Enum.Parse<TargetAudience>(request.TargetAudience, true);
        course.MaxCapacity = request.MaxCapacity;
        course.TrainerFirstName = request.TrainerFirstName;
        course.TrainerLastName = request.TrainerLastName;
        course.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapCourse(course);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (course is null)
        {
            return false;
        }

        _dbContext.Courses.Remove(course);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static readonly Expression<Func<Course, CourseSummaryDto>> MapCourseExpr =
        course => new CourseSummaryDto(
            course.Id,
            course.Name,
            course.ShortDescription,
            course.LongDescriptionMarkdown,
            course.DurationDays,
            course.TargetAudience.ToString(),
            course.MaxCapacity,
            course.TrainerFirstName,
            course.TrainerLastName);

    private static CourseSummaryDto MapCourse(Course course)
        => new(
            course.Id,
            course.Name,
            course.ShortDescription,
            course.LongDescriptionMarkdown,
            course.DurationDays,
            course.TargetAudience.ToString(),
            course.MaxCapacity,
            course.TrainerFirstName,
            course.TrainerLastName);
}
