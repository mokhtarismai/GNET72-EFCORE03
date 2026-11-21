using EntityFramework_Session03.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.Price).HasColumnType("decimal(10,2)");
        builder.Property(c => c.Level).IsRequired();

        builder.HasOne(c => c.Instructor)
               .WithMany(i => i.Courses)
               .HasForeignKey(c => c.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Category)
               .WithMany(cat => cat.Courses)
               .HasForeignKey(c => c.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
    new Course { Id = 1, Title = "ASP.NET Core Fundamentals", Description = "Learn the basics", Price = 499.99m, Level = CourseLevel.Beginner, IsPublished = true, PublishedAt = new DateTime(2024, 3, 1), InstructorId = 1, CategoryId = 3 },
    new Course { Id = 2, Title = "Advanced EF Core", Description = "Deep dive", Price = 799.99m, Level = CourseLevel.Advanced, IsPublished = true, PublishedAt = new DateTime(2024, 4, 15), InstructorId = 1, CategoryId = 3 },
    new Course { Id = 3, Title = "C# for Beginners", Description = null, Price = 299.00m, Level = CourseLevel.Beginner, IsPublished = true, PublishedAt = new DateTime(2024, 2, 10), InstructorId = 2, CategoryId = 2 },
    new Course { Id = 4, Title = "React from Scratch", Description = "Build UIs", Price = 599.00m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedAt = new DateTime(2024, 5, 20), InstructorId = 3, CategoryId = 4 },
    new Course { Id = 5, Title = "REST API Design", Description = "Coming soon", Price = 450.00m, Level = CourseLevel.Intermediate, IsPublished = false, PublishedAt = null, InstructorId = 2, CategoryId = 2 },
    new Course { Id = 6, Title = "UI/UX Basics", Description = null, Price = 350.00m, Level = CourseLevel.Beginner, IsPublished = false, PublishedAt = null, InstructorId = 3, CategoryId = 5 }
);
    }
}