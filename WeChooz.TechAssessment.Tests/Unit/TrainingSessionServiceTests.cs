using Microsoft.EntityFrameworkCore;
using WeChooz.TechAssessment.Web.Api.Admin;
using WeChooz.TechAssessment.Web.Data;
using WeChooz.TechAssessment.Web.Services.Admin;

namespace WeChooz.TechAssessment.Tests.Unit;

public class TrainingSessionServiceTests
{
    [Fact]
    public async Task CreateSessionReturnsNullWhenCourseMissing()
    {
        var db = CreateDbContext();
        var service = new TrainingSessionService(db);

        var request = new TrainingSessionCreateRequest(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.UtcNow), "PRESENTIEL", "Paris");

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateSessionPersistsWhenCourseExists()
    {
        var db = CreateDbContext();
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Course",
            ShortDescription = "Short",
            LongDescriptionMarkdown = "# Title",
            DurationDays = 1,
            TargetAudience = TargetAudience.ELU,
            MaxCapacity = 5,
            TrainerFirstName = "Alice",
            TrainerLastName = "Martin",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        db.Courses.Add(course);
        await db.SaveChangesAsync();

        var service = new TrainingSessionService(db);
        var request = new TrainingSessionCreateRequest(course.Id, DateOnly.FromDateTime(DateTime.UtcNow), "PRESENTIEL", "Paris");

        var result = await service.CreateAsync(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(course.Id, result!.CourseId);
        Assert.Equal(1, await db.TrainingSessions.CountAsync());
    }

    private static FormationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<FormationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FormationDbContext(options);
    }
}
