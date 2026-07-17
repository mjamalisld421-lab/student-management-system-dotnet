using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Students.AnyAsync()) return;

        var students = new[]
        {
            new Student { FirstName = "Aisha", LastName = "Khan", Email = "aisha.khan@example.com", Phone = "+1 555 0101", DateOfBirth = new(2002, 4, 18), Department = "Computer Science", EnrollmentDate = new(2023, 9, 1), Status = StudentStatus.Active },
            new Student { FirstName = "Daniel", LastName = "Reed", Email = "daniel.reed@example.com", Phone = "+1 555 0102", DateOfBirth = new(2001, 11, 3), Department = "Software Engineering", EnrollmentDate = new(2022, 9, 1), Status = StudentStatus.Active },
            new Student { FirstName = "Sofia", LastName = "Martinez", Email = "sofia.martinez@example.com", DateOfBirth = new(2000, 7, 22), Department = "Information Systems", EnrollmentDate = new(2020, 9, 1), Status = StudentStatus.Graduated }
        };
        var courses = new[]
        {
            new Course { Code = "CS101", Title = "Introduction to Programming", Description = "Programming fundamentals using C#.", Credits = 4, Department = "Computer Science" },
            new Course { Code = "SE220", Title = "Software Design", Description = "Maintainable design, testing, and team practices.", Credits = 3, Department = "Software Engineering" },
            new Course { Code = "IS310", Title = "Database Systems", Description = "Relational modeling, SQL, and data integrity.", Credits = 3, Department = "Information Systems" }
        };
        db.AddRange(students);
        db.AddRange(courses);
        await db.SaveChangesAsync();
        db.Enrollments.AddRange(
            new Enrollment { StudentId = students[0].Id, CourseId = courses[0].Id, EnrollmentDate = new(2023, 9, 1), Grade = Grade.A },
            new Enrollment { StudentId = students[0].Id, CourseId = courses[1].Id, EnrollmentDate = new(2024, 1, 15), Grade = Grade.InProgress },
            new Enrollment { StudentId = students[1].Id, CourseId = courses[1].Id, EnrollmentDate = new(2023, 9, 1), Grade = Grade.B },
            new Enrollment { StudentId = students[2].Id, CourseId = courses[2].Id, EnrollmentDate = new(2021, 1, 12), Grade = Grade.A });
        await db.SaveChangesAsync();
    }
}
