using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers;

public class EnrollmentsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
        => View(await db.Enrollments.AsNoTracking().Include(x => x.Student).Include(x => x.Course)
            .OrderByDescending(x => x.EnrollmentDate).ToListAsync());

    public async Task<IActionResult> Create(int? studentId)
    {
        await PopulateLists(studentId, null);
        return View(new Enrollment { StudentId = studentId ?? 0 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Enrollment item)
    {
        await Validate(item);
        if (!ModelState.IsValid) { await PopulateLists(item.StudentId, item.CourseId); return View(item); }
        db.Add(item); await db.SaveChangesAsync();
        TempData["Success"] = "Enrollment created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await db.Enrollments.FindAsync(id);
        if (item is null) return NotFound();
        await PopulateLists(item.StudentId, item.CourseId);
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Enrollment item)
    {
        if (id != item.Id) return NotFound();
        await Validate(item);
        if (!ModelState.IsValid) { await PopulateLists(item.StudentId, item.CourseId); return View(item); }
        db.Update(item); await db.SaveChangesAsync();
        TempData["Success"] = "Enrollment updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.Enrollments.Include(x => x.Student).Include(x => x.Course).FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await db.Enrollments.FindAsync(id);
        if (item is null) return NotFound();
        db.Remove(item); await db.SaveChangesAsync();
        TempData["Success"] = "Enrollment deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateLists(int? studentId, int? courseId)
    {
        ViewBag.Students = new SelectList(await db.Students.OrderBy(x => x.LastName).ToListAsync(), "Id", "FullName", studentId);
        ViewBag.Courses = new SelectList(await db.Courses.OrderBy(x => x.Code).ToListAsync(), "Id", "Title", courseId);
    }
    private async Task Validate(Enrollment item)
    {
        if (item.EnrollmentDate > DateTime.Today)
            ModelState.AddModelError(nameof(item.EnrollmentDate), "Enrollment date cannot be in the future.");
        if (await db.Enrollments.AnyAsync(x => x.StudentId == item.StudentId && x.CourseId == item.CourseId && x.Id != item.Id))
            ModelState.AddModelError(string.Empty, "This student is already enrolled in the selected course.");
    }
}
