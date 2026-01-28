using Microsoft.EntityFrameworkCore;

namespace WeChooz.TechAssessment.Web.Data;

public sealed class FormationDbContext : DbContext
{
    public FormationDbContext(DbContextOptions<FormationDbContext> options) : base(options) { }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<TrainingSession> TrainingSessions => Set<TrainingSession>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<SessionParticipant> SessionParticipants => Set<SessionParticipant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FormationDbContext).Assembly);
    }
}
