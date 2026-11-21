using EntityFramework_Session03.Data;
using EntityFramework_Session03.Enums;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Session03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new AppDbContext();
            context.Database.Migrate();
            SeedData(context);

            Console.WriteLine("Seeding completed.");
        }

        static void SeedData(AppDbContext context)
        {
            if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student { FullName = "Ahmed Ali", Email = "ahmed.ali@example.com", DateOfBirth = new DateTime(2000, 5, 12) },
                    new Student { FullName = "Mona Samir", Email = "mona.samir@example.com", DateOfBirth = new DateTime(2001, 8, 23) },
                    new Student { FullName = "Youssef Adel", Email = "youssef.adel@example.com", DateOfBirth = new DateTime(1999, 1, 30) },
                    new Student { FullName = "Nour Hany", Email = "nour.hany@example.com", DateOfBirth = new DateTime(2002, 11, 4) },
                    new Student { FullName = "Khaled Magdy", Email = "khaled.magdy@example.com", DateOfBirth = new DateTime(2000, 3, 17) }
                );
                context.SaveChanges();
            }

            if (!context.Enrollments.Any())
            {
                var ids = context.Students.OrderBy(s => s.Id).Select(s => s.Id).ToList();

                context.Enrollments.AddRange(
                    new Enrollment { StudentId = ids[0], CourseId = 1, EnrolledAt = new DateTime(2024, 6, 1), Progress = 30, Status = EnrollmentStatus.Active },
                    new Enrollment { StudentId = ids[0], CourseId = 2, EnrolledAt = new DateTime(2024, 6, 5), Progress = 100, Status = EnrollmentStatus.Completed, FinalGrade = 92.5m },
                    new Enrollment { StudentId = ids[1], CourseId = 1, EnrolledAt = new DateTime(2024, 6, 10), Progress = 100, Status = EnrollmentStatus.Completed, FinalGrade = 88m },
                    new Enrollment { StudentId = ids[1], CourseId = 3, EnrolledAt = new DateTime(2024, 6, 12), Progress = 55, Status = EnrollmentStatus.Active },
                    new Enrollment { StudentId = ids[2], CourseId = 1, EnrolledAt = new DateTime(2024, 6, 15), Progress = 20, Status = EnrollmentStatus.Dropped },
                    new Enrollment { StudentId = ids[2], CourseId = 4, EnrolledAt = new DateTime(2024, 6, 18), Progress = 10, Status = EnrollmentStatus.Active },
                    new Enrollment { StudentId = ids[3], CourseId = 3, EnrolledAt = new DateTime(2024, 6, 20), Progress = 15, Status = EnrollmentStatus.Dropped },
                    new Enrollment { StudentId = ids[4], CourseId = 2, EnrolledAt = new DateTime(2024, 6, 22), Progress = 70, Status = EnrollmentStatus.Active }
                );
                context.SaveChanges();
            }
        }
    }
}