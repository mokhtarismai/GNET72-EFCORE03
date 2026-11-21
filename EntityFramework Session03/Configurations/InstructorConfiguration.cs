using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FullName).IsRequired().HasMaxLength(100);
        builder.Property(i => i.Email).IsRequired().HasMaxLength(150);
        builder.Property(i => i.Bio).HasMaxLength(500);
        builder.Property(i => i.JoinedAt).IsRequired();

        builder.HasData(
    new Instructor { Id = 1, FullName = "Mokhtar Ismail", Email = "mokhtar.ismail@learn.com", Bio = "Senior .NET developer", JoinedAt = new DateTime(2023, 1, 10) },
    new Instructor { Id = 2, FullName = "Nour Rashed", Email = "nour.rashed@learn.com", Bio = null, JoinedAt = new DateTime(2023, 6, 5) },
    new Instructor { Id = 3, FullName = "Omar Khaled", Email = "omar.khaled@learn.com", Bio = "Frontend specialist", JoinedAt = new DateTime(2024, 2, 20) }
);
    }
}