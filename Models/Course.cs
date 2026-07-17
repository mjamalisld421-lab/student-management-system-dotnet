using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models;

public class Course
{
    public int Id { get; set; }

    [Required, StringLength(20), RegularExpression(@"^[A-Za-z]{2,6}[- ]?\d{2,4}$",
        ErrorMessage = "Use a code such as CS101 or CS-101.")]
    public string Code { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10.")]
    public int Credits { get; set; }

    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
