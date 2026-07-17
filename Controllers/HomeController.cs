using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Controllers;

public class HomeController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var graded = await db.Enrollments.Where(x => x.Grade != Grade.InProgress).Select(x => x.Grade).ToListAsync();
        var points = new Dictionary<Grade, int> { [Grade.A] = 4, [Grade.B] = 3, [Grade.C] = 2, [Grade.D] = 1, [Grade.F] = 0 };
        return View(new DashboardViewModel
        {
            TotalStudents = await db.Students.CountAsync(),
            TotalCourses = await db.Courses.CountAsync(),
            TotalEnrollments = await db.Enrollments.CountAsync(),
            AverageGrade = graded.Count == 0 ? null : graded.Average(x => points[x]),
            RecentEnrollments = await db.Enrollments.Include(x => x.Student).Include(x => x.Course)
                .OrderByDescending(x => x.EnrollmentDate).ThenByDescending(x => x.Id).Take(5).ToListAsync()
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    });
}
