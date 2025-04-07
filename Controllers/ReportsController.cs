using Microsoft.AspNetCore.Mvc;
using MunicipalityManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace MunicipalityManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly MunicipalityContext _context;

        public ReportsController(MunicipalityContext context)
        {
            _context = context;
        }

        // Index Action to display all Reports
        public async Task<IActionResult> Index()
        {
            var reports = await _context.Reports.ToListAsync();
            return View(reports);
        }

        // Create Action to create a new Report
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportID,Title,Description,CitizenID")] Report report)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(report);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Report created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log exception (use a logger in production)
                    TempData["ErrorMessage"] = "An error occurred while creating the report.";
                }
            }
            return View(report);
        }

        // Edit Action to edit an existing Report
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportID,Title,Description,CitizenID")] Report report)
        {
            if (id != report.ReportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Report updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Reports.Any(r => r.ReportID == report.ReportID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "An error occurred while updating the report.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // Delete Action to delete a Report
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .FirstOrDefaultAsync(m => m.ReportID == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Report deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Report not found or already deleted.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
