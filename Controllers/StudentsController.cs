using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers;

public class StudentsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search, StudentStatus? status)
    {
        var query = db.Students.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(search) ||
                x.Email.Contains(search) || x.Department.Contains(search));
        }
        if (status.HasValue) query = query.Where(x => x.Status == status);
        ViewBag.Search = search;
        ViewBag.Status = status;
        return View(await query.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var student = await db.Students.Include(x => x.Enrollments).ThenInclude(x => x.Course).FirstOrDefaultAsync(x => x.Id == id);
        return student is null ? NotFound() : View(student);
    }

    public IActionResult Create() => View(new Student());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        ValidateDates(student);
        if (await db.Students.AnyAsync(x => x.Email == student.Email))
            ModelState.AddModelError(nameof(student.Email), "A student with this email already exists.");
        if (!ModelState.IsValid) return View(student);
        db.Add(student);
        await db.SaveChangesAsync();
        TempData["Success"] = "Student added successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var student = await db.Students.FindAsync(id);
        return student is null ? NotFound() : View(student);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Student student)
    {
        if (id != student.Id) return NotFound();
        ValidateDates(student);
        if (await db.Students.AnyAsync(x => x.Email == student.Email && x.Id != id))
            ModelState.AddModelError(nameof(student.Email), "A student with this email already exists.");
        if (!ModelState.IsValid) return View(student);
        db.Update(student);
        await db.SaveChangesAsync();
        TempData["Success"] = "Student updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var student = await db.Students.Include(x => x.Enrollments).FirstOrDefaultAsync(x => x.Id == id);
        return student is null ? NotFound() : View(student);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await db.Students.FindAsync(id);
        if (student is null) return NotFound();
        db.Remove(student);
        await db.SaveChangesAsync();
        TempData["Success"] = "Student deleted.";
        return RedirectToAction(nameof(Index));
    }

    private void ValidateDates(Student student)
    {
        if (student.DateOfBirth > DateTime.Today)
            ModelState.AddModelError(nameof(student.DateOfBirth), "Date of birth cannot be in the future.");
        if (student.EnrollmentDate > DateTime.Today)
            ModelState.AddModelError(nameof(student.EnrollmentDate), "Enrollment date cannot be in the future.");
        if (student.DateOfBirth.HasValue && student.EnrollmentDate <= student.DateOfBirth)
            ModelState.AddModelError(nameof(student.EnrollmentDate), "Enrollment must be after the date of birth.");
    }
}
