using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers;

public class CoursesController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search)
    {
        var query = db.Courses.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(x => x.Title.Contains(search) || x.Code.Contains(search) || x.Department.Contains(search));
        }
        ViewBag.Search = search;
        return View(await query.OrderBy(x => x.Code).ToListAsync());
    }
    public async Task<IActionResult> Details(int id)
    {
        var item = await db.Courses.Include(x => x.Enrollments).ThenInclude(x => x.Student).FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : View(item);
    }
    public IActionResult Create() => View(new Course());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course item)
    {
        item.Code = item.Code.ToUpperInvariant().Replace(" ", "");
        await ValidateCode(item);
        if (!ModelState.IsValid) return View(item);
        db.Add(item); await db.SaveChangesAsync();
        TempData["Success"] = "Course added successfully.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id)
    {
        var item = await db.Courses.FindAsync(id);
        return item is null ? NotFound() : View(item);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Course item)
    {
        if (id != item.Id) return NotFound();
        item.Code = item.Code.ToUpperInvariant().Replace(" ", "");
        await ValidateCode(item);
        if (!ModelState.IsValid) return View(item);
        db.Update(item); await db.SaveChangesAsync();
        TempData["Success"] = "Course updated successfully.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.Courses.Include(x => x.Enrollments).FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : View(item);
    }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await db.Courses.FindAsync(id);
        if (item is null) return NotFound();
        db.Remove(item); await db.SaveChangesAsync();
        TempData["Success"] = "Course deleted.";
        return RedirectToAction(nameof(Index));
    }
    private async Task ValidateCode(Course item)
    {
        if (await db.Courses.AnyAsync(x => x.Code == item.Code && x.Id != item.Id))
            ModelState.AddModelError(nameof(item.Code), "A course with this code already exists.");
    }
}
