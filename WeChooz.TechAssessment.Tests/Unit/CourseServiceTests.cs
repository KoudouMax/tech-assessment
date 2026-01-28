using Microsoft.EntityFrameworkCore;
using WeChooz.TechAssessment.Web.Api.Admin;
using WeChooz.TechAssessment.Web.Data;
using WeChooz.TechAssessment.Web.Services.Admin;

namespace WeChooz.TechAssessment.Tests.Unit;

public class CourseServiceTests
{
    [Fact]
    public async Task CreateCoursePersistsAndReturnsDto()
    {
        var db = CreateDbContext();
        var service = new CourseService(db);

        var request = new CourseCreateRequest(
            "Test Course",
            "Short",
            "# Title",
            2,
            "ELU",
            5,
            "Alice",
            "Martin"
        );

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Test Course", result.Name);
        Assert.Equal(1, await db.Courses.CountAsync());
    }

    [Fact]
    public async Task UpdateCourseReturnsNullWhenMissing()
    {
        var db = CreateDbContext();
        var service = new CourseService(db);

        var request = new CourseUpdateRequest(
            "Updated",
            "Short",
            "# Title",
            1,
            "ELU",
            10,
            "John",
            "Doe"
        );

        var result = await service.UpdateAsync(Guid.NewGuid(), request, CancellationToken.None);

        Assert.Null(result);
    }

    private static FormationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<FormationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FormationDbContext(options);
    }
}
