using EntityFramework_Session03.Data;
using EntityFramework_Session03.Enums;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Session03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                context.Database.Migrate();
                SeedData(context);
            }

            // ===== CREATE (متعلقة عشان متتكررش الداتا) =====
            // Console.WriteLine("===== CREATE =====");
            // AddInstructor();
            // AddCourse(1, 3);
            // AddCourse(99, 3);
            // AddCourse(1, 99);
            // AddStudents();
            // Enroll(4, 1);
            // Enroll(1, 1);
            // Enroll(5, 5);
            // Enroll(99, 1);

            // ===== READ (متعلقة) =====
            // Console.WriteLine("\n===== READ =====");
            // FindCourse(1, "Advanced EF Core");
            // FindCourse(99, "Unknown Course");
            // FilterCourses(400, 800);
            // FilterCourses(5000, 9000);
            // PrintSubCategories(1);
            // PrintSubCategories(5);
            // PrintSubCategories(99);
            // PrintCourseRoster(1);
            // PrintCourseRoster(99);

            //Console.WriteLine("\n===== UPDATE =====");
            //ChangePrice(1, 549.99m);       
            //ChangePrice(99, 100m);          
            //ChangePrice(1, -5m);            
            //UpdateProgress(1, 1, 60);       
            //UpdateProgress(1, 1, 100);      
            //UpdateProgress(1, 1, 150);      
            //UpdateProgress(99, 1, 50);      
            //PublishCourse(5);               
            //PublishCourse(5);               
            //PublishCourse(99);


            //Console.WriteLine("\n===== DELETE =====");
            //Unenroll(4, 1);                    
            //Unenroll(4, 1);                    

            //DeleteCategoryUnsafe(1);           
            //DeleteCategorySafe(1);             
            //DeleteCategorySafe(99);            
            //int tempId = CreateTempCategory(); 
            //DeleteCategorySafe(tempId);        

            //DeleteCourse(4);                   
            //DeleteCourse(99);




            Console.WriteLine("\n===== BULK OPERATIONS =====");
            BulkRaisePrice(CourseLevel.Intermediate);
            BulkDeleteDroppedEnrollments();
        }

        // ===================== SEEDING =====================

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

        // ===================== CREATE =====================

        static void AddInstructor()
        {
            using var context = new AppDbContext();

            var instructor = new Instructor
            {
                FullName = "Hany Mostafa",
                Email = "hany.mostafa@learn.com",
                Bio = "Database expert",
                JoinedAt = DateTime.Now
            };

            context.Instructors.Add(instructor);
            context.SaveChanges();

            Console.WriteLine($"Instructor added. Generated Id = {instructor.Id}");
        }

        static void AddCourse(int instructorId, int categoryId)
        {
            using var context = new AppDbContext();

            if (!context.Instructors.Any(i => i.Id == instructorId))
            {
                Console.WriteLine($"Cannot add course: instructor {instructorId} does not exist.");
                return;
            }
            if (!context.Categories.Any(c => c.Id == categoryId))
            {
                Console.WriteLine($"Cannot add course: category {categoryId} does not exist.");
                return;
            }

            var course = new Course
            {
                Title = "Docker for Developers",
                Description = "Containers basics",
                Price = 650m,
                Level = CourseLevel.Intermediate,
                IsPublished = false,
                InstructorId = instructorId,
                CategoryId = categoryId
            };

            context.Courses.Add(course);
            context.SaveChanges();

            Console.WriteLine($"Course added. Generated Id = {course.Id}");
        }

        static void AddStudents()
        {
            using var context = new AppDbContext();

            var students = new List<Student>
            {
                new Student { FullName = "Laila Fawzy", Email = "laila.fawzy@example.com", DateOfBirth = new DateTime(2001, 2, 14) },
                new Student { FullName = "Tamer Salah", Email = "tamer.salah@example.com", DateOfBirth = new DateTime(1998, 9, 9) },
                new Student { FullName = "Dina Reda",   Email = "dina.reda@example.com",   DateOfBirth = new DateTime(2003, 7, 21) }
            };

            
            var emails = students.Select(s => s.Email).ToList();
            if (context.Students.Any(s => emails.Contains(s.Email)))
            {
                Console.WriteLine("Students already exist (duplicate emails), skipping AddRange.");
                return;
            }

            context.Students.AddRange(students);   
            context.SaveChanges();                 

            foreach (var s in students)
                Console.WriteLine($"Student '{s.FullName}' added. Id = {s.Id}");
        }

        static void Enroll(int studentId, int courseId)
        {
            using var context = new AppDbContext();

            var student = context.Students.Find(studentId);
            if (student == null)
            {
                Console.WriteLine($"Enrollment refused: student {studentId} not found.");
                return;
            }

            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Enrollment refused: course {courseId} not found.");
                return;
            }

            if (!course.IsPublished)
            {
                Console.WriteLine($"Enrollment refused: course '{course.Title}' is not published.");
                return;
            }

            if (context.Enrollments.Any(e => e.StudentId == studentId && e.CourseId == courseId))
            {
                Console.WriteLine($"Enrollment refused: {student.FullName} is already enrolled in '{course.Title}'.");
                return;
            }

            context.Enrollments.Add(new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrolledAt = DateTime.Now,
                Progress = 0,
                Status = EnrollmentStatus.Active
            });
            context.SaveChanges();

            Console.WriteLine($"{student.FullName} enrolled in '{course.Title}'.");
        }

        // ===================== READ =====================

        static void FindCourse(int id, string title)
        {
            using var context = new AppDbContext();

           
            var byId = context.Courses.Find(id);
            if (byId == null)
                Console.WriteLine($"Course with Id {id} not found.");
            else
                Console.WriteLine($"Find by Id: [{byId.Id}] {byId.Title} - {byId.Level} - {byId.Price:N2}");

           
            var byTitle = context.Courses.FirstOrDefault(c => c.Title == title);
            if (byTitle == null)
                Console.WriteLine($"Course with title '{title}' not found.");
            else
                Console.WriteLine($"FirstOrDefault by Title: [{byTitle.Id}] {byTitle.Title} - {byTitle.Level} - {byTitle.Price:N2}");
        }

        static void FilterCourses(decimal minPrice, decimal maxPrice)
        {
            using var context = new AppDbContext();

            var courses = context.Courses
                .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
                .OrderByDescending(c => c.Price)
                .Select(c => new { c.Title, c.Level, c.Price })   
                .ToList();

            Console.WriteLine($"Courses priced between {minPrice} and {maxPrice}:");
            if (courses.Count == 0)
            {
                Console.WriteLine("  No courses found in this range.");
                return;
            }

            foreach (var c in courses)
                Console.WriteLine($"  {c.Title} | {c.Level} | {c.Price:N2}");
        }

        static void PrintSubCategories(int categoryId)
        {
            using var context = new AppDbContext();

            var category = context.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine($"Category {categoryId} not found.");
            }
            else
            {
                var subs = context.Categories
                    .Where(c => c.ParentCategoryId == categoryId)
                    .Select(c => c.Name)
                    .ToList();

                Console.WriteLine($"Direct sub-categories of '{category.Name}':");
                if (subs.Count == 0)
                    Console.WriteLine("  (none)");
                foreach (var name in subs)
                    Console.WriteLine($"  - {name}");
            }

            var topLevel = context.Categories
                .Where(c => c.ParentCategoryId == null)
                .Select(c => c.Name)
                .ToList();

            Console.WriteLine("Top-level categories:");
            foreach (var name in topLevel)
                Console.WriteLine($"  - {name}");
        }

        static void PrintCourseRoster(int courseId)
        {
            using var context = new AppDbContext();

            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Course {courseId} not found.");
                return;
            }

           
            var studentIds = context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.StudentId)
                .ToList();

            Console.WriteLine($"Course '{course.Title}' has {studentIds.Count} enrolled student(s):");

            
            var names = context.Students
                .Where(s => studentIds.Contains(s.Id))
                .Select(s => s.FullName)
                .ToList();

            foreach (var name in names)
                Console.WriteLine($"  - {name}");
        }

        // ===================== UPDATE =====================

        static void ChangePrice(int courseId, decimal newPrice)
        {
            using var context = new AppDbContext();

            if (newPrice <= 0)
            {
                Console.WriteLine($"Price change refused: {newPrice} is not a valid price.");
                return;
            }

            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Price change refused: course {courseId} not found.");
                return;
            }

            decimal oldPrice = course.Price;
            course.Price = newPrice;      
            context.SaveChanges();        

            Console.WriteLine($"Price of '{course.Title}' changed from {oldPrice:N2} to {newPrice:N2}.");
        }

        static void UpdateProgress(int studentId, int courseId, int newProgress)
        {
            using var context = new AppDbContext();

            if (newProgress < 0 || newProgress > 100)
            {
                Console.WriteLine($"Progress update refused: {newProgress} is outside the range 0-100.");
                return;
            }

           
            var enrollment = context.Enrollments.Find(studentId, courseId);
            if (enrollment == null)
            {
                Console.WriteLine($"Progress update refused: no enrollment for student {studentId} in course {courseId}.");
                return;
            }

            enrollment.Progress = newProgress;

            if (newProgress == 100)
                enrollment.Status = EnrollmentStatus.Completed;

            context.SaveChanges();

            Console.WriteLine($"Enrollment (student {studentId}, course {courseId}): progress = {enrollment.Progress}, status = {enrollment.Status}.");
        }

        static void PublishCourse(int courseId)
        {
            using var context = new AppDbContext();

            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Publish refused: course {courseId} not found.");
                return;
            }

            if (course.IsPublished)
            {
                Console.WriteLine($"Publish refused: '{course.Title}' is already published.");
                return;
            }

            course.IsPublished = true;
            course.PublishedAt = DateTime.Now;
            context.SaveChanges();

            Console.WriteLine($"Course '{course.Title}' published at {course.PublishedAt:yyyy-MM-dd HH:mm}.");
        }

        // ===================== DELETE =====================

        static void Unenroll(int studentId, int courseId)
        {
            using var context = new AppDbContext();

            var enrollment = context.Enrollments.Find(studentId, courseId);
            if (enrollment == null)
            {
                Console.WriteLine($"Unenroll refused: no enrollment for student {studentId} in course {courseId}.");
                return;
            }

            context.Enrollments.Remove(enrollment);
            context.SaveChanges();

            Console.WriteLine($"Student {studentId} unenrolled from course {courseId}.");
        }

       
        static void DeleteCategoryUnsafe(int categoryId)
        {
            using var context = new AppDbContext();

            var category = context.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine($"Category {categoryId} not found.");
                return;
            }

            try
            {
                context.Categories.Remove(category);
                context.SaveChanges();
                Console.WriteLine($"Category '{category.Name}' deleted.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"[Unsafe delete failed] {ex.Message}");
                Console.WriteLine($"[Inner] {ex.InnerException?.Message}");
            }
        }

       
        static void DeleteCategorySafe(int categoryId)
        {
            using var context = new AppDbContext();

            var category = context.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine($"Category {categoryId} not found.");
                return;
            }

            if (context.Categories.Any(c => c.ParentCategoryId == categoryId))
            {
                Console.WriteLine($"Delete refused: '{category.Name}' still has sub-categories.");
                return;
            }

            if (context.Courses.Any(c => c.CategoryId == categoryId))
            {
                Console.WriteLine($"Delete refused: '{category.Name}' still has courses.");
                return;
            }

            context.Categories.Remove(category);
            context.SaveChanges();

            Console.WriteLine($"Category '{category.Name}' deleted safely.");
        }

       
        static int CreateTempCategory()
        {
            using var context = new AppDbContext();

            var category = new Category
            {
                Name = "Temp Category",
                Description = "Used to demo safe delete",
                ParentCategoryId = null
            };

            context.Categories.Add(category);
            context.SaveChanges();

            Console.WriteLine($"Temp category created. Id = {category.Id}");
            return category.Id;
        }

        static void DeleteCourse(int courseId)
        {
            using var context = new AppDbContext();

            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Delete refused: course {courseId} not found.");
                return;
            }

            int before = context.Enrollments.Count(e => e.CourseId == courseId);
            Console.WriteLine($"Course '{course.Title}' has {before} enrollment(s) before delete.");

            context.Courses.Remove(course);
            context.SaveChanges();

            int after = context.Enrollments.Count(e => e.CourseId == courseId);
            Console.WriteLine($"Course deleted. Enrollments after delete: {after}.");

            
        }
        // ===================== BULK OPERATIONS =====================

        static void BulkRaisePrice(CourseLevel level)
        {
            using var context = new AppDbContext();

            int affectedRows = context.Courses
                .Where(c => c.IsPublished && c.Level == level)
                .ExecuteUpdate(setters => setters
                    .SetProperty(c => c.Price, c => c.Price * 1.10m));

            Console.WriteLine($"Bulk price raise (+10%) for published '{level}' courses: {affectedRows} row(s) affected.");
        }

        static void BulkDeleteDroppedEnrollments()
        {
            using var context = new AppDbContext();

            int affectedRows = context.Enrollments
                .Where(e => e.Status == EnrollmentStatus.Dropped)
                .ExecuteDelete();

            Console.WriteLine($"Bulk delete of 'Dropped' enrollments: {affectedRows} row(s) affected.");
        }
    }
}