using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoNotTrustWebClient.Data;
using DoNotTrustWebClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using DoNotTrustWebClient.Models.ApplicationUsersViewModels;

namespace DoNotTrustWebClient.Controllers
{
	// [Authorize(Roles = "SystemAdministrator, CompanyAdministrator")]
	public class ApplicationUsersController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> userManager;

		public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			this.userManager = userManager;
		}

		// GET: ApplicationUsers
		public async Task<IActionResult> Index(IndexViewModel vm)
		{
			ApplicationUser currentUser = await GetCurrentUser();

			// v�choz� odd�len� je odd�len� u�ivatele
			if (vm.DepartmentId == null)
			{
				vm.DepartmentId = currentUser.Department.DepartmentId;
			}

			// administrator sm� na v�echny odd�len�, ostatn� jen na svoji spole�nost
			if (/* userIsAdministrator ||  */ User.Identity.Name == "haken@havit.cz")
			{
				vm.Departments = _context.Department
									.Select(d => new SelectListItem() { Value = d.DepartmentId.ToString(), Text = d.Name });
			}
			else
			{
				vm.Departments = _context.Department
									.Where(d => d.Company.CompanyId == currentUser.Department.Company.CompanyId)
									.Select(d => new SelectListItem() { Value = d.DepartmentId.ToString(), Text = d.Name });
			}

			// na�ti u�ivatele podle zvolen�ho odd�len�
			vm.Users = await _context.ApplicationUser
				.Where(u => u.Department.DepartmentId == vm.DepartmentId)
				.Include(u => u.Department.Company).ToListAsync();

			return View(vm);
		}

		private async Task<ApplicationUser> GetCurrentUser()
		{
			var currentUser = await userManager.GetUserAsync(HttpContext.User);
			await _context.Entry(currentUser).Reference(u => u.Department).LoadAsync();
			await _context.Entry(currentUser.Department).Reference(d => d.Company).LoadAsync();
			return currentUser;
		}

		// GET: ApplicationUsers/Details/5
		public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.ApplicationUser
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            _context.ApplicationUser.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }
    }
}
