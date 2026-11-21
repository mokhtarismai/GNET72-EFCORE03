using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => new { e.StudentId, e.CourseId });

        builder.Property(e => e.EnrolledAt).IsRequired();
        builder.Property(e => e.Progress).IsRequired();
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.FinalGrade).HasColumnType("decimal(5,2)");

        builder.HasOne(e => e.Student)
               .WithMany(s => s.Enrollments)
               .HasForeignKey(e => e.StudentId);

        builder.HasOne(e => e.Course)
               .WithMany(c => c.Enrollments)
               .HasForeignKey(e => e.CourseId);
    }
}