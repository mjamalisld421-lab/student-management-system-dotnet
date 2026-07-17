using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels;

public class DashboardViewModel
{
    public int TotalStudents { get; set; }
    public int TotalCourses { get; set; }
    public int TotalEnrollments { get; set; }
    public double? AverageGrade { get; set; }
    public IReadOnlyList<Enrollment> RecentEnrollments { get; set; } = [];
}
