using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models;

public enum Grade { A, B, C, D, F, InProgress }

public class Enrollment
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Select a student."), Display(Name = "Student")]
    public int StudentId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Select a course."), Display(Name = "Course")]
    public int CourseId { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "Enrollment date")]
    public DateTime EnrollmentDate { get; set; } = DateTime.Today;

    [Required]
    public Grade Grade { get; set; } = Grade.InProgress;

    public Student? Student { get; set; }
    public Course? Course { get; set; }
}
