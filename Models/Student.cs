using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models;

public enum StudentStatus { Active, Graduated, Suspended, Withdrawn }

public class Student
{
    public int Id { get; set; }

    [Required, StringLength(50), Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(50), Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(30)]
    public string? Phone { get; set; }

    [DataType(DataType.Date), Display(Name = "Date of birth")]
    public DateTime? DateOfBirth { get; set; }

    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required, DataType(DataType.Date), Display(Name = "Enrollment date")]
    public DateTime EnrollmentDate { get; set; } = DateTime.Today;

    [Required]
    public StudentStatus Status { get; set; } = StudentStatus.Active;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [Display(Name = "Student")]
    public string FullName => $"{FirstName} {LastName}";
}
