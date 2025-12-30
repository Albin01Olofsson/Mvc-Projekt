using DAL;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using ProjektApp.Viewmodels;
using System.Threading.Tasks;

namespace ProjektApp.Controllers
{
    public class CVController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public CVController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var cv = await _context.CVs
        .Include(c => c.Profile)
        .FirstOrDefaultAsync(c => c.ProfileId == id);

            if (cv == null)
                return NotFound();

            if (cv.Profile.IsPrivate && !User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View(cv);
        }

        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var userId = _userManager.GetUserId(User);

            var cv = await _context.CVs
                .Include(c => c.Profile)
                .FirstOrDefaultAsync(c => c.Profile.UserId == userId);

            var vm = new EditCVViewModel();

            if (cv != null)
            {
                vm.CVId = cv.CVId;
                vm.Education = cv.Education;
                vm.Experience = cv.Experience;
                vm.Skills = cv.Skills;
            }

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditCVViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
           
            var userId = _userManager.GetUserId(User);

            var cv = await _context.CVs
         .Include(c => c.Profile)
         .FirstOrDefaultAsync(c => c.CVId == model.CVId);

            if (cv == null)
            {
                var profile = await _context.Profile
            .FirstAsync(p => p.UserId == userId);

                cv = new CV
                {
                    ProfileId = profile.ProfileId,
                    Education = model.Education,
                    Experience = model.Experience,
                    Skills = model.Skills
                };
                _context.CVs.Add(cv);

            }
            else 
            {
                cv.Education = model.Education;
                cv.Experience = model.Experience;
                cv.Skills = model.Skills;

            }


                await _context.SaveChangesAsync();
            return RedirectToAction("MyProfile", "Profile");
        }

    }
}
