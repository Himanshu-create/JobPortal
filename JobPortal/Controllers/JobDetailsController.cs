using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobPortal.Areas.Identity.Data;
using JobPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal.Controllers
{
    [Authorize]
    public class JobDetailsController : Controller
    {
        private readonly JobPortalContext _context;
        private readonly UserManager<JobPortalUser> _userManager;

        public JobDetailsController(JobPortalContext context,UserManager<JobPortalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: JobDetails
        public async Task<IActionResult> Index()
        {

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var userID = await _userManager.GetUserAsync(User);
            if(userID.Email == "gdp@presidio.com")
              return _context.JobDetails != null ? 
                          View(await _context.JobDetails.ToListAsync()) :
                          Problem("Entity set 'JobPortalContext.JobDetails'  is null.");
            return Redirect("Identity/Account/AccessDenied");
        }

        // GET: JobDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var data = await _context.ApplicantData
                        .Where(x => x.JobApplied.Id == id)
                        .Include(s => s.JobApplied)
                        .Include(s => s.user)
                        .ToListAsync(); 
            if (id == null || _context.ApplicantData == null)
            {
                return NotFound();
            }

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        // GET: JobDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,jobName,Domain,LastDateOfRegistration")] JobDetails jobDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobDetails);
        }

        // GET: JobDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JobDetails == null)
            {
                return NotFound();
            }

            var jobDetails = await _context.JobDetails.FindAsync(id);
            if (jobDetails == null)
            {
                return NotFound();
            }
            return View(jobDetails);
        }

        // POST: JobDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,jobName,Domain,LastDateOfRegistration")] JobDetails jobDetails)
        {
            if (id != jobDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobDetailsExists(jobDetails.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jobDetails);
        }

        // GET: JobDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JobDetails == null)
            {
                return NotFound();
            }

            var jobDetails = await _context.JobDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobDetails == null)
            {
                return NotFound();
            }

            return View(jobDetails);
        }

        // POST: JobDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.JobDetails == null)
            {
                return Problem("Entity set 'JobPortalContext.JobDetails'  is null.");
            }
            var jobDetails = await _context.JobDetails.FindAsync(id);
            if (jobDetails != null)
            {
                _context.JobDetails.Remove(jobDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobDetailsExists(int id)
        {
          return (_context.JobDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
