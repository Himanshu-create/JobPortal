using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobPortal.Areas.Identity.Data;
using JobPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace JobPortal.Controllers
{
    [Authorize]
    public class ApplicantDatasController : Controller
    {
        private readonly JobPortalContext _context;
        private UserManager<JobPortalUser> _userManager;

        public ApplicantDatasController(JobPortalContext context, UserManager<JobPortalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ApplicantDatas
        public async Task<IActionResult> Index()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var userID = await _userManager.GetUserAsync(User);
            return _context.ApplicantData != null ?
                        View(await _context.ApplicantData
                        .Where(u => u.user == userID)
                        .Include(s=> s.JobApplied)
                        .Include(s=> s.user)
                        .ToListAsync()) :
                        Problem("Entity set 'JobPortalContext.ApplicantData'  is null.");
        }

        public async Task<IActionResult> JobList()
        {
            return _context.JobDetails != null ?
                        View(await _context.JobDetails.ToListAsync()) :
                        Problem("Entity set 'JobPortalContext.JobDetails'  is null.");
        }
        // GET: ApplicantDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ApplicantData == null)
            {
                return NotFound();
            }

            var applicantData = await _context.ApplicantData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantData == null)
            {
                return NotFound();
            }

            return View(applicantData);
        }

        // GET: ApplicantDatas/Create
        public IActionResult Create(int id)
        {

            TempData["jobAppliedId"] = id;
            return View();
        }

        // POST: ApplicantDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("PreviousExperience")] ApplicantData applicantData)
        {
            // get user info
            int jobId = (int)TempData["jobAppliedId"];
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var userID = await _userManager.GetUserAsync(User);
            //// get job info
            var jobApplied = _context.JobDetails.Where(data => data.Id == jobId).FirstOrDefault();
            applicantData.JobApplied = jobApplied;
            applicantData.user = userID;
            _context.Add(applicantData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ApplicantDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApplicantData == null)
            {
                return NotFound();
            }

            var applicantData = await _context.ApplicantData.FindAsync(id);
            if (applicantData == null)
            {
                return NotFound();
            }
            return View(applicantData);
        }

        // POST: ApplicantDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PreviousExperience")] ApplicantData applicantData)
        {
            if (id != applicantData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicantData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantDataExists(applicantData.Id))
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
            return View(applicantData);
        }

        // GET: ApplicantDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ApplicantData == null)
            {
                return NotFound();
            }

            var applicantData = await _context.ApplicantData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantData == null)
            {
                return NotFound();
            }

            return View(applicantData);
        }

        // POST: ApplicantDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ApplicantData == null)
            {
                return Problem("Entity set 'JobPortalContext.ApplicantData'  is null.");
            }
            var applicantData = await _context.ApplicantData.FindAsync(id);
            if (applicantData != null)
            {
                _context.ApplicantData.Remove(applicantData);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicantDataExists(int id)
        {
            return (_context.ApplicantData?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
