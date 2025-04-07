using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MunicipalityManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MunicipalityManagementSystem.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly MunicipalityContext _context;

        public ServiceRequestsController(MunicipalityContext context)
        {
            _context = context;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            ViewData["ActiveTab"] = "ServiceRequests"; // Set the active tab to Service Requests
            var serviceRequests = await _context.ServiceRequests.Include(s => s.Citizen).ToListAsync();
            return View(serviceRequests);
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(s => s.Citizen)
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Create
        public IActionResult Create()
        {
            ViewBag.Citizens = new SelectList(_context.Citizens, "CitizenID", "FullName");
            return View();
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestID,CitizenID,ServiceType,RequestDate,Status")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();

                // Flag the associated Report as used
                var report = await _context.Reports
                    .FirstOrDefaultAsync(r => r.CitizenID == serviceRequest.CitizenID && r.IsFlagged == false);

                if (report != null)
                {
                    report.IsFlagged = true;
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Service request created successfully.";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate citizens for the dropdown
            ViewBag.Citizens = new SelectList(_context.Citizens, "CitizenID", "FullName", serviceRequest.CitizenID);
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            ViewBag.Citizens = new SelectList(_context.Citizens, "CitizenID", "FullName", serviceRequest.CitizenID);
            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestID,CitizenID,ServiceType,RequestDate,Status")] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.RequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestExists(serviceRequest.RequestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["SuccessMessage"] = "Service request updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate citizens for the dropdown
            ViewBag.Citizens = new SelectList(_context.Citizens, "CitizenID", "FullName", serviceRequest.CitizenID);
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(s => s.Citizen)
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            _context.ServiceRequests.Remove(serviceRequest);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Service request deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequests.Any(e => e.RequestID == id);
        }
    }
}
