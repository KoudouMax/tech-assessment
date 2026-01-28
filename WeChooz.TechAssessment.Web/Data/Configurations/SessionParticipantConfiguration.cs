using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeChooz.TechAssessment.Web.Data.Configurations;

public sealed class SessionParticipantConfiguration : IEntityTypeConfiguration<SessionParticipant>
{
    public void Configure(EntityTypeBuilder<SessionParticipant> builder)
    {
        builder.ToTable("SessionParticipant");

        builder.HasKey(x => new { x.TrainingSessionId, x.ParticipantId });

        builder.Property(x => x.CreatedAtUtc)
            .HasPrecision(3);

        builder.HasOne(x => x.TrainingSession)
            .WithMany(x => x.SessionParticipants)
            .HasForeignKey(x => x.TrainingSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Participant)
            .WithMany(x => x.SessionParticipants)
            .HasForeignKey(x => x.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ParticipantId);
    }
}
