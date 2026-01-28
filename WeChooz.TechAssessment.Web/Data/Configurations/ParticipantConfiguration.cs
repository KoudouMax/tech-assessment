using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeChooz.TechAssessment.Web.Data.Configurations;

public sealed class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("Participant");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(x => x.CompanyName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasPrecision(3);

        builder.Property(x => x.UpdatedAtUtc)
            .HasPrecision(3);

        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}
