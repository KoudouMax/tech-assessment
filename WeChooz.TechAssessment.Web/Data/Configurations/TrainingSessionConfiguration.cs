using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeChooz.TechAssessment.Web.Data.Configurations;

public sealed class TrainingSessionConfiguration : IEntityTypeConfiguration<TrainingSession>
{
    public void Configure(EntityTypeBuilder<TrainingSession> builder)
    {
        builder.ToTable("TrainingSession");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StartDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.DeliveryMode)
            .HasMaxLength(20)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Location)
            .HasMaxLength(200);

        builder.Property(x => x.CreatedAtUtc)
            .HasPrecision(3);

        builder.Property(x => x.UpdatedAtUtc)
            .HasPrecision(3);

        builder.HasOne(x => x.Course)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.CourseId);
        builder.HasIndex(x => x.StartDate);
    }
}
