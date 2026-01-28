using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeChooz.TechAssessment.Web.Data.Configurations;

public sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Course");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.ShortDescription)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.LongDescriptionMarkdown)
            .IsRequired();

        builder.Property(x => x.DurationDays)
            .IsRequired();

        builder.Property(x => x.TargetAudience)
            .HasMaxLength(20)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.MaxCapacity)
            .IsRequired();

        builder.Property(x => x.TrainerFirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.TrainerLastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasPrecision(3);

        builder.Property(x => x.UpdatedAtUtc)
            .HasPrecision(3);

        builder.HasIndex(x => x.TargetAudience);
    }
}
