using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using WeChooz.TechAssessment.Web.Api.Admin;
using WeChooz.TechAssessment.Web.Data;
using WeChooz.TechAssessment.Web.Services.Admin;

namespace WeChooz.TechAssessment.Tests.Unit;

public class ParticipantServiceTests
{
    [Fact]
    public async Task AddToSessionReturnsNullWhenCapacityReached()
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
            MaxCapacity = 1,
            TrainerFirstName = "Alice",
            TrainerLastName = "Martin",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        var session = new TrainingSession
        {
            Id = Guid.NewGuid(),
            CourseId = course.Id,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            DeliveryMode = DeliveryMode.PRESENTIEL,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        var participant = new Participant
        {
            Id = Guid.NewGuid(),
            FirstName = "Bob",
            LastName = "One",
            Email = "bob@example.com",
            CompanyName = "Acme",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        db.Courses.Add(course);
        db.TrainingSessions.Add(session);
        db.Participants.Add(participant);
        db.SessionParticipants.Add(new SessionParticipant
        {
            TrainingSessionId = session.Id,
            ParticipantId = participant.Id,
            CreatedAtUtc = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var service = new ParticipantService(db, NullLogger<ParticipantService>.Instance);
        var request = new AddParticipantRequest("Jane", "Two", "jane@example.com", "Acme");

        var result = await service.AddToSessionAsync(session.Id, request, CancellationToken.None);

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
